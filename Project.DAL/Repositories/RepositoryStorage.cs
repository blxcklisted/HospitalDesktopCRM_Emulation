using Project.DAL.Context;

namespace Project.DAL.Repositories
{
    public class RepositoryStorage
    {
        private readonly ApplicationDbContext context;
        public RepositoryStorage(ApplicationDbContext context)
        {
            this.context = context;
            Context = context;
            DrugRepository = new GenericRepository<Drug>(context);
            ManufacturerRepository = new GenericRepository<Manufacturer>(context);
            CategoryRepository = new GenericRepository<Category>(context);
            RoleRepository = new GenericRepository<Role>(context);
            UserRepository = new GenericRepository<User>(context);
            AppointmentRepository = new GenericRepository<Appointment>(context);
        }
        public ApplicationDbContext Context { get; set; }
        public GenericRepository<Appointment> AppointmentRepository { get; set; }
        public GenericRepository<Drug> DrugRepository { get; set; }
        public GenericRepository<Manufacturer> ManufacturerRepository { get; set; }
        public GenericRepository<Category> CategoryRepository { get; set; }
        public GenericRepository<Role> RoleRepository { get; set; }
        public GenericRepository<User> UserRepository { get; set; }

    }
}
