
using Microsoft.EntityFrameworkCore;
using MySchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MySchool.MenyOptions
{
    internal class ReadFromSchool
    {
        public void ShowMenu()
        {
            Menu menu = new([
                "Visa studenter",
                "Visa Anställda",
                "Visa Kurser",
                "Visa betyg",
                "Visa Klasser",
                "Main Menu"
            ], "Visa Meny");

            switch (menu.MenuRun())
            {
                case 0:
                    ShowStudents();
                    Console.ReadKey();
                    break;
                case 1:
                    ShowEmployee();
                    Console.ReadLine();
                    break;
                case 2:
                    ShowCourses();
                    Console.ReadLine();
                    break;
                case 3:
                    ShowGradesLastMonth();
                    ShowAverageGradeForCourse();
                    Console.ReadLine();
                    break;
                case 4:
                    ShowClasses();
                    Console.ReadLine();
                    break;
            }
        }
        private void ShowStudents() // Show all students
        {
            Console.WriteLine("Vill du se alla studenter?(ja/nej): ");

            if (Console.ReadLine().ToLower() == "ja")
            {
                using (SchoolLabbDbContext dbContext = new())
                {
                    var students = dbContext.Students
                        .Include(s => s.FkClass) // Include the class for the student
                        .OrderBy(s => s.LastName);
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.FirstName} {student.LastName}, Class: {student.FkClass.ClassName}");
                    }
                }
            }
            else
            {
                ShowClasses();
                Console.WriteLine("Vilken klass?: ");
                string _class = Console.ReadLine();
                ShowStudents(_class);

            }
        } // Show students
        private void ShowStudents(string _class)
        {
            using (SchoolLabbDbContext dbContext = new()) 
            {
                var students = dbContext.Students // Get all students from the class and order them by last name
                    .Where(s => s.FkClass.ClassName == _class)
                    .OrderBy(s => s.LastName);
                if (students.Any())
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student}");
                    }
                }
                else
                {
                    Console.WriteLine("Ingen student i klassen");
                }
            }
        } // Show students by class
        private void ShowEmployee() // Show all teachers
        {
            Console.WriteLine("Vill du se alla anställda?(ja/nej)");
            if (Console.ReadLine().ToLower() == "ja")
            {
                using (SchoolLabbDbContext dbContext = new())
                {
                    var employees = dbContext.Employees;
                    foreach (var employee in employees)
                    {
                        Console.WriteLine($"{employee.FirstName} Role: {employee.EmployeeRole}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Vilken roll vill du se?: ");
                string role = Console.ReadLine();
                ShowEmployee(role);
            }

        }
        private void ShowEmployee(string role) // Show teachers by role
        {
            using (SchoolLabbDbContext dbContext = new())
            {
                var employees = dbContext.Employees.Where(e => e.EmployeeRole == role);

                if (employees.Any()) // If there are any employees with the role
                {
                    foreach (var employee in employees)
                    {
                        Console.WriteLine($"{employee.FirstName} Role: {employee.EmployeeRole}");
                    }
                }
                else
                {
                    Console.WriteLine("Ingen anställd ");
                }

            }
        }
        private void ShowGradesLastMonth()
        {
            Console.Clear();
            Dictionary<int, string> gradeDict = Grade.GetAlphabetGrade();
            Console.WriteLine("Betygen senaste månaden");
            using (SchoolLabbDbContext context = new())
            {
                var oneMonthAgo = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)); // Get the date one month ago
                var today = DateOnly.FromDateTime(DateTime.Now); // Get the current date

                var result = context.Grades
                    .Where(grade => grade.GradeDate >= oneMonthAgo && grade.GradeDate <= today)
                    .Select(grade => new // Create a new object with the student name, grade letter, course name and grade date
                    {
                        grade.FkStudent.FirstName,
                        GradeLetter = gradeDict[grade.GradeValue],
                        grade.FkCourse.CourseName,
                        grade.GradeDate
                    });

                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }

            }
            
        }
        private void ShowAverageGradeForCourse()
        {
            ShowCourses();

            Console.WriteLine("Vilken kurs vill du se genomsnittsbetyg för?: ");
            string courseName = Console.ReadLine();
            Dictionary<int, string> gradeDict = Grade.GetAlphabetGrade(); // Get the letter grade

            using (SchoolLabbDbContext context = new())
            {
                var courseGrades = context.Grades // Get the grades for the course
                    .Where(g => g.FkCourse.CourseName == courseName) // Get all grades for the course
                    .GroupBy(g => g.FkCourseId) // Group the grades by course
                    .Select(g => new //  Create a new object with the course name, average grade, highest grade and lowest grade
                    {
                        g.First().FkCourse.CourseName,
                        AverageGrade = gradeDict[(int)g.Average(grade => grade.GradeValue)], // Get the average grade for the course
                        HighestGrade = gradeDict[g.Max(grade => grade.GradeValue)], // Get the highest grade for the course
                        LowestGrade = gradeDict[g.Min(grade => grade.GradeValue)]   // Get the lowest grade for the course
                    }).FirstOrDefault();

                if (courseGrades != null)
                {
                    Console.WriteLine($"Kurs: {courseGrades.CourseName}");
                    Console.WriteLine($"Genomsnittsbetyg: {courseGrades.AverageGrade}");
                    Console.WriteLine($"Högsta betyg: {courseGrades.HighestGrade}");
                    Console.WriteLine($"Lägsta betyg: {courseGrades.LowestGrade}");
                }
                else
                {
                    Console.WriteLine("Ingen betyg hittades för kursen.");
                }
            }
        }
        private void ShowAvergeGrade() // Every student's average grade
        {
            Dictionary<int, string> gradeDict = Grade.GetAlphabetGrade(); // Get the letter grade
            using (SchoolLabbDbContext context = new())
            {
                var result = context.Grades // Get the average grade for each student
                    .GroupBy(grade => grade.FkStudentId)
                    .Select(group => new // Create a new object with the student name and the average grade
                    {
                        Student = group.First().FkStudent.FirstName,
                        AverageGrade = gradeDict[(int)group.Average(grade => grade.GradeValue)]
                    });
                foreach (var item in result)
                {
                    Console.WriteLine($"{item.Student} har i genomsnitt {item.AverageGrade}");
                }
            }
        }
        private void ShowClasses()
        {
            using (SchoolLabbDbContext dbContext = new())
            {
                var classes = dbContext.SchoolClasses; // Get all classes from the database
                foreach (var schoolClass in classes)
                {
                    Console.WriteLine($"{schoolClass.ClassName}");
                }
            }
        }
        private void ShowCourses()
        {
            using (SchoolLabbDbContext dbContext = new())
            {
                var courses = dbContext.Courses; // Get all courses from the database
                foreach (var course in courses)
                {
                    Console.WriteLine($"{course.CourseName}");
                }
            }
        }
    }
}