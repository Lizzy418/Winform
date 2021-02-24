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
    public partial class MyOrder : Form
    {
        public MyOrder()
        {
            InitializeComponent();
        }
        //DB 연결
        SqlConnection sConn = new SqlConnection(); //sql 사용하기 위한 class
        SqlCommand sCmd = new SqlCommand();
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ljw45\source\repos\Icecream_order\Icecream_order\IcereamOrder.mdf;Integrated Security=True;Connect Timeout=30";

        //ID에 맞는 주문 정보 불러오기
        public string GetDBdata(string field, string kValue)
        {
            sCmd.CommandText = $"Select {field} from tbOrderlist where ID='{kValue}'";
            if ((string)sCmd.ExecuteScalar() != null)
                return (string)sCmd.ExecuteScalar();
            else return "";
        }

        //주문 취소 시 DB 삭제
        public int RunSql(string sID)
        {
            string sSql = $"DELETE tbOrderlist where ID='{sID}'";
            sCmd.CommandText = sSql;
            sCmd.ExecuteNonQuery();
            MessageBox.Show("주문이 취소되었습니다.");
            this.Close();
            return 0;
        }

        public static string userID;
        private void MyOrder_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            try
            {
                sConn.ConnectionString = connString;
                sConn.Open(); //DB 열기(폼 닫을 때 종료한다는 코드 꼭 넣어줘야 함)
                sCmd.Connection = sConn; //sConn은 스키마 정보를 가지고 있음
            }
            catch (Exception e1)
            {
                lbInform.Text = "DB에 문제가 있습니다!!!";
            }
            
            lbID.Text = GetDBdata("ID", userID);
            lbDate.Text = GetDBdata("Date", userID);
            lbStore.Text = GetDBdata("Store", userID);
            lbTime.Text = GetDBdata("Time", userID);
            lbSize.Text = GetDBdata("Size", userID);
            int i;
            for (i = 1; i < 7; i++)
            {
                lbTaste.Text += GetDBdata($"Select{i}", userID) + "\r\n";
            }
            lbPrice.Text = GetDBdata("Price", userID);
            string sPayment = GetDBdata("Payment", userID).Trim();
            if (sPayment == "Card") rbCard.Checked = true;
            else rbCash.Checked = true;
            if (lbID.Text == "") lbInform.Text = "주문 내역이 없습니다.";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (lbID.Text != "") RunSql(userID);
        }
    }
}
