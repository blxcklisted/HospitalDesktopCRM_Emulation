using Project.BLL.DTO;
using Project.BLL.Interfaces;
using Project.DAL.Context;
using Project.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class CategoryService : IService<CategoryDTO>
    {
        private RepositoryStorage repository;
        public CategoryService(RepositoryStorage repository)
        {
            this.repository = repository;
        }
        public IEnumerable<CategoryDTO> GetAll()
        {
            return repository.CategoryRepository
                .GetAll()
                .Select(x => new CategoryDTO
                {
                    Id = x.CategoryId,
                    Name = x.CategoryName
                });
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllAsync() => await Task.Run(() => GetAll());

        public CategoryDTO Get(int id)
        {
            var category = repository.CategoryRepository.Get(id);
            return new CategoryDTO
            {
                Id = category.CategoryId,
                Name = category.CategoryName
            };
        }
        public async Task<CategoryDTO> GetAsync(int id) => await Task.Run(() => Get(id));

        public async Task<CategoryDTO> CreateAsync(CategoryDTO categoryDTO)
        {
            if (categoryDTO != null)
            {

                var category = new Category { CategoryName = categoryDTO.Name };

                await Task.Run(() =>
                {
                    repository.CategoryRepository.Create(category);
                    repository.CategoryRepository.Save();
                });
                categoryDTO.Id = category.CategoryId;
                return categoryDTO;
            }
            return null;

        }
        public async Task UpdateAsync(CategoryDTO categoryDTO)
        {
            if (categoryDTO != null)
            {
                var category = repository.CategoryRepository.Get(categoryDTO.Id);
                category.CategoryName = categoryDTO.Name;

                await Task.Run(() =>
                {
                    repository.CategoryRepository.AddOrUpdate(category);
                    repository.CategoryRepository.Save();
                });
            }
        }
        public async Task DeleteAsync(CategoryDTO categoryDTO)
        {
            if (categoryDTO != null)
            {
                var category = repository.CategoryRepository.Get(categoryDTO.Id);

                await Task.Run(() =>
                {
                    repository.CategoryRepository.Delete(category);
                    repository.CategoryRepository.Save();
                });
            }
        }
    }
}
