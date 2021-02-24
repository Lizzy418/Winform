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

namespace Icecream_order
{
    public partial class Main : Form
    {
        
        public Main()
        {
            InitializeComponent();
        }
        //DB 연결
        SqlConnection sConn = new SqlConnection(); //sql 사용하기 위한 class
        SqlCommand sCmd = new SqlCommand();
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ljw45\source\repos\Icecream_order\Icecream_order\IcereamOrder.mdf;Integrated Security=True;Connect Timeout=30";

        //주문 시 DB에 주문정보 삽입
        public int RunSql(string sSql)
        {
            try
            {
                string checkList = $"SELECT ID from tbOrderlist where ID='{userID}'";
                sCmd.CommandText = checkList;
                if ((string)sCmd.ExecuteScalar() != null)
                {
                    MessageBox.Show("이미 주문한 내역이 있습니다. MYORDER를 확인해주세요.");
                }
                else
                {
                    sCmd.CommandText = sSql;
                    sCmd.ExecuteNonQuery();
                    MessageBox.Show("주문이 완료되었습니다! :)");
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("DB에 문제가 있습니다!");
            }
            return 0;
        }
        //로그인 시 아이디 정보 저장할 전역변수
        public string userID;
        //로그인 버튼
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login dlg = new Login();

            DialogResult dr = dlg.ShowDialog(); //호출
            //로그인 폼에서 로그인 완료 시
            if (dr == DialogResult.OK)
            {
                try
                {
                    sConn.ConnectionString = connString;
                    sConn.Open(); //DB 열기
                    sCmd.Connection = sConn;
                }
                catch (Exception e1)
                {
                    MessageBox.Show("DB에 문제가 있습니다!!!");
                }
                MessageBox.Show("로그인 되었습니다!");
                userID = dlg.userID;
                MyOrder.userID = userID;
                btnLogin.Visible = false;
                btnLogout.Visible = true;
                btnJoin.Visible = false;
                btnMyorder.Visible = true;
                lbInform.Text = $"{userID}님 환영합니다!";
                //주문 창으로 바뀌기
                pictureBox1.Visible = false;
                lbSize.Visible = true;
                rbSize1.Visible = true;
                rbSize2.Visible = true;
                rbSize3.Visible = true;
                rbSize4.Visible = true;
                rbSize5.Visible = true;
                rbSize6.Visible = true;
                lbTaste.Visible = true;
                listView1.Visible = true;
                lbOrder.Visible = true;
                btnOrder.Visible = true;
                if (userID == "admin") btnAdmin.Visible = true;
            }
        }
        //회원가입 버튼
        private void btnJoin_Click(object sender, EventArgs e)
        {
            Join dlg = new Join();
            dlg.ShowDialog();
        }
        //로그아웃 버튼
        private void btnLogout_Click(object sender, EventArgs e)
        {
            sConn.Close();
            MessageBox.Show("로그아웃 되었습니다!");
            userID = "";
            btnAdmin.Visible = false;
            btnLogin.Visible = true;
            btnLogout.Visible = false;
            btnJoin.Visible = true;
            btnMyorder.Visible = false;
            lbInform.Text = "로그인 후 이용해주세요.";
            //메인 창으로 바뀌기
            pictureBox1.Visible = true;
            lbSize.Visible = false;
            rbSize1.Visible = false;
            rbSize2.Visible = false;
            rbSize3.Visible = false;
            rbSize4.Visible = false;
            rbSize5.Visible = false;
            rbSize6.Visible = false;
            lbTaste.Visible = false;
            listView1.Visible = false;
            lbOrder.Visible = false;
            btnOrder.Visible = false;
            //주문창 선택 초기화
            cbStore.Text = "";
            cbTime.Text = "";
            dateTimePicker1.Value = DateTime.Today;
            int i;
            if (listView1.CheckedItems.Count != 0)
            {
                int count = listView1.CheckedItems.Count;
                for (i = count - 1; i > -1; i--)
                {
                    listView1.CheckedItems[i].Checked = false;
                }
            }
                
            if(listView1.CheckedItems.Count == 0) rbSize1.Checked = true;
            lbOrder.Text = "지점, 시간, 사이즈, 맛 모두 선택 후 ORDER 버튼을 눌러주세요!";
            lbOrder.BackColor = Color.Empty;
            
        }
        //주문조회 버튼
        private void btnMyorder_Click(object sender, EventArgs e)
        {
            MyOrder dlg = new MyOrder();
            dlg.ShowDialog(); //호출
        }
        
