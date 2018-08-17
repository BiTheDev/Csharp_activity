using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using belt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace belt.Controllers
{
    public class HomeController : Controller
    {
        private beltContext _context;
        
            public HomeController(beltContext context)
            {
                _context = context;
            }
            public IActionResult Index()
            {
                return View("Index");
            }
            [HttpPost("RegisterProcess")]
            public IActionResult Register(RegisterViewModel user){
                    if(ModelState.IsValid){
                        var userList = _context.users.Where(p => p.email== user.register_email).FirstOrDefault();
                        if(userList != null){
                            if(user.register_email == userList.email){
                                ModelState.AddModelError("register_email", "email exists");
                                return View("Index");
                            }
                        }                                     
                    PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                    user.register_password = Hasher.HashPassword(user, user.register_password);
                    users User = new users(){
                        first_name = user.first_name,
                        last_name = user.last_name,
                        email = user.register_email,
                        password = user.register_password,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
                    };
                    _context.Add(User);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("Id", (int)User.id);
                    return RedirectToAction("dashboard");            
                }else{
                    return View("Index");
                }
            }

            [HttpPost("LoginProcess")]
            public IActionResult Login(LoginViewModel User){
                users user = _context.users.Where(p => p.email== User.login_Email).SingleOrDefault();
                if(ModelState.IsValid){
                    if(user == null){
                        ModelState.AddModelError("login_Email", "Not a valid email or password");
                        return View("Index");
                    }
                    else if(user != null && User.login_Password != null)
                        {
                            var Hasher = new PasswordHasher<users>();
                            if( 0 !=Hasher.VerifyHashedPassword(user, user.password, User.login_Password)){
                                HttpContext.Session.SetInt32("Id", (int)user.id);
                                int? id = HttpContext.Session.GetInt32("Id");
                                return RedirectToAction("dashboard");
                            }else{
                                ModelState.AddModelError("login_Password", "Not a valid email or password");
                                return View("Index");
                            }
                    }                 
                }
                return View("Index");
            }
            
            [HttpGet("dashboard")]
            public IActionResult Dashboard(){
                int? id = HttpContext.Session.GetInt32("Id");
                if(id == null){
                    return RedirectToAction("Index");
                }
                List<activity> allactivity = _context.activity.Where(x=>x.date >DateTime.Now)
                .OrderBy(x=>x.date).Include(x=>x.activityparticpants).ToList();
                users user = _context.users.FirstOrDefault(x=>x.id == (int)id);
                ViewBag.allactivity = allactivity;
                ViewBag.user = user;
                return View("dashboard");
            }

            [HttpPost("Join")]
            public IActionResult Join(int activityid){
                int? id = HttpContext.Session.GetInt32("Id");
                users user = _context.users.FirstOrDefault(x=>x.id == (int)id);
                List<activity> allactivity = _context.activity.Where(x=>x.date >DateTime.Now).Include(x=>x.activityparticpants).OrderBy(x=>x.date).ToList();
                activity current = _context.activity.FirstOrDefault(x=>x.id == activityid);
                foreach (var checking in allactivity)
                {          
                    foreach (var participant in checking.activityparticpants)
                    {
                        if(checking.date == current.date && checking.time == checking.time &&
                            checking.timetype == current.timetype ){
                                if(checking.timetype == "Hours"){
                                    if ((checking.time.TotalHours + checking.duration) >= (current.time.Hours) && participant.usersid == user.id){
                                        ViewBag.error = "time conflict";
                                        ViewBag.allactivity = allactivity;
                                        ViewBag.user = user;
                                        return View("dashboard");
                                    }else if(checking.timetype == "Minutes"){
                                        if ((checking.time.TotalMinutes + checking.duration) >= (current.time.TotalMinutes) && participant.usersid == user.id){
                                        ViewBag.error = "time conflicts";
                                        ViewBag.allactivity = allactivity;
                                        ViewBag.user = user;
                                        return View("dashboard");
                                        }
                                    }
                                    }
                                }
                                else if(checking.date == current.date && checking.time == checking.time && participant.usersid == user.id ){
                                    ViewBag.error = "date conflicts";
                                        ViewBag.allactivity = allactivity;
                                        ViewBag.user = user;
                                        return View("dashboard");
                                }
                        if(checking.timetype == "Days"){
                            if((checking.date.DayOfYear + checking.duration) >= (current.date.DayOfYear) && participant.usersid == user.id){
                            ViewBag.error = "date conflicts";
                            ViewBag.allactivity = allactivity;
                            ViewBag.user = user;
                            return View("dashboard");
                        }
                    }
                }
                }
                participants guest = new participants(){
                    usersid = (int)id,
                    activityid = activityid
                };
                _context.Add(guest);
                _context.SaveChanges();
                return RedirectToAction("dashboard");
            }
            [HttpPost("delete")]
            public IActionResult Delete(int activityid){
                int? id = HttpContext.Session.GetInt32("Id");
                List<participants> removeall = _context.participants.Where(x=>x.activityid == activityid).ToList();
                activity removeactivity = _context.activity.Where(x=>x.usersid == (int)id).Where(x=>x.id == activityid).FirstOrDefault();
                foreach (var guest in removeall)
                {
                    _context.participants.Remove(guest);
                }
                _context.Remove(removeactivity);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            [HttpPost("leave")]
            public IActionResult Leave(int activityid){
                int? id = HttpContext.Session.GetInt32("Id");
                participants removeguest = _context.participants.Where(x=>x.activityid == activityid).Where(x=>x.usersid == (int)id).SingleOrDefault();
                _context.Remove(removeguest);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            [HttpGet("createactivity")]
            public IActionResult CreateActivity(){
                int? id = HttpContext.Session.GetInt32("Id");
                users creator = _context.users.FirstOrDefault(x=>x.id == (int)id);
                ViewBag.creator = creator;
                return View("create");
            }

            [HttpPost("create")]
            public IActionResult Create(activityViewModel newactivity){
                int? id = HttpContext.Session.GetInt32("Id");
                users creator = _context.users.SingleOrDefault(x=>x.id == (int)id);
                DateTime now = DateTime.Now;
                TimeSpan diff = newactivity.Time - now.TimeOfDay;
                if(newactivity.date < DateTime.Today){
                        ModelState.AddModelError("date", "event date must be in the future");
                        return View("create"); 
                }
                if( newactivity.date == DateTime.Today && diff.Minutes < 0){
                        ModelState.AddModelError("Time", "event time must be in the future");
                        return View("create"); 
                }

                if(ModelState.IsValid){
                    if(newactivity.date < DateTime.Today){
                        ModelState.AddModelError("date", "event date must be in the future");
                        return View("create"); 
                    }
                    if( newactivity.date == DateTime.Today && diff.Minutes < 0){
                            ModelState.AddModelError("Time", "event time must be in the future");
                            return View("create"); 
                    }
                    activity active = new activity(){
                        usersid = (int)id,
                        name = newactivity.name,
                        date = newactivity.date,
                        coordinator = creator.first_name,
                        time = newactivity.Time,
                        duration = newactivity.duration,
                        description = newactivity.description,
                        timetype = newactivity.timetype,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
                    };
                    _context.Add(active);
                    _context.SaveChanges();
                    return RedirectToAction("Detail", new{activityid = active.id});
                }
                return View("create");
            }

            [HttpGet("activity/{activityid}")]
            public IActionResult Detail(int activityid){
                int? id = HttpContext.Session.GetInt32("Id");
                activity activitydetail = _context.activity.Where(x=>x.id == activityid).Include(x=>x.activityparticpants).ThenInclude(x=>x.user).FirstOrDefault();
                users user = _context.users.FirstOrDefault(x=>x.id == (int)id);
                ViewBag.activitydetail = activitydetail;
                ViewBag.user = user;
                return View("detail");
            }

            [HttpGet("logout")]
            public IActionResult logout(){
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
             public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
