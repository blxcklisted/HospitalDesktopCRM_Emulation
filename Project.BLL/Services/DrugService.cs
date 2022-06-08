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
    public class DrugService : IService<DrugDTO>
    {
        private RepositoryStorage repository;

        private IMapper mapper;
        public DrugService(RepositoryStorage repository)
        {
            this.repository = repository;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Drug, DrugDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(drug => drug.DrugId))
                .ForMember(x => x.Name, x => x.MapFrom(drug => drug.DrugName))
                .ForMember(x => x.ManufacturerName, x => x.MapFrom(drug => drug.Manufacturer.ManufacturerName))
                .ForMember(x => x.CategoryName, x => x.MapFrom(drug => drug.Category.CategoryName))
                .ForMember(x => x.DrugCount, x => x.MapFrom(drug => drug.DrugCount))
                .ForMember(x => x.DrugPrice, x => x.MapFrom(drug => drug.DrugPrice))
                .ForMember(x => x.Image, x => x.MapFrom(drug => drug.Image));

                cfg.CreateMap<DrugDTO, Drug>()
                .ForMember(x => x.DrugId, x => x.MapFrom(dto => dto.Id))
                .ForMember(x => x.DrugName, x => x.MapFrom(dto => dto.Name))
                .ForMember(x => x.ManufacturerId, x => x.MapFrom(dto => repository.DrugRepository.GetAll()
                                                          .FirstOrDefault(y => y.Manufacturer.ManufacturerName == dto.ManufacturerName)
                                                          .ManufacturerId))
                .ForMember(x => x.CategoryId, x => x.MapFrom(dto => repository.DrugRepository.GetAll()
                                                           .FirstOrDefault(y => y.Category.CategoryName == dto.CategoryName).CategoryId))
                .ForMember(x => x.DrugCount, x => x.MapFrom(dto => dto.DrugCount))
                .ForMember(x => x.DrugPrice, x => x.MapFrom(dto => dto.DrugPrice))
                .ForMember(x => x.Image, x => x.MapFrom(dto => dto.Image));
            });
            mapper = new Mapper(config);

        }
        public IEnumerable<DrugDTO> GetAll()
        {
            repository.Context.Drugs.Include("Manufacturer").Include("Category").ToList();
            return mapper.Map<IEnumerable<DrugDTO>>(repository.DrugRepository.GetAll());
        }

        public async Task<IEnumerable<DrugDTO>> GetAllAsync()
        {
            repository.Context.Drugs.Include("Manufacturer").Include("Category").ToList();
            return await Task.Run(() => mapper.Map<IEnumerable<DrugDTO>>(repository.DrugRepository.GetAllAsync()));
        }
        public DrugDTO Get(int id)
        {
            return mapper.Map<DrugDTO>(repository.DrugRepository.Get(id));
        }

        public async Task<DrugDTO> CreateAsync(DrugDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var drug = mapper.Map<Drug>(entityDTO);

                await Task.Run(() =>
                {
                    repository.DrugRepository.Create(drug);
                    repository.DrugRepository.Save();
                });
                entityDTO.Id = drug.DrugId;
                return entityDTO;
            }
            return null;
        }

        public async Task UpdateAsync(DrugDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var drug = repository.DrugRepository.Get(entityDTO.Id);
                drug = mapper.Map<Drug>(entityDTO);
                await Task.Run(() =>
                {
                    repository.DrugRepository.AddOrUpdate(drug);
                    repository.DrugRepository.Save();
                });
            }
        }

        public async Task DeleteAsync(DrugDTO entityDTO)
        {
            if (entityDTO != null)
            {
                var drug = repository.DrugRepository.Get(entityDTO.Id);

                await Task.Run(() =>
                {
                    repository.DrugRepository.Delete(drug);
                    repository.DrugRepository.Save();
                });
            }
        }
    }
}
