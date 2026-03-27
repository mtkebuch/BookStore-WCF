using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using BookStoreService;

namespace BookStoreHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(BookStoreService.BookStoreService)))
            {
                host.Open();

                
                Console.WriteLine("BookStore WCF Service is running...");
                Console.WriteLine("URL: http://localhost:8733/BookStoreService/");
                Console.WriteLine("Press [Enter] to stop the service.");
               

                Console.ReadLine();

                host.Close();
                Console.WriteLine("Service stopped.");
            }
        }
    }
}