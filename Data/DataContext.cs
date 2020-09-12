using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using word_company.Models;

namespace word_company.Data 
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasOne(x => x.UserStatusModel)
                .WithOne(x => x.UserModel)
                .HasForeignKey<UserStatusModel>(x => x.UserModelId);
        }


        public DbSet<personnel_word> Personnels { get; set; }

        public DbSet<UserModel> userModels { get; set; }

        public DbSet<ExamModel> examModels { get; set; }

        public DbSet<QClass> qClasses { get; set; }

        public DbSet<Examination_form> examination_Forms { get; set; }
    }
}

