using Project.DAL.Context;

namespace Project.DAL.Repositories
{
    public class DrugRepository : GenericRepository<Drug>
    {
        public DrugRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
