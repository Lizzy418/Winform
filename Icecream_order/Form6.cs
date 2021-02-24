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
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }
        //DB 연결
        SqlConnection sConn = new SqlConnection(); //sql 사용하기 위한 class
        SqlCommand sCmd = new SqlCommand();
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ljw45\source\repos\Icecream_order\Icecream_order\IcereamOrder.mdf;Integrated Security=True;Connect Timeout=30";
        
        //","를 default로 구분자(->sep) 설정
        public string GetToken(int n, string str, string sep = ",") 
        {
            int i; 
            int n1 = 0, n2 = 0, n3 = 0; //temp int 변수
            for (i = 0; i < n; i++)
            {
                n1 = str.IndexOf(sep, n1) + 1; // i번째 구분자
                if (n1 == 0) return "";
            }
            n2 = str.IndexOf(sep, n1); //n+1번째 구분자
            if (n2 == -1) n2 = str.Length;

            n3 = n2 - n1; //문자열 길이 계산
            string sRet = str.Substring(n1, n3);
            return sRet;
        }
        //SQL 실행 함수(DB 정보 띄우기)
        public int RunSql(string sSql)
        {
            try
            {
                sCmd.CommandText = sSql;
                int i, j, k;
                string s1 = GetToken(0, sSql, " ").ToUpper();
                if (s1 != "SELECT") sCmd.ExecuteNonQuery(); 
                else
                {
                    SqlDataReader sr = sCmd.ExecuteReader(); //select문 처리결과 수신
                    dataGridView1.Columns.Clear(); //읽어질 때마다 업뎃되려면 기존 내용들 지워줘야 함
                    dataGridView1.Rows.Clear();

                    for (i = 0; i < sr.FieldCount; i++)
                    {
                        dataGridView1.Columns.Add(sr.GetName(i), sr.GetName(i));
                    }
                    for (i = 0; sr.Read(); i++) //처리할 데이터가 있는 경우 수행
                    //i는 곧 y값
                    {
                        if (dataGridView1.RowCount < i + 2) dataGridView1.Rows.Add();
                        for (j = 0; j < sr.FieldCount; j++)
                        {
                            object oVal = sr.GetValue(j); //필드 개수만큼 각각의 값 처리
                            string buf = $"{oVal}";
                            //dataGridView1.Rows[i].Cells[0].Value = false;
                            dataGridView1.Rows[i].Cells[j].Value = buf;
                        }
                    }
                    sr.Close(); //실행이 다 된 후에는 꼭 닫아줘야 함
                }
                
            }
            catch (Exception e1)
            {
                MessageBox.Show("DB에 문제가 있습니다!");
            }

            return 0;
        }

        //현재 open되어 있는 DB의 Table 명을 cbTable 콤보박스에 넣기
        //tbOrderlist / tbCustomer
        public void GetDBTableNames()
        {
            cbDBtable.Items.Clear();
            //sConn.GetSchema 이용해서 콤보박스 만들기
            DataTable dt = sConn.GetSchema("Tables"); //반환값이 이미 tables로 있어서 따로 new 해줄 필요가 없음
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sTableName = dt.Rows[i].ItemArray[2].ToString();
                cbDBtable.Items.Add(sTableName);
            }
        }
        private void Admin_Load(object sender, EventArgs e)
        {
            try
            {
                sConn.ConnectionString = connString;
                sConn.Open(); //DB 열기
                sCmd.Connection = sConn;
                GetDBTableNames();
            }
            catch (Exception e1)
            {
                MessageBox.Show("DB에 문제가 있습니다!!!");
            }
        }

        private void cbDBtable_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sSel = $"select * from {cbDBtable.SelectedItem}"; //cdDBtable.Text도 가능
            RunSql(sSel);
            if (cbDBtable.Text == "tbCustomer")
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            else dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        // row header에 자동 일련번호 넣기
        private void dataGridView1_RowPostPaint_1(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            using (Brush brush = new SolidBrush(Color.Navy))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font,
                brush, e.RowBounds.Location.X + 35, e.RowBounds.Location.Y + 4, drawFormat);
            }
        }

        //ID로 리스트 조회
        private void btnID_Click(object sender, EventArgs e)
        {
            if(tbID.Text != "")
            {
                string sSql = $"Select * from {cbDBtable.SelectedItem} where ID = '{tbID.Text}'";
                RunSql(sSql);
            }
            tbID.Text = "";
        }

        //DELETE 버튼 클릭 시, 고객정보가 아닌 주문정보만 삭제할 수 있게 설정
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int nRow = dataGridView1.SelectedCells[0].RowIndex;
            string sId = dataGridView1.Rows[nRow].Cells[0].Value.ToString();
            
            if (cbDBtable.Text == "tbCustomer")
                MessageBox.Show("고객 정보는 삭제할 수 없습니다!");
            else
            {
                Delete.sID = sId;
                Delete dlg = new Delete();
                DialogResult dr = dlg.ShowDialog(); //호출
                                                    //OK 버튼 클릭 시
                if (dr == DialogResult.OK)
                {
                    string sSql = $"DELETE tbOrderlist where ID = '{sId}'";
                    RunSql(sSql);
                    cbDBtable_SelectedIndexChanged(sender, e);
                    return;
                }
            }
        }
    }
}
