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
    public partial class AVG : Form
    {
        public AVG()
        {
            InitializeComponent();
        }
        Authorization auth;
        DataTable GetReport()
        {
            string query = @"select Worker.FullName, Worker.ZP, Worker.DateStartWork
from Worker
Where Worker.DateStartWork BETWEEN '"+dateTimePicker1.Value.ToShortDateString()+"' AND '"+dateTimePicker2.Value.ToShortDateString()+"'";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataAdapter dataadapter = new SqlDataAdapter(command);
            SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(dataadapter);
            DataTable dt = new DataTable();
            dataadapter.Fill(dt);
            return dt;

        }
        SqlConnection conn;
        private void AVG_Load(object sender, EventArgs e)
        {
            auth = (Authorization)Application.OpenForms[0];
            conn = auth.conn;
        }
        void ReplaceExcel(Excel._Application app, string Find, string Replace)
        {
            Excel.Range curr = null;
            Excel.Range range = app.Range["A1", "X50"];
            curr = range.Find(Find);
            if (curr.Value != null)
                curr.Value = Replace;
        }
        Excel.Worksheet sheet;
        Excel._Application app;
        Excel.Workbook wb;
        private void button1_Click(object sender, EventArgs e)
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

            ReplaceExcel(app, "#Date#", "Дата формирования: " + DateTime.Now.ToShortDateString());
            Excel.Range range;
            range = app.Range["E1", Type.Missing];
            range.Value2 = "ОТЧЕТ ПО СРЕДНЕЙ ЗП ПО ВСЕМ ФИЛИАЛАМ";
            range.WrapText = true;
            range.Font.Bold = true;
            range.ColumnWidth = 50;
            range.EntireRow.AutoFit();

            range = app.Range["A3", "D3"];
            range.HorizontalAlignment = Excel.Constants.xlCenter;

            range = app.Range["A3", Type.Missing];
            range.Value2 = "#";
            range.EntireColumn.AutoFit();

            range = app.Range["B3", Type.Missing];
            range.Value2 = "Сотрудник";
            range.ColumnWidth = 15;

            range = app.Range["C3", Type.Missing];
            range.Value2 = "ЗП";
            range.ColumnWidth = 10;

            range = app.Range["D3", Type.Missing];
            range.Value2 = "Дата приема";
            range.EntireColumn.AutoFit();

            app.Visible = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 1; j < dt.Columns.Count + 1; j++)
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

            //Excel.ChartObjects chartobjects = (Excel.ChartObjects)sheet.ChartObjects(Type.Missing);
            //Excel.ChartObject chibject = chartobjects.Add(150, 50, 300, 200);
            //Excel.Chart mychart = (Excel.Chart)chibject.Chart;
            //mychart.HasTitle = true;
            //mychart.HasTitle = true;
            //mychart.ChartTitle.Text = "Средняя ЗП по всем филиалам";
            //Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)mychart.SeriesCollection(Type.Missing);
            //Excel.Series series = seriesCollection.NewSeries();

            //series.Values = app.Range["C4", "C" + (dt.Rows.Count + 3)];
            //series.XValues = app.Range["B4", "B" + (dt.Rows.Count + 3)];
            //mychart.ChartType = Excel.XlChartType.xlBarStacked;

        }
    }
}
