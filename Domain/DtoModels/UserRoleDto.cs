using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DtoModels
{
    public class UsersRoleDtp
    {
        public string UserId { get; set; }
        public string RoleName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UniversityId { get; set; } = null!;
        public string NationalCode { get; set; } = null!;
    }
}
