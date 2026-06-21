using API.DB;
using API.Models.DTO.Categorys;
using API.Models.DTO.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ExamApiContext db;
    public ProductsController(ExamApiContext db)
    {
        this.db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts()
    {
        var products = await db.Products.Include(p => p.Category)
            .Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToListAsync();
        
        return Ok(products);
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReadDto>> GetByIdProduct(uint id)
    {
        var product = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(c => c.Id  == id);
        if (product is null)
            return NotFound("Товар не найден");
        
        return Ok(new ProductReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name
        });
    }

    [HttpPost]
    public async Task<ActionResult<ProductCreateDto>> CreateProduct(ProductCreateDto request)
    {
        var category = await db.Categories.AnyAsync(c => c.Id == request.CategoryId);
        if (!category)
            return BadRequest("Категория не найдена");
        
        var now = DateTime.UtcNow;
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now,
            CategoryId = request.CategoryId
        };
        
        db.Products.Add(product);
        await db.SaveChangesAsync();

        var categoryName = await db.Categories.Where(c => c.Id == product.CategoryId)
            .Select(c => c.Name).FirstAsync();

        var responce = new ProductReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = categoryName
        };
        
        return CreatedAtAction("GetByIdProduct", new { id = product.Id }, responce);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductUpdateDto>> UpdateProduct(uint id, ProductUpdateDto request)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id  == id);
        if (product is null)
            return NotFound("Такого товара нет");

        var category = await db.Categories.AnyAsync(c => c.Id == product.CategoryId);
        if (!category)
            return BadRequest("Категория не найдена");
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync();

        var categorys = await  db.Categories.FirstOrDefaultAsync(c => c.Id == product.CategoryId);
        
        var response = new ProductReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = categorys.Name
        };
        
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(uint id)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
        if(product is null)
            return NotFound("Такого товара нет");

        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        
        return NoContent();
    }
}