using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
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

    [Authorize]
    public class UsersController: BaseApiController
    { 
        //_context is an instance of DataContext and is a dependancy injection 
        //allowing us to interact with our database
        private readonly IMapper _mapper;
        
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
       // endpoint 1 shows a list of all current users in our database
        [HttpGet]

         public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await  _userRepository.GetUsersAsync();

           return Ok(users);
          
        }

        // endpoint 2 returns a user  using an id
        //http get here translates to  api/Users/{id}

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await  _userRepository.GetMemberAsync(username);

          
        }


    }

}