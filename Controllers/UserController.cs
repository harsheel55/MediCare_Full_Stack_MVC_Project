using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs;
using MediCare_MVC_Project.Models;
using MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.ReceptionistManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DoctorManagement;

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly IReceptionistService _receptionistService;

        private readonly ISpecializationService _specializationService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, ISpecializationService specializationService, IDoctorService doctorService, IReceptionistService receptionistService)
        {
            _doctorService = doctorService;
            _receptionistService = receptionistService;
            _specializationService = specializationService;
            _userService = userService;
            _mapper = mapper;
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        public async Task<IActionResult> GetDoctorsDropdown()
        {
            var doctors = await _doctorService.GetAllDoctorAsync(); // or however you're fetching
            var result = doctors.Select(d => new
            {
                doctorId = d.DoctorId,
                doctorName = d.FirstName + " " + d.LastName,
                specialization = d.Specialization
            });

            return Json(result);
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------- Admin User APIs ----------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------------------------------------------------

        // -------------- Show all the Admin User list in User Module --------------
        public async Task<IActionResult> UserList()
        {
            ViewBag.HideLayoutElements = true;
            var users = await _userService.GetAllUsersAsync();
            var viewModelList = _mapper.Map<List<UserViewModel>>(users);
            return View(viewModelList);
        }


        // -------------- Load _UserForm form creating Admin User --------------
        public IActionResult CreateAdminUser()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_UserForm", new UserRegisterDTO());
        }

        // -------------- Take Data from form to add new Admin User --------------
        [HttpPost]
        public async Task<IActionResult> CreateAdminUser(UserRegisterDTO userDto)
        {
            ViewBag.HideLayoutElements = true;

            if (!ModelState.IsValid)
            {
                return View("_UserForm", userDto);
            }

            int loggedInUser = GetLoggedInUserId();
            await _userService.AddUserAsync(loggedInUser, userDto);

            return RedirectToAction("UserList", "User");
        }

        // -------------- Fill form for update data --------------
        [HttpGet]
        public async Task<IActionResult> EditAdmin(int id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }
            var userRegisterDto = _mapper.Map<UserRegisterDTO>(userDto);
            return View("_UserForm", userRegisterDto);
        }

        // -------------- Update data into Database --------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(int id, UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("_UserForm", dto);
            }

            await _userService.UpdateUserAsync(GetLoggedInUserId(), id, dto);
            return RedirectToAction("UserList", "User");
        }

        // -------------- Delete User by email --------------
        [HttpPost]
        public async Task<IActionResult> Delete(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                await _userService.DeleteUserAsync(email);
            }

            return RedirectToAction("UserList", "User");
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------- Doctor User APIs ---------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------------------------------------------------

        // -------------- Show all the Doctor User list in Doctor Module --------------
        public async Task<IActionResult> DoctorList()
        {
            ViewBag.HideLayoutElements = true;
            var doctors = await _doctorService.GetAllDoctorAsync();
            var viewModelList = _mapper.Map<List<DoctorViewModel>>(doctors);
            return View(viewModelList);
        }


        // -------------- Load _DoctorForm form creating Doctor User --------------
        public IActionResult CreateDoctor()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_DoctorForm", new UserDoctorDTO());
        }

        // -------------- Take Data from form to add new doctor User --------------
        [HttpPost]
        public async Task<IActionResult> CreateDoctor(UserDoctorDTO userDoctorDTO)
        {
            ViewBag.HideLayoutElements = true;
            if (!ModelState.IsValid)
            {
                return View("_DoctorForm", userDoctorDTO);
            }

            int loggedInUser = GetLoggedInUserId();
            await _doctorService.AddDoctorAsync(loggedInUser, userDoctorDTO);
            return RedirectToAction("DoctorList", "User");
        }

        // -------------- Fill form for update data --------------
        [HttpGet]
        public async Task<IActionResult> EditDoctor(string email)
        {
            var userDto = await _doctorService.GetDoctorByEmailAsync(email);
            if (userDto == null)
            {
                return NotFound();
            }
            return View("_DoctorForm", userDto);
        }

        // -------------- Update data into Database --------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(string email, UserDoctorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("_DoctorForm", dto);
            }
            var loggedIdUser = GetLoggedInUserId();
            await _doctorService.UpdateDoctorAsync(email, dto, loggedIdUser);
            return RedirectToAction("DoctorList", "User");
        }

        // -------------- Delete Doctor record using email --------------
        public async Task<IActionResult> DeleteDoctor(string email)
        {
            if (email == null)
                throw new Exception("Email is needed.");

            await _doctorService.DeleteDoctorAsync(email);
            return RedirectToAction("DoctorList", "User");
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------- Receptionist User APIs ------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------------------------------------------------

        // -------------- Show all the Receptionist list in Receptionist Module --------------
        public async Task<IActionResult> ReceptionistList()
        {
            ViewBag.HideLayoutElements = true;
            var receptionist = await _receptionistService.GetAllReceptionistAsync();
            var viewModelList = _mapper.Map<List<ReceptionistViewModel>>(receptionist);
            return View(viewModelList);
        }

        // -------------- Load _ReceptionistForm form creating Receptionist User --------------
        public IActionResult CreateReceptionist()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_ReceptionistForm", new UserReceptionistDTO());
        }

        // -------------- Take Data from form to add new Receptionist User --------------
        [HttpPost]
        public async Task<IActionResult> CreateReceptionist(UserReceptionistDTO userDto)
        {
            ViewBag.HideLayoutElements = true;

            if (!ModelState.IsValid)
            {
                return View("_ReceptionistForm", userDto);
            }

            int loggedInUser = GetLoggedInUserId();
            await _receptionistService.AddReceptionistAsync(loggedInUser, userDto);

            return RedirectToAction("ReceptionistList", "User");
        }

        // -------------- Fill form for update data --------------
        [HttpGet]
        public async Task<IActionResult> EditReceptionist(int id)
        {
            var userDto = await _receptionistService.GetReceptionistByIdAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }
            return View("_ReceptionistForm", userDto);
        }

        // -------------- Update data into Database --------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReceptionist(int id, UserReceptionistDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("_ReceptionistForm", dto);
            }
            var loggedIdUser = GetLoggedInUserId();
            await _receptionistService.UpdateReceptionistAsync(id, dto, loggedIdUser);
            return RedirectToAction("ReceptionistList", "User");
        }
    }
}
