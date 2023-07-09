using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamanApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamanApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NamanControler : ControllerBase
    {
        private static List<Naman> namans = new List<Naman>
            {
                //Enter The first Data
                new Naman{Id=1,
                    Name="Naman Jain",
                    FirstName="Naman",
                    LastName="Jain",
                    Address="41,Jaipur"},
                new Naman{Id=2,
                    Name="Mehul Jain",
                    FirstName="Mehul",
                    LastName="Jain",
                    Address="48,Jaipur"}
            };
        //Some Data
        private readonly DataContext _context;
        public NamanControler(DataContext context) {

            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Naman>>> Get(int id)
        {
            var namant = await _context.Namans.FindAsync(id);
            if (namant == null)
                return BadRequest("Naman not Found.");
            return Ok(await _context.Namans.ToListAsync());
        }
        //Add to the attribute
        [HttpPost]
        public async Task<ActionResult<List<Naman>>> Add(Naman naman)
        {
            _context.Namans.Add(naman);
            await _context.SaveChangesAsync();
            return Ok(await _context.Namans.ToListAsync());
        }
        //The Post Method
        [HttpPut]
        public async Task<ActionResult<List<Naman>>> Update(Naman request)
        {
            var naman = namans.Find(h => h.Id == request.Id);
            if (naman == null)
            {
                return BadRequest("The Name Not FOund");

            }
            naman.Name = request.Name;
            naman.LastName = request.LastName;
            naman.FirstName = request.FirstName;
            naman.Address = request.Address;

            return Ok(namans);
        }
        //Update the data
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Naman>>> Delete(int id)
        {
            var naman = namans.Find(h => h.Id == id);
            if (naman == null)
            {
                return BadRequest("The Name Not FOund");

            }
            namans.Remove(naman);
            return Ok(naman);

        }
        //Delete The Attribute
    }
}
