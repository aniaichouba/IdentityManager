using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityManager.Data
{
    public class ClaimStore
    {
        public static List<Claim> claimsList = new List<Claim>()
        {
            new Claim("Create","Create"),
            new Claim("Edite","Edite"),
            new Claim("Delete","Delete")
        };
    }
}
