using System;
using System.Data.Entity;
using System.Linq;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Admin.Middleware
{
    public class GetFullNameWhenLoggedIn
    {
        private BCMEntities db = new BCMEntities();

        public string IsLoggedIn(Object obj, string email)
        {
            var fullName = "";

            if (String.IsNullOrEmpty(fullName))
            {
                var currentVanLangUser = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();
                
                currentVanLangUser.Last_Access = DateTime.Now;
                fullName = currentVanLangUser.FullName;

                db.Entry(currentVanLangUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                fullName = obj.ToString();
            }

            return fullName;
        }
    } 
}