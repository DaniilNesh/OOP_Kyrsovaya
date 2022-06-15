using System;
using System.Windows.Forms;

namespace OOP_Kyrsovaya
{
    static class Program
    { 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Hello_Form());
            Application.Run(new Form1());
        }
    }
}
