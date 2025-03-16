using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArticalProject.Core;
using ArticalProject.Data;

namespace ArticalProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDataHelper<Core.Category> dataHelperForCategory;
        private readonly IDataHelper<Core.AuthorPost> dataHelperForPosts;
        public readonly int NoOfItems;

        public IndexModel(
            ILogger<IndexModel> logger,
            IDataHelper<Core.Category> dataHelperForCategory,
            IDataHelper<Core.AuthorPost> dataHelperForPosts
            )
        {
            _logger = logger;
            this.dataHelperForCategory = dataHelperForCategory;
            this.dataHelperForPosts = dataHelperForPosts;
            NoOfItems = 9;
            ListOfCaegories = new List<Core.Category>();
            ListOfPosts = new List<Core.AuthorPost>();
        }

        public List<Core.Category> ListOfCaegories { get; set; }
        public List<Core.AuthorPost> ListOfPosts { get; set; }

        public void OnGet(string LoadState, string CategoryName, string SearchName, int id)
        {
            GetAllCategories();
            if (LoadState == null || LoadState == "All")
            {
                GetAllPosts();
            }
            else if (LoadState == "ByCategory")
            {
                GetPostsByCategory(CategoryName);
            }
            else if (LoadState == "BySearch")
            {
                GetPostsBySearch(SearchName);
            }
            else if (LoadState == "Next")
            {
                GetNextData(id);
            }
            else if (LoadState == "Previous")
            {
                GetNextData(id - NoOfItems);
            }
        }

        private void GetAllCategories() => ListOfCaegories = dataHelperForCategory.GetAllData().Take(6).ToList();

        private void GetAllPosts() => ListOfPosts = dataHelperForPosts.GetAllData().Take(NoOfItems).ToList();

        private void GetPostsByCategory(string CategoryName) => ListOfPosts = dataHelperForPosts.GetAllData().Where(x => x.PostCategory == CategoryName).Take(NoOfItems).ToList();
        private void GetPostsBySearch(string SearchName) => ListOfPosts = dataHelperForPosts.Search(SearchName).Take(NoOfItems).ToList();
        private void GetNextData(int id) => ListOfPosts = dataHelperForPosts.GetAllData().Where(x => x.Id > id).Take(NoOfItems).ToList();
    }
}