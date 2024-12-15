
using Microsoft.IdentityModel.Abstractions;
using MySchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MySchool.MenyOptions
{
    internal class AddToSchool
    {
        public void ReadMenu()
        {
            Menu menu = new([
                "Lägg till elev",
                "Lägg till Personal",
                "BAck to main menu"
            ], "Update Menu");

            switch (menu.MenuRun())
            {
                case 0:
                    AddStudent();
                    break;
                case 1:
                    AddEmployee();
                    break;
            }
        }
        private void AddStudent()
        {
            Console.Clear();
            using(SchoolLabbDbContext context = new())
            {
                try
                {
                    Student student = StudentInfo();

                    if (student != null)
                    {
                        context.Students.Add(student);
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Något gick fel");
                        Console.ReadLine();
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }



        }
        private Student StudentInfo() // Only a admin can add a student
        {
            Console.Clear();
            Console.WriteLine("Lägg till ny Elev");

            Console.WriteLine("Förnamn: ");
            string fName = Console.ReadLine();

            Console.WriteLine("Efternamn: ");
            string lName = Console.ReadLine();

            Console.WriteLine("Personnummer: ");
            string social = Console.ReadLine();

            Console.WriteLine("Klass: ");
            if (!int.TryParse(Console.ReadLine(), out int classId))
            {
                Console.WriteLine("Använd ett gilitigt nr");
                return null; 
            }

            Console.WriteLine("Kön: ");
            string gender = Console.ReadLine();


            Console.WriteLine("Födelsedatum (yyyy-mm-dd): ");
            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly birthDate))
            {
                Console.WriteLine("Ange ett giltigt datum i formatet yyyy-mm-dd");
                return null;
            }
            if (ValidateEmployee()) // Validate the password of the employee
            {
                return new Student
                {
                    FirstName = fName,
                    LastName = lName,
                    PersonalNumber = social,
                    Gender = gender,
                    FkClassId = classId,
                    BirthDay = birthDate
                };
            }
            else
            {
                Console.WriteLine("Fel lösenord");
                Console.ReadLine();
                return null;
            }
        }
        
        private void AddEmployee()
        {
            using (SchoolLabbDbContext context = new())
            {
                Employee employee = AddEmployeeInfo();
                try
                {
                    if (employee != null)
                    {
                        context.Employees.Add(employee);
                        context.SaveChanges();
                        Console.WriteLine("Personal tillagd");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private Employee AddEmployeeInfo()
        {
            Console.Clear();
            Console.WriteLine("Lägg till ny Personal");

            Console.WriteLine("Förnamn: ");
            string fName = Console.ReadLine();

            Console.WriteLine("Efternamn: ");
            string lName = Console.ReadLine();

            Console.WriteLine("Personnummer: ");
            string social = Console.ReadLine();

            Console.WriteLine("Roll: ");
            string role = Console.ReadLine();

            Console.WriteLine("Lösenord: ");
            string password = MaskInput();

            if (ValidateEmployee())
            {
                return new Employee
                {
                    FirstName = fName,
                    LastName = lName,
                    PersonalNumber = social,
                    EmployeeRole = role,
                    PasswordHash = ComputeHash(password)
                };
            }
            else
            {
                Console.WriteLine("Något gick fel");
                return null;
            }
        }

        private byte[] ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create()) // Create a new instance of the SHA256 class
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(input)); // Compute the hash of the input string
            }
        }
        static string MaskInput()
        {
            SecureString password = new SecureString(); // SecureString is used to store sensitive data such as passwords
            ConsoleKeyInfo key; // ConsoleKeyInfo is used to read key inputs from the console
            do
            {
                key = Console.ReadKey(true); // ReadKey(true) is used to hide the input from the console
                if (!char.IsControl(key.KeyChar)) // Check if the key is a control key
                {
                    password.AppendChar(key.KeyChar);   // Append the key to the password
                    Console.Write("*");     // Write a * to the console
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)   //   Check if the key is a backspace key and the password length is greater than 0
                {
                    password.RemoveAt(password.Length - 1);  // Remove the last character from the password
                    Console.Write("\b \b");     // Remove the last character from the console
                }
            } while (key.Key != ConsoleKey.Enter);
            return new System.Net.NetworkCredential(string.Empty, password).Password; // Return the password as a string

        }
        private bool ValidateEmployee() // Validate the password of the employee
        {
            Console.WriteLine("\nSkriv lösenord för lägga till: ");
            string password = MaskInput();
            using (SchoolLabbDbContext dbContext = new())
            {
                var employee = dbContext.Employees.First(e => e.EmployeeRole == "Administratör"); // Get the first employee with the role "Administratör"

                string inputPassword = password;
                byte[] inputHash;

                using (var sha256 = SHA256.Create()) // Create a new instance of the SHA256 class
                {
                    inputHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
                }

                if (employee.PasswordHash.SequenceEqual(inputHash))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }
}
