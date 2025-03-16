using ArticalProject.Core;
using ArticalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArticalProject.Pages
{
    public class ArticleModel : PageModel
    {
        private readonly IDataHelper<Core.AuthorPost> dataHelper;

        public ArticleModel(IDataHelper<Core.AuthorPost> dataHelper)
        {
            this.dataHelper = dataHelper;
            Post = new Core.AuthorPost();
        }

        public Core.AuthorPost Post { get; set; }


        public void OnGet()
        {
            var res = HttpContext.Request.RouteValues["id"];
            int id = Convert.ToInt32(res);

            Post = dataHelper.Find(id);
        }
    }
}
