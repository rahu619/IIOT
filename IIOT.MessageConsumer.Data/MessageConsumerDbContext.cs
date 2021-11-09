using IIOT.MessageConsumer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace IIOT.MessageConsumer.Data
{
    public class MessageConsumerDbContext : DbContext
    {
        public DbSet<Temperature> Temperatures { get; set; }

        public string DbPath { get; private set; }

        public MessageConsumerDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}iiot.db";
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Temperature>().ToTable("Temperatures");
        }
    }
}
