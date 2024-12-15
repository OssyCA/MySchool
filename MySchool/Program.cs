using MySchool.Models;
using System.Security.Cryptography;
using System.Text;

namespace MySchool
{
    internal class Program
    {
        static void Main(string[] args)
        {







            HandleSchoolDatabase handleSchoolDatabase = new();

            handleSchoolDatabase.MainMenu();
        }
    }
}
