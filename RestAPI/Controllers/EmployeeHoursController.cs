using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Services;
using System;
using System.Collections.Generic;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeHoursController : ControllerBase
    {
        private readonly EmployeeHoursService _employeeHoursService;

        public EmployeeHoursController(EmployeeHoursService employeeHoursService)
        {
            _employeeHoursService = employeeHoursService;
        }

        // GET: api/EmployeeHours
        [HttpGet("{id:Length(4)}", Order = 1)]
        public ActionResult<List<HorasFuncionario>> Get(int id)
        {
            var hours = _employeeHoursService.Get(id);

            if (hours == null)
            {
                return NotFound();
            }

            return hours;
        }

        // GET: api/EmployeeHours/5
        [HttpGet("{id:Length(4)}&{monthSearch}", Name = "GetByMonth", Order = 2)]
        public ActionResult<List<HorasFuncionario>> Get(int id, DateTime monthSearch)
        {
            var hours = _employeeHoursService.Get(id, monthSearch);

            if (hours == null)
            {
                return NotFound();
            }

            return hours;
        }

        [HttpGet("{id:Length(4)}&{dateEnd}&{dateStart}", Name = "GetFiltered", Order = 3)]
        public ActionResult<List<HorasFuncionario>> Get(int id, DateTime dateEnd, DateTime dateStart)
        {
            var hours = _employeeHoursService.Get(id, dateEnd, dateStart);

            if (hours == null)
            {
                return NotFound();
            }

            return hours;
        }

        [HttpGet("{id:length(24)}", Name = "GetById", Order = 4)]
        public ActionResult<HorasFuncionario> Get(string id)
        {
            var hours = _employeeHoursService.Get(id);

            if (hours == null)
            {
                return NotFound();
            }

            return hours;
        }

        // POST: api/EmployeeHours
        [HttpPost]
        public ActionResult Create(HorasFuncionario employeeHours)
        {
            _employeeHoursService.Create(employeeHours);

            return CreatedAtRoute("GetById", new { id = employeeHours.Id }, employeeHours);
        }

        // PUT: api/EmployeeHours/5
        [HttpPut("{id:Length(24)}")]
        public ActionResult Update(string id, HorasFuncionario employeeHoursIn)
        {
            var hours = _employeeHoursService.Get(id);

            if (hours == null)
            {
                return NotFound();
            }

            _employeeHoursService.Update(employeeHoursIn);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:Length(24)}")]
        public void Delete(string id)
        {
            _employeeHoursService.Remove(id);
        }
    }
}