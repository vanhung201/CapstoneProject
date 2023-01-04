using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    public class BusinessUsersController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/BusinessUsers
        public ActionResult Index()
        {
            
            return View(db.BusinessUsers.ToList());
        }

        public class CooperationCategoriesDetail
        {
            public string name { get; set; }
            public string namee { get; set; }
            public int value { get; set; }
            public bool status { get; set; }
        }

        // GET: Faculty/BusinessUsers/Details/5
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

            return View(businessUser);
        }

        public ActionResult DetailsBusiness(int id)
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

            return View(businessUser);
        }

        // GET: Faculty/BusinessUsers/Create
        public ActionResult Create()
        {
            var viewbag = db.CooperationCategories.ToList();
            ViewBag.CooperationCategories = viewbag.ToList();

            return View();
        }

        // POST: Faculty/BusinessUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ArrCoopId, BusinessUser businessUser, HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                // add image
                using (var scope = new TransactionScope())
                {
                    businessUser.BusinessLogo = DateTime.Now.ToString("yymmssfff") + logo.FileName;
                    BusinessCooperationCategory BCC = new BusinessCooperationCategory();
                    var path = Server.MapPath("~/Image/");
                    logo.SaveAs(path + businessUser.BusinessLogo);
                    businessUser.Status_ID = 1;
                    db.BusinessUsers.Add(businessUser);
                    db.SaveChanges();
                    scope.Complete();
                }

                string[] arrayCoop = ArrCoopId.Split(',');

                for (int i = 0; i < arrayCoop.Length - 1; i++)
                {
                    BusinessCooperationCategory BCC = new BusinessCooperationCategory();
                    BCC.Business_ID = businessUser.ID;
                    BCC.CooperationCategories_ID = Int32.Parse(arrayCoop[i]);
                    db.BusinessCooperationCategories.Add(BCC);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(businessUser);
            }
        }

        
        // GET: Faculty/BusinessUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BusinessUser businessUser = db.BusinessUsers.Find(id);

            if (businessUser == null)
            {
                return HttpNotFound();
            }

            return View(businessUser);
        }

        // POST: Faculty/BusinessUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Details(string ArrCoopId, HttpPostedFileBase logo, BusinessUser businessUser)
        {
            if (ModelState.IsValid)
            {
                // Nếu logo != null => có thay đổi hình ảnh upload ảnh lên và tên
                if (logo != null)
                {
                    businessUser.BusinessLogo = DateTime.Now.ToString("yymmssfff") + logo.FileName;

                    var path = Server.MapPath("~/Image/");
                    logo.SaveAs(path + businessUser.BusinessLogo);
                    /*businessUser.Status = businessUser.Status_ID();*/
                }

                //Lưu BusinessUser
                db.Entry(businessUser).State = EntityState.Modified;
                db.SaveChanges();

                // Xóa toàn bộ liên kết theo Id của Bussiness
                var businessCooperationCategoryList = db.BusinessCooperationCategories.Where(x => x.Business_ID == businessUser.ID).ToList();

                foreach (var item in businessCooperationCategoryList)
                {
                    db.BusinessCooperationCategories.Remove(item);
                }

                // Add lại từ đầu với arrayCoop mới
                string[] arrayCoop = ArrCoopId.Split(',');

                for (int i = 0; i < arrayCoop.Length - 1; i++)
                {
                    BusinessCooperationCategory businessCooperationCategory1 = new BusinessCooperationCategory();
                    businessCooperationCategory1.Business_ID = businessUser.ID;
                    businessCooperationCategory1.CooperationCategories_ID = Int32.Parse(arrayCoop[i]);

                    db.BusinessCooperationCategories.Add(businessCooperationCategory1);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(businessUser);
            }
        }

        // GET: Faculty/BusinessUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BusinessUser businessUser = db.BusinessUsers.Find(id);

            if (businessUser == null)
            {
                return HttpNotFound();
            }

            return View(businessUser);
        }

        // POST: Faculty/BusinessUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BusinessUser businessUser = db.BusinessUsers.Find(id);

            db.BusinessUsers.Remove(businessUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("DeleteActivity")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessUser businessUser = db.BusinessUsers.Find(id);
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