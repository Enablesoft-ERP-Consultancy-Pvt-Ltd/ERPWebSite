
using System.Linq;
using System.Security.Principal;


namespace IExpro.Web.Models
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }
        public long UserId { get; set; }
        public int IExproId { get; set; }
        public long LoginId { get; set; }
        public int CultureId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string[] roles { get; set; }
        public string TimeZone { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public long UserTypeId { get; set; }

    }



    public class CustomSerializePrincipal
    {
        public long UserId { get; set; }
        public long LoginId { get; set; }
        public int CultureId { get; set; }

        public int IExproId { get; set; }

        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string[] roles { get; set; }
        public string TimeZone { get; set; }
        public long ScopeId { get; set; }
        public long ParentId { get; set; }
        public long UserTypeId { get; set; }


    }



















}