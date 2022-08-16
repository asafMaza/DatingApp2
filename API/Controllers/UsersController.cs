using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController: ControllerBase
    { 
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
       // endpoint 1 shows a list of all current users in our database
        [HttpGet]
         public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           return await  _context.Users.ToListAsync();
          
        }

        // endpoint 2 returns a user  using an id
        //http get here translates to  api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
           return await  _context.Users.FindAsync(id);
        }


    }

}