using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public static class ExtensionService
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddRepository();
            service.AddScoped<IUserService<User>, UserService>();
            service.AddScoped<IService<Ride>, RideService>();
            service.AddScoped<IService<RideParticipant>, RideParticipantService>();
            service.AddScoped<IService<Station>, StationService>();
            service.AddScoped<IService<Vehicle>, VehicleService>();
            service.AddScoped<IService<Payment>, PaymentService>();
            service.AddScoped<IGoogleMapsAlgorithm, GoogleMapsAlgorithm>();
            service.AddScoped<IFindVehicleAlgorithm, FindVehicleAlgorithm>();
            return service;

        }
    }
}
