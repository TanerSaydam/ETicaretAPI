using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories.FileRepository;
using ETicaretAPI.Application.Repositories.InvoiceFileRepository;
using ETicaretAPI.Application.Repositories.ProductImageFileRepository;
using ETicaretAPI.Application.Repositories.ProductRepository;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;        
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IStorageService _storageService;
        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IStorageService storageService)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            });
            return Ok(new { totalCount, products });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(_productReadRepository.GetByIdAsync(id,false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {            
            await _productWriteRepository.AddAsync(new() { 
                Name = model.Name, 
                Stock = model.Stock, 
                Price = model.Price }
            );
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            var product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Price = model.Price;
            product.Name = model.Name;            
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {            
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            var datas =  await _storageService.UploadAsync("files", Request.Form.Files);
            //var datas = await _fileService.UploadAsync("resourse/product-images", Request.Form.Files);
            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage = _storageService.StorageName
            }).ToList());
            await _productImageFileWriteRepository.SaveAsync();

            //await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(p => new InvoiceFile()
            //{
            //    FileName = p.fileName,
            //    Path = p.path,
            //    Price = new Random().Next()
            //}).ToList());
            //await _invoiceFileWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
