using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityManager.Authorize
{
    public class FirsNameAuthHandler : AuthorizationHandler<FirstNameAuthRequirement>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public FirsNameAuthHandler(UserManager<IdentityUser> userManager,ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FirstNameAuthRequirement requirement)
        {
            string userid = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _db.applicationUsers.FirstOrDefault(u=> u.Id == userid);
            var claims = Task.Run(async ()=>await _userManager.GetClaimsAsync(user)).Result;
            var claim = claims.FirstOrDefault(c=>c.Type =="FirstName");
            if (claim != null)
            {
                if(claim.Value.ToLower().Contains(requirement.Name.ToLower()))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }
}
