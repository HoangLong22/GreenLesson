using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GreenLesson.Models
{
    public partial class Users
    {
        public Users()
        {
            Course = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int Status { get; set; }
        public string Account { get; set; }
        public string Thumbnail { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Course> Course { get; set; }
    }



    public partial class UsersViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int Status { get; set; }
        public string Account { get; set; }

        [DisplayName("Upload file")]
        public IFormFile Thumbnail { get; set; }
    }
}
