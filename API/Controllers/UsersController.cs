using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    // controllers are classes where the api logic resides and is handled
    // endpoints are the methods within the controller class anotated with
    // metadata for the function they preform

    //the metadata has been moved to a different class and now controllers could just inherate the metadata
    //and controllerbase functionality by  jsut calling on BaseApiController 
    public class UsersController: BaseApiController
    { 
        //_context is an instance of DataContext and is a dependancy injection 
        //allowing us to interact with our database
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
       // endpoint 1 shows a list of all current users in our database
        [HttpGet]
        [AllowAnonymous]
         public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           return await  _context.Users.ToListAsync();
          
        }

        // endpoint 2 returns a user  using an id
        //http get here translates to  api/Users/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
           return await  _context.Users.FindAsync(id);
        }


    }

}