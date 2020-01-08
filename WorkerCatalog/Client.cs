using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace WorkerCatalog
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        int ToInt(string value)
        {
            return Convert.ToInt32(value);
        }
        DataTable Visualisation()
        {
            string query = @"Select id_client, fio as ФИО, phoneNumber as Телефон                        
                            from client where deleted=0";
            MySqlCommand command = new MySqlCommand(query, conn);           
            MySqlDataAdapter dataadapter = new MySqlDataAdapter(command);
            
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;
        }
        Authorization auth;
        MySqlConnection conn;
        private void Worker_Load(object sender, EventArgs e)
        {
            auth = (Authorization)Application.OpenForms[0];
            conn = auth.conn;
            dataGridView1.DataSource = Visualisation();
            dataGridView1.Columns[0].Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.RowCount != 0)
            //{
            //    if (dataGridView1.SelectedRows.Count != 0)
            //    {
            //        string query = "DELETE FROM Worker where ID_Worker=" + ToInt(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            //        SqlCommand command = conn.CreateCommand();
            //        command.CommandText = query;
            //        command.ExecuteNonQuery();
            //        dataGridView1.DataSource = Visualisation();
            //        MessageBox.Show("Запись удалена!");
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;

            //GetPost(comboBox2);
            //GetFilial(comboBox1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox1.Visible = !true;
        }
        void GetFilial(ComboBox cmb)
        {
            //string query = "Select ID_Filial, Name as Наименование From Filial";
            //SqlCommand command = new SqlCommand(query, conn);
            //SqlDataAdapter dataadapter = new SqlDataAdapter(command);
            //SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(dataadapter);
            //DataTable dt = new DataTable();
            //dataadapter.Fill(dt);
            //cmb.DataSource = dt;
            //cmb.ValueMember = dt.Columns[0].ToString();
            //cmb.DisplayMember = dt.Columns[1].ToString();

        }

        void GetPost(ComboBox cmb)
        {
            //string query = "Select ID_Post, Name as Наименование From Post";
            //SqlCommand command = new SqlCommand(query, conn);
            //SqlDataAdapter dataadapter = new SqlDataAdapter(command);
            //SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(dataadapter);
            //DataTable dt = new DataTable();
            //dataadapter.Fill(dt);
            //cmb.DataSource = dt;
            //cmb.ValueMember = dt.Columns[0].ToString();
            //cmb.DisplayMember = dt.Columns[1].ToString();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                string Name = textBox1.Text;
                Name = Name.Trim(new char[] { ' ' });
                Name = Regex.Replace(Name, @"\s+", " ");

                string Phone = textBox2.Text;
                Phone = Phone.Trim(new char[] { ' ' });
                Phone = Regex.Replace(Phone, @"\s+", " ");            

                if (Name.Length == 0 || Phone.Length == 0 )
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    string query = "Insert into libre.client values(uuid(),'" + Name + "','"+Phone+"','1','Admin',CURDATE(),'Admin',CURDATE(),'0');";
                    MySqlCommand command = new MySqlCommand(query, conn);                                      
                    command.ExecuteNonQuery();
                    dataGridView1.DataSource = Visualisation();
                    dataGridView1.CurrentCell = dataGridView1[1, dataGridView1.RowCount - 1];
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                    button1.Enabled = !false;
                    button2.Enabled = !false;
                    button3.Enabled = !false;
                    dataGridView1.Visible = !false;
                    groupBox1.Visible = !true;
                    textBox1.Text = "";
                    textBox2.Text = "";                   
                }
            }
        }
        int RedIndex;
        private void button2_Click(object sender, EventArgs e)
        {
            //string query = @"Select FullName, Series,Number, WhoG, WhenG, ZP as[ЗП],
            //            DateStartWork as [Дата приема],ID_Filial, ID_Post
            //            from Worker, Post,Filial WHERE Worker.Post_ID=Post.ID_Post and Worker.Filial_ID=Filial.ID_Filial and Worker.ID_Worker="+ToInt(dataGridView1.CurrentRow.Cells[0].Value.ToString());
      
            //SqlCommand command = new SqlCommand(query, conn);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //SqlCommandBuilder cb = new SqlCommandBuilder(da);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //GetPost(comboBox4);
            //GetFilial(comboBox3);
            //textBox6.Text = dt.Rows[0][0].ToString();
            //textBox7.Text = dt.Rows[0][1].ToString();
            //textBox8.Text = dt.Rows[0][2].ToString();
            //textBox9.Text = dt.Rows[0][3].ToString();
            //textBox10.Text = dt.Rows[0][5].ToString();
            //dateTimePicker3.Value = Convert.ToDateTime(dt.Rows[0][4]);
            //dateTimePicker4.Value = Convert.ToDateTime(dt.Rows[0][6]);
            //comboBox3.SelectedValue = dt.Rows[0][7];
            //comboBox4.SelectedValue = dt.Rows[0][8];

            //RedIndex = dataGridView1.CurrentRow.Index;
            //button1.Enabled = false;
            //button2.Enabled = false;
            //button3.Enabled = false;
            //dataGridView1.Visible = false;
            //groupBox2.Visible = true;
           
        }

        private void button11_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox2.Visible = !true;
            textBox1.Text = "";
            textBox2.Text = "";          
        }

        bool IsFormOpened<TForm>() where TForm : Form
        {
            return Application.OpenForms.OfType<TForm>().Any();
        }
         
        private void button10_Click(object sender, EventArgs e)
        {
            //if (textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "")
            //{
            //    MessageBox.Show("Заполните все поля!");
            //}
            //else
            //{
            //    string Name = textBox6.Text;
            //    Name = Name.Trim(new char[] { ' ' });
            //    Name = Regex.Replace(Name, @"\s+", " ");

            //    string Series = textBox7.Text;
            //    Series = Series.Trim(new char[] { ' ' });
            //    Series = Regex.Replace(Series, @"\s+", " ");

            //    string Number = textBox8.Text;
            //    Number = Number.Trim(new char[] { ' ' });
            //    Number = Regex.Replace(Number, @"\s+", " ");

            //    string Whog = textBox9.Text;
            //    Whog = Whog.Trim(new char[] { ' ' });
            //    Whog = Regex.Replace(Whog, @"\s+", " ");

            //    string ZP = textBox10.Text;
            //    ZP = ZP.Trim(new char[] { ' ' });
            //    ZP = Regex.Replace(ZP, @"\s+", " ");

            //    DateTime whenG = dateTimePicker3.Value;
            //    DateTime WorkStart = dateTimePicker4.Value;

            //    if (Name.Length == 0 || Series.Length == 0 || Number.Length == 0 || Whog.Length == 0 || ZP.Length == 0)
            //    {
            //        MessageBox.Show("Заполните все поля!");
            //    }
            //    else
            //    {
            //        string query = "Update Worker set FullName='" + Name + "',Series='" + Series + "',Number='" + Number + "',WhoG='" + Whog + "',WhenG='" + whenG.ToShortDateString() + "',ZP='" + ZP + "',DateStartWork='" + WorkStart.ToShortDateString() + "',Filial_ID='" + comboBox3.SelectedValue + "',Post_ID='" + comboBox4.SelectedValue + "' WHERE ID_Worker="+ ToInt(dataGridView1[0, RedIndex].Value.ToString());
   
            //        SqlCommand command = conn.CreateCommand();
            //        command.CommandText = query;
            //        command.ExecuteNonQuery();
            //        dataGridView1.DataSource = Visualisation();
            //        dataGridView1.CurrentCell = dataGridView1[1, RedIndex];
            //        dataGridView1.Rows[RedIndex].Selected = true;
            //        button1.Enabled = !false;
            //        button2.Enabled = !false;
            //        button3.Enabled = !false;
            //        dataGridView1.Visible = !false;
            //        groupBox2.Visible = !true;
            //        textBox1.Text = "";
            //        textBox2.Text = "";
            //        textBox3.Text = "";
            //        textBox4.Text = "";
            //        textBox5.Text = "";
            //        dateTimePicker1.Value = DateTime.Now;
            //        dateTimePicker2.Value = DateTime.Now;
            //    }
            //}
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
    }
}
