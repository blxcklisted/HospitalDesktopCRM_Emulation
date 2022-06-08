using Project.DAL.Context;

namespace Project.DAL.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
