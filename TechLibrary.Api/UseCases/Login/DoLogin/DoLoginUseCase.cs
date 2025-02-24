using System.Security.Cryptography.X509Certificates;
using TechLibrary.Api.Infrastructure.Security.Cryptography;
using TechLibrary.Api.Infrastructure.Security.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin
{
    public class DoLoginUseCase
    {

        public ResponseRegisteredUserJson Execute(RequestLoginJson request)
        {
            var dbContext = new TechLibraryDbContext();

            var entity = dbContext.Users.FirstOrDefault(user => user.Email.Equals(request.Email));

            if (entity == null)
                throw new InvalidLoginException();

            var cryptography = new BCryptAlgorithm();
            var passwordIsValid = cryptography.Verify(request.Password, entity);

            if(passwordIsValid == false)
                throw new InvalidLoginException();

            var tokenGenerator = new JwtTokenGenerator();

            return new ResponseRegisteredUserJson
            {
                Name = entity.Name,
                AccessToken = tokenGenerator.Generate(entity)
            };
        }

    }
}
