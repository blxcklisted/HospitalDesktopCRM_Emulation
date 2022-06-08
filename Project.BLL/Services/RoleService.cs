using AutoMapper;
using Project.BLL.DTO;
using Project.BLL.Interfaces;
using Project.DAL.Context;
using Project.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class RoleService : IService<RoleDTO>
    {
        private RepositoryStorage repository;

        private IMapper mapper;
        public RoleService(RepositoryStorage repository)
        {
            this.repository = repository;

            var configurationRoleDTO = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Role, RoleDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(role => role.RoleId))
                .ForMember(x => x.Name, x => x.MapFrom(role => role.RoleName))
                .ReverseMap();
            });
            mapper = new Mapper(configurationRoleDTO);
        }
        public IEnumerable<RoleDTO> GetAll()
        {
            return mapper.Map<IEnumerable<RoleDTO>>(repository.RoleRepository.GetAll());
        }

        public async Task<IEnumerable<RoleDTO>> GetAllAsync()
        {
            return await Task.Run(()=> mapper.Map<IEnumerable<RoleDTO>>(repository.RoleRepository.GetAllAsync()));
        }

        public RoleDTO Get(int id)
        {
            return mapper.Map<RoleDTO>(repository.RoleRepository.Get(id));
        }

        public async Task<RoleDTO> CreateAsync(RoleDTO entityDTO)
        {
            if(entityDTO != null )
            {
                var role = mapper.Map<Role>(entityDTO);

                await Task.Run(() =>
                {
                    repository.RoleRepository.Create(role);
                    repository.RoleRepository.Save();
                });
                entityDTO.Id = role.RoleId;
                return entityDTO;
            }
            return null;
        }

        public async Task DeleteAsync(RoleDTO entityDTO)
        {
            if(entityDTO != null)
            {
                var role = repository.RoleRepository.Get(entityDTO.Id);

                await Task.Run(() =>
                {
                    repository.RoleRepository.Delete(role);
                    repository.RoleRepository.Save();
                });
            }
        }


        public async Task UpdateAsync(RoleDTO entityDTO)
        {
            if(entityDTO != null )
            {
                var role = repository.RoleRepository.Get(entityDTO.Id);
                role = mapper.Map<Role>(entityDTO);

                await Task.Run(() =>
                {
                    repository.RoleRepository.AddOrUpdate(role);
                    repository.RoleRepository.Save();
                });
            }
        }
    }
}
