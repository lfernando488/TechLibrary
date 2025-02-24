using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.Security.Cryptography;
using TechLibrary.Api.Infrastructure.Security.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register
{
    public class RegisterUserUseCase
    {

        public ResponseRegisteredUserJson Execute(RequestUserJson requestUserJson)
        {
            var dbContext = new TechLibraryDbContext();
            Validate(requestUserJson, dbContext);

            var cryptography = new BCryptAlgorithm();
            var entity = new User
            {
                Email = requestUserJson.Email,
                Name = requestUserJson.Name,
                Password = cryptography.HashPassword(requestUserJson.Password)
            };
            
            dbContext.Users.Add(entity);
            dbContext.SaveChanges();

            var tokenGenerator = new JwtTokenGenerator();

            return new ResponseRegisteredUserJson
            {
                Name = entity.Name,
                AccessToken = tokenGenerator.Generate(entity)
            };
        }

        private void Validate(RequestUserJson requestUserJson, TechLibraryDbContext dbContext)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(requestUserJson);

            var existUserWithEmail = dbContext.Users.Any(user => user.Email.Equals(requestUserJson.Email));

            if (existUserWithEmail)
                result.Errors.Add(new ValidationFailure("Email","Email já cadastrado na plataforma"));

            if (result.IsValid == false)
            {
                var errorsMessage = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorsMessage);
            }
        }

    }

}
