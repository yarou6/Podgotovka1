using API.DB;
using API.Models.DTO.Categorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ExamApiContext db;
    
    public CategoriesController(ExamApiContext db)
    {
        this.db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetCategories()
    {
        var categories = await db.Categories
            .Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToListAsync();
        
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryReadDto>> GetByIdCategories(uint id)
    {
        var categories = await db.Categories.FirstOrDefaultAsync(c =>  c.Id == id);
        
        if(categories is null)
            return  NotFound("Такой категории нет");
        
        return Ok(new CategoryReadDto
        {
            Id = categories.Id,
            Name = categories.Name,
            Description = categories.Description,
        });
    }

    [HttpPost]
    public async Task<ActionResult<CategoryCreateDto>> CreateCategory([FromBody] CategoryCreateDto request)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };
        
        db.Categories.Add(category);
        await db.SaveChangesAsync();

        var response = new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        
        return CreatedAtAction(nameof(GetByIdCategories), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryReadDto>> UpdateCategory(uint id, [FromBody] CategoryUpdateDto request)
    {
        var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category is null)
            return NotFound("Такой категории нет");
        
        category.Name = request.Name;
        category.Description = request.Description;
        
        await db.SaveChangesAsync();

        var response = new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(uint id)
    {
        var category = await db.Categories.FirstOrDefaultAsync(c => c.Id  == id);

        if (category is null)
            return NotFound("Такой категории нет");
        
        var HasProducts = await db.Products.AnyAsync(p => p.CategoryId == id);
        if (HasProducts)
            return BadRequest("Нельзя удалить категорию, потому что к ней привязаны продукты");
        
        db.Categories.Remove(category);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
