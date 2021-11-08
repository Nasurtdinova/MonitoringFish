using System;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace FishHtmlForm
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"/home/guzel/formFish.html";
            //string path = @"C:\Users\nasur\source\repos\MonitoringFish\FishHtmlForm\formFish.html";
            string html;

            Console.WriteLine("Content-Type: text/html \n\n");
            string connStr = "server=192.168.70.254;user=guzel;database=Monitoring;port=3306;password=20032003";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "SELECT  distinct Name FROM Fish";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader nameFish = cmd.ExecuteReader();
            string str = "";

            while (nameFish.Read())
            {
                str = str + Environment.NewLine + $"<option>{nameFish[0]}</option>";
            }
            try
            {
                using (StreamReader s = new StreamReader(path))
                {
                    html = s.ReadToEnd();
                }
                html = html.Replace("<!--Option-->", str);
                Console.WriteLine(html);
                nameFish.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            conn.Close();
        }
    }
}
