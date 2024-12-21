using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.DTOs;
using server.Helpers;
using server.Models;
using server.Repositories.Interfaces;
using server.Services.ServiceInterfaces;

namespace server.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly GenerateToken _token;
        private readonly IValidationService _validate;
        private readonly IAuthRepository _authRepo;

        public AuthService( IValidationService validate, IAuthRepository authRepository, GenerateToken token)
        {
            _authRepo = authRepository;
            _token = token;
            _validate = validate;
        }

        public async Task<Result<UserModel>> RegisterAsync(SignUpDTO signUpData )
        {
            var isDataValid = await _validate.ValidateSignUpDTO(signUpData);
            var encryptPassword = new PasswordHasher<UserModel>();

            if (!isDataValid.IsSuccess)
            {
                return Result<UserModel>.Error(400, isDataValid.ErrorMessage);
            }
            var data = isDataValid.Data;

            var user = new UserModel()
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                Username = data.Username,
                FullName = data.FirstName,

            };
            user.Password = encryptPassword.HashPassword(user, signUpData.Password);
            return Result<UserModel>.Success(user, "User created successfully");
        }

        public async Task<Result<SignInDTO>> LoginAsync(SignInDTO signInData)
        {

            var isDataValid = await _validate.ValidateSignInDTO(signInData);
            if (!isDataValid.IsSuccess)
            {
                return Result<SignInDTO>.Error(400, isDataValid.ErrorMessage);
            }
            var data = isDataValid.Data;
            var user = await _authRepo.FindByUsernameAsync(data.EmailOrUsername);
            if(user is null)
            {
                return Result<SignInDTO>.Error(404, "User not found");
            }
            

            var token = _token.GenerateJWT(user.Data);

            var response = new SignInDTO
            {
                EmailOrUsername = user.Data.Username,
                Password = user.Data.Password,
            };
            return Result<SignInDTO>.Success(response, token);
        }

     
    }
}
