using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TSMC_Employee_Query
{
    public partial class Form1 : Form
    {
        clsJMY jmy = new clsJMY();
        public string ta { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            dataGridView1.DataSource = jmy.GetDataTable("select top 1000 * from Emp");
        }

        private void btnquiry_Click(object sender, EventArgs e)//搜尋
        {
            string textBox1 = "ta";
            try
            {
                dataGridView1.DataSource = jmy.GetDataTable("select * from Emp where Email like '%" + ta + "%' or phone like '%" 
                 + ta + "%'or Name_TW like '%" + ta + "%' or Name_EN like '%" + ta + "%'");
                toolStripStatusLabel1.Text = dataGridView1.Rows.Count + "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("");
            }

        }

         private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.ToString();
        }
        private void btnExit_Click(object sender, EventArgs e)//離開
        {
            Application.Exit();
        }

        private void btnsave_Click(object sender, EventArgs e)//保存
        {
            string update = string.Format("update 資料表 set 欄位1='{0}', 欄位2='{1}'");
            
            
        }


        
    }
}
