using ETicaretAPI.Application.RequestParameters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Product.DeleteProduct
{
    public class DeleteProductCommandRequest : BaseQueryRequest, IRequest<DeleteProductCommandResponse>
    {
    }
}
