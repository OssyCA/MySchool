using System;
using System.Collections.Generic;

namespace MySchool.Models;

public partial class SchoolClass
{
    public int ClassId { get; set; }

    public string ClassName { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
