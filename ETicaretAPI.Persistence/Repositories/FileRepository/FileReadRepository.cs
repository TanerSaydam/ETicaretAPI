using ETicaretAPI.Application.Repositories.FileRepository;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = ETicaretAPI.Domain.Entities.File;

namespace ETicaretAPI.Persistence.Repositories.FileRepository
{
    public class FileReadRepository : ReadRepository<File>, IFileReadRepository
    {
        public FileReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
