using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;    

namespace MySchool
{
    internal class Menu
    {
        // Fields
        private readonly string menuTitle;
        private readonly List<string> options;
        private int selected;
        //Construktor
        public Menu(List<string> _options, string _menuTitle)
        {
            menuTitle = _menuTitle;
            options = _options;
            selected = 0;  // De valet som de startar med.
        }
        private void DisplayMeny()
        {
            WriteLine(menuTitle); // Skriver ut meny titlen
            CursorVisible = false;
            for (int i = 0; i < options.Count; i++) // För varje element i listan.
            {
                string currentSelect = options[i]; // Vilken string använderen ligger på i menyn.
                string preCurrent; // Vad som visas framför alternativen

                if (i == selected)
                {
                    // Gör att nuvarde valet sticker ut.
                    preCurrent = ">> ";
                    ForegroundColor = ConsoleColor.Green;
                    BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    // Sätter dom andra valen till default färg.
                    preCurrent = " ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }
                WriteLine($"\n{preCurrent}[ {currentSelect} ]"); // Skriver ut alternativen
            }
            ResetColor();
        }
        public int MenuRun()
        {
            ConsoleKey keyPressed;
            do
            {
                Clear();
                DisplayMeny(); // Uppdaterar menyn med vart man ligger varje gång man trycker på Upp/Ner tangeterna.
                ConsoleKeyInfo keyInfo = ReadKey(true); // Kollar tanget är tryckt
                keyPressed = keyInfo.Key; // vilken tanget som trycks
                if (keyPressed == ConsoleKey.UpArrow) //Uppåt
                {
                    selected--; // Minskar med 1 för gå upp i meny.
                    if (selected == -1)
                    {
                        // Trycker man uppåt fast man är längst upp, hamnar man längst ner i menyn.
                        selected = options.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow) // Neråt
                {
                    selected++; // Ökar med 1 för gå ner i menyn.
                    if (selected == options.Count)
                    {
                        // Trycker man neråt fast man är längst ner, hamnar man längst upp i menyn.
                        selected = 0;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter); // Körs tills använderen tryckt på Enter tangeten.

            CursorVisible = true;
            return selected; // returnerar vilket meny val(index).
        }
    }
}
