using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using server.Data;
using server.DTOs;
using server.Helpers;
using server.Models;
using server.Services.ServiceInterfaces;
using System.Text.RegularExpressions;

namespace server.Services.Implementations
{
    public class ValidationService : IValidationService
    {
        private readonly AppDbContext _db;
        public ValidationService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<SignUpDTO>> ValidateSignUpDTO(SignUpDTO signUpData)
        {
            if (signUpData.Email is null || signUpData.Username is null)
            {
                return Result<SignUpDTO>.Error(400, "Username and Email is required");
            }

            if (signUpData.Username.Length > 20 || signUpData.Username.Length < 4)
            {
                return Result<SignUpDTO>.Error(400, "Username must be atleast 4 character but must not exceed 20");
            }

            var userAlreadyExists = await _db.Users.AnyAsync((u) => u.Email == signUpData.Email || u.Username == signUpData.Username);

            if (userAlreadyExists)
            {
                return Result<SignUpDTO>.Error(400, "User with this email or username already exists, Please sign in");
            }




            var regex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(signUpData.Email, regex))
            {
                return Result<SignUpDTO>.Error(400, "Invalid email address");
            }


            if (signUpData.Password.Length > 20 || signUpData.Password.Length < 6)
            {
                return Result<SignUpDTO>.Error(400, "Password must be atleast 6 character but must not exceed 20");
            }

            if (string.IsNullOrEmpty(signUpData.FullName) || string.IsNullOrEmpty(signUpData.FirstName) || string.IsNullOrEmpty(signUpData.LastName))
            {
                return Result<SignUpDTO>.Error(400, "Please fill all names");

            }
            return Result<SignUpDTO>.Success(signUpData, "Data validated successfully");
        }

        public async Task<Result<SignInDTO>> ValidateSignInDTO(SignInDTO data)
        {
            if (data.EmailOrUsername is null)
            {
                return Result<SignInDTO>.Error(400, "Username and Email is required");
            }

            var user = await _db.Users.FirstOrDefaultAsync((u) => u.Email == data.EmailOrUsername || u.Username == data.EmailOrUsername);

            if (user is null)
            {
                return Result<SignInDTO>.Error(404, "User not found");
            }

            var passwordHash = new PasswordHasher<UserModel>();
            var isPasswordValid = passwordHash.VerifyHashedPassword(user, user.Password, data.Password);

            if (isPasswordValid != PasswordVerificationResult.Success)
            {
                return Result<SignInDTO>.Error(400, "Invalid credentials");
            }
         

            return Result<SignInDTO>.Success(new SignInDTO { EmailOrUsername = user.Username }, "Logged in successfully");
        }
    }
}
