using server.Data;
using server.DTOs;
using server.Models;

namespace server.Services.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<Result<UserModel>> RegisterAsync(SignUpDTO signUpData);
        Task<Result<SignInDTO>> LoginAsync(SignInDTO signInData);
    }

}
