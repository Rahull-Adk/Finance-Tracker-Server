using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.DTOs;
using server.Models;
using server.Repositories.Interfaces;
using server.Services.ServiceInterfaces;

namespace server.Endpoints
{
    public static class ExpenseEndpoint
    {
        public static IEndpointRouteBuilder UseExpense(this IEndpointRouteBuilder _app)
        {
            var expenseGroup = _app.MapGroup("api/users/myExpenses");

            expenseGroup.MapPost("/", async (ExpenseDTO expense, [FromServices] AppDbContext _db, [FromServices] IAuthRepository _auth, [FromServices] IExpenseService _expenseService) =>
            {
                var responseExpense = await _expenseService.AddExpenseAsync(expense);
                if (!responseExpense.IsSuccess)
                {
                    return Results.BadRequest(responseExpense.ErrorMessage);
                }
                return Results.Ok(responseExpense.Data);
            }).RequireAuthorization();

            expenseGroup.MapGet("/", async ([FromServices] IExpenseService _expenseService) =>
            {
                var expenses = await _expenseService.GetAllExpensesAsync();
                if (expenses.Data is null)
                {
                    return Results.NotFound("No expenses found");
                }
                return Results.Ok(expenses.Data);
            }).RequireAuthorization();

            expenseGroup.MapGet("/{id}", async (int id, [FromServices] IExpenseService _expenseService) =>
            {
                var expense = await _expenseService.GetExpenseByIdAsync(id);
                if (!expense.IsSuccess)
                {
                    return Results.NotFound(expense.ErrorMessage);
                }
                return Results.Ok(expense.Data);
            }).RequireAuthorization();

            expenseGroup.MapPut("/{id}", async (int id, ExpenseDTO expense, [FromServices] IExpenseService _expenseService) =>
            {
                var updatedExpense = await _expenseService.UpdateExpenseAsync(id, expense);
                if (!updatedExpense.IsSuccess)
                {
                    return Results.BadRequest(updatedExpense.ErrorMessage);
                }
                return Results.Ok(updatedExpense.Data);
            }).RequireAuthorization();

            expenseGroup.MapDelete("/{id}", async (int id, [FromServices] IExpenseService _expenseService) =>
            {
                var result = await _expenseService.DeleteExpenseByIdAsync(id);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.ErrorMessage);
                }
                return Results.Ok(result.Message);
            }).RequireAuthorization();

            expenseGroup.MapGet("/category/{category}", async (string category, [FromServices] IExpenseService _expenseService) =>
            {
                var expenses = await _expenseService.GetExpensesByCategoryAsync(category);
                if (!expenses.IsSuccess)
                {
                    return Results.NotFound(expenses.ErrorMessage);
                }
                return Results.Ok(expenses.Data);
            }).RequireAuthorization();

            return _app;
        }
    }
}