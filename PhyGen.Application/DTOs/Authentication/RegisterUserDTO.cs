using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.Authentication
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Username must contain only letters and no special characters or numbers.")]
        [DefaultValue("NguyenHien")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format.")]
        [DefaultValue("hien.nguyen@gmail.com")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$", ErrorMessage = "Password must contain at least one letter and one number.")]
        [DefaultValue("abc12345")]
        public string Password { get; set; } = string.Empty;

    }
}
