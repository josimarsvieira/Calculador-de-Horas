using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Services;
using System;
using System.Collections.Generic;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankHoursController : ControllerBase
    {
        private readonly BankHoursService _bancoDeHorasService;

        public BankHoursController(BankHoursService bankHoursService)
        {
            _bancoDeHorasService = bankHoursService;
        }

        // GET: api/Books
        [HttpGet("{id:Length(4)}")]
        public ActionResult<List<BancoDeHoras>> Get(int id)
        {
            var bank = _bancoDeHorasService.Get(id);

            if (bank == null)
            {
                return NotFound();
            }

            return bank;
        }

        // GET: api/Books/5
        [HttpGet("{id:Length(24)}", Name = "GetBankById")]
        public ActionResult<BancoDeHoras> Get(string id)
        {
            var bankHours = _bancoDeHorasService.Get(id);

            if (bankHours == null)
            {
                return NotFound();
            }
            return bankHours;
        }

        // GET: api/BankHours/5
        [HttpGet("{id:Length(4)}&{dateSearch}")]
        public ActionResult<List<BancoDeHoras>> Get(int id, DateTime dateSearch) => _bancoDeHorasService.Get(id, dateSearch);

        // POST: api/Books
        [HttpPost]
        public ActionResult<BancoDeHoras> Create(BancoDeHoras bankIn)
        {
            _bancoDeHorasService.Create(bankIn);

            return CreatedAtRoute("GetBankById", new { id = bankIn.Id.ToString() }, bankIn);
        }

        // PUT: api/Books/5
        [HttpPut("{id:Length(24)}")]
        public IActionResult Update(string id, BancoDeHoras BankHoursIn)
        {
            var bank = _bancoDeHorasService.Get(id);

            if (bank == null)
            {
                return NotFound();
            }

            _bancoDeHorasService.Update(id, BankHoursIn);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:Length(24)}")]
        public IActionResult Delete(string id)
        {
            var bank = _bancoDeHorasService.Get(id);

            if (bank == null)
            {
                return NotFound();
            }

            _bancoDeHorasService.Remove(bank);

            return NoContent();
        }
    }
}