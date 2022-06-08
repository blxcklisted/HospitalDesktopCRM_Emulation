using System.Data.Entity;

namespace Project.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("HospitalCS") { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
