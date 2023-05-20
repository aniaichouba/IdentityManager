using IdentityManager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManager.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(ApplicationDbContext db, UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Upsert(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                var objFromDb = _db.Roles.FirstOrDefault(x => x.Id == id);
                return View(objFromDb);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole roleObj)
        {
            if (await _roleManager.RoleExistsAsync(roleObj.Name))
            {
                TempData[SD.Error] = "Role already exists";
                return RedirectToAction(nameof(Index));
            }
            if(string.IsNullOrEmpty(roleObj.Id)) 
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = roleObj.Name});
                TempData[SD.Error] = "Role created successfully";
                
            }
            else
            {
                var objRoleFromDb = _db.Roles.FirstOrDefault(u => u.Id == roleObj.Id);
                if(objRoleFromDb == null)
                {
                    TempData[SD.Error] = "Role not found";
                    return RedirectToAction(nameof(Index));
                }
                objRoleFromDb.Name = roleObj.Name;
                objRoleFromDb.NormalizedName = roleObj.Name.ToLower();
                var result = await _roleManager.UpdateAsync(objRoleFromDb);
                TempData[SD.Error] = "Role updated successfully";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var obgFromDb = _db.Roles.FirstOrDefault(u => u.Id == id);
            if (obgFromDb == null)
            {
                TempData[SD.Error] = "Role not found.";
                return RedirectToAction(nameof(Index));
            }
            var userRoleForThisRoel = _db.UserRoles.Where(u=>u.RoleId == id).Count();
            if (userRoleForThisRoel > 0)
            {
                TempData[SD.Error] = "Cannot delete this role, since there are users assigned to this role.";
                return RedirectToAction(nameof(Index));
            }
            await _roleManager.DeleteAsync(obgFromDb);
            TempData[SD.Error] = "Role deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
