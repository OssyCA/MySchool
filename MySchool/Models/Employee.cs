using System;
using System.Collections.Generic;

namespace MySchool.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string PersonalNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmployeeRole { get; set; }

    public byte[] PasswordHash { get; set; } // Tillagt efter, för att lagra lösenord för validerea att lägga till elever och personal

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
