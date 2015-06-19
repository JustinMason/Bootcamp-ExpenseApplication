using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Database.ContribDepot
{
    class Program
    {
        static readonly string DatabaseName = GetDatabaseName();
        static readonly string DbServer = GetDatabaseServer();

        static void Main()
        {
            Console.Title = "RoundhousE Database Migrations Visual Studio Runner";

            var connectionString =
                string.Format("Data Source={0};Initial Catalog={1};Integrated Security=true;", DbServer,
                    DatabaseName);
            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            var parentDirectory = currentDirectory.Parent.Parent.FullName;
            var scriptspath = Path.GetDirectoryName(parentDirectory + @"\db\");

            var knockoutExe = GetKnockOutExe(currentDirectory.Parent);

            var p = new Process();
            var keySelection = string.Empty;

            while (string.Compare(keySelection, "Exit", StringComparison.InvariantCultureIgnoreCase) != 0)
            {

                if (!string.IsNullOrEmpty(keySelection))
                {

                    Console.WriteLine("Enter the environment variable, or enter for none.");
                    var environment = Console.ReadLine();
                    Console.WriteLine("Execution for environmental variable '{0}'.", environment);

                    Console.WriteLine();
                    var cmdArguments = string.Format(" /c=\"{0}\" /f=\"{1}\" /env={2} {3} /ni /trx", connectionString, scriptspath, environment, keySelection);
                    p.StartInfo.FileName = knockoutExe.FullName;
                    p.StartInfo.Arguments = cmdArguments;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;

                    p.Start();

                    Console.WriteLine(p.StandardOutput.ReadToEnd());
                    Console.WriteLine("Press Any Key to Continue");
                }
                else
                {
                    DrawMenu();
                }

                var key = Console.ReadKey(true);
                keySelection = GetArgumentsForKeySelection(key);
            }
        }

        private static FileInfo GetKnockOutExe(DirectoryInfo parent)
        {
            if (parent == null)
                throw new ApplicationException("RoundhousE not located in the src/packages directory.  Try Nuget and load roundhouse.");

            var roundHouseExe = parent.GetFiles("rh.exe", SearchOption.AllDirectories);
            if (roundHouseExe.Length < 1)
                return GetKnockOutExe(parent.Parent);
            else
            {
                return roundHouseExe[0];
            }
        }

        private static void DrawMenu()
        {
            Console.Clear();

            Console.WriteLine(" Database: " + DatabaseName);
            Console.WriteLine(" Server: " + DbServer);
            Console.WriteLine(" ----------------------------------------------------------------------------");
            Console.WriteLine(" 1. Update");
            Console.WriteLine(" 2. Drop");
            Console.WriteLine(" 6. Exit program");
        }

        /// <summary>
        ///returns project name and removes the word ".database."
        /// </summary>
        /// <returns></returns>
        private static string GetDatabaseName()
        {
            var databasename = ConfigurationManager.AppSettings["DatabaseName"];

            if (string.IsNullOrEmpty(databasename))
            {
                var projectname = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);

                databasename = projectname.Replace("Database.", "").Replace(".Database", "").Replace("Database", "");
            }
            return databasename;
        }

        private static string GetDatabaseServer()
        {
            var servername = ConfigurationManager.AppSettings["DatabaseServer"];

            if (string.IsNullOrEmpty(servername))
            {
                servername = Environment.GetEnvironmentVariable("dbServer");
            }

            if (string.IsNullOrEmpty(servername))
            {
                servername = ".\\sqlexpress";
            }
            return servername;
        }

        private static string GetArgumentsForKeySelection(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return " ";
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return "/drop";
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    return "Exit";
                default:
                    return string.Empty;
            }
        }
    }
}