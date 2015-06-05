using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        frmDetails fa = new frmDetails();
        bool bIsInsert = true;
        public string sSeqNo = "";
        public string sNameCH = "", sNameEN = "", sphone = "", sDescription_TW = "";
        public string sEmpID = "", sSeq = "";
        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen; //顯示為在中央
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "TSMC 人員資料管理";
            btnquiry_Click(null, null);//初始顯式表單內容
            textBox1.Focus(); //游標初始所指定到的地方
            this.ActiveControl = this.textBox1;
            
        }
        private void btnquiry_Click(object sender, EventArgs e)//搜尋
        {
            string ta = textBox1.Text.Replace("'", "''"); //警告輸入單引號改為兩個單引號
            try
            {
                dataGridView1.DataSource = jmy.GetDataTable("select  EmpID,Name_TW AS [中文姓名],Name_EN  AS [英文姓名],Phone AS [電話],Description_TW AS [課別],Emp.SeqNo AS [編號]  from  Emp inner join Dept on Emp.DeptSeqNo = Dept.SeqNo where (Email like '%" + ta + "%' or phone like '%" +
                 ta + "%'or Name_TW like '%" + ta + "%' or Name_EN like '%" + ta + "%' or Description_TW like '%" + ta + "%') and Emp.IsDelete = 'false'");
                dataGridView1.Columns["EmpID"].Visible = false; // 值為false 隱藏字段
                dataGridView1.Columns["編號"].Visible = false; // 值為false 隱藏字段
                lblMsg.Text = "共 " + dataGridView1.Rows.Count + " 筆資料";
                if (dataGridView1.Rows.Count > 1)
                {
                    dataGridView1.Rows[0].Selected = true;
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnDel_Click(object sender, EventArgs e)//刪除
        {
            if (MessageBox.Show("請問是否要刪除資料呢!?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {  
                    string sqlStr = "update Emp set IsDelete = 'True' where SeqNo = '{0}' ";
                    string sSeqNo = dataGridView1.SelectedRows[0].Cells["編號"].Value.ToString();
                    sqlStr = String.Format(sqlStr, sSeqNo);
                    jmy.execSQL(sqlStr);
                    MessageBox.Show("刪除成功!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "刪除錯誤!");
                }
                btnquiry_Click(null, null);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)//離開
        {
            if (MessageBox.Show("請問是否要離開此程式!? ", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                Application.Exit();
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)//點兩下觸發事件
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                return;
            }

            frmDetails frm = new frmDetails();

            frm.sNameCH = dataGridView1.Rows[e.RowIndex].Cells["中文姓名"].Value.ToString();
            frm.sNameEN = dataGridView1.Rows[e.RowIndex].Cells["英文姓名"].Value.ToString();
            frm.sphone = dataGridView1.Rows[e.RowIndex].Cells["電話"].Value.ToString();
            frm.sDescription_TW = dataGridView1.Rows[e.RowIndex].Cells["課別"].Value.ToString();
            frm.sSeq = dataGridView1.Rows[e.RowIndex].Cells["編號"].Value.ToString();
            frm.sEmpID = dataGridView1.Rows[e.RowIndex].Cells["EmpID"].Value.ToString();
            frm.ShowDialog();
            btnquiry_Click(null, null);

        }
        public object ta { get; set; }

        private void btnAdd_Click_(object sender, EventArgs e)//新增
        {
            
                frmDetails frm = new frmDetails();
                frm.ShowDialog();
                btnquiry_Click(null, null);
                //dataGridView1.Rows[0].Selected = true;        
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            btnquiry_Click(null, null);
        }

        private void btnDs_Click(object sender, EventArgs e)//課別切換
        {
            frmDeptMaintain fdm = new frmDeptMaintain();
this.Hide();
            fdm.ShowDialog();
            this.Show();
        }

    }
}

