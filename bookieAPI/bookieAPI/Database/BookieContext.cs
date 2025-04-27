using bookieAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace bookieAPI.Database
{
    public class BookieContext : DbContext
    {
        public DbSet<Profil> Profilok { get; set; }
        public DbSet<Cseveges> Csevegesek { get; set; }
        public DbSet<EladoTermek> EladoTermekek { get; set; }
        public DbSet<Kapcsolo> Kapcsolok { get; set; }
        public DbSet<Kep> Kepek { get; set; }
        public DbSet<Konyv> Konyvek { get; set; }
        public DbSet<Szerzo> Szerzok { get; set; }
        public DbSet<Telepules> Telepulesek { get; set; }
        public BookieContext() : base("name=BookieContext") { }
        public BookieContext(DbConnection existingConnection, bool contextOwnsConnection)
        : base(existingConnection, contextOwnsConnection) { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}