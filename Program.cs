using System;
using System.Windows.Forms;

namespace AudioNormPlus
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initialize Windows Forms settings
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the main form
            Application.Run(new UI.MainForm());
        }
    }
}
