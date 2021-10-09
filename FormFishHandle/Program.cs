using System;

namespace FormFishHandle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Content-Type: text/html \n\n");
            //var queryStr = Environment.GetEnvironmentVariable("QUERY_STRING");
            //Console.WriteLine(queryStr);
            string queryStr = "TypeFish=fgdfg&maxStoreTemp=3&maxDeathTime=20&minStoreTemp=-3&minDeathTime=30&date=2021-10-05T12%3A32&temperature=-5+5+6+5+7";
            string[] dataValue = queryStr.Split('&');
            for(int i = 0; i < dataValue.Length; i++)
            {
                string valueParc = dataValue[i].Split('=')[1];
                Console.WriteLine(valueParc);
            }
           
        }
    }
}
