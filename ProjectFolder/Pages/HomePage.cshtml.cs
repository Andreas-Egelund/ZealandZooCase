﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ZealandZooCase.Data;
using ZealandZooCase.Models;
using ZealandZooCase.Services;
using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Packaging.Rules;

namespace ZealandZooCase.Pages
{
    public class HomePageModel : PageModel
    {
        public ZealandService _zealandService;

        public ZealandDBContext _context;

        private MailSenderService _mailsenderservice;

        public HomePageModel(ZealandService service, ZealandDBContext context, MailSenderService mailsenderservice)
        {
            _zealandService = service;
            _context = context;
            _mailsenderservice = mailsenderservice;
        }

        public OpenHour? OpenHour => _context.OpenHours.FirstOrDefault();
        public User ?CurrentUser => _zealandService.SetCurrentUser();

        public bool IsLoggedIn { get; set; }
        public List<OpenHour> OpenHours { get; set; }
        public string Error { get; set; }

        public List<OurEvent> AllEvents => _context.AllOurEvents.OrderBy(e => e.EventDate).ToList();

        public IActionResult OnPostVisKalander()
        {
            return RedirectToPage("/CalendarView");
        }

        public string ErrorMessage { get; set; }


        public void OnGet()
        {
            RegisterEvent = new List<int>();


            if (CurrentUser != null)
            {
                IsLoggedIn = true;
            }
            else
            {
                IsLoggedIn = false;
            }

            if (IsLoggedIn)
            {
                RegisterEvent = _context.AllEventSignups.Where(s => s.UserId == CurrentUser.UserId).Select(s => s.EventId).ToList();
            }
        }



        public List<AllEventSignup> AllEventSignups => _context.AllEventSignups.ToList();

        public List<int> RegisterEvent { get; set; }


        public IActionResult OnPostTilmeld(int EventId)
        {
            try
            {
            var currentEvent = _context.AllOurEvents.FirstOrDefault(e => e.EventId == EventId);
            var NumberofUsersSignedUp = _context.AllEventSignups.Where(s => s.EventId == EventId).Count();


            if (currentEvent.EventMaxAttendance <= NumberofUsersSignedUp)
            {
                ErrorMessage = "Der er ikke plads til flere på dette event.";
            }
            else
            {

                var user = _zealandService.SetCurrentUser();

                var TilmeldteEvent =_context.AllEventSignups.Add(new AllEventSignup
                {
                    EventId = EventId,
                    UserId = user.UserId,
                    SignupDate = DateTime.Now
                });
                _context.SaveChanges();

                _mailsenderservice.SendMail(user, currentEvent);


                return RedirectToPage("/UserProfile/ProfileSite");

            }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }


            return RedirectToPage("/UserProfile/ProfileSite");

        }



        public IActionResult OnPostAfmeld(int EventId)
        {
            try
            {
                var user = _zealandService.SetCurrentUser();
                var signup = _context.AllEventSignups.FirstOrDefault(s => s.EventId == EventId && s.UserId == user.UserId);
                if (signup != null)
                {
                    _context.AllEventSignups.Remove(signup);
                    _context.SaveChanges();

                }
                return RedirectToPage("/UserProfile/ProfileSite");
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return Page();

        }


        public IActionResult OnPostGoToCheckout(int EventId)
        {
            try
            {

                var user = _zealandService.SetCurrentUser();
                if (user != null)
                {
                    return RedirectToPage("/Checkout/CheckOut", new { EventId = EventId }); //Sender eventId til checkout siden. Ren Magi  - Andreas E.
                }
                else
                {
                    return RedirectToPage("/Login");
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;

            }
            return Page();
        }







    }
}
