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

       
        DataTable Visualisation()
        {
            string query = @"Select id_client, fio as ФИО, phoneNumber as Телефон                        
                            from client where deleted=0 and filial="+Filial;
            MySqlCommand command = new MySqlCommand(query, conn);           
            MySqlDataAdapter dataadapter = new MySqlDataAdapter(command);
            
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;
        }
        Authorization auth;
        MySqlConnection conn;
        int Filial;
        private void Worker_Load(object sender, EventArgs e)
        {
            auth = (Authorization)Application.OpenForms[0];
            conn = auth.conn;
            Filial = auth.Filial;
            dataGridView1.DataSource = Visualisation();
            dataGridView1.Columns[0].Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.RowCount != 0)
                {
                    if (dataGridView1.SelectedRows.Count != 0)
                    {
                        string query = "UPDATE client SET deleted='1',editedBy='Admin', editDate=CURDATE() WHERE id_client=\"" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "\"";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        dataGridView1.DataSource = Visualisation();
                        MessageBox.Show("Запись удалена!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить,так как в таблице Журнал имеются связаннные записи!");
            }           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Visible = false;
            groupBox1.Visible = true;            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = !false;
            button2.Enabled = !false;
            button3.Enabled = !false;
            dataGridView1.Visible = !false;
            groupBox1.Visible = !true;
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
                    string query = "Insert into libre.client values(uuid(),'" + Name + "','"+Phone+"',"+Filial+",'Admin',CURDATE(),'Admin',CURDATE(),'0');";
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
            string query = @"Select fio, phoneNumber                        
                        from client WHERE id_client='" + dataGridView1.CurrentRow.Cells[0].Value.ToString()+"'";

            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            
            DataTable dt = new DataTable();
            da.Fill(dt);           
            textBox6.Text = dt.Rows[0][0].ToString();
            textBox7.Text = dt.Rows[0][1].ToString();        
            RedIndex = dataGridView1.CurrentRow.Index;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Visible = false;
            groupBox2.Visible = true;

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
            if (textBox6.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                string Name = textBox6.Text;
                Name = Name.Trim(new char[] { ' ' });
                Name = Regex.Replace(Name, @"\s+", " ");

                string Phone = textBox7.Text;
                Phone = Phone.Trim(new char[] { ' ' });
                Phone = Regex.Replace(Phone, @"\s+", " ");                              
                               
                if (Name.Length == 0 || Phone.Length == 0 )
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    string query = "Update client set fio='" + Name + "',phoneNumber='"+Phone+ "',editedBy='Admin', editDate=CURDATE() WHERE id_client='" + dataGridView1[0, RedIndex].Value.ToString()+"'";

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
                    textBox1.Text = "";
                    textBox2.Text = "";                    
                }
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
        Main main;
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsFormOpened<Main>())
            {
                this.Close();
                main = (Main)Application.OpenForms["Main"];
                main.Focus();
            }
        }
    }
}
