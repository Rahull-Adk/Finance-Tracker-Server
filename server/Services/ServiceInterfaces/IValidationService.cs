using server.DTOs;
using server.Models;

namespace server.Services.ServiceInterfaces
{
    public interface IValidationService
    {
        Task<Result<SignUpDTO>> ValidateSignUpDTO(SignUpDTO signUp);
        Task<Result<SignInDTO>> ValidateSignInDTO(SignInDTO signIn);
    }
}
