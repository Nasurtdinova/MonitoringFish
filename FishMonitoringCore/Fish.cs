using System;
using System.Collections.Generic;
using System.Text;

namespace FishMonitoringCore
{
    public abstract class Fish
    {
        public string name;
        public Quality quality;
        public Dictionary<DateTime, double> dateMax = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> dateMin = new Dictionary<DateTime, double>();

        public Dictionary<Dictionary<DateTime, double>, Dictionary<DateTime, double>> dateMaxMin = new Dictionary<Dictionary<DateTime, double>, Dictionary<DateTime, double>>();

        public abstract Dictionary<Dictionary<DateTime, double>, Dictionary<DateTime, double>> isValid();

        public Fish(Quality q)
        {
            quality = q;
        }
    }

    public class FrozenFish : Fish
    {
        public double maxStoreTemp;
        public TimeSpan deathTime;

        public FrozenFish(Quality q, TimeSpan t, double max) : base(q)
        {
            maxStoreTemp = max;
            deathTime = t;
        }

        public override Dictionary<Dictionary<DateTime, double>, Dictionary<DateTime, double>> isValid()
        {
            TimeSpan threshold = (quality as TempQuality).GetTempUpperTime(maxStoreTemp);
            if (threshold > deathTime)
            {
                dateMax = (quality as TempQuality).GetDateTimeMax();
                dateMaxMin.Add(dateMax, dateMin);
                return dateMaxMin;
            }
            return dateMaxMin;
        }
    }

    public class ChilledFish : Fish
    {
        public double minStoreTemp;
        public double maxStoreTemp;
        public TimeSpan minDeathTime;
        public TimeSpan maxDeathTime;

        public ChilledFish(Quality q, TimeSpan maxd, double max, TimeSpan mind, double min) : base(q)
        {
            maxStoreTemp = max;
            minStoreTemp = min;
            minDeathTime = mind;
            maxDeathTime = maxd;
        }

        public override Dictionary<Dictionary<DateTime, double>, Dictionary<DateTime, double>> isValid()
        {
            TimeSpan threshold = (quality as TempQuality).GetTempUpperTime(maxStoreTemp);
            TimeSpan threshold2 = (quality as TempQuality).GetTempLowerTime(minStoreTemp);
            if (threshold2 > minDeathTime)
            {
                dateMin = (quality as TempQuality).GetDateTimeMin();
            }
            if (threshold > maxDeathTime)
            {
                dateMax = (quality as TempQuality).GetDateTimeMax();
            }
            dateMaxMin.Add(dateMax, dateMin);
            return dateMaxMin;
        }
    }
}
