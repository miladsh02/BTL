using Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BTL.Models
{
    public class StudentModel
    {
        [Key]
        public Guid Id { get; set; }
        public string UniversityId { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserId{ get; set; } = null!;
        public CustomIdentityUserModel CustomIdentityUserModel { get; set; }
        public ICollection<StudentOrderModel> Orders { get; set; }

        public DateTime CreatedDate { get; set; }


    }
}
