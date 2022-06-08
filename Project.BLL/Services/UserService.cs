using AutoMapper;
using Project.BLL.DTO;
using Project.BLL.Interfaces;
using Project.DAL.Context;
using Project.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class UserService : IService<UserDTO>
    {
        private RepositoryStorage repository;

        private IMapper mapper;
        public UserService(RepositoryStorage repository)
        {
            this.repository = repository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(user => user.UserId))
                .ForMember(x => x.Name, x => x.MapFrom(user => user.UserName))
                .ForMember(x => x.Password, x => x.MapFrom(user => user.UserPassword))
                .ForMember(x => x.Email, x => x.MapFrom(user => user.UserEmail))
                .ForMember(x => x.RoleName, x => x.MapFrom(user => user.Role.RoleName))
                .ForMember(x => x.AppointmentId, x => x.MapFrom(user => user.AppointmentId));

                cfg.CreateMap<UserDTO, User>()
                .ForMember(x => x.UserId, x => x.MapFrom(dto => dto.Id))
                .ForMember(x => x.UserName, x => x.MapFrom(dto => dto.Name))
                .ForMember(x => x.UserPassword, x => x.MapFrom(dto => dto.Password))
                .ForMember(x => x.UserEmail, x => x.MapFrom(dto => dto.Email))
                .ForMember(x => x.RoleId, x => x.MapFrom(dto => repository.RoleRepository
                                         .GetAll().FirstOrDefault(y => y.RoleName == dto.RoleName).RoleId))
                .ForMember(x => x.AppointmentId, x => x.MapFrom(dto => dto.AppointmentId));
            });

            mapper = new Mapper(configuration);
        }

        public IEnumerable<UserDTO> GetAll()
        {
            repository.Context.Users.Include("Role").ToList();
            return mapper.Map<IEnumerable<UserDTO>>(repository.UserRepository.GetAll());
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            repository.Context.Users.Include("Role").ToList();
            return await Task.Run(() => mapper.Map<IEnumerable<UserDTO>>(repository.UserRepository.GetAll()));
        }

        public UserDTO Get(int id)
        {
            return mapper.Map<UserDTO>(repository.UserRepository.Get(id));
        }

        public async Task<UserDTO> CreateAsync(UserDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var user = mapper.Map<User>(entityDTO);
                await Task.Run(() =>
                {
                    repository.UserRepository.Create(user);
                    repository.UserRepository.Save();

                });

                entityDTO.Id = user.UserId;
                return entityDTO;
            }
            return null;
        }

        public async Task UpdateAsync(UserDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var user = repository.UserRepository.Get(entityDTO.Id);
                user = mapper.Map<User>(entityDTO);
                await Task.Run(() =>
                {
                    repository.UserRepository.AddOrUpdate(user);
                    repository.UserRepository.Save();

                });

            }
        }

        public async Task DeleteAsync(UserDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var user = repository.UserRepository.Get(entityDTO.Id);

                await Task.Run(() =>
                {
                    repository.UserRepository.Delete(user);
                    repository.UserRepository.Save();
                });

            }
        }
    }
}
