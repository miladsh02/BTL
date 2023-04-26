using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTL.Models;
using Microsoft.AspNetCore.Identity;

namespace Data.Entity
{
    public class CustomIdentityUserModel:IdentityUser
    {
        public StudentModel Student { get; set; } = null!;

    }
}
