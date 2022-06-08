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
    public class AppointmentService : IService<AppointmentDTO>
    {
        private RepositoryStorage repository;
        private IMapper mapper;
        public AppointmentService(RepositoryStorage repository)
        {
            this.repository = repository;

            var configuration = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Appointment, AppointmentDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(app => app.AppointmentId))
                .ForMember(x => x.UserName, x => x.MapFrom(app => app.User.UserName))
                .ForMember(x => x.DateTime, x => x.MapFrom(app => app.DateTime))
                .ForMember(x => x.IsAppointed, x => x.MapFrom(app => app.IsAppointed));

                cfg.CreateMap<AppointmentDTO, Appointment>()
                .ForMember(x => x.AppointmentId, x => x.MapFrom(dto => dto.Id))
                .ForMember(x => x.UserId, x => x.MapFrom(dto => repository.UserRepository
                                                .GetAll().FirstOrDefault(y => y.UserName == dto.UserName).UserId))
                .ForMember(x => x.DateTime, x => x.MapFrom(dto => dto.DateTime))
                .ForMember(x => x.IsAppointed, x => x.MapFrom(dto => dto.IsAppointed));
            });
            mapper = new Mapper(configuration);
        }

        public IEnumerable<AppointmentDTO> GetAll()
        {
            repository.Context.Appointments.Include("User").ToList();
            return mapper.Map<IEnumerable<AppointmentDTO>>(repository.AppointmentRepository.GetAll());
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAllAsync()
        {
            return await Task.Run(() => mapper.Map<IEnumerable<AppointmentDTO>>(repository.AppointmentRepository.GetAllAsync()));
        }

        public AppointmentDTO Get(int id)
        {
            return mapper.Map<AppointmentDTO>(repository.AppointmentRepository.Get(id));
        }

        public async Task<AppointmentDTO> CreateAsync(AppointmentDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var app = mapper.Map<Appointment>(entityDTO);
                await Task.Run(() =>
                {
                    repository.AppointmentRepository.Create(app);
                    repository.AppointmentRepository.Save();

                });
                entityDTO.Id = app.AppointmentId;
                return entityDTO;
            }
            return null;
        }

        public async Task UpdateAsync(AppointmentDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var app = repository.AppointmentRepository.Get(entityDTO.Id);
                app = mapper.Map<Appointment>(entityDTO);
                await Task.Run(() =>
                {
                    repository.AppointmentRepository.AddOrUpdate(app);
                    repository.AppointmentRepository.Save();

                });
            }
        }
        public async Task DeleteAsync(AppointmentDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var app = repository.AppointmentRepository.Get(entityDTO.Id);

                await Task.Run(() =>
                {
                    repository.AppointmentRepository.Delete(app);
                    repository.AppointmentRepository.Save();

                });

            }
        }



    }
}
