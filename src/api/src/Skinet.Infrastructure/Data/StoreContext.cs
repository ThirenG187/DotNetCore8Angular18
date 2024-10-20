using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Infrastructure.EntityConfig;

namespace Skinet.Infrastructure.Data;

public class StoreContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<Product> Products { get; set; }

	public void SetModified<T>(T entity) where T : BaseEntity {
		Entry(entity).State = EntityState.Modified;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
	}	
}
