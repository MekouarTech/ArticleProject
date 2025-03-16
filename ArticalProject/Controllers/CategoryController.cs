using ArticalProject.Core;
using ArticalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArticalProject.Controllers
{
    [Authorize("Admin")]
    public class CategoryController : Controller
    {
        private readonly IDataHelper<Category> dataHelper;
        private int pageItem;

        public CategoryController(IDataHelper<Category> dataHelper)
        {
            this.dataHelper = dataHelper;
            this.pageItem = 10;
        }

        // GET: CategoryController
        public ActionResult Index(int? id)
        {
            if (id == 0 || id == null)
            {
                return View(this.dataHelper.GetAllData().Take(pageItem));
            }
            else
            {
                var data = dataHelper.GetAllData().Where(x => x.Id > id).Take(pageItem);
                return View(data);
            }
        }

        // GET: CategoryController
        public ActionResult Search(string SearchItem)
        {
            if(SearchItem == null)
            {
                return View("Index", dataHelper.GetAllData());
            }
            else
            {
                return View("Index",dataHelper.Search(SearchItem));
            }
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category collection)
        {
            try
            {
                int result = dataHelper.Add(collection);
                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(dataHelper.Find(id));
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category collection)
        {
            try
            {
                int result = dataHelper.Edit(id,collection);
                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(dataHelper.Find(id));
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category collection)
        {
            try
            {
                int result = dataHelper.Delete(id);
                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
