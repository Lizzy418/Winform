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
    public partial class Join : Form
    {
        //DB 연결
        SqlConnection sConn = new SqlConnection(); //sql 사용하기 위한 class
        SqlCommand sCmd = new SqlCommand();
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ljw45\source\repos\Icecream_order\Icecream_order\IcereamOrder.mdf;Integrated Security=True;Connect Timeout=30";

        public Join()
        {
            InitializeComponent();
        }
        //가입 아이디 중복 조회위한 DB 정보 읽기
        public string GetDBdata(string table, string field, string kField, string kValue)
        {
            sCmd.CommandText = $"Select {field} from {table} where {kField}='{kValue}'";
            if (sCmd.ExecuteScalar() == null) return "";
            else return (string)sCmd.ExecuteScalar(); //무조건 첫번째 레코드의 값
        }
        private void Join_Load(object sender, EventArgs e)
        {
            try
            {
                sConn.ConnectionString = connString;
                sConn.Open(); //DB 열기(폼 닫을 때 종료한다는 코드 꼭 넣어줘야 함)
                sCmd.Connection = sConn; //sConn은 스키마 정보를 가지고 있음

            }
            catch (Exception e1)
            {
                lbInform.Visible = true;
                lbInform.Text = "DB에 문제가 있습니다!!!";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            sConn.Close();
        }

        string chkID = "";
        string noID = "";
        //ID 중복확인
        protected void btnChk_Click(object sender, EventArgs e)
        {
            string sID = tbID.Text;
            //DB에서 사용자명 및 암호 조회해서 OK면 진행, 아니면 회원가입 메시지
            string str = "";
            str = GetDBdata("tbCustomer", "ID", "ID", sID).Trim();

            if (sID != str)
            {
                lbInform.Visible = true;
                lbInform.Text = "사용 가능한 아이디 입니다.";
                chkID = sID;
            }
            else
            {
                lbInform.Visible = true;
                lbInform.Text = "이미 사용중인 아이디 입니다.";
                noID = str;
            }

        }

        //회원가입 정보 입력 후 완료 버튼 클릭
        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (tbID.Text == "" || tbName.Text == "" || tbPwd.Text == "" || tbPhone.Text == "" || tbEmail.Text == "")
            {
                lbInform.Visible = true;
                lbInform.Text = "모든 정보를 기입해주세요!";
            }
                
            else
            {
                if (chkID != "")
                {
                    if (noID != "")
                    {
                        lbInform.Visible = true;
                        lbInform.Text = "아이디 중복여부를 확인해주세요.";
                    }
                    else
                    {
                        string sInsert = $"insert into tbCustomer values ('{tbID.Text}','{tbPwd.Text}','{tbName.Text}','{tbPhone.Text}','{tbEmail.Text}')";
                        sCmd.CommandText = sInsert;
                        sCmd.ExecuteNonQuery();
                        lbInform.Text = "가입이 완료되었습니다!";
                    }
                }
                else
                {
                    lbInform.Visible = true;
                    lbInform.Text = "아이디 중복여부를 확인해주세요.";
                }
            }
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
