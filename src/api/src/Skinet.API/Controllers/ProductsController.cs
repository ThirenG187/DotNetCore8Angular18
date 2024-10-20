using Microsoft.AspNetCore.Mvc;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;

namespace Skinet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
	{
		return Ok(await repo.GetProductsAsync(brand, type, sort).ConfigureAwait(false));
	}

	[HttpGet("{id:long}")]
	public async Task<ActionResult<Product?>> GetProductById(long id)
	{
		var product = await repo.GetProductByIdAsync(id);

		if (product == null)
			return NotFound();

		return product;
	}

	[HttpPut("{id:long}")]
	public async Task<ActionResult> UpdateProduct(long id, Product product)
	{
		if (!repo.ProductExists(id))
			return NotFound();

		repo.UpdateProduct(product);

		if (await repo.SaveChangesAsync())
			return NoContent();

		return BadRequest("Problem updating this product");
	}

	[HttpDelete("{id:long}")]
	public async Task<ActionResult> DeleteProduct(long id)
	{
		var product = await repo.GetProductByIdAsync(id);

		if (product == null)
			return NotFound();

		repo.DeleteProduct(product);

		if (await repo.SaveChangesAsync())
			return NoContent();

		return BadRequest("Problem deleting this product");
	}

	[HttpPost]
	public async Task<ActionResult<Product>> CreateProduct(Product product)
	{
		repo.AddProduct(product);

		if (await repo.SaveChangesAsync())
			return CreatedAtAction("GetProduct", new { id = product.Id }, product);

		return BadRequest("Problem creating product");
	}

	[HttpGet("brands")]
	public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
	{
		return Ok(await repo.GetBrandsAsync());
	}

	[HttpGet("types")]
	public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
	{
		return Ok(await repo.GetTypesAsync());
	}
}
