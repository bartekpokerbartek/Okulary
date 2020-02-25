using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Okulary.Model;

namespace Okulary.Repo
{
    public class PersonService
    {
        public async Task<List<Person>> GetAll()
        {
            using (var context = new MineContext())
            {
                return await context.Persons.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Person> GetById(int personId)
        {
            using (var context = new MineContext())
            {
                return await context.Persons.AsNoTracking().SingleAsync(x => x.PersonId == personId);
            }
        }

        public async Task<bool> Exists(string firstName, string LastName, DateTime birth)
        {
            using (var context = new MineContext())
            {
                return await context.Persons.AnyAsync(x => x.FirstName == firstName && x.LastName == LastName && x.BirthDate == birth.Date);
            }
        }

        public async Task<List<Person>> GetWithFilter(Expression<Func<Person, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context
                           .Persons
                           .AsNoTracking()
                           .AsQueryable()
                           .Where(filter)
                           .ToListAsync();
            }
        }

        public async Task<List<Person>> GetWithFilterWithIncludes(Expression<Func<Person, bool>> filter)
        {
            using (var context = new MineContext())
            {
                return await context
                           .Persons
                           .Include(x => x.Binocles)
                           .AsNoTracking()
                           .AsQueryable()
                           .Where(filter)
                           .ToListAsync();
            }
        }

        public async Task<Person> Create(Person person)
        {
            using (var context = new MineContext())
            {
                context.Persons.Add(person);
                await context.SaveChangesAsync();
            }

            return person;
        }

        public async Task<Person> Update(Person person)
        {
            using (var context = new MineContext())
            {
                context.Persons.Attach(person);
                context.Entry(person).State = EntityState.Unchanged;

                context.Entry(person).Property(a => a.Address).IsModified = true;
                context.Entry(person).Property(a => a.FirstName).IsModified = true;
                context.Entry(person).Property(a => a.LastName).IsModified = true;
                context.Entry(person).Property(a => a.BirthDate).IsModified = true;
                context.Entry(person).Property(a => a.Lokalizacja).IsModified = true;

                await context.SaveChangesAsync();
            }

            return person;
        }

        public async Task Delete(int personId)
        {
            using (var context = new MineContext())
            {
                var person = await context.Persons.SingleAsync(x => x.PersonId == personId);
                context.Entry(person).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }
    }
}
