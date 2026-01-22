using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using VinPocket.Api.Common.DataShaping;
using VinPocket.Api.Common.Pagination;
using VinPocket.Api.Data;
using VinPocket.Api.Dtos.Budgets;
using VinPocket.Api.Dtos.Categories;
using VinPocket.Api.Extensions;
using VinPocket.Api.Models;

namespace VinPocket.Api.Controllers;

[Route("api/budgets")]
[Authorize]
[ApiController]
public class BudgetsController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all budgets")]
    [EndpointDescription("Retrieves a list of budgets")]
    [ProducesResponseType<PaginationResult<BudgetDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBudgets(
        BudgetsParameters budgetParameters,
        IValidator<BudgetsParameters> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        await validator.ValidateAndThrowAsync(budgetParameters, cancellationToken);

        var (startDate, endDate, sort, fields, page, pageSize) = budgetParameters;

        ShapedPaginationResult<BudgetDto> paginationResult = await context.Budgets.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Where(x => startDate == null || x.StartDate >= startDate)
            .Where(x => endDate == null || x.EndDate <= endDate)
            .SortByQueryString(sort, BudgetMappings.SortMapping.Mappings)
            .Select(BudgetQueries.ProjectToDto())
            .ToShapedPaginationResultAsync(page, pageSize, fields, cancellationToken);

        return Ok(paginationResult);

    }

    [HttpGet("{id}")]
    [EndpointSummary("Get budget by id")]
    [EndpointDescription("Retrieves a specific budget by its unique identifier with optional field selection.")]
    [ProducesResponseType<BudgetWithCategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBuget(
        string id,
        BudgetParameters budgetParameters,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        string? fields = budgetParameters.Fields;

        ShapedResult<BudgetWithCategoryDto>? result = await context.Budgets.AsNoTracking()
            .Where(x => x.Id == id && x.UserId == userId)
            .Select(BudgetQueries.ProjectToDtoWithCategory())
            .ToShapedFirstOrDefaultAsync(fields, cancellationToken);

        return result is null ? NotFound() : Ok(result.Item);
    }

    [HttpPost]
    [EndpointSummary("Create a new budget")]
    [EndpointDescription("Creates a new budget with provided details")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType<BudgetDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BudgetDto>> CreateBudget(
        CreateBudgetDto createBudgetDto,
        IValidator<CreateBudgetDto> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        await validator.ValidateAndThrowAsync(createBudgetDto, cancellationToken);

        Budget budget = createBudgetDto.ToEntity(userId!);

        context.Budgets.Add(budget);
        await context.SaveChangesAsync(cancellationToken);

        BudgetDto budgetDto = budget.ToDto();

        return CreatedAtAction(nameof(GetBuget), new { id = budgetDto.Id }, budgetDto);
    }

    [HttpPut("{id}")]
    [EndpointSummary("Update a budget")]
    [EndpointDescription("Update an existing budget's details with provided information")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBudget(
        string id,
        UpdateBudgetDto updateBudgetDto,
        IValidator<UpdateBudgetDto> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        await validator.ValidateAndThrowAsync(updateBudgetDto, cancellationToken);

        Budget? budget = await context.Budgets
             .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

        if (budget is null)
        {
            return NotFound();
        }

        budget.UpdateFromDto(updateBudgetDto);

        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [EndpointSummary("Delete a budget")]
    [EndpointDescription("Permanently removes a budget from the system by its unique identifier.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBudget(
       string id,
       CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        Budget? budget = await context.Budgets
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

        if (budget is null)
        {
            return NotFound();
        }

        context.Budgets.Remove(budget);

        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
