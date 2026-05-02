using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.DTOs.Application
{
    public class ApplicationDto
    {
        public int JobId { get; set; }
        public FileDto? CV { get; set; }
        public FileDto? CoverLetter { get; set; }
    }
}
