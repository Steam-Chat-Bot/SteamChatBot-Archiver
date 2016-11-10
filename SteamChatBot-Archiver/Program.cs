using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace SteamChatBot_Archiver
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> files = new List<string>();
            try
            {
                WriteLeft("Loading bin/Release directory...");
                files = Directory.EnumerateFiles("SteamChatBot/bin/Release/", "*.dll").ToList();
                files.Add("SteamChatBot/bin/Release/SteamChatBot.exe");
                WriteRight();
            }
            catch(DirectoryNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Directory not found! Make sure this program is in the SteamChatBot solution directory.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(e.Message + ": " + e.StackTrace);
                Console.ReadLine();
            }

            WriteLeft("Creating Archive directory...");
            try
            {
                Directory.CreateDirectory("SteamChatBot/bin/Release/Archive");
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message + ": " + e.StackTrace);
                Console.ReadLine();
            }
            try
            {
                foreach (string file in files)
                {
                    File.Copy(file, "SteamChatBot/bin/Release/Archive/" + file.Substring(file.IndexOf("Release/") + "Release/".Length));
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message + ": " + e.StackTrace);
                Directory.Delete("SteamChatBot/bin/Release/Archive");
                Console.ReadLine();
            }
            WriteRight();

            Console.Write("Creating archive SteamChatBot.zip...");
            ZipFile.CreateFromDirectory("SteamChatBot/bin/Release/Archive/", "SteamChatBot/bin/Release/SteamChatBot.zip");
            WriteRight();

            WriteLeft("Removing bin/Release/Archive...");
            List<string> toDelete = Directory.EnumerateFiles("SteamChatBot/bin/Release/Archive/").ToList();
            foreach(string file in toDelete)
            {
                File.Delete(file);
            }
            Directory.Delete("SteamChatBot/bin/Release/Archive/");
            Console.CursorLeft = Console.BufferWidth - 5;
            WriteRight();
            Console.WriteLine("\n");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Writes something to the left side of the console
        /// </summary>
        /// <param name="written">What to write</param>
        /// <remarks>This method calls Console.Write(), not Console.WriteLine() so there will not be a line terminator at the end.</remarks>
        private static void WriteLeft(string written)
        {
            Console.Write(written);
        }

        /// <summary>
        /// Writes the word "Done." to the console with a line terminator
        /// </summary>
        /// <remarks>This method calls Console.Write(), not Console.WriteLine() but a line terminator is included</remarks>
        private static void WriteRight()
        {
            Console.CursorLeft = Console.BufferWidth - "Done.".Length - 5;
            Console.Write("Done.\n");
        }

        /// <summary>
        /// Writes something to the right side of the console
        /// </summary>
        /// <param name="written">What to write</param>
        /// <remarks>This method uses the length of the string to calculate how far to the right it should write. It calls Console.Write(), not Console.WriteLine(), but a line terminator is included so the cursor will go to the next line after this method is called.</remarks>
        private static void WriteRight(string written)
        {
            Console.CursorLeft = Console.BufferWidth - written.Length - 5;
            Console.Write(written + "\n");
        }
    }
}