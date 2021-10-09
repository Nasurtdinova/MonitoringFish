using System;
using System.Collections.Generic;

namespace MonitoringFish
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Content-Type: text/html \n\n");
            //var queryStr = Environment.GetEnvironmentVariable("QUERY_STRING");
            string queryStr = "TypeFish=&interval=10&maxStoreTemp=3&maxDeathTime=20&minStoreTemp=-3&minDeathTime=30&date=2021-10-09T08%3A54&temperature=-5+5+6+5+7";
            string[] dataValue = queryStr.Split('&');

            string[] temps = dataValue[7].Split('=')[1].Split('+');
            double[] doubleTemps = new double[temps.Length];
            for (int i = 0; i < temps.Length; i++)
            {
                doubleTemps[i] = Convert.ToDouble(temps[i]);
            }

            DateTime dateFish = ConvertQueryDateToDateTime(dataValue[6].Split('=')[1]);
            var quality = new TempQuality(dateFish, new TimeSpan (0,Convert.ToInt32(dataValue[1].Split('=')[1]),0), doubleTemps);
            //Fish mentai = new FrozenFish(quality, new TimeSpan(0, Convert.ToInt32(dataValue[3].Split('=')[1]), 0), Convert.ToDouble(dataValue[2].Split('=')[1]));

            Fish gorbusha = new ChilledFish(quality, new TimeSpan(0, Convert.ToInt32(dataValue[3].Split('=')[1]), 0), Convert.ToDouble(dataValue[2].Split('=')[1]), new TimeSpan(0, Convert.ToInt32(dataValue[5].Split('=')[1]), 0), Convert.ToDouble(dataValue[4].Split('=')[1]));
            Console.WriteLine("<html><head><title>Otchet</title></head>");
            Console.WriteLine("<body>");
            Console.WriteLine($"<p><h1>   Time             Fact    Norm    Deviation</h1><p>");
            foreach (KeyValuePair <Dictionary<DateTime, double>, Dictionary<DateTime, double>> val in gorbusha.isValid())
            {
                foreach(KeyValuePair <DateTime, double> value in val.Key)
                {
                    Console.WriteLine($"<p><h2>{value.Key}   {value.Value}       {Convert.ToDouble(dataValue[2].Split('=')[1])}          {value.Value - Convert.ToDouble(dataValue[2].Split('=')[1])}</h2></p>");
                   
                }
                foreach (KeyValuePair<DateTime, double> value in val.Value)
                {
                    Console.WriteLine($"<p><h2>{value.Key}   {value.Value}       {Convert.ToDouble(dataValue[2].Split('=')[1])}          {value.Value - Convert.ToDouble(dataValue[2].Split('=')[1])}</h2></p>");

                }
                Console.WriteLine($"{val.Value}");
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

