using ArticalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArticalProject.Pages
{
    public class AllUsersModel : PageModel
    {
        private readonly IDataHelper<Core.Author> dataHelper;
        public readonly int NoOfItems;

        public AllUsersModel(
            IDataHelper<Core.Author> dataHelper
            )
        {
            this.dataHelper = dataHelper;
            NoOfItems = 9;

            ListOfAuthor = new List<Core.Author>();
        }

        public List<Core.Author> ListOfAuthor { get; set; }

        public void OnGet(string LoadState, string SearchName, int id)
        {
            if (LoadState == null || LoadState == "All")
            {
                GetAllAuthors();
            }
            else if (LoadState == "BySearch")
            {
                GetAuthorsBySearch(SearchName);
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

        private void GetAllAuthors() => ListOfAuthor = dataHelper.GetAllData().Take(NoOfItems).ToList();

        private void GetAuthorsBySearch(string SearchName) => ListOfAuthor = dataHelper.Search(SearchName).Take(NoOfItems).ToList();
        private void GetNextData(int id) => ListOfAuthor = dataHelper.GetAllData().Where(x => x.Id > id).Take(NoOfItems).ToList();
    }
}
