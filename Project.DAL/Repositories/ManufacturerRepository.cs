using Project.DAL.Context;

namespace Project.DAL.Repositories
{
    public class ManufacturerRepository : GenericRepository<Manufacturer>
    {
        public ManufacturerRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
