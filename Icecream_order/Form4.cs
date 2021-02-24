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

        Point fPt;
        bool isMove;
        private void pnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            //눌렀을 때 움직이게
            isMove = true;
            //눌렀을 당시 위치 저장
            fPt = new Point(e.X, e.Y);
        }
        private void pnlTop_MouseUp(object sender, MouseEventArgs e)
        {
            //마우스 떼면 움직이지 않게
            isMove = false;
        }
        private void pnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            //마우스 왼쪽 클릭 시
            if (isMove && (e.Button & MouseButtons.Left) == MouseButtons.Left)
                //폼 위치 재지정
                Location = new Point(this.Left - (fPt.X - e.X), this.Top - (fPt.Y - e.Y));
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
