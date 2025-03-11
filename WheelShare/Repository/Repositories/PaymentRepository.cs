using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class PaymentRepository:IRepository<Payment>
    {
        private readonly IContext context;
        public PaymentRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<Payment> Add(Payment item)
        {
            await context.Payments.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task Delete(int id)
        {
            context.Payments.Remove(await GetById(id));
            await context.Save();
        }

        public async Task<List<Payment>> GetAll()
        {
            return await context.Payments.ToListAsync();
        }

        public async Task<Payment> GetById(int id)
        {
            return await context.Payments.FirstOrDefaultAsync(payment => payment.Id == id);
        }

        public async Task<Payment> Update(int id, Payment item)
        {
            Payment payment=await GetById(id);
            payment.RideId = item.RideId;
            payment.Ride=item.Ride;
            payment.UserId = item.UserId;
            payment.User = item.User;
            payment.Amount = item.Amount;
            payment.Status = item.Status;
            await context.Save();
            return payment;
       
    }
    }
}
