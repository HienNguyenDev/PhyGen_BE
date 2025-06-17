using PhyGen.Domain.Users;

namespace PhyGen.Application.DTOs.Users
{
    public class UpdateUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}
