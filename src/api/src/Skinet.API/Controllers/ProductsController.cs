using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Infrastructure.Data;

namespace Skinet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(StoreContext context) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
	{
		return await context.Products.ToListAsync();
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<Product>> GetProductById(Guid id)
	{
		var product = await context.Products.FindAsync(id);

		if (product == null)
			return NotFound();

		return product;
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> UpdateProduct(Guid id, Product product)
	{
		if (product.Id != id || !ProductExists(id))
			return BadRequest("Cannot update this product");

		context.SetModified(product);

		await context.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> DeleteProduct(Guid id)
	{
		var product = await context.Products.FindAsync(id);

		if (product == null)
			return NotFound();

		context.Products.Remove(product);
		await context.SaveChangesAsync();

		return NoContent();
	}

	private bool ProductExists(Guid id)
	{
		return context.Products.Any(x => x.Id == id);
	}

	[HttpPost]
	public async Task<ActionResult<Product>> CreateProduct(Product product)
	{
		context.Products.Add(product);
		await context.SaveChangesAsync();
		return product;
	}
}
