using Project.DAL.Context;

namespace Project.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
