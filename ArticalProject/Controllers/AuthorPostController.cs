using ArticalProject.Code;
using ArticalProject.Core;
using ArticalProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ArticalProject.CoreView;

namespace ArticalProject.Controllers
{
    [Authorize]
    public class AuthorPostController : Controller
    {
        private readonly IDataHelper<AuthorPost> _dataHelper;
        private readonly IDataHelper<Author> dataHelperForAuthor;
        private readonly IDataHelper<Category> dataHelperForCategory;
        private readonly IWebHostEnvironment webHost;
        private readonly FilesHelper filesHelper;
        private readonly IAuthorizationService authorizationService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private int pageItem;
        private Task<AuthorizationResult> result;
        private string UserId;

        public AuthorPostController(
            IDataHelper<AuthorPost> dataHelper,
            IDataHelper<Author> dataHelperForAuthor,
            IDataHelper<Category> dataHelperForCategory,
            IWebHostEnvironment webHost,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _dataHelper = dataHelper;
            this.dataHelperForAuthor = dataHelperForAuthor;
            this.dataHelperForCategory = dataHelperForCategory;
            this.webHost = webHost;
            filesHelper = new FilesHelper(this.webHost);
            this.authorizationService = authorizationService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            pageItem = 10;
        }

        // GET: AuthorPostController
        public ActionResult Index(int? id)
        {
            SetUser();
            // Admin
            if (result.Result.Succeeded)
            {

                if (id == 0 || id == null)
                {
                    return View(this._dataHelper.GetAllData().Take(pageItem));
                }
                else
                {
                    var data = _dataHelper.GetAllData().Where(x => x.Id > id).Take(pageItem);
                    return View(data);
                }
            } // User
            else
            {
                if (id == 0 || id == null)
                {
                    return View(this._dataHelper.GetDataByUserID(UserId).Take(pageItem));
                }
                else
                {
                    var data = _dataHelper.GetDataByUserID(UserId).Where(x => x.Id > id).Take(pageItem);
                    return View(data);
                }
            }
        }

        // GET: AuthorController
        public ActionResult Search(string SearchItem)
        {
            SetUser();
            if (result.Result.Succeeded)
            {

                // Admin
                if (SearchItem == null)
                {
                    return View("Index", _dataHelper.GetAllData());
                }
                else
                {
                    return View("Index", _dataHelper.Search(SearchItem));
                }
            }
            else{

                // User
                if (SearchItem == null)
                {
                    return View("Index", _dataHelper.GetDataByUserID(UserId));
                }
                else
                {
                    return View("Index", _dataHelper.Search(SearchItem).Where(x=>x.UserId == UserId));
                }
            }
        }

        // GET: AuthorPostController/Details/5
        public ActionResult Details(int id)
        {
            SetUser();
            return View(_dataHelper.Find(id));
        }

        // GET: AuthorPostController/Create
        public ActionResult Create()
        {
            SetUser();
            return View();
        }

        // POST: AuthorPostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AuthorPostView collection)
        {
            try
            {
                SetUser();

                // Fetching author details
                var authorData = dataHelperForAuthor.GetAllData().Where(x => x.UserId == UserId);
                var authorUserName = authorData.Select(x => x.UserName).FirstOrDefault();
                var authorFullName = authorData.Select(x => x.FullName).FirstOrDefault();
                var authorId = authorData.Select(x => x.Id).FirstOrDefault();

                // Handle case when no author is found
                if (authorUserName == null || authorFullName == null || authorId == 0)
                {
                    ModelState.AddModelError("", "Author information is missing.");
                    return View(collection);
                }

                // Fetching category details
                var categoryData = dataHelperForCategory.GetAllData().Where(x => x.Name == collection.PostCategory);
                var categoryId = categoryData.Select(x => x.Id).FirstOrDefault();

                // Handle case when no category is found
                if (categoryId == 0)
                {
                    ModelState.AddModelError("", "Category is invalid.");
                    return View(collection);
                }

                // Creating the post
                AuthorPost Post = new AuthorPost
                {
                    UserId = UserId,
                    UserName = authorUserName,
                    FullName = authorFullName,
                    PostCategory = collection.PostCategory,
                    PostTitle = collection.PostTitle,
                    PostDecription = collection.PostDecription,
                    PostImageURL = filesHelper.UploadFile(collection.PostImageURL, "Images"),
                    AddedDate = DateTime.Now,
                    AuthorId = authorId,
                    Author = collection.Author,
                    CategoryId = categoryId,
                    Category = collection.Category,
                };

                _dataHelper.Add(Post);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // You can log the exception or display the error in the UI
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(collection);
            }
        }

