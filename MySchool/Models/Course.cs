using System;
using System.Collections.Generic;

namespace MySchool.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; }

    public int FkTeacherId { get; set; }

    public virtual Employee FkTeacher { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
