using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DtoModels
{
    public class UserRoleDetailDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UniversityId { get; set; } = null!;
        public string NationalCode { get; set; } = null!;
        public List<string>? UserRoleName { get; set; }
        public List<string>? RoleNameList { get; set; }
    }
}
