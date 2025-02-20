using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register
{
    public class RegisterUserUseCase
    {

        public ResponseRegisteredUserJson Execute(RequestUserJson requestUserJson)
        {
            Validate(requestUserJson);

            var entity = new User
            {
                Email = requestUserJson.Email,
                Name = requestUserJson.Name,
                Password = requestUserJson.Password
            };

            var dbContext = new TechLibraryDbContext();
            dbContext.Users.Add(entity);
            dbContext.SaveChanges();

            return new ResponseRegisteredUserJson 
            { 
                Name = entity.Name
            };
        }

        private void Validate(RequestUserJson requestUserJson)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(requestUserJson);

            if (result.IsValid == false)
            {
                var errorsMessage = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorsMessage);
            }
        }

    }

}
