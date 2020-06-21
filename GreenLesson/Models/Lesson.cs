using System;
using System.Collections.Generic;

namespace GreenLesson.Models
{
    public partial class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public int SectionId { get; set; }
        public string Contents { get; set; }
        public int Status { get; set; }

        public virtual Course Course { get; set; }
        public virtual Section Section { get; set; }

        //public object Short_description { get; internal set; }
    }

    public class LessonViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public int Status { get; set; }
        public string CourseName { get; set; }
        public string SectionName { get; set; }

        //public virtual Course Course { get; set; }
        //public virtual Section Section { get; set; }
    }


}
