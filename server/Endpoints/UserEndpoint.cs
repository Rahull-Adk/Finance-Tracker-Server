using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.DTOs;
using server.Models;
using server.Repositories.Interfaces;
using server.Services.ServiceInterfaces;

namespace server.Controllers
{
    public static class UserEndpoint
    {
        public static IEndpointRouteBuilder UseUserAuthenticaion(this IEndpointRouteBuilder _app)
        {

            var authEndpoints = _app.MapGroup("/api/auth");
            authEndpoints.MapPost("/signUp", async ([FromBody] SignUpDTO requestModel, [FromServices] IAuthService _auth, [FromServices] IAuthRepository _authRepo) =>
            {
                try
                {
                    var result = await _auth.RegisterAsync(requestModel);
                    var user = result.Data;
                    await _authRepo.AddUserAsync(user);
                    user.Password = null;
                    return Results.Created($"/api/auth/{user.Username}", user);

                }
                catch (Exception ex)
                {
                    return Results.StatusCode(500);
                }

            });

            authEndpoints.MapPost("/signIn", async ([FromBody] SignInDTO requestModel, [FromServices] IAuthService _auth, HttpContext
                httpContext) =>
            {
                try
                {
                    var result = await _auth.LoginAsync(requestModel);
                    if (result.Data is null)
                    {
                        return Results.BadRequest(result.ErrorMessage);
                    }
                    var token = result.Message;
                    httpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                    });
                    return Results.Ok(token);
                }
                catch (Exception ex)
                {
                    return Results.StatusCode(500);
                }

            });
            authEndpoints.MapGet("/logout", async (HttpContext httpContext) =>
            {
                httpContext.Response.Cookies.Delete("AuthToken");
                return Results.Ok("User logged out successfully");
            }).RequireAuthorization().RequireAuthorization();

            authEndpoints.MapGet("/me", async ([FromServices] IUserService _userService) =>
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                currentUser.Data.Password = null;
                return Results.Ok(currentUser);
            }).RequireAuthorization();

            return _app;
        }



    }
}
