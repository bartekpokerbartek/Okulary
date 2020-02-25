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
    public class BinocleService
    {
        public async Task<List<Binocle>> GetAll()
        {
            using (var context = new MineContext())
            {
                return await context.Binocles.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Binocle> GetById(int binocleId)
        {
            using (var context = new MineContext())
            {
                return await context.Binocles.AsNoTracking().SingleAsync(x => x.BinocleId == binocleId);
            }
        }

        public async Task<List<Binocle>> GetWithFilter(Expression<Func<Binocle, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context
                           .Binocles
                           .AsNoTracking()
                           .AsQueryable()
                           .Where(filter)
                           .ToListAsync();
            }
        }

        public async Task<List<Binocle>> GetWithFilterWithIncludes(Expression<Func<Binocle, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context
                           .Binocles
                           .AsNoTracking()
                           .Include(x => x.Person)
                           .AsQueryable()
                           .Where(filter)
                           .ToListAsync();
            }
        }

        public async Task<Binocle> Create(Binocle binocle)
        {
            using (var context = new MineContext())
            {
                context.Binocles.Add(binocle);
                await context.SaveChangesAsync();
            }

            return binocle;
        }

        //TODO: uzupełnić o propercje
        //public async Task<Binocle> Update(Binocle binocle)
        //{
        //    using (var context = new MineContext())
        //    {
        //        context.Binocles.Attach(binocle);
        //        context.Entry(binocle).State = EntityState.Unchanged;

        //        context.Entry(binocle).Property(a => a.BlizOL).IsModified = true;
        //        context.Entry(binocle).Property(a => a.BlizOP).IsModified = true;
        //        context.Entry(binocle).Property(a => a.BuyDate).IsModified = true;
        //        context.Entry(binocle).Property(a => a.CenaOprawekBliz).IsModified = true;
        //        context.Entry(binocle).Property(a => a.CenaOprawekDal).IsModified = true;
        //        context.Entry(binocle).Property(a => a.DalOL).IsModified = true;
        //        context.Entry(binocle).Property(a => a.DalOP).IsModified = true;
        //        context.Entry(binocle).Property(a => a.DataOdbioru).IsModified = true;
        //        context.Entry(binocle).Property(a => a.Description).IsModified = true;
        //        context.Entry(binocle).Property(a => a.FormaPlatnosci).IsModified = true;
        //        context.Entry(binocle).Property(a => a.NumerZlecenia).IsModified = true;
        //        context.Entry(binocle).Property(a => a.IsDataOdbioru).IsModified = true;
        //        context.Entry(binocle).Property(a => a.BlizOL).IsModified = true;
        //        //TODO: Uzupełnić pozostałe, jeśli będzie potrzeba
        //        //context.Entry(binocle).Property(a => a.BlizOP).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.BuyDate).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.CenaOprawekBliz).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.CenaOprawekDal).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.DalOL).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.DalOP).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.DataOdbioru).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.Description).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.FormaPlatnosci).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.NumerZlecenia).IsModified = true;
        //        //context.Entry(binocle).Property(a => a.IsDataOdbioru).IsModified = true;

        //        await context.SaveChangesAsync();
        //    }

        //    return binocle;
        //}

        public async Task Delete(int binocleId)
        {
            using (var context = new MineContext())
            {
                var binocle = await context.Binocles.SingleAsync(x => x.BinocleId == binocleId);
                context.Entry(binocle).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }
    }
}
