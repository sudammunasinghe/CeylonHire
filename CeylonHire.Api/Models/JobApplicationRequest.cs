using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.DTOs.Application
{
    public class JobApplicationRequest
    {
        public int JobId { get; set; }
        public IFormFile? CVFile { get; set; }
        public IFormFile? CoverLetterFile { get; set; }
    }
}
