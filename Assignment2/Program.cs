
/* *******************************************************
 * Name: Courtney Grover                                 *
 * Description: Watches a file or directory for changes  *
 *              and logs the changes in a log file while *
 *              also printing to the screen.             *
 * ***************************************************** */
using System;
using System.IO;
using System.Security.Permissions;


namespace Assignment2
{
    public class Watcher
    {
        public static void Main()
        {
            Run();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            string[] args = System.Environment.GetCommandLineArgs();

            // If a directory is not specified, exit program.
            if (args.Length != 2)
            {
                // Display the proper way to call the program.
                Console.WriteLine("Usage: Watcher.exe (directory)");
                return;
            }

            using (StreamWriter sw = new StreamWriter("watcher.log", true))
            {
                sw.WriteLine("\nDirectory being watched: " + args[1]);
                sw.Close();
            }

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = args[1];
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            //watcher.Filter = "*.txt";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.

            using (StreamWriter sw = new StreamWriter("watcher.log", true))
            {
                if (!e.Name.Equals("watcher.log"))
                {
                    sw.WriteLine("File/Directory: " + e.FullPath + " " + e.ChangeType + " " + DateTime.Now);
                    Console.WriteLine("File/Directory: " + e.FullPath + " " + e.ChangeType + " " + DateTime.Now);
                    sw.Close();
                }
            }
            
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.

            using (StreamWriter sw = new StreamWriter("watcher.log", true))
            {
                if (!e.Name.Equals("watcher.log"))
                {
                    sw.WriteLine("File: " + e.OldFullPath + " renamed to " + e.FullPath + " " + DateTime.Now);
                    Console.WriteLine("File: " + e.OldFullPath + " renamed to " + e.FullPath + " " + DateTime.Now);
                    sw.Close();
                }
            }
        }
    }
}