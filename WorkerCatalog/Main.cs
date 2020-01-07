using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
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
        Filial filial;
        private void филиалыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsFormOpened<Filial>())
            {
                filial = new Filial();
                filial.Show();
            }
            else
            {
                filial = (Filial)Application.OpenForms["Filial"];
                filial.Focus();
            }
        }
        Post post;
        private void квалификацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsFormOpened<Post>())
            {
                post = new Post();
                post.Show();
            }
            else
            {
                post = (Post)Application.OpenForms["Post"];
                post.Focus();
            }
        }
        Worker worker;
        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsFormOpened<Worker>())
            {
                worker = new Worker();
                worker.Show();
            }
            else
            {
                worker = (Worker)Application.OpenForms["Worker"];
                worker.Focus();
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult result;
            //if (Application.OpenForms.Count > 2)
            //{
            //    result = MessageBox.Show("Вы уверены,что хотите выйти? Все несохраненные данные будут утеряны", "Внимание", MessageBoxButtons.OKCancel);
            //    if (result == DialogResult.OK)
            //    {
            //        Application.Exit();
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //    }

            //}
            //else
                Application.Exit();
        }
        DataTable Visualisation()
        {
            string query = @"Select ID_Worker, FullName as ФИО,Filial.Name as [Филиал], Post.Name as [Квалификация]
                        from Worker, Post,Filial WHERE Worker.Post_ID=Post.ID_Post and Worker.Filial_ID=Filial.ID_Filial";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataAdapter dataadapter = new SqlDataAdapter(command);
            SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(dataadapter);
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;
        }
        SqlConnection conn;
        private void Main_Load(object sender, EventArgs e)
        {
            //auth = (Authorization)Application.OpenForms[0];
            //conn = auth.conn;
            //dataGridView1.DataSource = Visualisation();
            //dataGridView1.Columns[0].Visible = false;
        }
        DataTable GetReport()
        {
            string query = @"select Filial.Name,ROUND(AVG(Worker.ZP), 2)
            from Worker, Filial
            Where Worker.Filial_ID = Filial.ID_Filial
            Group by Filial.Name";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataAdapter dataadapter = new SqlDataAdapter(command);
            SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(dataadapter);
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;

        }
        void ReplaceExcel(Excel._Application app, string Find, string Replace)
        {
            Excel.Range curr = null;
            Excel.Range range = app.Range ["A1", "X50"];
            curr = range.Find(Find);
            if (curr.Value != null)
                curr.Value = Replace;
        }
        Excel.Worksheet sheet;
        Excel._Application app;
        Excel.Workbook wb;
        private void средняяЗПToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process[] exproc = Process.GetProcessesByName("EXCEL");
                foreach (Process proc in exproc)
            {
                proc.Kill();
            }
            object path = @"C:\Users\Рустам\Desktop\Report.xlsx";
            app = new Excel.Application();
            wb = app.Workbooks.Add(path);            
          sheet = (Excel.Worksheet)app.ActiveSheet;
            DataTable dt = GetReport();

            ReplaceExcel(app, "#Date#", "Дата формирования: "+DateTime.Now.ToShortDateString());
            Excel.Range range;
            range = app.Range["D1", Type.Missing];
            range.Value2 = "ОТЧЕТ ПО СРЕДНЕЙ ЗП ПО ВСЕМ ФИЛИАЛАМ";
            range.WrapText = true;
            range.Font.Bold = true;
            range.ColumnWidth=50;
            range.EntireRow.AutoFit();

            range = app.Range["A3", Type.Missing];
            range.Value2 = "#";
            range.EntireColumn.AutoFit();
            
            range = app.Range["B3", Type.Missing];
            range.Value2 = "Филиал";

            range = app.Range["C3", Type.Missing];
            range.Value2 = "Средняя ЗП";            
            range.EntireColumn.AutoFit();
                  
            app.Visible = true;
            for (int i=0;i<dt.Rows.Count;i++)
            {               
                    for (int j = 1; j < dt.Columns.Count+1; j++)
                    {
                        sheet.Cells[i + 4, 1] = i + 1;
                    try
                    {
                        double zp = Math.Round(Convert.ToDouble(dt.Rows[i][j - 1]), 2);
                        sheet.Cells[i + 4, j + 1] = zp;
                    }
                    catch
                    {
                        sheet.Cells[i + 4, j + 1] = dt.Rows[i][j - 1];
                    }
                       
                    }                
            }

            Excel.ChartObjects chartobjects = (Excel.ChartObjects)sheet.ChartObjects(Type.Missing);
            Excel.ChartObject chibject = chartobjects.Add(150, 50, 300, 200);
            Excel.Chart mychart = (Excel.Chart)chibject.Chart;
            mychart.HasTitle = true;
            mychart.HasTitle = true;
            mychart.ChartTitle.Text = "Средняя ЗП по всем филиалам";            
            Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)mychart.SeriesCollection(Type.Missing);
            Excel.Series series = seriesCollection.NewSeries();
            
            series.Values = app.Range["C4", "C" + (dt.Rows.Count+3)];
            series.XValues = app.Range["B4", "B" + (dt.Rows.Count+3)];
            mychart.ChartType = Excel.XlChartType.xlBarStacked;

        }

        private void заПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AVG avg = new AVG();
            avg.Show();
        }
    }
}
