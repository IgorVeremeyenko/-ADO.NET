using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private SqlConnection conn = null;
        private SqlDataAdapter adapter = null;
        private DataSet set = null;

        public Form1()
        {
            InitializeComponent();
            button2.Visible = false;
            button3.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            label18.Visible = false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            try
            {
                string connectionString = @"Data Source=DESKTOP-JMV89FI;Initial Catalog=VegetablesAndFruits;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                conn = new SqlConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
                richTextBox1.Text = "Соединено!";
                button2.Visible = true;                
                button1.Enabled = false;

                //Макс. калорий
                label1.Text = MaxCalories();
                //Мин. калорий
                label4.Text = MinCalories();
                //Среднее значение калорий
                label6.Text = AvgCalories();
                //количество овощей
                countVagit.Text = CountOfVagitables();
                //количество фруктов
                countFruits.Text = CountOfFruits();
                //количество овощей и фруктов
                countFruitsAndVagits.Text = CountOfFruitsAndVagitables();
                label18.Visible = true;
                               
            }
            catch
            {
                MessageBox.Show("Нет связи с БД!");
                richTextBox1.Text = "Ошибка подключения";
            }
        }

        private void CountByColourEveryFruit()
        {
            adapter = new SqlDataAdapter("select colour, count(*) as 'Count' from MainTable group by Colour", conn);
            set = new DataSet();
            adapter.Fill(set);
            int count = set.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                listBox1.Items.Add(set.Tables[0].Rows[i][0].ToString() 
                    + ": " + set.Tables[0].Rows[i][1].ToString());
            }
            
        }

        private string MaxCalories()
        {
            adapter = new SqlDataAdapter(
                    "select top(1) Calories from MainTable order by Calories desc", conn);
            set = new DataSet();
            adapter.Fill(set);
            return set.Tables[0].Rows[0][0].ToString();
        }

        private string MinCalories()
        {
            adapter = new SqlDataAdapter(
                    "select top(1) Calories from MainTable order by Calories", conn);
            set = new DataSet();
            adapter.Fill(set);
            return set.Tables[0].Rows[0][0].ToString();
        }

        private string AvgCalories()
        {
            adapter = new SqlDataAdapter(
                   "select sum(Calories)/count(Calories) from MainTable", conn);
            set = new DataSet();
            adapter.Fill(set);
            return set.Tables[0].Rows[0][0].ToString();
        }

        private void CountByColour()
        {            
            adapter = new SqlDataAdapter("select Colour from MainTable group by Colour", conn);
            set = new DataSet();
            adapter.Fill(set);
            int count = set.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                comboBox1.Items.Add(set.Tables[0].Rows[i][0].ToString());
            }
        }

        private string CountOfVagitables()
        {
            adapter = new SqlDataAdapter(
                    "select count(*) from MainTable where type='овощ'", conn);
            set = new DataSet();
            adapter.Fill(set);
            return set.Tables[0].Rows[0][0].ToString();
        }

        private string CountOfFruits()
        {
            adapter = new SqlDataAdapter(
                    "select count(*) from MainTable where type='фрукт'", conn);
            set = new DataSet();
            adapter.Fill(set);
            return set.Tables[0].Rows[0][0].ToString();
        }

        private string CountOfFruitsAndVagitables()
        {
            adapter = new SqlDataAdapter(
                    "select count(*) from MainTable where type='фрукт' or type='овощ'", conn);
            set = new DataSet();
            adapter.Fill(set);
            return set.Tables[0].Rows[0][0].ToString();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            label18.Visible = false;
            adapter = new SqlDataAdapter("select * from MainTable", conn);
            set = new DataSet();
            adapter.Fill(set);
            dataGridView1.DataSource = set.Tables[0];
            //Кол-во овощей и фруктов заданного цвета
            CountByColour();
            //Количество фруктов и овощей каждого цвета
            CountByColourEveryFruit();
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            button3.Visible = true;
            radioButton1.Visible = true;
            radioButton2.Visible = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                adapter = new SqlDataAdapter("select name from MainTable", conn);
                set = new DataSet();
                adapter.Fill(set);
                dataGridView1.DataSource = set.Tables[0];
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton2 = (RadioButton)sender;
            if (radioButton2.Checked)
            {
                adapter = new SqlDataAdapter("select colour from MainTable group by colour", conn);
                set = new DataSet();
                adapter.Fill(set);
                dataGridView1.DataSource = set.Tables[0];
            }
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = comboBox1.SelectedItem.ToString();
            adapter = new SqlDataAdapter("select count(*) from MainTable where colour = '" + query + "'", conn);
            set = new DataSet();
            adapter.Fill(set);
            label15.Text = set.Tables[0].Rows[0][0].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            set.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            dataGridView1.DataSource = null;
            comboBox1.Items.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            if (textBox4.Text == "")
            {
                listBox2.Items.Clear();
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                return;
            }
            if(listBox2 != null)
                listBox2.Items.Clear();
            adapter = new SqlDataAdapter("select name from MainTable where Calories <= " +
                textBox4.Text + " group by name", conn);
            set = new DataSet();
            adapter.Fill(set);
            int count = set.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                listBox2.Items.Add(set.Tables[0].Rows[i][0].ToString());
            }            
        }


        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            if (textBox5.Text == "")
            {
                listBox2.Items.Clear();
                textBox4.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                return;
            }
            if (listBox2 != null)
                listBox2.Items.Clear();
            adapter = new SqlDataAdapter("select name from MainTable where Calories >= " +
                textBox5.Text + " group by name", conn);
            set = new DataSet();
            adapter.Fill(set);
            int count = set.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                listBox2.Items.Add(set.Tables[0].Rows[i][0].ToString());
            }
        }

        private void CaloriesValueBetween()
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            if (textBox6.Text == "" || textBox7.Text == "")
            {
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                listBox2.Items.Clear();
                return;
            }
            if (!textBox7.Text.Equals(""))
            {
                adapter = new SqlDataAdapter("select * from MainTable where Calories between "
                    + textBox6.Text + " and " + textBox7.Text, conn);
                set = new DataSet();
                adapter.Fill(set);
                int count = set.Tables[0].Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    listBox2.Items.Add(set.Tables[0].Rows[i][0].ToString());
                }
            }

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            if (textBox7.Text == "" || textBox6.Text == "")
            {
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                listBox2.Items.Clear();
                return;
            }
            if (!textBox6.Text.Equals(""))
            {
                adapter = new SqlDataAdapter("select name from MainTable where Calories between "
                    + textBox6.Text + " and " + textBox7.Text, conn);
                set = new DataSet();
                adapter.Fill(set);
                int count = set.Tables[0].Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    listBox2.Items.Add(set.Tables[0].Rows[i][0].ToString());
                }
            }
        }
    }
}
