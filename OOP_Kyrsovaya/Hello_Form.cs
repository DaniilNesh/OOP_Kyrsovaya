using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOP_Kyrsovaya
{
    public partial class Hello_Form : Form
    {
        public Hello_Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();            
        }

        private void Hello_Form_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(650, 280);
            this.MaximumSize = new Size(670, 287);
        }
    }
}
