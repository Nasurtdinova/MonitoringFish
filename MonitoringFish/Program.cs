using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using FishMonitoringCore;

namespace MonitoringFish
{
    class Program
    {
        static void Main(string[] args)
        {          
            Console.WriteLine("Content-Type: text/html \n\n");
            var dataLen = int.Parse(Environment.GetEnvironmentVariable("CONTENT_LENGTH"));
           
            //string queryStr = "Name=Mentai&TypeFish=Frozen&interval=10&date=2021-10-07T18%3A18&temperature=-5+5+-6+5+7+8+9+6+7";
            string connStr = "server=192.168.69.254;user=guzel;database=Monitoring;port=3306;password=20032003";

            Values values = new Values(dataLen);

            string TypeFish = values.GetResult("TypeFish");
            string Name = values.GetResult("Name");
            TimeSpan interval = values.GetInterval();
            DateTime dateFish = values.ConvertQueryDateToDateTime();

            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            try
            {
                string sql = "SELECT * FROM Fish";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader res = cmd.ExecuteReader();              
                
                Console.WriteLine("<html><head><title>Otchet</title></head>");
                Console.WriteLine("<body bgcolor=azure>");
                Console.WriteLine("<table border = 1 width = 40%>");
                Console.WriteLine("<tr align = center>");
                Console.WriteLine("<th>Time</th><th> Fact</th><th>Norm</th><th>Deviation</th></tr>");

                while (res.Read())
                {                  
                    var quality = new TempQuality(dateFish, interval, values.GetTemps());
                    Fish frozen = new FrozenFish(quality, new TimeSpan(0, Convert.ToInt32(res[4]), 0), Convert.ToDouble(res[3]));
                    Fish chilled = new ChilledFish(quality, new TimeSpan(0, Convert.ToInt32(res[4]), 0), Convert.ToDouble(res[3]), new TimeSpan(0, Convert.ToInt32(res[6]), 0), Convert.ToDouble(res[5]));
                    if (Convert.ToString(res[1]) == Name && Convert.ToString(res[2]) == TypeFish)
                    {
                        int count = 0;
                        switch (TypeFish)
                        {
                            case "Frozen":
                                foreach (KeyValuePair<Dictionary<DateTime, double>, Dictionary<DateTime, double>> val in frozen.isValid())
                                {                                    
                                    foreach (KeyValuePair<DateTime, double> value in val.Key)
                                    {
                                        count++;
                                        Console.WriteLine("<tr align = center >");
                                        Console.WriteLine($"<td>{value.Key.ToString("dd.MM.yyyy hh:mm")}</td><td>{value.Value}</td><td>{res[3]}</td><td>{value.Value - Convert.ToDouble(res[3])}</td></tr>");
                                    }
                                    foreach (KeyValuePair<DateTime, double> value in val.Value)
                                    {
                                        count++;
                                        Console.WriteLine($"<tr align=center><td>{value.Key.ToString("dd.MM.yyyy hh:mm")}</td><td>{value.Value}</td><td>{Convert.ToDouble(res[5])} </td><td>{value.Value - Convert.ToDouble(res[5])}</td></tr>");
                                    }
                                }
                                break;

                            case "Chilled":
                                foreach (KeyValuePair<Dictionary<DateTime, double>, Dictionary<DateTime, double>> val in chilled.isValid())
                                {
                                    foreach (KeyValuePair<DateTime, double> value in val.Key)
                                    {
                                        count++;
                                        Console.WriteLine("<tr align = center >");
                                        Console.WriteLine($"<td>{value.Key.ToString("dd.MM.yyyy hh:mm")}</td><td>{value.Value}</td><td>{Convert.ToDouble(res[3])}</td><td>{value.Value - Convert.ToDouble(res[3])}</td></tr>");
                                    }
                                    foreach (KeyValuePair<DateTime, double> value in val.Value)
                                    {
                                        count++;
                                        Console.WriteLine($"<tr align=center><td>{value.Key.ToString("dd.MM.yyyy hh:mm")}</td><td>{value.Value}</td><td>{Convert.ToDouble(res[5])} </td><td>{value.Value - Convert.ToDouble(res[5])}</td></tr>");
                                    }                                   
                                }
                                break;
                        }
                        TimeSpan thresold = new TimeSpan(0, count * Convert.ToInt16(interval.TotalMinutes), 0);

                        Console.WriteLine($"<p><h3>Type fish: {Name}</h3></p>");
                        Console.WriteLine($"<p><h3>Type fish: {TypeFish}</h3></p>");
                        Console.WriteLine($"<p><h3>Date ant time fish: {dateFish.ToString("dd.MM.yyyy hh:mm")}</h3></p>");
                        Console.WriteLine($"<p><h3>Interval: {interval.TotalMinutes} minute</h3></p>");
                        Console.WriteLine($"<p><h3>Max temp: {res[3]} graduce</h3></p>");
                        Console.WriteLine($"<p><h3>Max death time: {res[4]} minute</h3></p>");
                        Console.WriteLine($"<p><h3>Min temp: {res[5]} graduce</h3></p>");
                        Console.WriteLine($"<p><h3>Min death time: {res[6]} minute</h3></p>");
                        Console.WriteLine($"<p><h3>The threshold is exceeded by {thresold.TotalMinutes} minutes</h3></p>");
                    }  
                }
                Console.WriteLine("</body></html>");
                res.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            conn.Close();
        }        
    }
}

