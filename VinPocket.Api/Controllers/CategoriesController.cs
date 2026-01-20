using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinPocket.Api.Data;
using VinPocket.Api.Dtos.Categories;
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
    [ProducesResponseType<CategoryResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await context.Categories
            .AsNoTracking()
            .Select(x => new CategoryResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Get category by id")]
    [EndpointDescription("Retrieve a specific by id")]
    [ProducesResponseType<CategoryResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound();
        }

        return Ok(new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt
        });
    }

    [HttpPost]
    [EndpointSummary("Add category")]
    [EndpointDescription("Add new category")]
    [ProducesResponseType<CategoryResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryDto request, 
        IValidator<CreateCategoryDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(errors);
        }

        var category = new Category { Name = request.Name };

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.Id },
            category);
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Update category")]
    [EndpointDescription("Update a category by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCategory(
        Guid id,
        [FromBody] UpdateCategoryDto request,
        IValidator<UpdateCategoryDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(errors);
        }

        var existingCategory = await context.Categories.FindAsync([id], cancellationToken);

        if (existingCategory is null)
        {
            return NotFound();
        }

        existingCategory.Name = request.Name;
        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Delete category")]
    [EndpointDescription("Delete a category by id")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCategory(
       Guid id,
       CancellationToken cancellationToken)
    {
        var existingCategory = await context.Categories.FindAsync([id], cancellationToken);

        if (existingCategory is null)
        {
            return NotFound();
        }

        context.Categories.Remove(existingCategory);
        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
