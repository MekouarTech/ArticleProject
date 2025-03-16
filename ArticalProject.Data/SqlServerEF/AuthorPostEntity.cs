using ArticalProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticalProject.Data.SqlServerEF
{
    public class AuthorPostEntity : IDataHelper<AuthorPost>
    {
        private readonly DBContext _dbContext;

        // Constructor with dependency injection for DBContext
        public AuthorPostEntity(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(AuthorPost authorPost)
        {
            if (_dbContext.Database.CanConnect())
            {
                _dbContext.AuthorPost.Add(authorPost);
                _dbContext.SaveChanges();
                return 1;
            }
            return 0; // Return 0 if database connection fails
        }

        public int Delete(int id)
        {
            if (_dbContext.Database.CanConnect())
            {
                var authorPost = Find(id);
                if (authorPost != null)
                {
                    _dbContext.AuthorPost.Remove(authorPost);
                    _dbContext.SaveChanges();
                    return 1;
                }
                return 0; // AuthorPost not found
            }
            return 0; // Database connection failed
        }

        public int Edit(int id, AuthorPost authorPost)
        {
            if (_dbContext.Database.CanConnect())
            {
                var existingAuthorPost = _dbContext.AuthorPost.SingleOrDefault(a => a.Id == id);
                if (existingAuthorPost != null)
                {
                    // Update the properties of the existing entity
                    existingAuthorPost.FullName = authorPost.FullName;
                    existingAuthorPost.UserId = authorPost.UserId;
                    existingAuthorPost.UserName = authorPost.UserName;
                    existingAuthorPost.PostTitle = authorPost.PostTitle;
                    existingAuthorPost.PostCategory = authorPost.PostCategory;
                    existingAuthorPost.PostDecription = authorPost.PostDecription;
                    existingAuthorPost.PostImageURL = authorPost.PostImageURL;
                    existingAuthorPost.AuthorId = authorPost.AuthorId;
                    existingAuthorPost.CategoryId = authorPost.CategoryId;
                    existingAuthorPost.AddedDate = authorPost.AddedDate;

                    _dbContext.SaveChanges();
                    return 1;
                }
                return 0; // AuthorPost not found
            }
            return 0; // Database connection failed
        }

        public AuthorPost Find(int id)
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.AuthorPost.SingleOrDefault(x => x.Id == id);
            }
            return null; // Database connection failed or not found
        }

        public List<AuthorPost> GetAllData()
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.AuthorPost.ToList();
            }
            return null; // Database connection failed
        }

        public List<AuthorPost> GetDataByUserID(string userId)
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.AuthorPost.Where(x => x.UserId == userId).ToList();
            }
            return null; // Database connection failed
        }

        public List<AuthorPost> Search(string searchItem)
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.AuthorPost.Where(x =>
                    x.FullName.Contains(searchItem) ||
                    x.UserId.ToString().Contains(searchItem) ||
                    x.UserName.Contains(searchItem) ||
                    x.PostTitle.Contains(searchItem) ||
                    x.PostCategory.Contains(searchItem) ||
                    x.PostDecription.Contains(searchItem) ||
                    x.PostImageURL.Contains(searchItem) ||
                    x.AuthorId.ToString().Contains(searchItem) ||
                    x.Author.FullName.Contains(searchItem) ||
                    x.CategoryId.ToString().Contains(searchItem) ||
                    x.Category.Name.Contains(searchItem) ||
                    x.AddedDate.ToString().Contains(searchItem) ||
                    x.Id.ToString().Contains(searchItem)
                ).ToList();
            }
            return null; // Database connection failed
        }
    }
}
