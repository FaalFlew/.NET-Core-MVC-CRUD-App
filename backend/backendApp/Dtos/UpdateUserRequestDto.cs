using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backendApp.Dtos
{
    public class UpdateUserRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? DateJoined { get; set; }
    }
}