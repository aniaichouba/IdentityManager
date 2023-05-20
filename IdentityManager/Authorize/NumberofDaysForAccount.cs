using IdentityManager.Data;
using System;
using System.Linq;

namespace IdentityManager.Authorize
{
    public class NumberofDaysForAccount : INumberofDaysForAccount
    {
        private readonly ApplicationDbContext _db;

        public NumberofDaysForAccount(ApplicationDbContext db)
        {
            _db = db;
        }

        public DateTime Datetime { get; private set; }

        public int Get(string userId)
        {
            var user = _db.applicationUsers.FirstOrDefault(u=>u.Id== userId);
            if (user != null && user.DateCreated != DateTime.MinValue)
            {
                return (DateTime.Today - user.DateCreated).Days;
            }
            return 0;
        }
    }
}
