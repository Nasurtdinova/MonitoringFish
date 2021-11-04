using System;
using System.Collections.Generic;
using System.Text;

namespace MonitoringFish
{
    public class Values
    {
        private Dictionary<string, string> result = new Dictionary<string, string>();

        public Values(int dataLen)
        {
            var rowPostData = new char[dataLen + 1];
            for (int i = 0; i < dataLen; ++i)
            {
                rowPostData[i] = (char)Console.Read();
            }

            var fields = new String(rowPostData).Split("&");

            foreach (var field in fields)
            {
                var fieldData = field.Split("=");
                result.Add(fieldData[0], fieldData[1]);
            }
        }

        public double[] GetTemps()
        {
            string[] temps = result["temperature"].Split('+');
            double[] doubleTemps = new double[temps.Length];
            for (int i = 0; i < temps.Length; i++)
            {
                doubleTemps[i] = Convert.ToDouble(temps[i]);
            }
            return doubleTemps;
        }

        public TimeSpan GetInterval()
        {
            return new TimeSpan(0, Convert.ToInt32(result["interval"]), 0);
        }

        public DateTime ConvertQueryDateToDateTime()
        {
            string[] dateAndTimeFish = result["date"].Split('T');
            string[] dateNumbers = dateAndTimeFish[0].Split('-');
            string[] timeNumbers = dateAndTimeFish[1].Split('%');
            int minute = Convert.ToInt32(timeNumbers[1].Split('A')[1]);
            DateTime dateFish = new DateTime(Convert.ToInt32(dateNumbers[0]), Convert.ToInt32(dateNumbers[1]), Convert.ToInt32(dateNumbers[2]), Convert.ToInt32(timeNumbers[0]), minute, 0);
            return dateFish;
        }

        public string GetResult(string key)
        {
            return result[key];
        }
    }
}
