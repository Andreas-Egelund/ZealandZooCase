using System.Net.Mail;
using System.Net;
using ZealandZooCase.Data;
using ZealandZooCase.Models;

namespace ZealandZooCase.Services
{
    public class MailSenderService
    {






        public void SendMail(User user, OurEvent events)
        {



            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("gekija29@gmail.com");
            mail.To.Add(user.UserEmail);
            mail.Subject = "Signup confirmation - Zealand Zoo";
            mail.Body = $"We are sending you this mail because you have signed up for the following event:\n" +
                $"\n" +
                $"Event name: {events.EventName}\n" +
                $"Event date: {events.EventDate.ToString("dd-MMMM-yyyy")}\n" +
                $"Price: {events.EventTicketPrice}\n" +
                $"Description: {events.EventDescription}\n" +
                $"\n" +
                $"We look forward to seeing you there!!!";

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
