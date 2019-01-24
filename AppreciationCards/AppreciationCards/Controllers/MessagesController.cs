using AppreciationCards.Models;
using AppreciationProject.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppreciationCards.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MessagesRepository messagesRepository;
        private readonly AppreciationContext _context;

        public MessagesController(AppreciationContext context,MessagesRepository messagesRepository)
        {
            _context = context;
            this.messagesRepository = messagesRepository;
        }

        [HttpGet("Messages")]
        public async Task<IActionResult> Index()
        {
            var appreciationContext = _context.Messages.Include(m => m.Value);
            return View(await appreciationContext.ToListAsync());
        }

        // GET: Messages/Create
        [HttpGet("Messages/Create")]
        public IActionResult Create()
        {
            ViewData["ValueId"] = new SelectList(_context.XeroValues, "ValueId", "ValueName");
            return View();
        }

        // GET: Messages/Details/5
        [HttpGet("Messages/Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Messages message = await _context.Messages
                                         .Include(m => m.Value)
                                         .FirstOrDefaultAsync(m => m.MessageId == id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }


        // POST: Messages/Create
        [ValidateAntiForgeryToken]
        [HttpPost("Messages/Create")]
        public async Task<IActionResult> Create(Messages message)
        {
            message.MessageDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (_context.Messages.Any(m => m.MessageId == message.MessageId))
                {
                    return BadRequest();
                }

                _context.Add(message);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        // Get: Messages/Get
        [HttpGet("Messages/Get")]
        public async Task<IActionResult> GetMessagesAll()
        {
            return Ok(await _context.Messages.ToListAsync());
        }

        // Get: Messages/Get/5
        [HttpGet("Messages/Get/{id}")]
        public async Task<IActionResult> GetMessageId(int id)
        {
            if (!await _context.Messages.AnyAsync(m => m.MessageId == id))
            {
                return NotFound();
            }

            Messages message = await _context.Messages.Include(m => m.Value).FirstAsync(m => m.MessageId == id);
            return Ok(message);
        }

        // Get: Messages/Get/CurrentWeek
        [HttpGet("Messages/Get/CurrentWeek")]
        public async Task<IActionResult> GetMessagesCurrentWeek()
        {
            DateTime lastSaturday, thisFriday, dateToday = DateTime.Now;

            lastSaturday = dateToday.AddDays(dateToday.DayOfWeek == DayOfWeek.Saturday ? 0 : -1 - (int)dateToday.DayOfWeek);
            thisFriday = dateToday.AddDays(dateToday.DayOfWeek == DayOfWeek.Sunday ? -2 : 5 - (int)dateToday.DayOfWeek);

            IQueryable<Messages> messagesCurrentWeek = _context.Messages.Include(m => m.Value)
                                                                        .Where(m => lastSaturday.Year < m.MessageDate.Year || lastSaturday.Year == m.MessageDate.Year && lastSaturday.DayOfYear <= m.MessageDate.DayOfYear)
                                                                        .Where(m => m.MessageDate.Year < thisFriday.Year || thisFriday.Year == m.MessageDate.Year && m.MessageDate.DayOfYear <= thisFriday.DayOfYear);

            List<Messages> messages = await messagesCurrentWeek.ToListAsync();
            return Ok(messages);
        }

        [HttpGet("Messages/Get/{dateFrom}/{dateTo}")]
        public async Task<IActionResult> GetCardsPeriod(DateTime? dateFrom, DateTime? dateTo)
        {
            List<Messages> messages;

            if (dateFrom != null && dateTo != null)
            {
                messages = await _context.Messages.Include(m => m.Value)
                                                           .Where(m => ((DateTime)dateFrom).Year < m.MessageDate.Year || ((DateTime)dateFrom).Year == m.MessageDate.Year && ((DateTime)dateFrom).DayOfYear <= m.MessageDate.DayOfYear)
                                                           .Where(m => m.MessageDate.Year < ((DateTime)dateTo).Year || ((DateTime)dateTo).Year == m.MessageDate.Year && m.MessageDate.DayOfYear <= ((DateTime)dateTo).DayOfYear)
                                                           .ToListAsync();
            }

            else if (dateTo != null)
            {
                messages = await _context.Messages.Include(m => m.Value)
                                                            .Where(m => m.MessageDate.Year < ((DateTime)dateTo).Year || ((DateTime)dateTo).Year == m.MessageDate.Year && m.MessageDate.DayOfYear <= ((DateTime)dateTo).DayOfYear)
                                                            .ToListAsync();
            }

            else if (dateFrom != null)
            {
                messages = await _context.Messages.Include(m => m.Value)
                                                            .Where(m => ((DateTime)dateFrom).Year < m.MessageDate.Year || ((DateTime)dateFrom).Year == m.MessageDate.Year && ((DateTime)dateFrom).DayOfYear <= m.MessageDate.DayOfYear)
                                                            .ToListAsync();
            }

            else
            {
                messages = await _context.Messages.Include(m => m.Value)
                                                           .ToListAsync();
            }

            return Ok(messages);
        }


        // POST: Messages/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost("Messages/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Messages message)
        {
            if (id != message.MessageId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid && await _context.Messages.FirstOrDefaultAsync(m => m.MessageId == id) != null)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Messages.Any(e => e.MessageId == message.MessageId))
                    {
                        return BadRequest();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return BadRequest();
        }
        // Delete: Messages/Delete/5
        [ValidateAntiForgeryToken]
        [HttpDelete("Messages/Delete")]
        public IActionResult Delete([FromQuery]string toName, [FromQuery]long dateTime)
        {
            messagesRepository.DelAppreciation(toName,dateTime);
            return Ok();

        }
    }
}