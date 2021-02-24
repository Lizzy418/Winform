using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Icecream_order
{
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }

        //주문 내용 저장 변수들
        public static int tasteCount;
        public static string sID, sDate, sStore, sTime, sSize, sPrice, sPayment = "Card";

        private void rbCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCash.Checked) sPayment = "Cash";
        }

        private void rbCard_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCard.Checked) sPayment = "Card";
        }

        public static string[] sTaste = new string[7];
        int i;
        private void Order_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            lbID.Text = sID;
            lbDate.Text = sDate;
            lbStore.Text = sStore;
            lbTime.Text = sTime;
            lbSize.Text = sSize;
            for(i = 0; i<tasteCount; i++)
            {
                lbTaste.Text += sTaste[i] + "\r\n";
            }
            switch (tasteCount)
            {
                case 1: sPrice = "3,200"; break;
                case 2: sPrice = "4,300"; break;
                case 3: sPrice = "8,200"; break;
                case 4: sPrice = "15,500"; break;
                case 5: sPrice = "22,000"; break;
                case 6: sPrice = "26,500"; break;
            }
            lbPrice.Text = sPrice + "원"; 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        
    }
}
