using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.UserDtos
{
    public class AuthResponseDto
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}