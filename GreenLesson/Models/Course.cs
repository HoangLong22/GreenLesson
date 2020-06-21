using System;
using System.Collections.Generic;

namespace GreenLesson.Models
{
    public partial class Course
    {
        public Course()
        {
            Lesson = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string Thumbnail { get; set; }
        public int Status { get; set; }

        public virtual Category Category { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Lesson> Lesson { get; set; }
    }


    public class CoursesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string CategoryName { get; set; }
        public string UserAccount { get; set; }
        public int Status { get; set; }
    }
}
