using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using VinPocket.Api.Common.DataShaping;
using VinPocket.Api.Common.Pagination;
using VinPocket.Api.Data;
using VinPocket.Api.Dtos.Budgets;
using VinPocket.Api.Dtos.Expenses;
using VinPocket.Api.Extensions;
using VinPocket.Api.Models;

namespace VinPocket.Api.Controllers;

[ApiController]
[Route("api/expenses")]
[Authorize]
public class ExpensesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all expenses")]
    [EndpointDescription("Retrieves a list of expenses")]
    [ProducesResponseType<PaginationResult<ExpenseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExpenses(
        ExpensesParameters expensesParameters,
        IValidator<ExpensesParameters> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        await validator.ValidateAndThrowAsync(expensesParameters, cancellationToken);

        var (expenseDate, sort, fields, page, pageSize) = expensesParameters;

        ShapedPaginationResult<ExpenseDto> paginationResult = await context.Expenses.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Where(x => expenseDate == null || x.ExpenseDate >= expenseDate)
            .Where(x => expenseDate == null || x.ExpenseDate <= expenseDate)
            .SortByQueryString(expensesParameters.Sort, ExpensesMappings.SortMapping.Mappings)
            .Select(ExpenseQueries.ProjectToDto())
            .ToShapedPaginationResultAsync(page, pageSize, fields, cancellationToken);

        return Ok(paginationResult);

    }

    [HttpGet("{id}")]
    [EndpointSummary("Get expense by id")]
    [EndpointDescription("Retrieves a specific expense by its unique identifier with optional field selection.")]
    [ProducesResponseType<ExpenseWithCategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExpense(
        string id,
        ExpenseParameters expenseParamters,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        string? fields = expenseParamters.Fields;

        ShapedResult<ExpenseWithCategoryDto>? result = await context.Expenses.AsNoTracking()
            .Where(x => x.Id == id && x.UserId == userId)
            .Select(ExpenseQueries.ProjectToDtoWithCategory())
            .ToShapedFirstOrDefaultAsync(fields, cancellationToken);

        return result is null ? NotFound() : Ok(result.Item);
    }

    [HttpPost]
    [EndpointSummary("Create a new expense")]
    [EndpointDescription("Creates a new expense with provided details")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType<ExpenseDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExpenseDto>> CreateExpense(
        CreateExpenseDto createExpenseDto,
        IValidator<CreateExpenseDto> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();//User.GetUserId();

        await validator.ValidateAndThrowAsync(createExpenseDto, cancellationToken);

        Expense expense = createExpenseDto.ToEntity(userId!);

        context.Expenses.Add(expense);
        await context.SaveChangesAsync(cancellationToken);

        ExpenseDto expenseDto = expense.ToDto();

        return CreatedAtAction(nameof(GetExpense), new { id = expenseDto.Id }, expenseDto);
    }

    [HttpPut("{id}")]
    [EndpointSummary("Update an expense")]
    [EndpointDescription("Update an existing expense's details with provided information")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateExpense(
        string id,
        UpdateExpenseDto updateExpenseDto,
        IValidator<UpdateExpenseDto> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        await validator.ValidateAndThrowAsync(updateExpenseDto, cancellationToken);

        Expense? expense = await context.Expenses
             .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

        if (expense is null)
        {
            return NotFound();
        }

        expense.UpdateFromDto(updateExpenseDto);

        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [EndpointSummary("Delete an expense")]
    [EndpointDescription("Permanently removes an expense from the system by its unique identifier.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExpense(
       string id,
       CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        Expense? expense = await context.Expenses
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

        if (expense is null)
        {
            return NotFound();
        }

        context.Expenses.Remove(expense);

        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
