using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    // this is an entity we want to have in our database and to interact with
    public class AppUser
    {
     public int Id { get; set; }
     public string UserName { get; set; }
        
    }
}