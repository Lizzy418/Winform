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
    public partial class Delete : Form
    {
        public Delete()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static string sID;
        private void Delete_Load(object sender, EventArgs e)
        {
            lbID.Text = sID;
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
