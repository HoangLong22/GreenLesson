using System;
using System.Collections.Generic;

namespace GreenLesson.Models
{
    public partial class Section
    {
        public Section()
        {
            Lesson = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Lesson> Lesson { get; set; }
    }
}
