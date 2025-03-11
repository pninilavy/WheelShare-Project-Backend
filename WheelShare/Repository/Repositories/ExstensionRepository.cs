using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public static class ExtensionRepository
    {

        public static IServiceCollection AddRepository(this IServiceCollection service)
        {

            service.AddScoped<IRepository<User>,UserRepository>();
            service.AddScoped<IRepository<Ride>, RideRepository>();
            service.AddScoped<IRepository<RideParticipant>, RideParticipantRepository>();
            service.AddScoped<IRepository<Station>, StationRepository>();
            service.AddScoped<IRepository<Vehicle>, VehicleRepository>();
            service.AddScoped<IRepository<Payment>, PaymentRepository>();

            return service;

        }
    }
}
