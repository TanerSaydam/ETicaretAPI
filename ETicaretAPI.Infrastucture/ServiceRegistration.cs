using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Infrastucture.Enums;
using ETicaretAPI.Infrastucture.Services;
using ETicaretAPI.Infrastucture.Services.Storage;
using ETicaretAPI.Infrastucture.Services.Storage.Azure;
using ETicaretAPI.Infrastucture.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretAPI.Infrastucture
{
    public static class ServiceRegistration
    {
        public static void AddInfrastuctureService(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
        }

        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }
        public static void AddStorage(this IServiceCollection services, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    services.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    //services.AddScoped<IStorage, LocalStorage>();
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
