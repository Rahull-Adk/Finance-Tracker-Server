using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.DTOs;
using server.Models;
using server.Repositories.Interfaces;
using server.Services.ServiceInterfaces;

namespace server.Endpoints
{
    public static class IncomeEndpoint
    {
        public static IEndpointRouteBuilder UseIncome(this IEndpointRouteBuilder _app)
        {
            var incomeGroup = _app.MapGroup("api/users/myIncomes");
            incomeGroup.MapGet("/", async ([FromServices] IIncomeService _incomeService) =>
            {
                var incomes = await _incomeService.GetAllIncomeAsync();
                if (incomes.Data is null)
                {
                    return Results.NotFound("No income found");
                }
                return Results.Ok(incomes.Data);
            }).RequireAuthorization();


            incomeGroup.MapGet("/{id}", async (int id, [FromServices] IIncomeService _incomeService) =>
            {
                var incomes = await _incomeService.GetIncomeByIdAsync(id);
                if (incomes.Data is null)
                {
                    return Results.NotFound("No income found");
                }
                return Results.Ok(incomes.Data);
            }).RequireAuthorization();


            incomeGroup.MapPost("/", async (IncomeDTO income, [FromServices] AppDbContext _db, [FromServices] IAuthRepository _auth, [FromServices] IIncomeService _incomeService) =>
            {
                var responseIncome = await _incomeService.AddIncomeAsync(income);
                if (!responseIncome.IsSuccess)
                {
                    return Results.BadRequest(responseIncome.ErrorMessage);
                }
                return Results.Ok(responseIncome.Data);
            }).RequireAuthorization();


            incomeGroup.MapPost("/{id}", async (int id, [FromServices] IIncomeService _incomeService, [FromBody] IncomeModel income) =>
            {
                var updatedIncome = await _incomeService.UpdateIncomeAsync(id, income);
                if (!updatedIncome.IsSuccess)
                {
                    return Results.StatusCode(500);
                }
                return Results.Ok(updatedIncome);
            }).RequireAuthorization();


            incomeGroup.MapDelete("/{id}", async (int id, [FromServices] IIncomeService _incomeService) =>
            {
                var response = await _incomeService.DeleteIncomeByIdAsync(id);
                if (!response.IsSuccess)
                {
                    return Results.NotFound(response.ErrorMessage);
                }
                return Results.Ok("Income Deleted Successfully");
            }).RequireAuthorization();


            return _app;

        }


    }
}
