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
    public partial class Login : Form
    {
        //DB 연결
        SqlConnection sConn = new SqlConnection(); //sql 사용하기 위한 class
        SqlCommand sCmd = new SqlCommand();
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ljw45\source\repos\Icecream_order\Icecream_order\IcereamOrder.mdf;Integrated Security=True;Connect Timeout=30";

        //로그인 시 정보 조회
        public string GetDBdata(string table, string field, string kField, string kValue)
        {
            sCmd.CommandText = $"Select {field} from {table} where {kField}='{kValue}'";
            if ((string)sCmd.ExecuteScalar() != null)
                return (string)sCmd.ExecuteScalar(); //무조건 첫번째 레코드의 값
            else return "";
        }

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                sConn.ConnectionString = connString;
                sConn.Open(); //DB 열기(폼 닫을 때 종료한다는 코드 꼭 넣어줘야 함)
                sCmd.Connection = sConn; //sConn은 스키마 정보를 가지고 있음
            }
            catch (Exception e1)
            {
                lbInform.Text = "로그인 DB에 문제가 있습니다!!!";
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            sConn.Close();
        }

        //아이디 저장할 전역변수
        public string userID;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string sID = tbID.Text;
            string sPwd = tbPwd.Text;

            //DB에서 사용자명 및 암호 조회해서 OK면 진행, 아니면 회원가입 메시지
            string str = GetDBdata("tbCustomer", "PWD", "ID", sID).Trim();

            //아이디, 암호 제대로 입력한 경우
            if (sPwd == str && str != "")
            {
                userID = sID;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            //암호 틀렸을 경우
            else if (sPwd != str && str != "") lbInform.Text = "암호를 다시 입력해주세요.";
            //DB에 아이디가 없는 경우
            else if (sID != "" && sPwd !="" && str=="")
                lbInform.Text = "회원가입 정보가 없습니다. \n\r 먼저 가입해 주세요.";
            else if (sID == "" || sPwd == "") lbInform.Text = "아이디와 비밀번호를 입력하세요";
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
