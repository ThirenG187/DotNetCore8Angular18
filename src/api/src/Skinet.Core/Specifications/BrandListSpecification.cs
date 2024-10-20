using Skinet.Core.Entities;
using System.Linq.Expressions;

namespace Skinet.Core.Specifications;
public class BrandListSpecification : BaseSpecification<Product, string>
{
	public BrandListSpecification()
	{
		AddSelect(x => x.Brand);
		ApplyDistinct();
	}
}
