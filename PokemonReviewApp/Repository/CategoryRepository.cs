using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly DataContext _dataContext;
        public CategoryRepository(DataContext dataContext) 
        {
            this._dataContext=  dataContext;
        }
        public bool CategoriesExists(int categoryId)
        {
            return _dataContext.Categories.Any(c => c.Id==categoryId);
        }

        public bool CreateCategory(Category category)
        {
            //ChangeTracker
            _dataContext.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _dataContext.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _dataContext.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _dataContext.Categories.Where(e => e.Id==id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _dataContext.PokemonCategories.Where(e => e.CategoryId==categoryId).Select(c => c.Pokemon).ToList();
        }

        public bool Save()
        {
            var savechange = _dataContext.SaveChanges();
            return savechange>0?true:false;
        }

        public bool UpdateCategory(Category category)
        {
            _dataContext.Update(category);
            return Save();
        }
    }
}
