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
using System.Text.RegularExpressions;

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
            string query = @"Select id_journal, dateOfIssue as Дата_выдачи, returnDatePlan as Дата_возврата_п, returnDateFact as Дата_возврата_ф, book.name as Книга, client.fio as Клиент,librarian.fio as Библиотекарь, article as Артикул
                                from journal, client, book, librarian
                                where journal.client_id=client.id_client and journal.book_id=book.id_book and journal.librarian_id=librarian.id_librarian and journal.deleted=0 and journal.filial="+Filial;
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataAdapter dataadapter = new MySqlDataAdapter(command);           
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;
        }
        MySqlConnection conn;
        int Filial;
        private void Main_Load(object sender, EventArgs e)
        {
            auth = (Authorization)Application.OpenForms[0];
            conn = auth.conn;
            Filial = auth.Filial;
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        void GetBook(ComboBox cmb)
        {
            string query = "Select id_book, name From book where deleted='0' and filial=" + Filial;
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataAdapter dataadapter = new MySqlDataAdapter(command);
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            cmb.DataSource = dt;
            cmb.ValueMember = dt.Columns[0].ToString();
            cmb.DisplayMember = dt.Columns[1].ToString();
        }
        void GetClient(ComboBox cmb)
        {
            string query = "Select id_client, fio From client where deleted='0' and filial=" + Filial;
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataAdapter dataadapter = new MySqlDataAdapter(command);
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            cmb.DataSource = dt;
            cmb.ValueMember = dt.Columns[0].ToString();
            cmb.DisplayMember = dt.Columns[1].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;
            checkBox1.Checked = true;
            GetBook(comboBox4);
            GetClient(comboBox3);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox1.Visible = !true;
        }
        
        private void button8_Click(object sender, EventArgs e)
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
            book.dataGridView1.Click += (sender1, e1) =>
            {
                this.comboBox1.SelectedValue = book.dataGridView1.CurrentRow.Cells[0].Value;
            };
        }

        private void button5_Click(object sender, EventArgs e)
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
            client.dataGridView1.Click += (sender1, e1) =>
            {
                this.comboBox3.SelectedValue = client.dataGridView1.CurrentRow.Cells[0].Value;
            };
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                string Article = textBox1.Text;
                Article = Article.Trim(new char[] { ' ' });
                Article = Regex.Replace(Article, @"\s+", " ");              
                if (Article.Length == 0)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    
                    DateTime date1 = dateTimePicker6.Value;
                    string date11 = date1.ToString("yyyy-MM-dd");
                    DateTime date2 = dateTimePicker5.Value;
                    string date22 = date2.ToString("yyyy-MM-dd");
                    DateTime date3=DateTime.Now;
                    string date33 = "NULL";
                    if (checkBox1.Checked != true)
                    {
                       date3 = dateTimePicker4.Value;
                       date33 = date3.ToString("yyyy-MM-dd");
                    }                       
                    
                    string query;
                    if (checkBox1.Checked == true)
                        query = "Insert Into journal Values(uuid(),'" + date11 + "','" + date22 + "',NULL,"+Filial+",'" + comboBox4.SelectedValue + "','" + comboBox3.SelectedValue + "',1,'Admin',CURDATE(),'Admin',CURDATE(),'0','"+ Article + "')";
                    else
                        query = "Insert Into journal Values(uuid(),'" + date11 + "','" + date22 + "','"+date33+"',"+Filial+",'" + comboBox4.SelectedValue + "','" + comboBox3.SelectedValue + "',1,'Admin',CURDATE(),'Admin',CURDATE(),'0','" + Article + "')";
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
                }
            }

                      
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.RowCount != 0)
                {
                    if (dataGridView1.SelectedRows.Count != 0)
                    {
                        string query = "UPDATE journal SET deleted='1',editedBy='Admin', editDate=CURDATE() WHERE id_journal=\"" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "\"";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        dataGridView1.DataSource = Visualisation();
                        MessageBox.Show("Запись удалена!");
                    }
                }
            }

            catch
            {
                MessageBox.Show("Невозможно удалить,так как в таблице Сотрудники имеются связаннные записи!");
            }
        }
        int RedIndex;
        private void button2_Click(object sender, EventArgs e)
        {           
            string query = @"Select dateOfIssue, returnDatePlan, returnDateFact, book_id, client_id, article
                                from journal, client, book, librarian
                                where journal.client_id=client.id_client and journal.book_id=book.id_book and journal.librarian_id=librarian.id_librarian
                                and journal.id_journal='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            GetBook(comboBox1);
            GetClient(comboBox2);
            dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[0][0].ToString());
            dateTimePicker2.Value = Convert.ToDateTime(dt.Rows[0][1].ToString());
           
            if (dt.Rows[0][2].ToString()=="")
            {
                checkBox2.Checked = true;
            }
            else
            {
                dateTimePicker3.Value = Convert.ToDateTime(dt.Rows[0][2].ToString());
            }           
            comboBox1.SelectedValue = dt.Rows[0][3];
            comboBox2.SelectedValue = dt.Rows[0][4];
            textBox2.Text = dt.Rows[0][5].ToString();
            RedIndex = dataGridView1.CurrentRow.Index;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Visible = false;
            groupBox2.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox2.Visible = !true;
            checkBox2.Checked = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {           
            if (textBox2.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                string Article = textBox2.Text;
                Article = Article.Trim(new char[] { ' ' });
                Article = Regex.Replace(Article, @"\s+", " ");
                if (Article.Length == 0)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    DateTime date1 = dateTimePicker1.Value;
                    string date11 = date1.ToString("yyyy-MM-dd");
                    DateTime date2 = dateTimePicker2.Value;
                    string date22 = date2.ToString("yyyy-MM-dd");
                    DateTime date3 = DateTime.Now;
                    string date33 = "NULL";
                    if (checkBox1.Checked != true)
                    {
                        date3 = dateTimePicker3.Value;
                        date33 = date3.ToString("yyyy-MM-dd");
                    }

                    string query;
                    if (checkBox2.Checked == true)
                         query = "UPDATE journal SET dateOfIssue='" + date11 + "',returnDatePlan='" + date22 + "',returnDateFact=NULL,book_id='"+comboBox1.SelectedValue+"',client_id='"+comboBox2.SelectedValue+"',article='"+Article+"',editedBy='Admin',editDate=CURDATE() WHERE id_journal='" + dataGridView1[0, RedIndex].Value.ToString() + "'";
                       
                    else
                        query = "UPDATE journal SET dateOfIssue='" + date11 + "',returnDatePlan='" + date22 + "',returnDateFact='"+date33+"',book_id='" + comboBox1.SelectedValue + "',client_id='" + comboBox2.SelectedValue + "',article='" + Article + "',editedBy='Admin',editDate=CURDATE() WHERE id_journal='" + dataGridView1[0, RedIndex].Value.ToString() + "'";

                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    dataGridView1.DataSource = Visualisation();
                    dataGridView1.CurrentCell = dataGridView1[1, RedIndex];
                    dataGridView1.Rows[RedIndex].Selected = true;
                    button1.Enabled = !false;
                    button2.Enabled = !false;
                    button3.Enabled = !false;
                    dataGridView1.Visible = !false;
                    groupBox2.Visible = !true;
                    textBox2.Text = "";
                    checkBox2.Checked = false;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
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
            book.dataGridView1.Click += (sender1, e1) =>
            {
                this.comboBox4.SelectedValue = book.dataGridView1.CurrentRow.Cells[0].Value;
            };
        }

        private void button4_Click(object sender, EventArgs e)
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
            client.dataGridView1.Click += (sender1, e1) =>
            {
                this.comboBox2.SelectedValue = client.dataGridView1.CurrentRow.Cells[0].Value;
            };
        }
    }
}
