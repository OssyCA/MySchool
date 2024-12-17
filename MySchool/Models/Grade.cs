using System;
using System.Collections.Generic;

namespace MySchool.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int FkStudentId { get; set; }

    public int FkCourseId { get; set; }

    public int GradeValue { get; set; }

    public DateOnly GradeDate { get; set; }

    public int FkTeacherId { get; set; }

    public virtual Course FkCourse { get; set; }

    public virtual Student FkStudent { get; set; }

    public virtual Employee FkTeacher { get; set; }

    public static Dictionary<int, string> GetAlphabetGrade()
    {
        return new Dictionary<int, string>
        {
            {6, "A"},
            {5, "B"},
            {4, "C"},
            {3, "D"},
            {2, "E"},
            {1, "F"}
        };
    }
    
}
