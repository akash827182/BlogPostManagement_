using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q3.Shared.DTO.MainData
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        public required string Username { get; set; }
       
        public string? Password { get; set; }

        public string Role { get; set; } = "User";
    }
}
