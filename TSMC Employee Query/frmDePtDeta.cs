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
    public partial class frmDePtDeta : Form
    {
        public string sNO = ""; public string sName = "";
        public string sSeqNo = "";
        public frmDePtDeta()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; //置中
        }
        clsJMY jmy = new clsJMY();
        bool bIsInsert = false;
        private void btnsave_Click(object sender, EventArgs e) //保存
        {
            if (MessageBox.Show("請問是否保存資料!?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                if (txtNameCH.Text == "")
                {
                    MessageBox.Show("請輸入正確值!!");
                    return;
                }

                try
                {
                    if (bIsInsert) // 新增
                    {
                        string str = "SELECT Description_TW FROM Dept WHERE Description_TW = '{0}' and IsDelete  = 'false' "; // 新增判斷是否有重複
                        str = string.Format(str, txtNameCH.Text);
                        DataTable da = jmy.GetDataTable(str);
                        if (da.Rows.Count == 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("已有此筆資料");
                            txtNameCH.Focus();
                            return;
                        }
                        if (da.Rows.Count == 0)
                        {
                            string Str = "INSERT INTO Dept (Description_TW) VALUES('" + txtNameCH.Text.Replace("'", "''") + "')";
                            jmy.execSQL(Str);
                        }
                        else
                        {
                            MessageBox.Show("已有此筆資料");
                            return;
                        }
                        MessageBox.Show("新增成功!!");
                        this.Close();
                        return;
                    }
                    else // 更新
                    {
                        string str = "SELECT Description_TW FROM Dept WHERE  Description_TW = '0' and IsDelete  = 'true' "; //更新判斷是否有重複
                        str = string.Format(str, sName);
                        DataTable da = jmy.GetDataTable(str);
                        if (txtNameCH.Text == sName)
                        {
                            string Str = "update Dept set Description_TW  = '{0}' where SeqNo = {1} ";
                            Str = string.Format(Str, txtNameCH.Text.Replace("'", "''"), sSeqNo);
                            jmy.execSQL(Str);
                            MessageBox.Show("更新成功");
                            this.Close();
                            return;
                        }
                        else
                        {
                            if (txtNameCH.Text == sName)
                            {
                                MessageBox.Show("更新成功");

                            }
                            else
                            {
                                if (txtNameCH.Text == sName)
                                {
                                    MessageBox.Show("已有此筆資料");
                                    txtNameCH.Focus();
                                    this.Close();
                                    return;
                                }
                                else
                                {

                                }
                            }
                        }
                        if (da.Rows.Count == 0)
                        {

                            string Str = "update Dept set Description_TW  = '{0}' where SeqNo = {1}";
                            Str = string.Format(Str, txtNameCH.Text.Replace("'", "''"), sSeqNo);
                            jmy.execSQL(Str);
                            this.Close();
                            return;
                        }
                        else
                        {
                            MessageBox.Show("已有此筆資料");
                            this.Close();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "資料錯誤");
                }
            }
        }

        private void frmDePtDeta_Load(object sender, EventArgs e)
        {
            this.Text = "新增資料";
            if (String.IsNullOrEmpty(sSeqNo))
            {
                bIsInsert = true;
            }
            else
            {
                bIsInsert = false;
            }
            txtNameCH.Text = sName;
            jmy.getConnect();

            ToolTip tip = new ToolTip();
            tip.SetToolTip(txtNameCH, "請輸入你的單位");
            tip.ToolTipIcon = ToolTipIcon.Info;
            tip.ForeColor = Color.Blue;
            tip.BackColor = Color.Green;
            tip.AutoPopDelay = 5000;
            tip.ToolTipTitle = "提示";
        }
        protected override bool ProcessDialogKey(Keys keyData)//按下enter 後立即觸發button鈕
        {
            if (keyData == Keys.Enter && !this.btnsave.Focused)
                btnsave_Click(null, null);
            {
                return base.ProcessDialogKey(keyData);
            }
        }
    }
}