        //주문 버튼
        private void btnOrder_Click(object sender, EventArgs e)
        {
            //선택사항 모두 체크 후 오더버튼 클릭 시 db에 정보 insert
            if (userID != "" && dateTimePicker1.Text != "" && cbStore.Text != "" &&
                cbTime.Text != "" && tasteCount == listView1.CheckedItems.Count)
            {
                lbOrder.BackColor = Color.Empty;
                string sDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                int i;
                string[] sTaste = new string[7];
                for (i = 0; i < tasteCount; i++)
                {
                    sTaste[i] = listView1.CheckedItems[i].Text;
                }

                Order.sID = userID;
                Order.sDate = sDate;
                Order.sStore = cbStore.Text;
                Order.sTime = cbTime.Text;
                Order.sSize = sSize;
                Order.tasteCount = tasteCount;

                for (i = 0; i < tasteCount; i++)
                {
                    Order.sTaste[i] = sTaste[i];
                }

                Order dlg = new Order();
                DialogResult dr = dlg.ShowDialog(); //호출
                //주문 완료 시
                if (dr == DialogResult.OK)
                {
                    string sPrice = Order.sPrice;
                    string sPayment = Order.sPayment;
                    string sInsert = $"INSERT INTO tbOrderlist values('{userID}', '{sDate}', " +
                    $"'{cbStore.Text}', '{cbTime.Text}', '{sSize}', '{sPrice}', '{sPayment}', "+
                    $"'{sTaste[0]}', '{sTaste[1]}', " +
                    $"'{sTaste[2]}','{sTaste[3]}','{sTaste[4]}','{sTaste[5]}')";
                    RunSql(sInsert);
                    cbStore.Text = "";
                    cbTime.Text = "";
                    dateTimePicker1.Value = DateTime.Today;
                    for (i = tasteCount - 1; i > -1; i--)
                    {
                        listView1.CheckedItems[i].Checked = false;
                    }
                    rbSize1.Checked = true; 
                }
            }
            else
            {
                lbOrder.BackColor = Color.Orange;
            }
        }

        //맛 선택 개수 확인 함수
        int tasteCount = 1;
        int tasteCount2 = 1;
        public string sSize = "SINGLE[1]";
        //선택한 개수보다 하나 더 선택하는 경우, 마지막 하나만 선택 취소
        public void CountTaste(int tasteCount)
        {
            if (tasteCount < listView1.CheckedItems.Count)
            {
                int a = listView1.FocusedItem.Index;
                MessageBox.Show($"선택한 사이즈의 맛 개수는 {tasteCount}개 입니다.");
                listView1.Items[a].Checked = false;
            }
        }
        //선택하다가 더 작은 사이즈로 수정하는 경우, 기존 선택된 맛 초기화
        public void CountTaste2(int tasteCount)
        {
            if (tasteCount < listView1.CheckedItems.Count)
            {
                int total = listView1.CheckedItems.Count;
                MessageBox.Show($"선택한 사이즈의 맛 개수는 {tasteCount}개 입니다.");
                foreach (ListViewItem listitem in listView1.Items)
                { if (listitem.Checked == true) listitem.Checked = false; }
            }
        }
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if(tasteCount == tasteCount2) CountTaste(tasteCount); 
        }

        private void rbSize1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize1.Checked)
            {
                tasteCount = 1;
                sSize = rbSize1.Text;
                if (tasteCount2 > tasteCount) CountTaste2(tasteCount);
                else CountTaste(tasteCount);
                tasteCount2 = tasteCount;
            }
        }

        private void rbSize2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize2.Checked)
            {
                tasteCount = 2;
                sSize = rbSize2.Text;
                if (tasteCount2 > tasteCount) CountTaste2(tasteCount);
                else CountTaste(tasteCount);
                tasteCount2 = tasteCount;
            }
        }
        private void rbSize3_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize3.Checked)
            {
                tasteCount = 3;
                sSize = rbSize3.Text;
                if (tasteCount2 > tasteCount) CountTaste2(tasteCount);
                else CountTaste(tasteCount);
                tasteCount2 = tasteCount;
            } 
        }
        private void rbSize4_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize4.Checked) 
            {
                tasteCount = 4;
                sSize = rbSize4.Text;
                if (tasteCount2 > tasteCount) CountTaste2(tasteCount);
                else CountTaste(tasteCount);
                tasteCount2 = tasteCount;
            }
        }
        private void rbSize5_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize5.Checked)
            {
                tasteCount = 5;
                sSize = rbSize5.Text;
                if (tasteCount2 > tasteCount) CountTaste2(tasteCount);
                else CountTaste(tasteCount);
                tasteCount2 = tasteCount;
            }
        }
        private void rbSize6_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize6.Checked)
            {
                tasteCount = 6;
                sSize = rbSize6.Text;
                if (tasteCount2 > tasteCount) CountTaste2(tasteCount);
                else CountTaste(tasteCount);
                tasteCount2 = tasteCount;
            }
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            Admin dlg = new Admin();
            dlg.ShowDialog(); //호출
        }
    }
}
