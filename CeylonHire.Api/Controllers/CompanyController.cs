using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
    }
}
