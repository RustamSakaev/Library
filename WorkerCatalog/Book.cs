using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace WorkerCatalog
{
    public partial class Book : Form
    {
        public Book()
        {
            InitializeComponent();
        }
        MySqlConnection conn;
        DataTable Visualisation()
        {
            string query = @"Select id_book, book.name as Наименование, quantity as Колво, publisher.name as Издательство
                                From book, publisher
                               WHERE book.publisher_id=publisher.id_publisher and book.deleted='0' and filial=" + auth.Filial;
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataAdapter dataadapter = new MySqlDataAdapter(command);
            
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;
        }
        Authorization auth;
        int Filial;
        private void Filial_Load(object sender, EventArgs e)
        {
            auth = (Authorization)Application.OpenForms[0];
            conn = auth.conn;
            Filial = auth.Filial;
            dataGridView1.DataSource = Visualisation();
            dataGridView1.Columns[0].Visible = false;

        }

        void GetPublisher(ComboBox cmb)
        {
            string query = "Select id_publisher, name as Наименование From publisher where deleted='0'";
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
            GetPublisher(comboBox3);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox2.Visible = !true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox1.Visible = !true;
        }

        private void Filial_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Visible==false)
            {
                if (groupBox1.Visible ==true)
                {
                    MessageBox.Show("Вы не можете закрыть форму в режиме добавления данных!");
                }
                if (groupBox2.Visible==true)
                    MessageBox.Show("Вы не можете закрыть форму в режиме добавления данных!");
                e.Cancel = true;
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
                        string query = "UPDATE book SET deleted='1',editedBy='Admin', editDate=CURDATE() WHERE id_book=\"" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "\"";
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                string Name = textBox1.Text;
                Name = Name.Trim(new char[] { ' ' });
                Name = Regex.Replace(Name, @"\s+", " ");

                int quantity =Convert.ToInt32(numericUpDown1.Value);
                if (Name.Length == 0)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    string query = "Insert Into book Values(uuid(),'"+Name+"','"+quantity+"','"+comboBox3.SelectedValue+ "',"+Filial+",'Admin',CURDATE(),'Admin',CURDATE(),'0')";
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

        int RedIndex;
        private void button2_Click(object sender, EventArgs e)
        {
            string query = @"Select book.name, quantity, id_publisher from libre.book, libre.publisher
                             where book.publisher_id=publisher.id_publisher and book.id_book='" + dataGridView1.CurrentRow.Cells[0].Value.ToString()+"'";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataAdapter da = new MySqlDataAdapter(command);            
            DataTable dt = new DataTable();
            da.Fill(dt);
            GetPublisher(comboBox1);
            textBox2.Text = dt.Rows[0][0].ToString();
            numericUpDown2.Value = Convert.ToInt32(dt.Rows[0][1]);
            comboBox1.SelectedValue = dt.Rows[0][2];

            RedIndex = dataGridView1.CurrentRow.Index;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Visible = false;
            groupBox2.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                string Name = textBox2.Text;
                Name = Name.Trim(new char[] { ' ' });
                Name = Regex.Replace(Name, @"\s+", " ");
                int quantity = Convert.ToInt32(numericUpDown2.Value);
                if (Name.Length == 0)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {                    
                    string query = "UPDATE book SET name='" + Name + "',quantity='" + quantity + "',publisher_id='" + comboBox1.SelectedValue + "',editedBy='Admin',editDate=CURDATE() WHERE id_book='" + dataGridView1[0, RedIndex].Value.ToString()+"'";
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
                    numericUpDown2.Value = 1;
                }
            }
        }
        bool IsFormOpened<TForm>() where TForm : Form
        {
            return Application.OpenForms.OfType<TForm>().Any();
        }
        Main main;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsFormOpened<Main>())
            {
                this.Close();
                main = (Main)Application.OpenForms["Main"];
                main.Focus();
            }
        }
        Publisher publisher;
        private void button8_Click(object sender, EventArgs e)
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
            publisher.dataGridView1.Click += (sender1, e1) =>
            {
                this.comboBox3.SelectedValue = publisher.dataGridView1.CurrentRow.Cells[0].Value;
            };
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
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
            publisher.dataGridView1.Click += (sender1, e1) =>
            {
                this.comboBox1.SelectedValue = publisher.dataGridView1.CurrentRow.Cells[0].Value;
            };
        }
    }
}
