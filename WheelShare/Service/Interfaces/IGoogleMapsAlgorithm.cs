using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Service.Models;
using Service.NewFolder;

namespace Service.Interfaces
{
    public interface IGoogleMapsAlgorithm
    {

        public Task OptimalPlaceMent(Ride ride);
        public Task<Help> OptimalDriver(Ride driver, Ride partner, double driverPrice, double partnerPrice, double part1);


    }
}
