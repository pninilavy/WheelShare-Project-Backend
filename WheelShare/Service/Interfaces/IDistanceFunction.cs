using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace Service.Interfaces
{
    public interface IDistanceFunction
    {
        public Task<Coordinate> GetCoordinatesAsync(string address);
    }
}
