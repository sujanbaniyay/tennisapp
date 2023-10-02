using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tennis.Web.Models.Domain;

namespace Tennis.Web.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly DatabaseContext _context;
        public ScheduleController(DatabaseContext context)
        {

            _context = context;

        }

        // Add schedule
        public IActionResult AddSchedule()
        {
            ViewBag.Coaches = _context.People.Where(p => p.Role == "coach").ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddSchedule(SchedulesMD schedule)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _context.Add(schedule);
                    _context.SaveChanges();
                    TempData["msg"] = "Schedule added successfully";
                    return RedirectToAction("ViewSchedules", "Schedule");
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Error: " + ex.Message;
                    return View();
                }
            }
            return View();
        }

        // View schedules
        public IActionResult ViewSchedules()
        {
            // Get all schedules into a ViewBag
            ViewBag.Schedules = _context.Schedules.ToList();
            ViewBag.Coaches = _context.People.Where(p => p.Role == "coach").ToList();
            return View();
        }

    }
}
