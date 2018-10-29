using Microsoft.AspNetCore.Identity;

namespace DutchTreatTwo.Data.Entities
{
    public class StoreUser : IdentityUser
    {
       public string FirstName { get; set; }
       public string LastName { get; set; }
    }
}
