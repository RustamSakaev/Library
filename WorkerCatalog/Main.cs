using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WorkerCatalog
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        bool IsFormOpened<TForm>() where TForm:Form
        {
            return Application.OpenForms.OfType<TForm>().Any();
        }

        Authorization auth;
        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            auth = (Authorization)Application.OpenForms[0];
            auth.Visible = true;
            this.Visible=false;
        }
        
        Book book;       
        Publisher publisher;       
        Client client;
       
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;
            if (Application.OpenForms.Count > 2)
            {
                result = MessageBox.Show("Вы уверены,что хотите выйти? Все несохраненные данные будут утеряны", "Внимание", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
                Application.Exit();
        }
        DataTable Visualisation()
        {
            string query = @"Select id_journal, dateOfIssue, returnDatePlan, returnDateFact, book_id, client_id,librarian_id
                                from journal";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataAdapter dataadapter = new MySqlDataAdapter(command);           
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;
        }
        MySqlConnection conn;
        private void Main_Load(object sender, EventArgs e)
        {
            auth = (Authorization)Application.OpenForms[0];
            conn = auth.conn;
            dataGridView1.DataSource = Visualisation();
            dataGridView1.Columns[0].Visible = false;
        }
          

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsFormOpened<Client>())
            {
                client = new Client();
                client.Show();
            }
            else
            {
                client = (Client)Application.OpenForms["Client"];
                client.Focus();
            }
        }

        private void издательстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsFormOpened<Publisher>())
            {
                publisher = new Publisher();
                publisher.Show();
            }
            else
            {
                publisher = (Publisher)Application.OpenForms["Publisher"];
                publisher.Focus();
            }
        }

        private void книгиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsFormOpened<Book>())
            {
                book = new Book();
                book.Show();
            }
            else
            {
                book = (Book)Application.OpenForms["Book"];
                book.Focus();
            }
        }
    }
}
