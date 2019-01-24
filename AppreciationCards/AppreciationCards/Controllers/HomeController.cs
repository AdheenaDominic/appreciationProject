using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppreciationCards.Models;
using AppreciationProject.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace AppreciationCards.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppreciationContext _context;
        private readonly MessagesRepository messagesRepository;


        public HomeController(AppreciationContext context, MessagesRepository messagesRepository)
        {
            this.messagesRepository = messagesRepository;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }

        // GET: Messages
        [HttpGet]
        public async Task<IActionResult> CheckAppreciation()
        {
            var appreciationContext = _context.Messages.Include(m => m.Value);
            return View(await appreciationContext.ToListAsync());
        }

        [HttpGet]
        public IActionResult CreateAppreciation()
        {
            ViewData["ValueId"] = new SelectList(_context.XeroValues, "ValueId", "ValueName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAppreciation(Messages messages)
        {
            AppreciationProject.DBEntities.Messages entity = new AppreciationProject.DBEntities.Messages
            {
                Content = messages.Content,
                Date = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                From_name = messages.FromName,
                To_name = messages.ToName,
                Unread = "true", 
                Value = messages.Value
            };

            messagesRepository.SaveAppreciation(entity);

            return RedirectToAction(nameof(ViewAppreciation));
        }

        [HttpGet]
        public IActionResult Presentation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewAppreciation(DateTime? dateFrom, DateTime? dateTo)
        {
                return View(messagesRepository.LoadAppreciationWithinPeriod(dateFrom, dateTo));
        }
       
        [HttpGet]
        public async Task<List<Messages>> GetWeeksMessagesAsList()
        {
                return messagesRepository.LoadUnreadAppreciation();
        }

        [HttpPost]
        public async Task MarkAsRead(string toName, double dateTime)
        {
            messagesRepository.MarkReadAppreciation(toName,dateTime);
        }
    }
}