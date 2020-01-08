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
        //public SqlConnection conn;
        public MySqlConnection conn;
        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn != null)
                conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // login = textBox1.Text;
           // password = textBox2.Text;
            try
            {

                //string ConnectionString = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=WorkerCatalog; User ID=" + login + ";Password=" + password + "";
                string ConnectionString = @"server=127.0.0.1;user=root;database=libre;password=1596315963;OldGuids=True;";
                 conn = new MySqlConnection(ConnectionString);
                //conn = new SqlConnection(ConnectionString);
                conn.Open();
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
