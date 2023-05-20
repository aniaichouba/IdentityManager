using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityManager.Authorize
{
    public class AdminWithMoreThan1000DaysHandler : AuthorizationHandler<AdminWithMoreThan1000DaysRequirement>
    {
        private readonly INumberofDaysForAccount _numberofDaysForAccount;

        public AdminWithMoreThan1000DaysHandler(INumberofDaysForAccount numberofDaysForAccount)
        {
            _numberofDaysForAccount = numberofDaysForAccount;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminWithMoreThan1000DaysRequirement requirement)
        {
            if (!context.User.IsInRole("Admin"))
            {
                return Task.CompletedTask;
            }
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int numberOfDays = _numberofDaysForAccount.Get(userId);
            if (numberOfDays >= requirement.Days)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
