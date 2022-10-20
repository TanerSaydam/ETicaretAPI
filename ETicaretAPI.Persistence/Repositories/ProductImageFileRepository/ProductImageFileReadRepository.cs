using ETicaretAPI.Application.Repositories.ProductImageFileRepository;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories.ProductImageFileRepository
{
    public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
