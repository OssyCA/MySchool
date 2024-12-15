using System;
using System.Collections.Generic;

namespace MySchool.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string PersonalNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int FkClassId { get; set; }

    public string Gender { get; set; }

    public DateOnly BirthDay { get; set; }

    public virtual SchoolClass FkClass { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public override string ToString()
    {
        return $"Name: {FirstName} {LastName}, Personal Number: {PersonalNumber}";
    }
}
