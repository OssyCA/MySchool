
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySchool.MenyOptions;

namespace MySchool
{
    internal class HandleSchoolDatabase
    {
        public void MainMenu()
        {

            Menu menu = new(["Add to School", "Read from School", "Exit"], "Main Menu");

            while (true)
            {
                switch (menu.MenuRun())
                {
                    case 0:
                        AddToSchool add = new();
                        add.ReadMenu();
                        break;
                    case 1:
                        ReadFromSchool read = new();
                        read.ShowMenu();
                        break;
                    case 2:
                        Environment.Exit(0);
                        break;

                }
                
            }
        }
    }
}
