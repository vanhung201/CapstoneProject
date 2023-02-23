using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using Microsoft.Ajax.Utilities;
using static BusinessConnectManagement.Areas.Faculty.Controllers.BusinessUsersController;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    [LoginVerification]
    public class BusinessCooperationCategoriesController : Controller
    {
        private BCMEntities db = new BCMEntities();

        public class CooperationCategoriesDetail
        {
            public string name { get; set; }
            public int value { get; set; }
            public bool status { get; set; }

        }

        public class CooperationCategoriesGet
        {
            public string name { get; set; }
            public int id { get; set; }
            public string value { get; set; }
        }

        // GET: Faculty/BusinessCooperationCategories
        public ActionResult Index(BusinessCooperationCategory businessCooperationCategory)
        {
            var businessUser = db.BusinessUsers.ToList();
            int count = 0;
            var CoopListCoop = db.BusinessCooperationCategories.ToList();

            List<CooperationCategoriesGet> cooperationCategoriesget = new List<CooperationCategoriesGet>();
            List<CooperationCategoriesGet> cooperationCategoriesgetTest = new List<CooperationCategoriesGet>();

            foreach (var item in CoopListCoop)
            {
                cooperationCategoriesget.Add(new CooperationCategoriesGet { name = item.Business_ID.ToString(), value = item.CooperationCategory.CooperationCategoriesName });
                count++;
            }
            var listBusinessId = db.BusinessCooperationCategories.Select(x => x.Business_ID).Distinct().ToList();
            //var listBusinessName = db.BusinessCooperationCategories.OrderBy(a => a.Business_ID).Select(x => x.BusinessUser.BusinessName).Distinct().ToList();
            var demo = db.BusinessCooperationCategories.OrderBy(a => a.Business_ID).DistinctBy(x => x.Business_ID).ToList();
            var listBusinessName = demo.Select(x => x.BusinessUser.BusinessName).ToList();
            var listBusinessValue = new string[listBusinessId.Count()];
            int index = 0;
            foreach (var buId in listBusinessId)
            {
                string listData = "";
                foreach (var itemGet in cooperationCategoriesget)
                {
                    if (int.Parse(itemGet.name) == buId)
                    {
                        listData += itemGet.value + ", ";

                    }
                }
                listBusinessValue[index] = listData;
                index++;
            }
            for (int i = 0; i + 1 < listBusinessId.Count + 1; i++)
            {
                cooperationCategoriesgetTest.Add(new CooperationCategoriesGet { id = listBusinessId[i], name = listBusinessName[i].ToString(), value = listBusinessValue[i] });
            }
           
            ViewBag.demo = cooperationCategoriesgetTest;
            ViewBag.Business_ID = db.MOUs.ToList();
            var viewbag = db.CooperationCategories.ToList();
            ViewBag.CooperationCategories = viewbag.ToList();
           
            return View(db.BusinessUsers.ToList());
        }

        // GET: Faculty/BusinessCooperationCategories/Details/5
        public ActionResult Details(int id)
        {
            BusinessUser businessUser = db.BusinessUsers.Find(id);

            var BusinessCoopList = db.BusinessCooperationCategories.Where(x => x.Business_ID == id).ToList();
            var CoopList = db.CooperationCategories.ToList();

            List<CooperationCategoriesDetail> cooperationCategoriesDetails = new List<CooperationCategoriesDetail>();

            foreach (var item in CoopList)
            {
                bool isExist = BusinessCoopList.Where(x => x.CooperationCategories_ID == item.ID).Any();
                cooperationCategoriesDetails.Add(new CooperationCategoriesDetail { name = item.CooperationCategoriesName, value = item.ID, status = isExist });
            }

            ViewBag.CooperationCategoriesDetail = cooperationCategoriesDetails;
            ViewBag.StatusList = db.Status.ToList();

            var viewbag = db.CooperationCategories.ToList();

            ViewBag.CooperationCategories = viewbag.ToList();

            return Json(ViewBag.CooperationCategoriesDetail, JsonRequestBehavior.AllowGet);

        }

        // GET: Faculty/BusinessCooperationCategories/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = db.MOUs.ToList();
            ViewBag.CooperationCategories_ID = new SelectList(db.CooperationCategories, "ID", "CooperationCategoriesName");

            var viewbag = db.CooperationCategories.ToList();

            ViewBag.CooperationCategories = viewbag.ToList();

            return View();
        }

        // POST: Faculty/BusinessCooperationCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(BusinessCooperationCategory BCC, string ArrCoopId, int ddlBussines_ID)
        {
            if (ModelState.IsValid)
            {
                string[] arrayCoop = ArrCoopId.Split(',');
                for (int i = 0; i < arrayCoop.Length - 1; i++)
                {
                    BCC.Business_ID = ddlBussines_ID;
                    BCC.CooperationCategories_ID = Int32.Parse(arrayCoop[i]);

                    db.BusinessCooperationCategories.Add(BCC);
                    db.SaveChanges();
                }

                ViewBag.Business_ID = db.MOUs.ToList();

                var viewbag = db.CooperationCategories.ToList();

                ViewBag.CooperationCategories = viewbag.ToList();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Thêm thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");

            }
            else
            {
                return View(BCC);
            }
        }

        // GET: Faculty/BusinessCooperationCategories/Edit/5
        public ActionResult Edit(int id)
        {
            BusinessUser businessUser = db.BusinessUsers.Find(id);

            var BusinessCoopList = db.BusinessCooperationCategories.Where(x => x.Business_ID == id).ToList();
            var CoopList = db.CooperationCategories.ToList();

            List<CooperationCategoriesDetail> cooperationCategoriesDetails = new List<CooperationCategoriesDetail>();

            foreach (var item in CoopList)
            {
                bool isExist = BusinessCoopList.Where(x => x.CooperationCategories_ID == item.ID).Any();
                cooperationCategoriesDetails.Add(new CooperationCategoriesDetail { name = item.CooperationCategoriesName, value = item.ID, status = isExist });
            }

            ViewBag.CooperationCategoriesDetail = cooperationCategoriesDetails;
            ViewBag.StatusList = db.Status.ToList();

            var viewbag = db.CooperationCategories.ToList();

            ViewBag.CooperationCategories = viewbag.ToList();

            return View(businessUser);
        }

        // POST: Faculty/BusinessCooperationCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ArrCoopId, HttpPostedFileBase logo, BusinessUser businessUser, BusinessCooperationCategory businessCooperationCategory)
        {

            if (ModelState.IsValid)
            {
                // Nếu logo != null => có thay đổi hình ảnh upload ảnh lên và tên

                // Xóa toàn bộ liên kết theo Id của Bussiness
                var businessCooperationCategoryList = db.BusinessCooperationCategories.Where(x => x.Business_ID == businessUser.ID).ToList();

                foreach (var item in businessCooperationCategoryList)
                {
                    db.BusinessCooperationCategories.Remove(item);
                }

                // Add lại từ đầu với arrayCoop mới
                string[] arrayCoop = ArrCoopId.Split(',');
                if(arrayCoop != null)
                {
                    for (int i = 0; i < arrayCoop.Length - 1; i++)
                    {
                        BusinessCooperationCategory businessCooperationCategory1 = new BusinessCooperationCategory();
                        businessCooperationCategory1.Business_ID = businessUser.ID;
                        businessCooperationCategory1.CooperationCategories_ID = Int32.Parse(arrayCoop[i]);

                        db.BusinessCooperationCategories.Add(businessCooperationCategory1);
                    }
                    db.SaveChanges();
                    TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(businessCooperationCategory);
                }
              

            }
            else
            {
                return View(businessCooperationCategory);
            }
        }

        // GET: Faculty/BusinessCooperationCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BusinessCooperationCategory businessCooperationCategory = db.BusinessCooperationCategories.Find(id);

            if (businessCooperationCategory == null)
            {
                return HttpNotFound();
            }

            return View(businessCooperationCategory);
        }

        // POST: Faculty/BusinessCooperationCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, BusinessUser businessUser)
        {
            var businessCooperationCategoryList = db.BusinessCooperationCategories.Where(x => x.Business_ID == businessUser.ID).ToList();

            foreach (var item in businessCooperationCategoryList)
            {
                db.BusinessCooperationCategories.Remove(item);
            }

            db.SaveChanges();

            return RedirectToAction("Index", "BusinessCooperationCategories");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
