using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OOP_Kyrsovaya
{
    public partial class EditForm : Form
    {
        static Form1 sourceForm;
        /// <summary>
        /// Старое название
        /// </summary>
        static string oldTitle;
        public EditForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Считываем данные из полей
        /// </summary>
        /// <param name="form"></param>
        public EditForm(Form1 form)
        {
            InitializeComponent();
            foreach (DataGridViewRow row in form.dataGridView1.SelectedRows)
            {
                textBox1.Text = form.dataGridView1[0, row.Index].Value.ToString();
                oldTitle = form.dataGridView1[0, row.Index].Value.ToString();
                textBox2.Text = form.dataGridView1[1, row.Index].Value.ToString();
                textBox3.Text = form.dataGridView1[2, row.Index].Value.ToString();
            }
            sourceForm = form;            
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(348, 220);
            this.MinimumSize = new Size(348, 220);
        }
        /// <summary>
        /// Редактирование
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string newTitle = textBox1.Text;
            string newIllness = textBox2.Text;
            if(newTitle == "" || newIllness == "")
            {
                Form1.printError("Введены не все значения!");
                return;
            }
            string newStrPrice = textBox3.Text;
            double newPrice = 0;
            try
            {
                newPrice = double.Parse(newStrPrice);
            }
            catch (FormatException)
            {
                Form1.printError("Неправильный формат числа!");
                return;
            }
            if(newPrice < 0)
            {
                Form1.printError("Неправильный формат числа!");
                return;
            }
            Form1.EditDB(oldTitle, newTitle, newIllness, newPrice);
        }

        private void EditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataSet medData = new DataSet("MedicStore");
            DataTable medTable = new DataTable("Lekarstva");
            // добавляем таблицу в dataSet
            medData.Tables.Add(medTable);

            DataColumn nameColumn = new DataColumn("Название", Type.GetType("System.String"));
            nameColumn.Unique = true;
            DataColumn illColumn = new DataColumn("Болезнь", Type.GetType("System.String"));
            DataColumn priceColumn = new DataColumn("Цена", Type.GetType("System.Double"));

            medTable.Columns.Add(nameColumn);
            medTable.Columns.Add(illColumn);
            medTable.Columns.Add(priceColumn);
            medTable.PrimaryKey = new DataColumn[] { medTable.Columns["Title"] };

            sourceForm.dataGridView1.DataSource = medData.Tables[0];
            sourceForm.dataGridView1.Columns[0].Width = 190;
            sourceForm.dataGridView1.Columns[1].Width = 190;
            sourceForm.dataGridView1.Columns[2].Width = 137;

            List<Medicines> medicines = Form1.ReadAllFromDB();
            foreach (var med in medicines)
            {
                medTable.Rows.Add(new object[] { med.Title, med.Illness, med.Price });
            }
        }
    }
}
