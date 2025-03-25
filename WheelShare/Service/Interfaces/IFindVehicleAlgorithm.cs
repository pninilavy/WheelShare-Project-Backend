using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Repository.Entities;
using Repository.Interfaces;

namespace Service.Interfaces
{
    public interface IFindVehicleAlgorithm
    {
        public  Task<Vehicle> GetCar(Ride ride);
        public Task<double> WalkTimeCalculation(string origin, string destination);


    }
}
