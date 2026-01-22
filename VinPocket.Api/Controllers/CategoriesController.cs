using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinPocket.Api.Common.DataShaping;
using VinPocket.Api.Common.Pagination;
using VinPocket.Api.Data;
using VinPocket.Api.Dtos.Budgets;
using VinPocket.Api.Dtos.Categories;
using VinPocket.Api.Extensions;
using VinPocket.Api.Models;

namespace VinPocket.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(
    AppDbContext context) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all categories")]
    [EndpointDescription("Retrieves a list of categories")]
    [ProducesResponseType<PaginationResult<CategoryDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(
        CategoriesParameters categoriesParameters,
        IValidator<CategoriesParameters> validator,
        CancellationToken cancellationToken)
    {

        await validator.ValidateAndThrowAsync(categoriesParameters, cancellationToken);

        var (sort, fields, page, pageSize) = categoriesParameters;

        ShapedPaginationResult<CategoryDto> paginationResult = await context.Categories.AsNoTracking()
            .SortByQueryString(sort, CategoryMappings.SortMapping.Mappings)
            .Select(CategoryQueries.ProjectToDto())
            .ToShapedPaginationResultAsync(page, pageSize, fields, cancellationToken);

        return Ok(paginationResult);
    }

    [HttpGet("{id}")]
    [EndpointSummary("Get category by id")]
    [EndpointDescription("Retrieves a specific category by its unique identifier with optional field selection.")]
    [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory(
        string id,
        CategoryParameters categoryParameters,
        CancellationToken cancellationToken)
    {
        string? fields = categoryParameters.Fields;

        ShapedResult<CategoryDto>? result = await context.Categories.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(CategoryQueries.ProjectToDto())
            .ToShapedFirstOrDefaultAsync(fields, cancellationToken);

        return result is null ? NotFound() : Ok(result.Item);
    }

    [HttpPost]
    [EndpointSummary("Create a new category")]
    [EndpointDescription("Creates a new category with provided information")]
    [ProducesResponseType<CategoryDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory(
        CreateCategoryDto createCategoryDto,
        IValidator<CreateCategoryDto> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createCategoryDto, cancellationToken);

        Category category = createCategoryDto.ToEntity();

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        CategoryDto categoryDto = category.ToDto();

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = categoryDto.Id },
            categoryDto);
    }

    [HttpPut("{id}")]
    [EndpointSummary("Update a category")]
    [EndpointDescription("Update an existing category's details with provided information")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(
        string id,
        UpdateCategoryDto updateCategoryDto,
        IValidator<UpdateCategoryDto> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updateCategoryDto, cancellationToken);

        Category? category = await context.Categories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound();
        }

        category.UpdateFromDto(updateCategoryDto);

        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [EndpointSummary("Delete a category")]
    [EndpointDescription("Permanently removes a category from the system by its unique identifier.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(
       string id,
       CancellationToken cancellationToken)
    {
        Category? category = await context.Categories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound();
        }

        context.Categories.Remove(category);

        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
