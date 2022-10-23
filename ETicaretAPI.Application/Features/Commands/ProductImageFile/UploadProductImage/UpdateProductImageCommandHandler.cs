using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories.ProductImageFileRepository;
using ETicaretAPI.Application.Repositories.ProductRepository;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommandRequest, UpdateProductImageCommandResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UpdateProductImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IProductReadRepository productReadRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
            _productReadRepository = productReadRepository;
        }

        public async Task<UpdateProductImageCommandResponse> Handle(UpdateProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _storageService.UploadAsync("ptoho-images", request.Files);

            ETicaretAPI.Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ETicaretAPI.Domain.Entities.ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<ETicaretAPI.Domain.Entities.Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
