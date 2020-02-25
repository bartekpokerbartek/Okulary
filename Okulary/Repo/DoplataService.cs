using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Okulary.Model;

namespace Okulary.Repo
{
    public class DoplataService
    {
        public async Task<List<Doplata>> GetAll()
        {
            using (var context = new MineContext())
            {
                return await context.Doplaty.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Doplata> GetById(int doplataId)
        {
            using (var context = new MineContext())
            {
                return await context.Doplaty.AsNoTracking().SingleAsync(x => x.DoplataId == doplataId);
            }
        }

        public async Task<List<Doplata>> GetWithFilter(Expression<Func<Doplata, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context
                           .Doplaty
                           .AsNoTracking()
                           .AsQueryable()
                           .Where(filter)
                           .ToListAsync();
            }
        }

        public async Task<List<Doplata>> GetWithFilterWithIncludes(Expression<Func<Doplata, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context
                           .Doplaty
                           .AsNoTracking()
                           .Include(x => x.Binocle)
                           .Include(x => x.Binocle.Person)
                           .AsQueryable()
                           .Where(filter)
                           .ToListAsync();
            }
        }

        public async Task<Doplata> Create(Doplata doplata)
        {
            using (var context = new MineContext())
            {
                context.Doplaty.Add(doplata);
                await context.SaveChangesAsync();
            }

            return doplata;
        }

        public async Task<Doplata> Update(Doplata doplata)
        {
            using (var context = new MineContext())
            {
                context.Doplaty.Attach(doplata);
                context.Entry(doplata).State = EntityState.Unchanged;

                context.Entry(doplata).Property(a => a.DataDoplaty).IsModified = true;
                context.Entry(doplata).Property(a => a.Kwota).IsModified = true;
                context.Entry(doplata).Property(a => a.FormaPlatnosci).IsModified = true;

                await context.SaveChangesAsync();
            }

            return doplata;
        }

        public async Task Delete(int doplataId)
        {
            using (var context = new MineContext())
            {
                var doplata = await context.Doplaty.SingleAsync(x => x.DoplataId == doplataId);
                context.Entry(doplata).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }
    }
}
