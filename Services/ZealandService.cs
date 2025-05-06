using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Services
{
    public class ZealandService
    {
        private readonly ZealandDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; // Add IHttpContextAccessor  // SKAL FINDE UD AF HVA FUCK DEN GØR - ANDREAS E

        public ZealandService(ZealandDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor; // Initialize IHttpContextAccessor
        }

        public User SetCurrentUser()
        {
            if(_httpContextAccessor.HttpContext?.Session.GetString("Username") != null)
            {
                string username = _httpContextAccessor.HttpContext?.Session.GetString("Username");

                return _context.Users.FirstOrDefault(u => u.UserName == username);

            }
            // Use _httpContextAccessor.HttpContext to access HttpContext

            return null; //Kan give problemer da username helst ikke skal være null. Måske default til "Guest"?
        }


        public void SendMail(User user)
        {
            user = SetCurrentUser();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("gekija29@gmail.com");
                mail.To.Add(user.UserEmail);
                mail.Subject = "Mail Bekræftelse";
                mail.Body = $"Kære, Tak for din tidsbestilling. Vi bekræfter hermed din aftale:\r\n\r\nHvis du har spørgsmål eller har brug for at ændre din tid, er du velkommen til at kontakte os på vores email som er: gekija29@gmail.com.\r\n\r\nVi ser frem til at se dig!";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(OURcredentials.GetUsername(), OURcredentials.GetPas()),
                    EnableSsl = true
                };


                smtp.Send(mail);

        }






    }




}
