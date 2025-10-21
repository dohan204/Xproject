using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.General
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SchoolLevelId { get; set; }
        public SchoolLevel SchoolLevel { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SchoolCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set ; }
        public string ModifiedBy { get; set ; }
        public School()
        {
        }
        public School(int Id, string name, int schoolLevelId, string address, string phoneNumber, string email, string schoolCode)
        {
            this.Id = Id;
            this.Name = name;
            this.SchoolLevelId = schoolLevelId;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.SchoolCode = schoolCode;
        }
    }
}
