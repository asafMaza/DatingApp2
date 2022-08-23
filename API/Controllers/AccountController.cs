using System.Text;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{

    //derives webapi controller functionality by inhereting the BaseApiController
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;

        // injecting db into our controller for manipulation and interactability with the db
        private readonly DataContext _context;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
            
        }


        //this method registers new users and processes  thier password by hashing and salting
        [HttpPost("register")]
        
        //method called asynchronously for interacting with db
        public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
        {
            //this if statement is used to first check that we dont have a matching username
            //before it can move onward and register  the user 
            //return a bad request if a match is found within the db 

            if(await UserExists(registerDto.Username)) return BadRequest("Username taken!");
            // hmac is used to implement hashing and salting  for our passwords,
            //the using is to make sure that the class will be disposed of when not in use 
            using var hmac = new HMACSHA512();

            //we are passing variables to the user object for injecting into our db
            var user =new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            //passing the user object to _contaxt and then saving the changes 
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username= user.UserName,
                Token = _tokenService.CreateToken(user)
            };
            
        }

        //this method is a login method
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(
                x=> x.UserName == loginDto.Username
            );

            if (user== null) return Unauthorized("Invalid username");
            
            using var hmac =new HMACSHA512(user.PasswordSalt);
            
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++ )
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username= user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        //this is a helper method to check if username already exists
        //since it is working with our db it must be asynchronous

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync( x=> x.UserName == username.ToLower());
        }
    }
}