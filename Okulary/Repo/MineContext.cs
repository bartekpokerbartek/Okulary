﻿using Okulary.Model;
using System.Data.Entity;

namespace Okulary.Repo
{
    public class MineContext : DbContext
    {
        public MineContext() : base ("name=OkularyDB")
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Binocle> Binocles { get; set; }

        public DbSet<Element> Elements { get; set; }
    }
}