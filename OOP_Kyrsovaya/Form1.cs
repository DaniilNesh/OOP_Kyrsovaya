using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;


namespace OOP_Kyrsovaya
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        static string fileDB = "C:\\Users\\Public\\Documents\\Medicaments.txt";
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(Hello_Form form)
        {
            InitializeComponent();
            form.Close();
        }
        /// <summary>
        /// Вывод окна ошибки с введенным текстом
        /// </summary>
        /// <param name="text">Текст ошибки</param>
        public static void printError(string text)
        {
            MessageBox.Show(
                    text,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
        } 
        /// <summary>
        /// Создание файла
        /// </summary>
        /// <param name="file">Путь к файлу</param>
        public static void CreateFile(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(770, 366);
            this.MaximumSize = new Size(770, 366);
        }
        /// <summary>
        /// Чтение из базы данных
        /// </summary>
        /// <returns>Коллекция медикаментов</returns>
        public static List<Medicines> ReadAllFromDB()
        {
            string json = File.ReadAllText(fileDB);

            List<Medicines> allMedicaments = JsonConvert.DeserializeObject<List<Medicines>>(json); // перевод обратно из json

            return allMedicaments ?? new List<Medicines>();
        }
        /// <summary>
        /// Запись в базу данных
        /// </summary>
        /// <param name="medicament">Медикамент</param>
        static void SaveToDB(Medicines medicament)
        {
            List<Medicines> allMedicaments = ReadAllFromDB();
            if(allMedicaments.Count(u => u.Title == medicament.Title) > 0)
            {
                printError("Такой медикамент уже есть в БД!");
                return;
            }
            allMedicaments.Add(medicament);

            string serializedMedicaments = JsonConvert.SerializeObject(allMedicaments); // перевод в json
            File.WriteAllText(fileDB, serializedMedicaments);
        }
        /// <summary>
        /// Запись в базу данных
        /// </summary>
        /// <param name="medicaments">Коллекция медикаментов</param>
        static void SaveToDB(List<Medicines> medicaments)
        {
            string serializedMedicaments = JsonConvert.SerializeObject(medicaments);
            File.WriteAllText(fileDB, serializedMedicaments);
        }
        /// <summary>
        /// Удаление из БД
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        static bool DeletFromDB(string title)
        {
            bool result;
            List<Medicines> allMedicaments = ReadAllFromDB();

            Medicines medicamentForDeletion = allMedicaments.FirstOrDefault(u => u.Title == title);

            if (medicamentForDeletion != null)
            {
                allMedicaments.Remove(medicamentForDeletion);
                SaveToDB(allMedicaments);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Удаление БД
        /// </summary>
        static void DeleteDB()
        {
            File.WriteAllText(fileDB, "");
            MessageBox.Show(
                    "База данных успешно удалена!",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
        }
        /// <summary>
        /// Редактировать все
        /// </summary>
        /// <param name="title">Ключ записи для редактирования</param>
        /// <param name="newTitle">Новое название</param>
        /// <param name="newIllness">Новая болезнь</param>
        /// <param name="newPrice">Новая цена</param>
        public static void EditDB(string title, string newTitle, string newIllness, double newPrice)
        {
            string json = File.ReadAllText(fileDB);

            List<Medicines> allMedicaments = JsonConvert.DeserializeObject<List<Medicines>>(json);
            int index = allMedicaments.FindIndex(m => m.Title == title);
            if (index == -1) return;
            allMedicaments[index].Title = newTitle;
            allMedicaments[index].Illness = newIllness;
            allMedicaments[index].Price = newPrice;

            SaveToDB(allMedicaments);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Сохранение введенных данных в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            CreateFile(fileDB);
            string title = textBox1.Text;
            string illness = textBox2.Text;
            double price = 0;
            if(title == "" || illness == "")
            {
                MessageBox.Show(
                    "Введены не все данные!",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                return;
            }
            try
            {
                price = double.Parse(textBox3.Text);
            }
            catch (FormatException)
            {
                printError("Неправильный формат цены!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                return;
            }
            if(price < 0)
            {
                printError("Неправильный формат цены!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                return;
            }
            Medicines medicament = new Medicines(title, illness, price);
            SaveToDB(medicament);
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }
        /// <summary>
        /// Обновить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            CreateFile(fileDB);
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

            dataGridView1.DataSource = medData.Tables[0];
            dataGridView1.Columns[0].Width = 190;
            dataGridView1.Columns[1].Width = 190;
            dataGridView1.Columns[2].Width = 137;

            List<Medicines> medicines = ReadAllFromDB();
            foreach(var med in medicines)
            {
                medTable.Rows.Add(new object[] { med.Title, med.Illness, med.Price });
            }

        }
        /// <summary>
        /// Удалить БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            DeleteDB();
            dataGridView1.DataSource = null;
        }
        /// <summary>
        /// Удалить элемент(-ы)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if(dataGridView1[0, row.Index].Value == null)
                {
                    printError("Это пустая строка!");
                    return;
                }
                DeletFromDB(dataGridView1[0, row.Index].Value.ToString());
                dataGridView1.Rows.Remove(row);
            }
        }
        /// <summary>
        /// Кнопка редактировать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            int count = 0;
            Form f = Application.OpenForms["EditForm"];
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (dataGridView1[0, row.Index].Value == null)
                {
                    printError("Это пустая строка!");
                    return;
                }
                if (row.Index != -1) count++;
            }
            if (count > 1 || count == 0) return;
            else
            {
                EditForm editForm = new EditForm(this);
                editForm.Show();                
                if(f != null) f.Close();
            }
        }
        /// <summary>
        /// Кнопка сортировка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            Form form = Application.OpenForms["SortForm"];
            SortForm sortForm = new SortForm(this);
            sortForm.Show();
            if (form != null) form.Close();
        }
        /// <summary>
        /// Поиск и фильтрация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            Form form = Application.OpenForms["FiltrForm"];
            FiltrForm filtr = new FiltrForm(this);
            filtr.Show();
            if (form != null) form.Close();
        }
    }
}
