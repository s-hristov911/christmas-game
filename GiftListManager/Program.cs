using System;
using System.Windows.Forms;

namespace SantaGiftListManager
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Forms.MainForm());
        }
    }
}
