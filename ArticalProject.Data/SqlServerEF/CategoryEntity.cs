using ArticalProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticalProject.Data.SqlServerEF
{
    public class CategoryEntity : IDataHelper<Category>
    {
        private readonly DBContext _dbContext;

        // Constructor with dependency injection for DBContext
        public CategoryEntity(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Category category)
        {
            if (_dbContext.Database.CanConnect())
            {
                _dbContext.Category.Add(category);
                _dbContext.SaveChanges();
                return 1;
            }
            return 0; // Return 0 if database connection fails
        }

        public int Delete(int id)
        {
            if (_dbContext.Database.CanConnect())
            {
                var category = Find(id);
                if (category != null)
                {
                    _dbContext.Category.Remove(category);
                    _dbContext.SaveChanges();
                    return 1;
                }
                return 0; // Category not found
            }
            return 0; // Database connection failed
        }

        public int Edit(int id, Category category)
        {
            if (_dbContext.Database.CanConnect())
            {
                var existingCategory = _dbContext.Category.SingleOrDefault(c => c.Id == id);
                if (existingCategory != null)
                {
                    existingCategory.Name = category.Name;
                    _dbContext.SaveChanges();
                    return 1;
                }
                return 0; // Category not found
            }
            return 0; // Database connection failed
        }

        public Category Find(int id)
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.Category.SingleOrDefault(x => x.Id == id);
            }
            return null; // Database connection failed or not found
        }

        public List<Category> GetAllData()
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.Category.ToList();
            }
            return null; // Database connection failed
        }

        public List<Category> GetDataByUserID(string userId)
        {
            throw new NotImplementedException();
        }

        public List<Category> Search(string searchItem)
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.Category.Where(x =>
                    x.Name.Contains(searchItem) ||
                    x.Id.ToString().Contains(searchItem)
                ).ToList();
            }
            return null; // Database connection failed
        }
    }

}
