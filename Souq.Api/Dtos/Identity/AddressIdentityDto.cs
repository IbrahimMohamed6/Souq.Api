using Souq.Core.Entites.Identity;

namespace Souq.Api.Dtos.Identity
{
    public class AddressIdentityDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;

        
    }
}
