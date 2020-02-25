using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Okulary.Model;

namespace Okulary.Repo
{
    public class MoneyCountService
    {
        public async Task<List<MoneyCount>> GetWithFilter(Expression<Func<MoneyCount, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context
                           .Kasa
                           .AsNoTracking()
                           .AsQueryable()
                           .Where(filter)
                           .ToListAsync();
            }
        }

        public async Task<MoneyCount> Create(MoneyCount kasa)
        {
            using (var context = new MineContext())
            {
                context.Kasa.Add(kasa);
                await context.SaveChangesAsync();
            }

            return kasa;
        }
    }
}
