using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace OH.Utilities
{
    /// <summary>
    /// The rapid identity.
    /// </summary>
    public class UserPrincipal : GenericPrincipal
    {
        public int Id { get; set; }
        public int Account_Profile_Id { get; set; }
        public string Account_Username { get; set; }
        public string Account_Email { get; set; }
        public string Account_Avatar { get; set; }
        public string Account_Name { get; set; }
        public int Account_Type_Id { get; set; }
        public int Account_Status { get; set; }
        public bool Account_HasSetPassword { get; set; }
        
        public UserPrincipal(GenericIdentity identity, string[] roles)
            : base(identity, roles)
        {
        }

        public UserPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles)
        {
        }

        //public override bool IsInRole(string roleName)
        //{
        //    roleName = roleName.Trim().ToLower();
        //    return Roles.Any(e => e.ToLower() == roleName);
        //}
    }
}
