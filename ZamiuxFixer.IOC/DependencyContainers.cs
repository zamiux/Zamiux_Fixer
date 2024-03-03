using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.Application.SendEmail;
using ZamiuxFixer.DataLayer.Contract;
using ZamiuxFixer.DataLayer.Repositories;

namespace ZamiuxFixer.IOC
{
    public static class DependencyContainers
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMailSender, SendEmail>();
        }
    }
}
