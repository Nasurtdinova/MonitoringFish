using System;
using System.Collections;
using System.Collections.Generic;

namespace FishMonitoringCore
{
    public abstract class Quality { }

    public class TempQuality : Quality, IEnumerable
    {
        Dictionary<DateTime, double> temperature;

        Dictionary<DateTime, double> dateMax = new Dictionary<DateTime, double>();
        Dictionary<DateTime, double> dateMin = new Dictionary<DateTime, double>();
        TimeSpan timeMin = new TimeSpan(0, 0, 0);
        TimeSpan timeMax = new TimeSpan(0, 0, 0);
        TimeSpan interval;

        public IEnumerator GetEnumerator()
        {
            return temperature.GetEnumerator();
        }

        public TempQuality()
        {
            temperature = new Dictionary<DateTime, double>();
        }

        public TempQuality(Dictionary<DateTime, double> temp)
        {
            this.temperature = temp;
        }

        public TempQuality(DateTime begin, TimeSpan interval, double[] data) : this()
        {
            temperature.Add(begin, data[0]);
            for (int i = 1; i < data.Length; i++)
            {
                DateTime beginNext = begin + interval;
                temperature.Add(beginNext, data[i]);
                begin = beginNext;
            }
            this.interval = interval;
        }

        public TempQuality(int timeInterval, string temperatureData) : this()
        {
            var time = DateTime.Now;
            var interval = new TimeSpan(0, timeInterval, 0);
            foreach (var t in temperatureData.Split())
            {
                temperature.Add(time, Double.Parse(t));
                time += interval;
            }
            this.interval = interval;
        }

        public Dictionary<DateTime, double> GetDateTimeMax()
        {
            return dateMax;
        }

        public Dictionary<DateTime, double> GetDateTimeMin()
        {
            return dateMin;
        }

        public TimeSpan GetTempUpperTime(double temp)
        {
            foreach (KeyValuePair<DateTime, double> val in temperature)
            {
                if (temp < val.Value)
                {
                    dateMax.Add(val.Key, val.Value);
                    timeMax += interval;
                }
            }
            return timeMax;
        }

        public TimeSpan GetTempLowerTime(double temp)
        {
            foreach (KeyValuePair<DateTime, double> val in temperature)
            {
                if (temp > val.Value)
                {
                    dateMin.Add(val.Key, val.Value);
                    timeMin += interval;
                }
            }
            return timeMin;
        }
    }
}
