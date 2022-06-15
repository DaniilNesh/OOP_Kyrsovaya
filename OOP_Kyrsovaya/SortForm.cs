using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OOP_Kyrsovaya
{
    public partial class SortForm : Form
    {
        static Form1 sourceForm;
        public SortForm(Form1 form)
        {
            InitializeComponent();
            sourceForm = form;
        }
        /// <summary>
        /// сортировка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string key = comboBox1.Text;
            string how = comboBox2.Text;
            List<Medicines> list = new List<Medicines>();
            List<Medicines> medicines = new List<Medicines>();
            DataGridView view = sourceForm.dataGridView1;
            for (int i = 0; i < view.RowCount - 1; i++)
            {
                Medicines med = new Medicines(view[0, i].Value.ToString(), view[1, i].Value.ToString(), double.Parse(view[2, i].Value.ToString()));
                list.Add(med);
            }
            switch (key)
            {
                case "Название":
                    if (how == "По возрастанию")
                    {
                        var data = from m in list
                                   orderby m.Title
                                   select m;

                        foreach (var m in data)
                            medicines.Add(m);
                    }
                    else if (how == "По убыванию")
                    {
                        var data = from m in list
                                   orderby m.Title descending
                                   select m;
                        foreach (var m in data)
                            medicines.Add(m);
                    }
                    break;
                case "Болезнь":
                    if (how == "По возрастанию")
                    {
                        var data = from m in list
                                   orderby m.Illness
                                   select m;
                        foreach (var m in data)
                            medicines.Add(m);
                    }
                    else if (how == "По убыванию")
                    {
                        var data = from m in list
                                   orderby m.Illness descending
                                   select m;
                        foreach (var m in data)
                            medicines.Add(m);
                    }
                    break;
                case "Цена":
                    if (how == "По возрастанию")
                    {
                        var data = from m in list
                                   orderby m.Price
                                   select m;
                        foreach (var m in data)
                            medicines.Add(m);
                    }
                    else if (how == "По убыванию")
                    {
                        var data = from m in list
                                   orderby m.Price descending
                                   select m;
                        foreach (var m in data)
                            medicines.Add(m);
                    }
                    break;
                default:
                    break;
            }
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
            foreach (var med in medicines)
            {
                medTable.Rows.Add(new object[] { med.Title, med.Illness, med.Price });
            }
        }
        /// <summary>
        /// Задает размеры окна 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortForm_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new System.Drawing.Size(350, 160);
            this.MinimumSize = new System.Drawing.Size(350, 160);
        }
    }
}
