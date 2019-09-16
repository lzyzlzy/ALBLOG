using System;
using System.Diagnostics;
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Service;
using ALBLOG.Domain.Repository;

namespace ConsoleTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            LogService logService = new LogService();
            var logs = logService.GetAllAsync(i => true).Result;
            Console.WriteLine("ok");
            Console.ReadLine();
        }

    }
}
