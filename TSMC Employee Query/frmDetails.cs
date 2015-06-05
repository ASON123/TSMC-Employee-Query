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
    public partial class frmDetails : Form
    {
        public string sNameCH = "", sNameEN = "", sphone = "", sDescription_TW = "";
        public string sEmpID = "", sSeq = "", sSeqNo = "";
        clsJMY jmy = new clsJMY();
        bool bIsInsert = false;
        public frmDetails()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void frmDetails_Load(object sender, EventArgs e)
        {
            this.Text = "新增資料";
            if (String.IsNullOrEmpty(sSeq))
            {
                bIsInsert = true;
            }
            else
            {
                bIsInsert = false;
            }
            tbNameCH.Text = sNameCH;
            tbNameEN.Text = sNameEN;
            tbPhone.Text = sphone;
            jmy.getDBtoCmb("select distinct [Description_TW] from Dept where IsDelete = 'false'", ref comboBox1);
            comboBox1.Items.Insert(0, "");
            comboBox1.Text = sDescription_TW;
            comboBox1.Text = "1";

            ToolTip tip = new ToolTip();//ToolTip：當游標停滯在某個控制項時，就會跳出一個小視窗
            tip.SetToolTip(tbNameCH, "請輸入您的中文姓名");//SetToolTip：定義控制項會跳出提示的文字
            tip.SetToolTip(tbNameEN, "請輸入您的英文姓名");
            tip.SetToolTip(tbPhone, "請輸入您的電話");
            tip.ToolTipIcon = ToolTipIcon.Info;//以下為提示視窗的設定(通常會設定的部分)
            tip.ForeColor = Color.Blue;//ForeColor：顧名思義就是前景顏色
            tip.BackColor = Color.Green;//BackColor：顧名思義就是背景顏色
            tip.AutoPopDelay = 5000;//AutoPopDelay：當游標停滯在控制項，顯示提示視窗的時間。(以毫秒為單位)
            tip.ToolTipTitle = "提示";//ToolTipTitle：設定提示視窗的標題。

        }

        private void btnsave_Click(object sender, EventArgs e)//更新
        {
            if (MessageBox.Show("請問是否保存資料!?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (tbNameCH.Text == "" || tbPhone.Text == "")
                    {
                        MessageBox.Show("請正確輸入");
                        return;
                    }
                    if (bIsInsert)
                    { //保存
                        string sqlStr = "INSERT INTO Emp (Name_TW,Name_EN,DeptSeqNo,Phone)VALUES('" +
                                     tbNameCH.Text.Replace("'", "''") + "','" +
                                     tbNameEN.Text.Replace("'", "''") + "'," +
                                     "(select top 1 SeqNo from Dept where  Description_TW = '" + comboBox1.Text.Replace("'", "''") + "' )" + ",'" + tbPhone.Text.Replace("'", "''") + "')";
                        jmy.execSQL(sqlStr);
                        MessageBox.Show("新增成功!!");
                    }
                    else
                    { //更新
                        string sNametw = tbNameCH.Text.Replace("'", "''");
                        string sNameen = tbNameEN.Text.Replace("'", "''");
                        string sPhone = tbPhone.Text.Replace("'", "''");
                        string sDeptID = "(select top 1 SeqNo from Dept where Description_TW = '" + comboBox1.Text.Replace("'", "''") + "'and Emp.IsDelete = 'false')";
                        string sqlStr = "update Emp set Name_TW = '{0}',Name_EN = '{1}' ,Phone = '{2}' , DeptSeqNo = {3} where SeqNo = '{4}'";
                        sqlStr = string.Format(sqlStr, sNametw, sNameen, sPhone, sDeptID, sSeq);
                        jmy.execSQL(sqlStr);
                        MessageBox.Show("保存成功!!");
                    }
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "保存發生錯誤");
                }
            }
        }
        private class Comboboxltems
        {
            private string sEmpID;
            public Comboboxltems(string sEmpID)
            {
                this.sEmpID = sEmpID;
            }
            public void Comboboxltem(string value, string text)
            {
                Value = value;
                Text = text;
            }
            public string Value
            {
                get;
                set;
            }
            public string Text
            {
                get;
                set;
            }
            public override string ToString()
            {
                return Text;
            }
        }
        private void tbPhone_TextChanged(object sender, EventArgs e)
        {

        }
        protected override bool ProcessDialogKey(Keys keyData) //按下enter 後立即觸發button鈕
        {
            if (keyData == Keys.Enter && !this.btnsave.Focused)
            {
                btnsave_Click(null, null);
            }
            return base.ProcessDialogKey(keyData);
        }

    }
}



