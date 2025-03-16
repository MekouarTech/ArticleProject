using ArticalProject.Code;
using ArticalProject.Core;
using ArticalProject.CoreView;
using ArticalProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace ArticalProject.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IDataHelper<Author> dataHelper;
        private readonly IAuthorizationService authorizationService;
        private readonly IWebHostEnvironment webHost;
        private readonly FilesHelper filesHelper;
        private readonly int pageItem;

        public AuthorController(IDataHelper<Author> dataHelper,IAuthorizationService authorizationService, IWebHostEnvironment webHost)
        {
            this.dataHelper = dataHelper;
            this.authorizationService = authorizationService;
            this.webHost = webHost;
            filesHelper = new FilesHelper(this.webHost);
            this.pageItem = 10;
        }

        // GET: AuthorController
        [Authorize("Admin")]
        public ActionResult Index(int? id)
        {
            if (id == 0 || id == null)
            {
                return View(dataHelper.GetAllData().Take(pageItem));
            }
            else
            {
                var data = dataHelper.GetAllData().Where(x => x.Id > id).Take(pageItem);
                return View(data);
            }
        }

        // GET: AuthorController
        [Authorize("Admin")]
        public ActionResult Search(string SearchItem)
        {
            if (SearchItem == null)
            {
                return View("Index", dataHelper.GetAllData());
            }
            else
            {
                return View("Index", dataHelper.Search(SearchItem));
            }
        }

        // GET: AuthorController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var author = dataHelper.Find(id);
            var authorView = new AuthorView
            {
                Id = author.Id,
                UserId = author.UserId,
                UserName = author.UserName,
                FullName = author.FullName,
                Bio = author.Bio,
                Facebook = author.Facebook,
                Instagram = author.Instagram,
                Twitter = author.Twitter,
            };

            return View(authorView);
        }

        // POST: AuthorController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AuthorView collection)
        {
            try
            {
                var author = new Author
                {
                    Id = collection.Id,
                    UserId = collection.UserId,
                    UserName = collection.UserName,
                    FullName = collection.FullName,
                    Bio = collection.Bio,
                    Facebook = collection.Facebook,
                    Instagram = collection.Instagram,
                    Twitter = collection.Twitter,
                    ProfileImageURL = filesHelper.UploadFile(collection.ProfileImageURL, "Images")
                };
                dataHelper.Edit(id, author);

                var result = authorizationService.AuthorizeAsync(User, "Admin");
                if (result.Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect("/AdminIndex");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Delete/5
        [Authorize("Admin")]
        public ActionResult Delete(int id)
        {
            return View(dataHelper.Find(id));
        }

        // POST: AuthorController/Delete/5
        [Authorize("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Author collection)
        {
            try
            {
                dataHelper.Delete(id);

                var filePath = "~/Images/" + collection.ProfileImageURL;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
