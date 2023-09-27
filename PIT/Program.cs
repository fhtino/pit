using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PIT
{

    internal class Program
    {

        private static bool _dirOnly;
        private static bool _brokenOnly;


        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"PIT.exe - ver. {Assembly.GetExecutingAssembly().GetName().Version}");
                Console.WriteLine();

                if (args.Contains("--help", StringComparer.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Usage: PIT.EXE [folderName] [flags]");
                    Console.WriteLine("");
                    Console.WriteLine("Flags:");
                    Console.WriteLine("  --dironly    : process directories only");
                    Console.WriteLine("  --brokenonly : show only directories and files with broken permissions inheritance");
                    Console.WriteLine("");
                    return;
                }


                // Flags
                _dirOnly = args.Contains("--dironly", StringComparer.InvariantCultureIgnoreCase);
                _brokenOnly = args.Contains("--brokenonly", StringComparer.InvariantCultureIgnoreCase);

                // Start path
                string startPath = args.Where(x => !x.StartsWith("--")).FirstOrDefault();
                if (startPath == null) startPath = ".";
                startPath = Path.GetFullPath(startPath);

                // Do work
                
                Console.WriteLine($"Processing: {startPath}");
                Console.WriteLine();
                Traverse(startPath, 0);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.ToString()}");
            }
        }


        private static string OK = "-";
        private static string KO = "X";


        private static void Traverse(string path, int level)
        {
            string fill = new string(' ', level * 4);

            // current directory
            bool inheritanceEnabled = !(Directory.GetAccessControl(path).AreAccessRulesProtected);
            if (!_brokenOnly)
            {
                Console.WriteLine($"{fill} {(inheritanceEnabled ? OK : KO)} {Path.GetFileName(path)}");
            }
            else
            {
                if (!inheritanceEnabled)
                {
                    Console.WriteLine($"{(inheritanceEnabled ? OK : KO)} {path}");
                }
            }

            // process subdirectory
            var subDirs = Directory.GetDirectories(path);
            foreach (var subDir in subDirs) { Traverse(subDir, level + 1); }

            // current directory files
            if (!_dirOnly)
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    bool fileInheritanceEnabled = !(File.GetAccessControl(file).AreAccessRulesProtected);

                    if (!_brokenOnly)
                    {
                        Console.WriteLine($"{fill}    {(fileInheritanceEnabled ? OK : KO)} {Path.GetFileName(file)}");
                    }
                    else
                    {
                        if (!fileInheritanceEnabled)
                        {
                            Console.WriteLine($"{(fileInheritanceEnabled ? OK : KO)} {file}");
                        }
                    }
                }
            }

        }

    }

}
