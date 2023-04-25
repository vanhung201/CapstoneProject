using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace BusinessConnectManagement.Models
{
    public class Outlook
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CC { get; set; }

        public Outlook(string To, string Subject, string Body, string CC)
        {
            this.To = To;
            this.Subject = Subject;
            this.Body = Body;
            this.CC = CC;
        }

        public void SendMail()
        {
            try
            {
                //SMTP client
                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("taikhoan.quantri.sep@gmail.com", "zdkhhtrmxocjjvaw");

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                //Add mail
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ketnoidoanhnghiep.vlu@gmail.com", "Website Quản Lý Kết Nối Doanh Nghiệp VLU");
                mail.To.Add(To);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(CC))
                {
                    mail.CC.Add(CC);
                }
                //send
                smtpClient.Send(mail);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public async void SendMailAsync()
        //{
        //    try
        //    {
        //        //SMTP client
        //        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = new System.Net.NetworkCredential("taikhoan.quantri.sep@gmail.com", "zdkhhtrmxocjjvaw");

        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpClient.EnableSsl = true;

        //        //Add mail
        //        MailMessage mail = new MailMessage();
        //        mail.From = new MailAddress("taikhoan.quantri.sep@gmail.com", "no-reply");
        //        mail.To.Add(To);
        //        mail.Subject = Subject;
        //        mail.Body = Body;
        //        mail.IsBodyHtml = true;

        //        if (!string.IsNullOrEmpty(CC))
        //        {
        //            mail.CC.Add(CC);
        //        }

        //        //Send mail
        //        await smtpClient.SendMailAsync(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}