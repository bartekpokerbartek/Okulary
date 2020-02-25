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
    public class ElementsService
    {
        public async Task<List<Element>> GetAll()
        {
            using (var context = new MineContext())
            {
                return await context.Elements.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Element> GetById(int elementId)
        {
            using (var context = new MineContext())
            {
                return await context.Elements.AsNoTracking().SingleAsync(x => x.ElementId == elementId);
            }
        }

        public async Task<List<Element>> GetWithFilter(Expression<Func<Element, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context
                           .Elements
                           .AsNoTracking()
                           .AsQueryable()
                           .Where(filter)
                           .ToListAsync();
            }
        }

        public async Task<Element> Create(Element element)
        {
            using (var context = new MineContext())
            {
                context.Elements.Add(element);
                await context.SaveChangesAsync();
            }

            return element;
        }

        public async Task<Element> Update(Element element)
        {
            using (var context = new MineContext())
            {
                context.Elements.Attach(element);
                context.Entry(element).State = EntityState.Unchanged;

                context.Entry(element).Property(a => a.Nazwa).IsModified = true;
                context.Entry(element).Property(a => a.DataSprzedazy).IsModified = true;
                context.Entry(element).Property(a => a.FormaPlatnosci).IsModified = true;
                context.Entry(element).Property(a => a.Ilosc).IsModified = true;
                context.Entry(element).Property(a => a.Cena).IsModified = true;

                await context.SaveChangesAsync();
            }

            return element;
        }

        public async Task Delete(int elementId)
        {
            using (var context = new MineContext())
            {
                var element = await context.Elements.SingleAsync(x => x.ElementId == elementId);
                context.Entry(element).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }
    }
}
