using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Models.DatabaseEntities;

namespace DAL.Context
{
    public class PostgreSqlEFContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Models.DatabaseEntities.TranslationSubstring> Strings { get; set; }

        public PostgreSqlEFContext(DbContextOptions<PostgreSqlEFContext> options) : base(options) { }        
    }
}
