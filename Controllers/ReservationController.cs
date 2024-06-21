using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingManagementSystem.Data;
using ParkingManagementSystem.Models.Domain;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        public ReservationController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult AddParkingSlot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddParkingSlot(AddParkingMV addParkingMV)
        {
            if (ModelState.IsValid)
            {
                var parks = new ParkingSlot()
                {
                    SlotNumber = addParkingMV.SlotNumber,
                    IsAvaiable = true,
                };
                await applicationDbContext.ParkingSlots.AddAsync(parks);
                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(addParkingMV);
        }

        [HttpGet]
        public async Task<IActionResult> ShowParking()
        {
            var parks = await applicationDbContext.ParkingSlots.ToListAsync();
            return View(parks);
        }

        [HttpGet]
        public IActionResult SelectDate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectDate(SelectDateViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("SelectSlot", new { bookingDate = model.BookingDate });
            }
            return View(model);
        }



        //[HttpGet]
        //public async Task<IActionResult> SelectSlot(DateTime bookingDate)
        //{
        //    // Reset availability of all parking slots for the selected date
        //    var parkingSlots = await applicationDbContext.ParkingSlots.ToListAsync();
        //    foreach (var slot in parkingSlots)
        //    {
        //        if (applicationDbContext.Bookings.Any(booking => booking.SlotId == slot.SlotId && booking.BookingDate.Date == bookingDate.Date))
        //        {
        //            slot.IsAvaiable = false;
        //        }
        //        else
        //        {
        //            slot.IsAvaiable = true;
        //        }
        //    }
        //    await applicationDbContext.SaveChangesAsync();

        //    // Get all parking slots
        //    var allSlots = await applicationDbContext.ParkingSlots.ToListAsync();

        //    // Filter available parking slots for the selected booking date
        //    var availableSlots = allSlots.Where(slot => !applicationDbContext.Bookings.Any(booking => booking.SlotId == slot.SlotId && booking.BookingDate.Date == bookingDate.Date)).ToList();

        //    var model = new SelectSlotViewModel
        //    {
        //        BookingDate = bookingDate,
        //        AvailableSlots = availableSlots
        //    };

        //    return View(model);
        //}



        //upadate code in copy2
        [HttpGet]
        public async Task<IActionResult> SelectSlot(DateTime bookingDate)
        {
            var parkingSlots = await applicationDbContext.ParkingSlots.ToListAsync();
            foreach (var slot in parkingSlots)
            {
                slot.IsAvaiable = !applicationDbContext.Bookings.Any(booking => booking.SlotId == slot.SlotId && booking.BookingDate.Date == bookingDate.Date);
            }

            var model = new SelectSlotViewModel
            {
                BookingDate = bookingDate,
                AvailableSlots = parkingSlots.Where(slot => slot.IsAvaiable).ToList(),
                Slots = parkingSlots
            };

            return View(model);
        }

        ////main code
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult SelectSlot(SelectSlotViewModel model)
        //{


        //    Console.WriteLine($"BookingDate: {model.BookingDate}, SelectedSlotId: {model.SelectedSlotId}");
        //    return RedirectToAction("CreateBooking", new { bookingDate = model.BookingDate, slotId = model.SelectedSlotId });


        //}

        //update code in copy2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectSlot(SelectSlotViewModel model)
        {
            return RedirectToAction("CreateBooking", new { bookingDate = model.BookingDate, slotId = model.SelectedSlotId });
        }








        [HttpGet]
        public IActionResult CreateBooking(DateTime bookingDate, int slotId)
        {
            Console.WriteLine($"CreateBooking - BookingDate: {bookingDate}, SlotId: {slotId}");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            var model = new CreateBookingViewModel
            {
                UserId = userId.Value,
                SlotId = slotId,
                BookingDate = bookingDate
            };

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateBooking(CreateBookingViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var booking = new Booking
        //        {
        //            UserId = model.UserId,
        //            SlotId = model.SlotId,
        //            BookingDate = model.BookingDate,
        //            CarNumber = model.CarNumber
        //        };

        //        applicationDbContext.Bookings.Add(booking);

        //        // Update slot availability
        //        var slot = await applicationDbContext.ParkingSlots.FindAsync(model.SlotId);
        //        if (slot != null)
        //        {
        //            slot.IsAvaiable = false;
        //        }

        //        await applicationDbContext.SaveChangesAsync();
        //        return RedirectToAction("Confirmation", new { bookingId = booking.BookingId });

        //    }
        //    return View(model);
        //}


        //update code in copy2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking(CreateBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking
                {
                    UserId = model.UserId,
                    SlotId = model.SlotId,
                    BookingDate = model.BookingDate,
                    CarNumber = model.CarNumber
                };

                applicationDbContext.Bookings.Add(booking);

                var slot = await applicationDbContext.ParkingSlots.FindAsync(model.SlotId);
                if (slot != null)
                {
                    slot.IsAvaiable = false;
                }

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Confirmation", new { bookingId = booking.BookingId });
            }
            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> Confirmation(int bookingId)
        {
            var booking = await applicationDbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.ParkingSlot)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            var model = new ConfirmationViewModel
            {
                UserName = booking.User.UserName,
                CarNumber = booking.CarNumber,
                SlotNumber = booking.ParkingSlot.SlotNumber,
                BookingDate = booking.BookingDate
            };

            return View(model);
        }
    }
}
