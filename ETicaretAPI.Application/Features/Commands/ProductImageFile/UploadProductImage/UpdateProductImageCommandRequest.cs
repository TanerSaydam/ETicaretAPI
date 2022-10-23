using ETicaretAPI.Application.RequestParameters;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UpdateProductImageCommandRequest : BaseQueryRequest, IRequest<UpdateProductImageCommandResponse>
    {
        public IFormFileCollection Files { get; set; }
    } 
}
