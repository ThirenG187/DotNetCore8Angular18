using Microsoft.AspNetCore.Mvc;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Core.Specifications;

namespace Skinet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
	{
		var spec = new ProductSpecification(brand, type, sort);
		var products = await repo.ListAsync(spec);
		return Ok(products);
	}

	[HttpGet("{id:long}")]
	public async Task<ActionResult<Product?>> GetProductById(long id)
	{
		var product = await repo.GetByIdAsync(id);

		if (product == null)
			return NotFound();

		return product;
	}

	[HttpPut("{id:long}")]
	public async Task<ActionResult> UpdateProduct(long id, Product product)
	{
		if (!repo.Exists(id))
			return NotFound();

		repo.Update(product);

		if (await repo.SaveAllAsync())
			return NoContent();

		return BadRequest("Problem updating this product");
	}

	[HttpDelete("{id:long}")]
	public async Task<ActionResult> DeleteProduct(long id)
	{
		var product = await repo.GetByIdAsync(id);

		if (product == null)
			return NotFound();

		repo.Remove(product);

		if (await repo.SaveAllAsync())
			return NoContent();

		return BadRequest("Problem deleting this product");
	}

	[HttpPost]
	public async Task<ActionResult<Product>> CreateProduct(Product product)
	{
		repo.Add(product);

		if (await repo.SaveAllAsync())
			return CreatedAtAction("GetProduct", new { id = product.Id }, product);

		return BadRequest("Problem creating product");
	}

	[HttpGet("brands")]
	public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
	{
		var spec = new BrandListSpecification();
		return Ok(await repo.ListAsync(spec));
	}

	[HttpGet("types")]
	public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
	{
		var spec = new TypeListSpecification();
		return Ok(await repo.ListAsync(spec));
	}
}
