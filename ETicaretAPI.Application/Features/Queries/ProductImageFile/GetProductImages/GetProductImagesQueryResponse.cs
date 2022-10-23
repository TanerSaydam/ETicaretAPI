using ETicaretAPI.Application.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryResponse : BaseQueryRequest
    {
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
