using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Services;
using System.Collections.Generic;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employee
        [HttpGet]
        public ActionResult<List<Funcionario>> Get() => _employeeService.Get();

        [HttpGet("{ativo}", Order = 3)]
        public ActionResult<List<Funcionario>> Get(bool ativo) => _employeeService.Get(ativo);

        // GET: api/Employee/5
        [HttpGet("{id:Length(24)}", Name = "GetEmployee", Order = 2)]
        public ActionResult<Funcionario> Get(string id)
        {
            var employee = _employeeService.Get(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpGet("{idEmp:Length(6)}", Order = 1)]
        public ActionResult<Funcionario> Get(int idEmp) => _employeeService.Get(idEmp);

        // POST: api/Employee
        [HttpPost]
        public ActionResult<Funcionario> Create(Funcionario employee)
        {
            _employeeService.Create(employee);

            return CreatedAtRoute("GetEmployee", new { registro = employee.Id }, employee);
        }

        // PUT: api/Employee/5
        [HttpPut("{id:Length(24)}")]
        public ActionResult Update(string id, Funcionario employeeIn)
        {
            var employee = _employeeService.Get(id);

            if (employee == null)
            {
                return NotFound();
            }

            _employeeService.Update(id, employeeIn);

            return NoContent();
        }
    }
}