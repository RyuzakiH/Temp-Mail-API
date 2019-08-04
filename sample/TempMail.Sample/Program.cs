using System;

namespace TempMail.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Synchronous sample:\n===================");
            //TempMailSample.Sample();

            Console.WriteLine("Asynchronous sample:\n====================");
            TempMailSampleAsync.Sample().Wait();
        }
    }
}
