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
      double[] scores = new double[] { 80, 90, 85, 70, 95 }; // double[] 배열 scores저장
      chart1.Series[0].Points.DataBindY(scores);  //Y값 데이터바인딩

      // (2) X, Y값 List<T> 데이타바인딩
      List<string> x = new List<string> { "철수", "영희", "길동", "재동", "민희" }; //리스트 스트링 x저장
      List<double> y = new List<double> { 80, 90, 85, 70, 95 }; //리스트 더블 y저장
      chart2.Series[0].Points.DataBindXY(x, y); //x y값 데이터바인딩

      // (3) 객체 컬렉션에 대한 데이타바인딩
      List<Student> students = new List<Student>(); // 스큐던트 클래스에 리스트 student변수 저장
      students.Add(new Student("철수", 80)); // 스튜던트변수에 철수,80 추가
      students.Add(new Student("영희", 90));// 스튜던트변수에 영희,90 추가
      students.Add(new Student("길동", 85));// 스튜던트변수에 길동,85 추가
      // X축: Name, Y축: Score

      chart3.Series[0].Points.DataBind(students, "Name", "Score", null); // 차트3에 student객체 속성 name과 데이터바인딩

      // (4) DataSource와 DataBind() MySql 사용
      string strConn = "datasource=127.0.0.1;port=3306;database=dawoon;username=root;password=ekdnsel;Charset=utf8";
      string sql = "SELECT 문서.careSeq, 등록.number, 문서.careStart, 문서.careFinish, 문서.time, 문서.sympton, 문서.count, 문서.injection, " +
        "문서.oral, 등록.enrollSeq, 문서.age, 등록.birth, 문서.memo, 문서.flagYN, 문서.regDate, 문서.issueDate, 문서.issueID FROM dc_careenroll " +
        "등록 inner JOIN dc_caredocument 문서 ON(등록.enrollSeq = 문서.enrollSeq) WHERE 문서.flagYN = 'Y' GROUP BY 문서.number";
      using (MySqlConnection conn = new MySqlConnection(strConn)) // 연결클래스 conn 생성
      {
        conn.Open(); // conn 연결
        MySqlCommand cmd = new MySqlCommand(sql, conn); //명령클래스 cmd 생성
        DataSet ds = new DataSet(); //데이터셋 ds 생성
        MySqlDataAdapter sa = new MySqlDataAdapter(cmd); //cmd인자를 받은 데이터어뎁터 sa 생성
        sa.Fill(ds); //sa 데이터어뎁터 변수를 ds데이터셋에 채워라
        // DataTable 객체를 DataSource에 지정하고,
        // X,Y축 컬럼을 XValueMember와 YValueMembers에 지정
        chart6.DataSource = ds.Tables[0]; //chart6.데이터소스는 ds,Tables의 첫번째로 지정
        chart6.Series[0].XValueMember = "number"; // x축 컬럼 number필드
        chart6.Series[0].YValueMembers = "time"; // y축 컬럼 time필드
        chart6.DataBind(); //chart6 데이터바인딩
      }

      // (5) Points.DataBind() 사용
      using (MySqlConnection conn = new MySqlConnection(strConn)) //연결클래스 conn 생성
      {
        conn.Open(); //conn 연결
        MySqlCommand cmd = new MySqlCommand(sql, conn); //명령클래스 cmd 생성
        MySqlDataReader dataReader = cmd.ExecuteReader(); // cmd 데이터 읽은값을 dataReader에 저장
        // IDataReader 객체와 X, Y축 컬럼 지정. 
        // 4번째 Param: 툴팁과 같은 다른 필드 지정가능. 여기선 time 컬럼값을 툴팁에 표시
        chart7.Series[0].Points.DataBind(dataReader, "number", "time", "Tooltip=time"); 

        // (6) DataBindCrossTable() 사용
        chart9.Series.Clear(); //초기화
        DataTable dt = new DataTable("Order"); //데이터테이블 오더라는 dt 생성
        dt.Columns.Add("customer"); //dt 컬럼 customer
        dt.Columns.Add("product");//dt 컬럼 product
        dt.Columns.Add("orders");//dt 컬럼 orders
        dt.Rows.Add("Tom", "USB", 10);//dt 로우 tom, usb, 10
        dt.Rows.Add("Tom", "HDD", 2);//dt 로우 tom, hdd, 2
        dt.Rows.Add("Tom", "Monitor", 1); //dt 로우 tom,moniter,1
        dt.Rows.Add("Jane", "USB", 3); //dt 로우 jane,usb,3
        dt.Rows.Add("Jane", "HDD", 1); //dt 로우 jane, hdd, 1
        dt.Rows.Add("Jane", "Monitor", 2);//dt 로우 jane, Monitor, 2
        // Product별로 Series를 하나씩 자동으로 생성
        // X축은 customer 컬럼, Y축은 orders 컬럼
        // 각 그래프 상단에 product명으로 Label을 붙임
        chart9.DataBindCrossTable(dt.AsEnumerable(), "product", "customer", "orders", "Label=product");
        
      }


    }
    class Student
    {
      public string Name { get; set; } //스트링타입 Name 속성
      public double Score { get; set; } //더블타입 Score 속성

      public Student(string name, double score)//인자 name과 score를 받은 student 함수
      {
        this.Name = name; //name은 this.Name
        this.Score = score; //score은 =this.Score
      }
    }
    private void Form1_Load(object sender, EventArgs e) //폼로드
		{
      btnData_Click(sender, e); //버튼실행
		}
	}
}
