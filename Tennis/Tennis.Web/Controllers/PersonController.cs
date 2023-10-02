using Microsoft.AspNetCore.Mvc;
using Tennis.Web.Models.Domain;
namespace Tennis.Web.Controllers
{
    public class PersonController : Controller
    {
        private readonly DatabaseContext _context;
        public PersonController(DatabaseContext context)
        {

            _context = context;

        }

        // Add person
        public IActionResult Add()
        {
            ViewBag.UsersCount = _context.People.Count();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Person person)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    // check if email already exists
                    var emailExists = _context.People.Any(p => p.EmailAddress == person.EmailAddress);
                    if(emailExists)
                    {
                        // email already exists
                        TempData["msg"] = "Email already exists! ";
                        return View();
                    }                    
                    _context.Add(person);
                    _context.SaveChanges();
                    TempData["msg"] = "Person added successfully";
                    return RedirectToAction("Add", "Person");
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Error: " + ex.Message;
                    return View();
                }
            }
            return View();
        }

        // Login person
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginMD person)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var personExists = _context.People.Any(p => p.EmailAddress == person.EmailAddress && p.Password == person.Password);
                    if(personExists)
                    {
                        // print message
                        // login successful
                        TempData["msg"] = "Login successful";
                        // check role
                        var role = _context.People.Where(p => p.EmailAddress == person.EmailAddress).Select(p => p.Role).FirstOrDefault();
                        // store email in session
                        HttpContext.Session.SetString("email", person.EmailAddress);
                        // store role in session
                        HttpContext.Session.SetString("role", role);
                        // store id in session
                        var id = _context.People.Where(p => p.EmailAddress == person.EmailAddress).Select(p => p.Id).FirstOrDefault();
                        HttpContext.Session.SetInt32("id", id);
                        // store name in session
                        var name = _context.People.Where(p => p.EmailAddress == person.EmailAddress).Select(p => p.FirstName).FirstOrDefault() + " " +
                            _context.People.Where(p => p.EmailAddress == person.EmailAddress).Select(p => p.LastName).FirstOrDefault();
                        HttpContext.Session.SetString("name", name);
                        var birthDate = _context.People.Where(p => p.EmailAddress == person.EmailAddress).Select(p => p.BirthDate).FirstOrDefault();
                        HttpContext.Session.SetString("birthDate", birthDate.ToString());
                        // redirect to appropriate page
                        if (role == "admin")
                        {
                            // admin
                            return RedirectToAction("AdminDashboard","Person");
                        }
                        else if(role == "coach")
                        {
                            // coach
                            return RedirectToAction("CoachBio" , "Person");
                        }
                        // user
                        return RedirectToAction("UserDashboard", "Person");
                    }
                    // login failed
                    TempData["msg"] = "Invalid email or password";
                    return View();
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Error: " + ex.Message;
                    return View();
                }
            }
            return View();
        }

        // logout person
        public IActionResult Logout()
        {
            TempData["msg"] = "Logout successful";
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Person");
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("email") != null)
            {
                // logged in
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        // Admin dashboard
        public IActionResult AdminDashboard()
        {
            if(HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "admin")
            {
                // logged in
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        // Coach dashboard
        public IActionResult CoachDashboard()
        {
            if(HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "coach")
            {
                // logged in
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        // User dashboard
        public IActionResult UserDashboard()
        {
            if(HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "user")
            {
                // logged in
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        // View people (admin)
        public IActionResult ViewPeople()
        {
            if(HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "admin")
            {
                // logged in
                ViewBag.People = _context.People.ToList();
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        // Add person
        public IActionResult AddCoach()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCoach(Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // check if email already exists
                    var emailExists = _context.People.Any(p => p.EmailAddress == person.EmailAddress);
                    if (emailExists)
                    {
                        // email already exists
                        TempData["msg"] = "Email already exists! ";
                        return View();
                    }
                    _context.Add(person);
                    _context.SaveChanges();
                    TempData["msg"] = "Coach added successfully";
                    return RedirectToAction("ViewPeople", "Person");
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Error: " + ex.Message;
                    return View();
                }
            }
            return View();
        }

        // Coach bio
        public IActionResult CoachBio()
        {
            if(HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "coach")
            {
                // logged in
                // check if coach bio exists
                var coachId = HttpContext.Session.GetInt32("id");
                var coachBioExists = _context.CoachBio.Any(c => c.CoachId == coachId);
                if(coachBioExists)
                {
                    // coach bio exists
                    // get coach bio
                    var coachBio = _context.CoachBio.Where(c => c.CoachId == coachId).Select(c => c.Biography).FirstOrDefault();
                    ViewBag.CoachBioValue = coachBio;
                    return View();
                }
                // coach bio does not exist
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        [HttpPost]
        public IActionResult CoachBio(CoachBioMD coachBio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // check if coach bio exists
                    var coachId = HttpContext.Session.GetInt32("id");
                    // check if id is null
                    if(coachId == null)
                    {
                        // id is null
                        TempData["msg"] = "Error: Coach id is null";
                        return View();
                    }
                    var coachBioExists = _context.CoachBio.Any(c => c.CoachId == coachId);
                    if (coachBioExists)
                    {
                        // coach bio exists
                        // update coach bio
                        var coachBioToUpdate = _context.CoachBio.Where(c => c.CoachId == coachId).FirstOrDefault();
                        coachBioToUpdate.Biography = coachBio.Biography;
                        _context.SaveChanges();
                        TempData["msg"] = "Coach bio updated successfully";
                        return RedirectToAction("CoachBio", "Person");
                    }
                    // coach bio does not exist
                    // add coach bio
                    coachBio.CoachId = coachId;
                    _context.Add(coachBio);
                    _context.SaveChanges();
                    TempData["msg"] = "Coach bio added successfully";
                    return RedirectToAction("CoachBio", "Person");
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Error: " + ex.Message;
                    return View();
                }
            }
            return View();
        }

        // View Schedules meant for coach
        public IActionResult ViewSchedules()
        {
            if(HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "coach")
            {
                // logged in
                // get coach id
                var coachId = HttpContext.Session.GetInt32("id");
                // get schedules which are not yet started
                var schedules = _context.Schedules.Where(s => s.CoachId == coachId && s.Date > DateTime.Now).ToList();
                // get users
                var users = _context.People.Where(p => p.Role == "user").ToList();
                // get schedule assignments
                var scheduleAssigns = _context.ScheduleAssign.ToList();
                ViewBag.ScheduleAssigns = scheduleAssigns;
                ViewBag.Users = users;
                ViewBag.Schedules = schedules;
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        // View all schedules
        public IActionResult EnrollForSchedule()
        {
            if(HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "user")
            {
                // logged in
                // get schedules which are not yet started
                var schedules = _context.Schedules.ToList();
                // get schedule assignments
                var scheduleAssigns = _context.ScheduleAssign.ToList();
                ViewBag.Coaches = _context.People.Where(p => p.Role == "coach").ToList();
                ViewBag.CoachBio = _context.CoachBio.ToList();
                ViewBag.ScheduleAssigns = scheduleAssigns;
                ViewBag.Schedules = schedules;
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        // User enroll for schedule
        [HttpPost]
        public IActionResult EnrollForSchedule(int id)
        {
            if (HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "user")
            {
                // logged in
                // check if user is already enrolled for schedule
                var userId = HttpContext.Session.GetInt32("id");
                // enroll user for schedule
                var scheduleAssign = new ScheduleAssign
                {
                    UserId = userId,
                    ScheduleId = id
                };
                _context.Add(scheduleAssign);
                _context.SaveChanges();
                TempData["msg"] = "You have been enrolled for this schedule";
                return RedirectToAction("EnrollForSchedule", "Person");
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

        // Coach List
        public IActionResult CoachList()
        {
            if(HttpContext.Session.GetString("email") != null && HttpContext.Session.GetString("role") == "user")
            {
                // logged in
                ViewBag.Coaches = _context.People.Where(p => p.Role == "coach").ToList();
                ViewBag.CoachBio = _context.CoachBio.ToList();
                return View();
            }
            // not logged in
            return RedirectToAction("Login", "Person");
        }

    }
}
