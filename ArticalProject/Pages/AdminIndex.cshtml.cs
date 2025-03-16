using ArticalProject.Core;
using ArticalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Security.Claims;

namespace ArticalProject.Pages
{
    [Authorize]
    public class AdminIndexModel : PageModel
    {
        private readonly IDataHelper<Core.AuthorPost> dataHelper;

        public AdminIndexModel(IDataHelper<Core.AuthorPost> dataHelper)
        {
            this.dataHelper = dataHelper;
        }
        public int AllPosts { get; set; }
        public int PostsLastMonth { get; set; }
        public int PostsThisYear { get; set; }

        public void OnGet()
        {
            var dateM = DateTime.Now.AddMonths(-1);
            var dateY = DateTime.Now.AddYears(-1);

            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            AllPosts = dataHelper.GetDataByUserID(userid).Count;
            PostsLastMonth = dataHelper.GetDataByUserID(userid).Where(x => x.AddedDate >= dateM).ToList().Count;
            PostsThisYear = dataHelper.GetDataByUserID(userid).Where(x => x.AddedDate >= dateY).ToList().Count;
        }
    }
}