        // GET: AuthorPostController/Edit/5
        public ActionResult Edit(int id)
        {
            SetUser();

            var authorPost = _dataHelper.Find(id);

            var authorPostView = new AuthorPostView
            {
                Id = authorPost.Id,
                UserId = UserId,
                UserName = authorPost.UserName,
                FullName = authorPost.FullName,
                PostCategory = authorPost.PostCategory,
                PostTitle = authorPost.PostTitle,
                PostDecription = authorPost.PostDecription,
                AddedDate = authorPost.AddedDate,
                AuthorId = authorPost.AuthorId,
                Author = authorPost.Author,
                CategoryId = authorPost.CategoryId,
                Category = authorPost.Category,
            };

            return View(authorPostView);
        }

        // POST: AuthorPostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AuthorPostView collection)
        {
            try
            {
                SetUser();

                // Fetching author details
                var authorData = dataHelperForAuthor.GetAllData().Where(x => x.UserId == UserId);
                var authorUserName = authorData.Select(x => x.UserName).FirstOrDefault();
                var authorFullName = authorData.Select(x => x.FullName).FirstOrDefault();
                var authorId = authorData.Select(x => x.Id).FirstOrDefault();

                // Handle case when no author is found
                if (authorUserName == null || authorFullName == null || authorId == 0)
                {
                    ModelState.AddModelError("", "Author information is missing.");
                    return View(collection);
                }

                // Fetching category details
                var categoryData = dataHelperForCategory.GetAllData().Where(x => x.Name == collection.PostCategory);
                var categoryId = categoryData.Select(x => x.Id).FirstOrDefault();

                // Handle case when no category is found
                if (categoryId == 0)
                {
                    ModelState.AddModelError("", "Category is invalid.");
                    return View(collection);
                }

                // Creating the post
                AuthorPost Post = new AuthorPost
                {
                    Id = collection.Id,
                    UserId = UserId,
                    UserName = authorUserName,
                    FullName = authorFullName,
                    PostCategory = collection.PostCategory,
                    PostTitle = collection.PostTitle,
                    PostDecription = collection.PostDecription,
                    PostImageURL = filesHelper.UploadFile(collection.PostImageURL, "Images"),
                    AddedDate = DateTime.Now,
                    AuthorId = authorId,
                    Author = collection.Author,
                    CategoryId = categoryId,
                    Category = collection.Category,
                };

                _dataHelper.Edit(id,Post);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // You can log the exception or display the error in the UI
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(collection);
            }
        }

        // GET: AuthorPostController/Delete/5
        public ActionResult Delete(int id)
        {
            SetUser();

            var authorPost = _dataHelper.Find(id);

            var authorPostView = new AuthorPostView
            {
                Id = authorPost.Id,
                UserId = UserId,
                UserName = authorPost.UserName,
                FullName = authorPost.FullName,
                PostCategory = authorPost.PostCategory,
                PostTitle = authorPost.PostTitle,
                PostDecription = authorPost.PostDecription,
                AddedDate = authorPost.AddedDate,
                AuthorId = authorPost.AuthorId,
                Author = authorPost.Author,
                CategoryId = authorPost.CategoryId,
                Category = authorPost.Category,
            };

            return View(authorPost);
        }

        // POST: AuthorPostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AuthorPost collection)
        {
            try
            {
                _dataHelper.Delete(id);

                var filePath = "~/Images/" + collection.PostImageURL;
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

        private void SetUser()
        {
            result = authorizationService.AuthorizeAsync(User, "Admin");
            UserId = User.FindFirst(type: ClaimTypes.NameIdentifier).Value;
        }
    }
}
