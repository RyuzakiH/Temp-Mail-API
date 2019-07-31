namespace TempMail.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Here, we test the synchronous way and the asynchronous one (uncomment any)

            //var client = new Client(new WebProxy("163.172.220.221", 8888));

            //var client = new Client()
            //{
            //    Proxy = new WebProxy("163.172.220.221", 8888)
            //};

            TempMailSample.Sample();

            TempMailSampleAsync.Sample().Wait();
        }
    }
}
