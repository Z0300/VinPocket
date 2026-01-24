using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using VinPocket.Api.Common.DataShaping;
using VinPocket.Api.Common.Pagination;
using VinPocket.Api.Data;
using VinPocket.Api.Dtos.Budgets;
using VinPocket.Api.Dtos.Incomes;
using VinPocket.Api.Extensions;
using VinPocket.Api.Models;

namespace VinPocket.Api.Controllers;

[Route("api/incomes")]
[ApiController]
public class IncomesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all incomes")]
    [EndpointDescription("Retrieves a list of incomes")]
    [ProducesResponseType<PaginationResult<IncomeDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncomes(
        IncomesParameters incomeParamters,
        IValidator<IncomesParameters> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        await validator.ValidateAndThrowAsync(incomeParamters, cancellationToken);

        var (incomeDate, sort, fields, page, pageSize) = incomeParamters;

        ShapedPaginationResult<IncomeDto> paginationResult = await context.Incomes.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Where(x => incomeDate == null || x.IncomeDate >= incomeDate)
            .Where(x => incomeDate == null || x.IncomeDate <= incomeDate)
            .SortByQueryString(sort, IncomeMappings.SortMapping.Mappings)
            .Select(IncomeQueries.ProjectToDto())
            .ToShapedPaginationResultAsync(page, pageSize, fields, cancellationToken);

        return Ok(paginationResult);

    }

    [HttpGet("{id}")]
    [EndpointSummary("Get income by id")]
    [EndpointDescription("Retrieves a specific income by its unique identifier with optional field selection.")]
    [ProducesResponseType<BudgetWithCategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIncome(
        string id,
        IncomeParameters incomeParamters,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        string? fields = incomeParamters.Fields;

        ShapedResult<IncomeWithCategoryDto>? result = await context.Incomes.AsNoTracking()
            .Where(x => x.Id == id && x.UserId == userId)
            .Select(IncomeQueries.ProjectToDtoWithCategory())
            .ToShapedFirstOrDefaultAsync(fields, cancellationToken);

        return result is null ? NotFound() : Ok(result.Item);
    }

    [HttpPost]
    [EndpointSummary("Create a new income")]
    [EndpointDescription("Creates a new income with provided details")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType<IncomeDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BudgetDto>> CreateBudget(
        CreateIncomeDto createIncomeDto,
        IValidator<CreateIncomeDto> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        await validator.ValidateAndThrowAsync(createIncomeDto, cancellationToken);

        Income income = createIncomeDto.ToEntity(userId!);

        context.Incomes.Add(income);
        await context.SaveChangesAsync(cancellationToken);

        IncomeDto incomeDto = income.ToDto();

        return CreatedAtAction(nameof(GetIncome), new { id = incomeDto.Id }, incomeDto);
    }

    [HttpPut("{id}")]
    [EndpointSummary("Update an income")]
    [EndpointDescription("Update an existing income's details with provided information")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateIncome(
        string id,
        UpdateIncomeDto updateIncomeDto,
        IValidator<UpdateIncomeDto> validator,
        CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        await validator.ValidateAndThrowAsync(updateIncomeDto, cancellationToken);

        Income? income = await context.Incomes
             .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

        if (income is null)
        {
            return NotFound();
        }

        income.UpdateFromDto(updateIncomeDto);

        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [EndpointSummary("Delete an income")]
    [EndpointDescription("Permanently removes an income from the system by its unique identifier.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBudget(
       string id,
       CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        Income? income = await context.Incomes
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

        if (income is null)
        {
            return NotFound();
        }

        context.Incomes.Remove(income);

        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
