using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WorkerCatalog
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void Authorization_Load(object sender, EventArgs e)
        {

        }
        string login, password;
        public int Filial;
        public MySqlConnection conn;
        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn != null)
                conn.Close();
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
           login = textBox1.Text;
           password = textBox2.Text;
            try
            {                
                string ConnectionString = @"server=127.0.0.1;user="+login+";database=libre;password="+password+";OldGuids=True;";
                conn = new MySqlConnection(ConnectionString);               
                conn.Open();

                string query = "Select filial From librarian where login='" + login + "'";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataAdapter dataadapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                dataadapter.Fill(dt);
                Filial = Convert.ToInt32(dt.Rows[0][0]);

                Main main = new Main();
                this.Visible = false;
                main.Show();
                textBox1.Text = "";
                textBox2.Text = "";
                                      
            }
            catch
            {
                MessageBox.Show("Неправильный логин или пароль!");
            }
        }
    }
}
