using System;
using System.IO;
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
            try
            {
                using (StreamReader s = new StreamReader(path))
                {
                    html = s.ReadToEnd();
                    Console.WriteLine(html);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
