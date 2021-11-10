using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;

namespace prjXISD
{
    class Employee
    {
        private Database db = new Database();
        private string empID, empName, empContact, empPassword;
        private int empNum;

        public string EmpID { get => empID; set => empID = value; }
        public string EmpName { get => empName; set => empName = value; }
        public string EmpContact { get => empContact; set => empContact = value; }
        public int EmpNum { get => empNum; set => empNum = value; }
        public string EmpPassword { get => empPassword; set => empPassword = value; }

        public Employee Login(string id, string pass)
        {
            try
            {
                db.cnn.Open();
                String sql =
                    "SELECT * " +
                    "FROM tblEmployee where empNum = '" + id + "' " +
                    "AND empPassword ='" + pass + "' ;";
                SqlCommand command = new SqlCommand(sql, db.cnn);
                SqlDataReader r = command.ExecuteReader();
                r.Read();
                if (r.HasRows)
                {
                    this.EmpID = Convert.ToString(r.GetInt32(0));
                    this.EmpName = r.GetString(1);
                    this.EmpNum = r.GetInt32(2);
                    this.EmpContact = Convert.ToString(r.GetString(3));
                    this.EmpPassword = r.GetString(4);
                }
                else
                {
                    return null;
                }
                r.Close();
                command.Dispose();

                db.cnn.Close();
                return this;
            }
            catch (SqlException ex)
            {
                //ConsoleAllocator.ShowConsoleWindow();
                Console.WriteLine(ex.ToString() + " Pauw");
                return null;
            }
        }

        public Employee Register(string id, string pass)
        {
            if (id.Length <= 0 || pass.Length <= 0)
            {
                return null;
            }
            else
            {
                try
                {
                    string sql =
                        "INSERT INTO tblEmployee (empName, empNum, empContact, empPassword)" +
                        $"VALUES('', {id}, '', '{pass}');";
                    db.cnn.Open();
                    runSQL(sql, db.cnn);
                    db.cnn.Close();
                }
                catch (SqlException ex)
                {
                    //ConsoleAllocator.ShowConsoleWindow();
                    Console.WriteLine(ex.ToString() + " Pauw");
                    return null;
                }

                return Login(id, pass);
            }
        }

        public void runSQL(string SQL, SqlConnection conn)
        {
            SqlCommand com = new SqlCommand(SQL, conn);
            com.ExecuteNonQuery();
        } //Works
    }

    internal static class ConsoleAllocator
    {
        [DllImport(@"kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport(@"kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport(@"user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SwHide = 0;
        const int SwShow = 5;


        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                ShowWindow(handle, SwShow);
            }
        }

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SwHide);
        }
    }
}
