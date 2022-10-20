using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Repositories.FileRepository
{
    public interface IFileWriteRepository : IWriteRepository<File::File>
    {
    }
}
