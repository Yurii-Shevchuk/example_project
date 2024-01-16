using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.UserDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "UserName is Required")]
        [MaxLength(24, ErrorMessage = "Username cannot be over 24 characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}