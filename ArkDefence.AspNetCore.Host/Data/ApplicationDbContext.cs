using System;
using System.Collections.Generic;
using System.Text;
using ArkDefence.AspNetCore.Host.Models;
using Coravel.Pro.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArkDefence.AspNetCore.Host.Data
{
    public class ApplicationDbContext : IdentityDbContext, ICoravelProDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CoravelJobHistory> Coravel_JobHistory { get; set; }
        public DbSet<CoravelScheduledJob> Coravel_ScheduledJobs { get; set; }
        public DbSet<CoravelScheduledJobHistory> Coravel_ScheduledJobHistory { get; set; }

        public DbSet<MessageHistory> App_MessageHistory { get; set; }

        public DbSet<Tennant> ArkDefence_Tennant { get; set; }
        public DbSet<SystemController> ArkDefence_SystemController { get; set; }
        public DbSet<Person> ArkDefence_Users { get; set; }
        public DbSet<PersonSystemController> ArkDefence_PersonSystemController { get; set; }
        public DbSet<Card> ArkDefence_Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // enable auto history functionality.
            modelBuilder.EnableAutoHistory(changedMaxLength: null);

            modelBuilder.Entity<PersonSystemController>()
                .HasKey(ps => new { ps.PersonId, ps.SystemControllerId });
            modelBuilder.Entity<PersonSystemController>()
                .HasOne(ps => ps.Person)
                .WithMany(p => p.PersonSystemControllers)
                .HasForeignKey(ps => ps.PersonId);
            modelBuilder.Entity<PersonSystemController>()
                .HasOne(ps => ps.SystemController)
                .WithMany(p => p.PersonSystemControllers)
                .HasForeignKey(s => s.SystemControllerId);
        }
    }
}
