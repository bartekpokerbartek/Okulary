using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Okulary.Model;

namespace Okulary.Repo
{
    public class PayoutService
    {
        public async Task<List<Payout>> GetAll()
        {
            using (var context = new MineContext())
            {
                return await context.Wyplaty.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Payout> GetById(int payoutId)
        {
            using (var context = new MineContext())
            {
                return await context.Wyplaty.AsNoTracking().SingleAsync(x => x.PayoutId == payoutId);
            }
        }

        public async Task<List<Payout>> GetWithFilter(Expression<Func<Payout, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context.Wyplaty.AsNoTracking().AsQueryable().Where(filter).ToListAsync();
            }
        }

        public async Task<Payout> Create(Payout payout)
        {
            using (var context = new MineContext())
            {
                 context.Wyplaty.Add(payout);
                 await context.SaveChangesAsync();
            }

            return payout;
        }

        public async Task<Payout> Update(Payout payout)
        {
            using (var context = new MineContext())
            {
                context.Wyplaty.Attach(payout);
                context.Entry(payout).State = EntityState.Unchanged;

                context.Entry(payout).Property(a => a.Amount).IsModified = true;
                context.Entry(payout).Property(a => a.Description).IsModified = true;
                context.Entry(payout).Property(a => a.CreatedOn).IsModified = true;
                context.Entry(payout).Property(a => a.Lokalizacja).IsModified = true;

                await context.SaveChangesAsync();
            }

            return payout;
        }

        public async Task Delete(int payoutId)
        {
            using (var context = new MineContext())
            {
                var payout = await context.Wyplaty.SingleAsync(x => x.PayoutId == payoutId);
                context.Entry(payout).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }
    }
}
