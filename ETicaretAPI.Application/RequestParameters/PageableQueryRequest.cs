using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.RequestParameters
{
    public abstract class PageableQueryRequest
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
