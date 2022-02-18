using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace chartStudy2
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}


    private void btnData_Click(object sender, EventArgs e)
    {
    
  
      // (1) Y값 배열 데이타바인딩
      double[] scores = new double[] { 80, 90, 85, 70, 95 };
      chart1.Series[0].Points.DataBindY(scores);

      // (2) X, Y값 List<T> 데이타바인딩
      List<string> x = new List<string> { "철수", "영희", "길동", "재동", "민희" };
      List<double> y = new List<double> { 80, 90, 85, 70, 95 };
      chart2.Series[0].Points.DataBindXY(x, y);

      // (3) 객체 컬렉션에 대한 데이타바인딩
      List<Student> students = new List<Student>();
      students.Add(new Student("철수", 80));
      students.Add(new Student("영희", 90));
      students.Add(new Student("길동", 85));
      // X축: Name, Y축: Score
      chart3.Series[0].Points.DataBind(students, "Name", "Score", null);

      string strConn = "datasource=127.0.0.1;port=3306;database=dawoon;username=root;password=ekdnsel;Charset=utf8";
      string sql = "SELECT 문서.careSeq, 등록.number, 문서.careStart, 문서.careFinish, 문서.time, 문서.sympton, 문서.count, 문서.injection, " +
        "문서.oral, 등록.enrollSeq, 문서.age, 등록.birth, 문서.memo, 문서.flagYN, 문서.regDate, 문서.issueDate, 문서.issueID FROM dc_careenroll " +
        "등록 inner JOIN dc_caredocument 문서 ON(등록.enrollSeq = 문서.enrollSeq) WHERE 문서.flagYN = 'Y' GROUP BY 문서.number";


      // (2) DataSource와 DataBind() 사용
      using (MySqlConnection conn = new MySqlConnection(strConn))
      {
        conn.Open();
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        DataSet ds = new DataSet();
        MySqlDataAdapter sa = new MySqlDataAdapter(cmd);
        sa.Fill(ds);

        // DataTable 객체를 DataSource에 지정하고,
        // X,Y축 컬럼을 XValueMember와 YValueMembers에 지정
        chart6.DataSource = ds.Tables[0];
        chart6.Series[0].XValueMember = "number";
        chart6.Series[0].YValueMembers = "time";
        chart6.DataBind();
      }

      // (3) Points.DataBind() 사용
      using (MySqlConnection conn = new MySqlConnection(strConn))
      {
        conn.Open();
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataReader dataReader = cmd.ExecuteReader();

        // IDataReader 객체와 X, Y축 컬럼 지정. 
        // 4번째 Param: 툴팁과 같은 다른 필드 지정가능. 여기선 sales 컬럼값을 툴팁에 표시
        chart7.Series[0].Points.DataBind(dataReader, "number", "time", "Tooltip=time");

        // DataBindCrossTable() 사용

        DataTable dt = new DataTable("Order");
        dt.Columns.Add("customer");
        dt.Columns.Add("product");
        dt.Columns.Add("orders");
        dt.Rows.Add("Tom", "USB", 10);
        dt.Rows.Add("Tom", "HDD", 2);
        dt.Rows.Add("Tom", "Monitor", 1);
        dt.Rows.Add("Jane", "USB", 3);
        dt.Rows.Add("Jane", "HDD", 1);
        dt.Rows.Add("Jane", "Monitor", 2);

        // Product별로 Series를 하나씩 자동으로 생성
        // X축은 customer 컬럼, Y축은 orders 컬럼
        // 각 그래프 상단에 product명으로 Label을 붙임
        chart9.DataBindCrossTable(dt.AsEnumerable(), "product", "customer", "orders", "Label=product");
      }


    }
    class Student
    {
      public string Name { get; set; }
      public double Score { get; set; }

      public Student(string name, double score)
      {
        this.Name = name;
        this.Score = score;


      }
    }
    private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
}
