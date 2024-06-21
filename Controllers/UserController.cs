using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingManagementSystem.Data;
using ParkingManagementSystem.Models.Domain;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        public UserController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var user = applicationDbContext.Users.ToList();
            return View(user);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserMV addUserMV)
        {
            //adding modal state incopy2
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = addUserMV.UserName,
                    Email = addUserMV.Email,
                    Password = addUserMV.Password,
                };
                await applicationDbContext.Users.AddAsync(user);
                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("UserLogin");
            }
            return View(addUserMV);

        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginMV userLoginMV)
        {
            if (ModelState.IsValid)
            {
                var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == userLoginMV.Email && u.Password == userLoginMV.Password);

                if (user != null)
                {
                    // Successful login
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return RedirectToAction("SelectDate", "Reservation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }

            }

            return View(userLoginMV);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
