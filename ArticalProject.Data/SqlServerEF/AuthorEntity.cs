using ArticalProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticalProject.Data.SqlServerEF
{
    public class AuthorEntity : IDataHelper<Author>
    {
        private readonly DBContext _dbContext;

        // Constructor with dependency injection for DBContext
        public AuthorEntity(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Author author)
        {
            if (_dbContext.Database.CanConnect())
            {
                _dbContext.Author.Add(author);
                _dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }

        public int Delete(int id)
        {
            if (_dbContext.Database.CanConnect())
            {
                var author = _dbContext.Author.SingleOrDefault(a => a.Id == id);
                if (author != null)
                {
                    _dbContext.Author.Remove(author);
                    _dbContext.SaveChanges();
                    return 1;
                }
                return 0; // Author not found
            }
            return 0; // Database connection failed
        }

        public int Edit(int id, Author author)
        {
            if (_dbContext.Database.CanConnect())
            {
                var existingAuthor = _dbContext.Author.SingleOrDefault(a => a.Id == id);
                if (existingAuthor != null)
                {
                    // Update author properties
                    existingAuthor.FullName = author.FullName;
                    existingAuthor.UserId = author.UserId;
                    existingAuthor.UserName = author.UserName;
                    existingAuthor.Bio = author.Bio;
                    existingAuthor.Facebook = author.Facebook;
                    existingAuthor.Instagram = author.Instagram;
                    existingAuthor.Twitter = author.Twitter;

                    _dbContext.SaveChanges();
                    return 1;
                }
                return 0; // Author not found
            }
            return 0; // Database connection failed
        }

        public Author Find(int id)
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.Author.SingleOrDefault(a => a.Id == id);
            }
            return null; // Database connection failed
        }

        public List<Author> GetAllData()
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.Author.ToList();
            }
            return null; // Database connection failed
        }

        public List<Author> GetDataByUserID(string userId)
        {
            return _dbContext.Author.Where(a => a.UserId == userId).ToList();
        }

        public List<Author> Search(string searchItem)
        {
            if (_dbContext.Database.CanConnect())
            {
                return _dbContext.Author.Where(a => a.FullName.Contains(searchItem) ||
                                                     a.UserName.Contains(searchItem) ||
                                                     a.Bio.Contains(searchItem) ||
                                                     a.Facebook.Contains(searchItem) ||
                                                     a.Instagram.Contains(searchItem) ||
                                                     a.Twitter.Contains(searchItem))
                                        .ToList();
            }
            return null; // Database connection failed
        }
    }
}

