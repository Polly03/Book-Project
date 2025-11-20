using System.Diagnostics;
using System.IO;
using System.Windows;


namespace BookDatabase
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SetupDatabase();

            Main.Content = new BooksWindow();

           
        }

        // executing bat file to setup database
        public void SetupDatabase()
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            string batPath = Path.Combine(path, "db", "db_setup", "buildDB.bat");
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c \"{batPath}\"";
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(batPath);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            Console.WriteLine(output);
            Console.WriteLine(error);
        }
    }
}