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
            if (_httpContextAccessor.HttpContext?.Session.GetString("Username") != null)
            {
                string username = _httpContextAccessor.HttpContext?.Session.GetString("Username");

                return _context.Users.FirstOrDefault(u => u.UserName == username);

            }
            // Use _httpContextAccessor.HttpContext to access HttpContext

            return null; //Kan give problemer da username helst ikke skal være null. Måske default til "Guest"?
        }


        public void SendMail(User user, OurEvent events)
        {
            
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("gekija29@gmail.com");
            mail.To.Add(user.UserEmail);
            mail.Subject = "Ny Event Information";
            mail.Body = $"Kære {user.UserName}\n" +
                $"Vær opmærksom på, at der er kommet en ny event:\n" +
                $"{events.EventName} - {events.EventDate.ToString("dd-MM-yyyy")}\n" +
                $"Beskrivelse: {events.EventDescription}\n" +
                $"Med venlig hilsen,\nZealand Zoo";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(OURcredentials.GetUsername(), OURcredentials.GetPas()),
                EnableSsl = true
            };


            smtp.Send(mail);


        }



        public void SendMailTilmeldteUsers(User user, OurEvent ourEvent)
        {
            user = SetCurrentUser();

            
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("gekija29@gmail.com");
                mail.To.Add(user.UserEmail);
                mail.Subject = "New Event posted On Zealand Zoo";
                mail.Body = $"Hello {user.UserName}\n" +
                    $"A new event has been posted for you to join!!!\n" +
                    $"{ourEvent.EventName}\n" +
                    $"Event date: {ourEvent.EventDate.ToString("dd-MM-yyyy")}\n" +
                    $"Description: {ourEvent.EventDescription}\n" +
                    $"We hope to see you there!\n" +
                    $"Zealand Zoo";

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
