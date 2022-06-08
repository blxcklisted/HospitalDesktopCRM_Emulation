using Project.DAL.Context;

namespace Project.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
