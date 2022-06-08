using Project.DAL.Context;

namespace Project.DAL.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
