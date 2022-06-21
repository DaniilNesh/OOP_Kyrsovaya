using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OOP_Kyrsovaya
{
    public partial class FiltrForm : Form
    {
        Form1 form1;
        public FiltrForm()
        {
            InitializeComponent();
        }
        public FiltrForm(Form1 form)
        {
            InitializeComponent();
            form1 = form;
        }
        /// <summary>
        /// Обновить таблицу
        /// </summary>
        /// <param name="medicines"></param>
        private void Update(List<Medicines> medicines)
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

            form1.dataGridView1.DataSource = medData.Tables[0];
            form1.dataGridView1.Columns[0].Width = 190;
            form1.dataGridView1.Columns[1].Width = 190;
            form1.dataGridView1.Columns[2].Width = 137;

            //List<Medicines> medicines = Form1.ReadAllFromDB();
            foreach (var med in medicines)
            {
                medTable.Rows.Add(new object[] { med.Title, med.Illness, med.Price });
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// Кнопка поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string razdel = comboBox1.Text;
            string znach = textBox1.Text;
            string sign = comboBox2.Text;
            double price = (double)numericUpDown1.Value;
            List<Medicines> values = new List<Medicines>();
            List<Medicines> buffer = new List<Medicines>();
            buffer = Form1.ReadAllFromDB();
            if (razdel == "Название" && znach != "")
            {
                var find = buffer.Where(f => f.Title.ToUpper().StartsWith(znach.ToUpper()));
                foreach (var i in find)
                    values.Add(i);

            }else if(razdel == "Болезнь" && znach != "")
            {
                var find = buffer.Where(f => f.Illness.ToUpper().StartsWith(znach.ToUpper()));
                foreach (var i in find)
                    values.Add(i);
            }
            
            if(sign != "" && comboBox1.SelectedIndex != 2)
            {
                buffer.Clear();
                if (sign == ">=")
                {
                    var find = values.Where(f => f.Price >= price);
                    foreach (var i in find)
                        buffer.Add(i);
                }else if(sign == "<=")
                {
                    var find = values.Where(f => f.Price <= price);
                    foreach (var i in find)
                        buffer.Add(i);
                }
                Update(buffer);
            }
            else if(sign == "" && comboBox1.SelectedIndex != 2)
            {
                Update(values);

            }else if(comboBox1.SelectedIndex == 2)
            {
                price = Convert.ToDouble(textBox1.Text);
                var find = buffer.Where(f => f.Price == price);
                foreach (var i in find)
                    values.Add(i);
                Update(values);
            }
            
        }

        private void FiltrForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(330, 210);
            this.MaximumSize = new Size(330, 210);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 2)
            {
                comboBox2.Enabled = false;
                numericUpDown1.Enabled = false;
            }
            else
            {
                comboBox2.Enabled = true;
                numericUpDown1.Enabled = true;
            }
        }
    }
}
