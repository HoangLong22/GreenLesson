using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GreenLesson.Models
{
    public partial class Course
    {
        public Course()
        {
            Lesson = new HashSet<Lesson>();
        }

        public static int Count { get; internal set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string Thumbnail { get; set; }
        public int Status { get; set; }
        internal static object Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        public virtual Category Category { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Lesson> Lesson { get; set; }
    }


    public partial class CoursesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }

        [DisplayName("Upload file")]
        public IFormFile Thumbnail { get; set; }
    }
}
