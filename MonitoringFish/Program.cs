using System;
using System.Collections.Generic;

namespace MonitoringFish
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Content-Type: text/html \n\n");
            var queryStr = Environment.GetEnvironmentVariable("QUERY_STRING");
            //string queryStr = "TypeFish=Chilled&interval=10&maxStoreTemp=3&maxDeathTime=40&minStoreTemp=-3&minDeathTime=20&date=2021-10-07T18%3A18&temperature=-5+5+-6+5+7+8";
            string[] dataValue = queryStr.Split('&');

            string[] temps = dataValue[7].Split('=')[1].Split('+');
            double[] doubleTemps = new double[temps.Length];
            for (int i = 0; i < temps.Length; i++)
            {
                doubleTemps[i] = Convert.ToDouble(temps[i]);
            }

            string TypeFish = dataValue[0].Split('=')[1];
            TimeSpan maxDeathTime = new TimeSpan(0, Convert.ToInt32(dataValue[3].Split('=')[1]), 0);
            TimeSpan minDeathTime = new TimeSpan(0, Convert.ToInt32(dataValue[5].Split('=')[1]), 0);
            double maxStoreTemp = Convert.ToDouble(dataValue[2].Split('=')[1]);
            double minStoreTemp = Convert.ToDouble(dataValue[4].Split('=')[1]);
            TimeSpan interval = new TimeSpan(0, Convert.ToInt32(dataValue[1].Split('=')[1]), 0);

            DateTime dateFish = ConvertQueryDateToDateTime(dataValue[6].Split('=')[1]);
            var quality = new TempQuality(dateFish, interval, doubleTemps);

            Fish mentai = new FrozenFish(quality, maxDeathTime, maxStoreTemp);
            Fish gorbusha = new ChilledFish(quality, maxDeathTime, maxStoreTemp, minDeathTime, minStoreTemp);

            Console.WriteLine("<html><head><title>Otchet</title></head>");
            Console.WriteLine("<body>");
            Console.WriteLine("<table border = 1 width = 40%>");
            Console.WriteLine("<tr align = center>");
            Console.WriteLine($"<th>Time</th><th> Fact</th><th>Norm</th><th>Deviation</th></tr>");
            if (TypeFish == "Chilled")
            {
                foreach (KeyValuePair<Dictionary<DateTime, double>, Dictionary<DateTime, double>> val in gorbusha.isValid())
                {
                    int count = 0;
                    foreach (KeyValuePair<DateTime, double> value in val.Key)
                    {
                        count++;
                        Console.WriteLine("<tr align = center >");
                        Console.WriteLine($"<td>{value.Key.ToString("dd.MM.yyyy hh:mm")}</td><td>{value.Value}</td><td>{maxStoreTemp}</td><td>{value.Value - maxStoreTemp}</td></tr>");
                    }
                    foreach (KeyValuePair<DateTime, double> value in val.Value)
                    {
                        count++;
                        Console.WriteLine($"<tr align=center><td>{value.Key.ToString("dd.MM.yyyy hh:mm")}</td><td>{value.Value}</td><td>{minStoreTemp} </td><td>{value.Value - minStoreTemp}</td></tr>");
                    }
                    TimeSpan thresold = new TimeSpan(0, count * Convert.ToInt32(dataValue[1].Split('=')[1]), 0);
                    Console.WriteLine($"<p><h3>Type fish: {TypeFish}</h3></p>");
                    Console.WriteLine($"<p><h3>Date ant time fish: {dateFish.ToString("dd.MM.yyyy hh:mm")}</h3></p>");
                    Console.WriteLine($"<p><h3>Interval: {interval.TotalMinutes} minute</h3></p>");
                    Console.WriteLine($"<p><h3>The threshold is exceeded by {thresold.TotalMinutes} minutes</h3></p>");
                }
            }
            else if(TypeFish == "Frozen")
            {
                foreach (KeyValuePair<Dictionary<DateTime, double>, Dictionary<DateTime, double>> val in mentai.isValid())
                {
                    int count = 0;
                    foreach (KeyValuePair<DateTime, double> value in val.Key)
                    {
                        count++;
                        Console.WriteLine("<tr align = center >");
                        Console.WriteLine($"<td>{value.Key.ToString("dd.MM.yyyy hh:mm")}</td><td>  {value.Value}</td><td>  {maxStoreTemp} </td><td> {value.Value - maxStoreTemp}</td></tr>");
                    }
                    TimeSpan thresold = new TimeSpan(0, count * Convert.ToInt32(dataValue[1].Split('=')[1]), 0);
                    Console.WriteLine($"<p><h3>Type fish: {TypeFish}</h3></p>");
                    Console.WriteLine($"<p><h3>Date ant time fish: {dateFish.ToString("dd.MM.yyyy hh:mm")}</h3></p>");
                    Console.WriteLine($"<p><h3>Interval: {interval.TotalMinutes} minute</h3></p>");
                    Console.WriteLine($"<p><h3>The threshold is exceeded by {thresold.TotalMinutes} minutes</h3></p>");
                }
            }
            Console.WriteLine("</body></html>");
        }

        public static DateTime ConvertQueryDateToDateTime(string date)
        {
            string[] dateAndTimeFish = date.Split('T');
            string[] dateNumbers = dateAndTimeFish[0].Split('-');
            string[] timeNumbers = dateAndTimeFish[1].Split('%');
            int minute = Convert.ToInt32(timeNumbers[1].Split('A')[1]);
            DateTime dateFish = new DateTime(Convert.ToInt32(dateNumbers[0]), Convert.ToInt32(dateNumbers[1]), Convert.ToInt32(dateNumbers[2]), Convert.ToInt32(timeNumbers[0]),minute, 0);
            return dateFish;
        }

        public static DateTime ConvertStringToDateTime(string date)
        {
            string[] dateAndTimeFish = date.Split(' ');
            string[] dateNumbers = dateAndTimeFish[0].Split('.');
            string[] timeNumbers = dateAndTimeFish[1].Split(':');
            DateTime dateFish = new DateTime(Convert.ToInt32(dateNumbers[2]), Convert.ToInt32(dateNumbers[1]), Convert.ToInt32(dateNumbers[0]), Convert.ToInt32(timeNumbers[0]), Convert.ToInt32(timeNumbers[1]), 0);
            return dateFish;
        }
    }
}

