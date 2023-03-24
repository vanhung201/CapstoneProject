using BusinessConnectManagement.Middleware;
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
    [LoginVerification]
    public class BusinessUsersController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/BusinessUsers
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.BusinessUsers.ToList());
        }
        public JsonResult getDataList()
        {
            var test = (from bu in db.BusinessUsers
                        join s in db.Status on bu.Status_ID equals s.ID into status
                        select new
                        {
                            id = bu.ID,
                            username = bu.Username,
                            password = bu.Password,
                            lastAccess = bu.Last_Access,
                            status = bu.Status.Status1,
                            buName = bu.BusinessName,
                            address = bu.Address,
                            buPhone = bu.BusinessPhone,
                            website = bu.Website,
                            fanpage = bu.Fanpage,
                            buLogo = bu.BusinessLogo,
                            contactName = bu.ContactName,
                            contactPhone1 = bu.ContactPhone_1,
                            contactPhone2 = bu.ContactPhone_2,
                            emailContact = bu.EmailContact
                        });
            return Json(test, JsonRequestBehavior.AllowGet);
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

            var buUser = (from bu in db.BusinessUsers
                          join s in db.Status on bu.Status_ID equals s.ID into status
                          where bu.ID == id
                          select new
                          {
                              id = bu.ID,
                              username = bu.Username,
                              password = bu.Password,
                              lastAccess = bu.Last_Access,
                              status = bu.Status.Status1,
                              buName = bu.BusinessName,
                              address = bu.Address,
                              buPhone = bu.BusinessPhone,
                              website = bu.Website,
                              fanpage = bu.Fanpage,
                              buLogo = bu.BusinessLogo,
                              contactName = bu.ContactName,
                              contactPhone1 = bu.ContactPhone_1,
                              contactPhone2 = bu.ContactPhone_2,
                              emailContact = bu.EmailContact,
                              sem_id = bu.Semester_ID
                          });



            return Json(buUser, JsonRequestBehavior.AllowGet);
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
                if (db.BusinessUsers.Where(x => x.BusinessName == businessUser.BusinessName).Any() == true)
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">Tên Doanh Nghiệp Đã Tồn Tại</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                    return RedirectToAction("Index");
                }
                else
                {
                    // add image
                    using (var scope = new TransactionScope())
                    {
                        var checkSem = db.Semesters.Where(x => x.Status == true).FirstOrDefault();
                        businessUser.BusinessLogo = DateTime.Now.ToString("yymmssfff") + logo.FileName;
                        BusinessCooperationCategory BCC = new BusinessCooperationCategory();
                        var path = Server.MapPath("~/Uploads/Images/");
                        logo.SaveAs(path + businessUser.BusinessLogo);
                        businessUser.Status_ID = 1;
                        businessUser.Semester_ID = checkSem.ID;
                        db.BusinessUsers.Add(businessUser);
                        db.SaveChanges();
                        scope.Complete();
                    }


                    TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Thêm thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(businessUser);
            }
        }

        public JsonResult IsUserExists(string UserName)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.BusinessUsers.Any(x => x.Username == UserName), JsonRequestBehavior.AllowGet);
        }


        // POST: Faculty/BusinessUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase logo, BusinessUser businessUser)
        {
            if (ModelState.IsValid)
            {
                // Nếu logo != null => có thay đổi hình ảnh upload ảnh lên và tên
                if (logo != null)
                {
                    businessUser.BusinessLogo = DateTime.Now.ToString("yymmssfff") + logo.FileName;
                    var path = Server.MapPath("~/Uploads/Images/");
                    logo.SaveAs(path + businessUser.BusinessLogo);
                    /*businessUser.Status = businessUser.Status_ID();*/
                }
                if (db.BusinessUsers.Any(x=>x.ID != businessUser.ID && x.BusinessName == businessUser.BusinessName))
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">Tên Doanh Nghiệp Đã Tồn Tại</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                    return RedirectToAction("Index");
                }
                //Lưu BusinessUser
                db.Entry(businessUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
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
            TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Xóa thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
            return View(businessUser);
        }

        // POST: Faculty/BusinessUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            BusinessUser businessUser = db.BusinessUsers.Find(id);
            if (db.MOUs.Where(x => x.Business_ID == id).Any() == true)
            {
                TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Xóa không thành công vì doanh nghiệp này đã ký kết MOU.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");
            }
            if (db.Posts.Where(x => x.Business_ID == id).Any() == true)
            {
                TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Xóa không thành công vì doanh nghiệp đang có bài đăng tuyển dụng.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");
            }
            if (db.Registrations.Where(x => x.Business_ID == id).Any() == true)
            {
                TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Xóa không thành công vì đang có sinh viên đăng ký thực tập.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");
            }
            if (db.BusinessCooperationCategories.Where(x => x.Business_ID == id).Any() == true)
            {
                TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Xóa không thành công vì doanh nghiệp đang có hoạt động hợp tác.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");
            }
            db.BusinessUsers.Remove(businessUser);
            db.SaveChanges();
            TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Xóa Doanh Nghiệp Thành Công</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
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
            TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Xóa Thành Công</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
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