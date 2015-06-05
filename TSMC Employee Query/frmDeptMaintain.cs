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
    public partial class frmDeptMaintain : Form
    {
        public frmDeptMaintain()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;//置中 
        }
        clsJMY jmy = new clsJMY();
        private void btnQuery_Click(object sender, EventArgs e)//查詢
        {
            string ta = tbQuery.Text.Replace("'", "''");
            try
            {
                dataGridView1.DataSource = jmy.GetDataTable("select  DeptNo as [號碼] , SeqNO AS [編號],Description_TW as [部門] from Dept where (SeqNo like  '%" + ta + "%' or Description_TW like '%" + ta + "%') and IsDelete = 'False' ");
                lblMsg.Text = "共 " + dataGridView1.Rows.Count + " 筆資料";
                dataGridView1.Columns["號碼"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void frmDeptMaintain_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";//顯示數值
            btnQuery_Click(null, null);//表單開啟直接顯式資料庫
            this.Text = "課別資料管理";
            tbQuery.Focus();
            this.ActiveControl = this.tbQuery;

          
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)//點兩下跳出可選擇的室窗
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) //防止他人點選其他地方報錯
            {
                return;
            }
            frmDePtDeta frm = new frmDePtDeta();
            frm.sName = dataGridView1.Rows[e.RowIndex].Cells["部門"].Value.ToString();
            frm.sNO = dataGridView1.Rows[e.RowIndex].Cells["號碼"].Value.ToString();
            frm.sSeqNo = dataGridView1.Rows[e.RowIndex].Cells["編號"].Value.ToString();
            frm.ShowDialog();
            btnQuery_Click(null, null); //在查詢一次          
        }
        private void btnAdd_Click(object sender, EventArgs e)//新增
        {
            
                frmDePtDeta frm = new frmDePtDeta();
                frm.ShowDialog();
                btnQuery_Click(null, null); //資料及時更新                 
            
        }
        private void btnDel_Click(object sender, EventArgs e)//刪除
        {
            if (MessageBox.Show("請問是否刪除該資料!?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string strSql = "update Dept set IsDelete = 'True' where SeqNO = '{0}'";
                    string SeqNO = dataGridView1.SelectedRows[0].Cells["編號"].Value.ToString();
                    strSql = String.Format(strSql, SeqNO);
                    jmy.execSQL(strSql);
                    MessageBox.Show("刪除成功!!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "刪除失敗");
                }
                btnQuery_Click(null, null);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)//離開程式的判斷
        {
            if (MessageBox.Show("請問是否要離開此程式!? ", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
            }
        }
        private void tbQuery_TextChanged(object sender, EventArgs e)
        {
            btnQuery_Click(null, null); //在輸入時會即時更新
        }
    }
}
