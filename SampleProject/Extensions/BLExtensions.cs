using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleProject.DAL;
using SampleProject.DAL.Interface;
using System.CodeDom;

namespace SampleProject.Extensions
{
    public static class BLExtensions
    {

        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUserProductRepository , UserProductRepository>();    
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); 
            return services; 
        }
    }
}
