using Project.BLL.DTO;
using Project.BLL.Interfaces;
using Project.DAL.Context;
using Project.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class ManufacturerService : IService<ManufacturerDTO>
    {
        private RepositoryStorage repository;
        public ManufacturerService(RepositoryStorage repository)
        {
            this.repository = repository;
        }
        public IEnumerable<ManufacturerDTO> GetAll()
        {
            return repository.ManufacturerRepository
                .GetAll()
                .Select(x => new ManufacturerDTO
                {
                    Id = x.ManufacturerId,
                    Name = x.ManufacturerName
                });
        }
        public async Task<IEnumerable<ManufacturerDTO>> GetAllAsync() => await Task.Run(() => GetAll());
        public ManufacturerDTO Get(int id)
        {
            var manufacturer = repository.ManufacturerRepository.Get(id);
            return new ManufacturerDTO
            {
                Id = manufacturer.ManufacturerId,
                Name = manufacturer.ManufacturerName
            };
        }

        public async Task<ManufacturerDTO> CreateAsync(ManufacturerDTO manufacturerDTO)
        {
            if (manufacturerDTO != null)
            {
                var manufacturer = new Manufacturer { ManufacturerName = manufacturerDTO.Name };
                await Task.Run(() =>
                {
                    repository.ManufacturerRepository.Create(manufacturer);
                    repository.ManufacturerRepository.Save();
                });
                manufacturerDTO.Id = manufacturer.ManufacturerId;
                return manufacturerDTO;
            }
            return null;

        }
        public async Task UpdateAsync(ManufacturerDTO manufacturerDTO)
        {
            if (manufacturerDTO != null)
            {
                var manufacturer = repository.ManufacturerRepository.Get(manufacturerDTO.Id);
                manufacturer.ManufacturerName = manufacturerDTO.Name;

                await Task.Run(() =>
                {

                    repository.ManufacturerRepository.AddOrUpdate(manufacturer);
                    repository.ManufacturerRepository.Save();
                });
            }
        }
        public async Task DeleteAsync(ManufacturerDTO manufacturerDTO)
        {
            if (manufacturerDTO != null)
            {
                var manufacturer = repository.ManufacturerRepository.Get(manufacturerDTO.Id);

                await Task.Run(() =>
                {
                    repository.ManufacturerRepository.Delete(manufacturer);
                    repository.ManufacturerRepository.Save();
                });
            }
        }
    }
}
