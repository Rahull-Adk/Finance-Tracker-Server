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

            incomeGroup.MapPost("/", async (IncomeDTO income, [FromServices] AppDbContext _db, [FromServices] IAuthRepository _auth, [FromServices] IIncomeService _incomeService) =>
            {
                var responseIncome = await _incomeService.AddIncomeAsync(income);
                if (!responseIncome.IsSuccess)
                {
                    return Results.BadRequest(responseIncome.ErrorMessage);
                }
                return Results.Ok(responseIncome.Data);
            }).RequireAuthorization();

            incomeGroup.MapGet("/", async ([FromServices] IIncomeService _incomeService) =>
            {
                var incomes = await _incomeService.GetAllIncomeAsync();
                if (incomes.Data is null)
                {
                    return Results.NotFound("No incomes found");
                }
                return Results.Ok(incomes.Data);
            }).RequireAuthorization();

            incomeGroup.MapGet("/{id}", async (int id, [FromServices] IIncomeService _incomeService) =>
            {
                var income = await _incomeService.GetIncomeByIdAsync(id);
                if (!income.IsSuccess)
                {
                    return Results.NotFound(income.ErrorMessage);
                }
                return Results.Ok(income.Data);
            }).RequireAuthorization();

            incomeGroup.MapPut("/{id}", async (int id, IncomeDTO income, [FromServices] IIncomeService _incomeService) =>
            {
                var updatedIncome = await _incomeService.UpdateIncomeAsync(id, income);
                if (!updatedIncome.IsSuccess)
                {
                    return Results.BadRequest(updatedIncome.ErrorMessage);
                }
                return Results.Ok(updatedIncome.Data);
            }).RequireAuthorization();

            incomeGroup.MapDelete("/{id}", async (int id, [FromServices] IIncomeService _incomeService) =>
            {
                var result = await _incomeService.DeleteIncomeByIdAsync(id);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.ErrorMessage);
                }
                return Results.Ok(result.Message);
            }).RequireAuthorization();

            return _app;
        }
    }
}