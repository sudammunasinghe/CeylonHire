using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.DTOs.Auth
{
    public class ResetPasswordDto
    {
        public Guid TokenId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
