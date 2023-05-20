using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    [Authorize]
    public class AccessCheckerController : Controller
    {
        [AllowAnonymous]
        public IActionResult Allaccess()
        {
            return View();
        }
        [Authorize]
        public IActionResult AuthorizedAccess()
        {
            return View();
        }
        //[Authorize(Roles ="User,Admin")]
        [Authorize(Policy= "UserandAdmin")]
        public IActionResult UserAccess()
        {
            return View();
        }
        [Authorize(Policy = "Admin")]
        public IActionResult AdminAccess()
        {
            return View();
        }
        [Authorize(Policy = "Admin_CreateAccess")]
        public IActionResult Admin_CreateAccess()
        {
            return View();
        }
        [Authorize(Policy = "Admin_Create_Edite_DeleteAccess")]
        public IActionResult Admin_Create_Edite_DeleteAccess()
        {
            return View();
        }
        //[Authorize(Policy = "Admin_Create_Edite_DeleteAccess_SuperAdmin")]
        [Authorize(Policy = "OnlySuperAdminChecker")]
        public IActionResult Admin_Create_Edite_DeleteAccess_SuperAdmin()
        {
            return View();
        }
        [Authorize(Policy = "AdminWithMoreThan1000Days")]
        
        public IActionResult OnlyBhrugen()
        {
            return View();
        }
        [Authorize(Policy = "FirstNameAuth")]
        public IActionResult FirstNameAuth()
        {
            return View();
        }
    }
}
