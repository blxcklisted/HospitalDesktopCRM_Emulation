using Project.BLL.DTO;
using Project.BLL.Interfaces;
using Project.DAL.Repositories;

namespace Project.BLL.Services
{
    public class ServiceStorage
    {
        private readonly RepositoryStorage repository;
        public ServiceStorage(RepositoryStorage repository)
        {
            this.repository = repository;
            AppointmentService = new AppointmentService(repository);
            CategoryService = new CategoryService(repository);
            DrugService = new DrugService(repository);
            ManufacturerService = new ManufacturerService(repository);
            RoleService = new RoleService(repository);
            UserService = new UserService(repository);

        }
        public IService<AppointmentDTO> AppointmentService { get; set; }
        public IService<CategoryDTO> CategoryService { get; set; }
        public IService<DrugDTO> DrugService { get; set; }
        public IService<ManufacturerDTO> ManufacturerService { get; set; }
        public IService<RoleDTO> RoleService { get; set; }
        public IService<UserDTO> UserService { get; set; }
    }
}
