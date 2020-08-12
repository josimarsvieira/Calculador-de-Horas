using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Services;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: api/Company
        [HttpGet]
        public ActionResult<Empresa> Get() => _companyService.Get();

        // GET: api/Company/5
        [HttpGet("{id:Length(24)}", Name = "GetCompany")]
        public ActionResult<Empresa> Get(string id) => _companyService.Get();

        // POST: api/Company
        [HttpPost]
        public ActionResult<Empresa> Create(Empresa company)
        {
            _companyService.Create(company);

            return CreatedAtRoute("GetCompany", new { id = company.Id }, company);
        }

        // PUT: api/Company/5
        [HttpPut("{id:Length(24)}")]
        public ActionResult Update(string id, Empresa companyIn)
        {
            var company = _companyService.Get(id);

            if (company == null)
            {
                return NotFound();
            }

            _companyService.Update(id, companyIn);

            return NoContent();
        }

        // DELETE: app/Company/5
        [HttpDelete("id:Length(24)")]
        public void Remove(string id)
        {
            _companyService.Remove(id);
        }
    }
}