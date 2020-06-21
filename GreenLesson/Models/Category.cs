using System;
using System.Collections.Generic;

namespace GreenLesson.Models
{
    public partial class Category
    {
        public Category()
        {
            Course = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Parent { get; set; }
        public string Slug { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
}
