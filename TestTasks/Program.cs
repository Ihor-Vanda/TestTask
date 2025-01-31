﻿using System;
using System.IO;
using System.Threading.Tasks;
using TestTasks.InternationalTradeTask;
using TestTasks.VowelCounting;
using TestTasks.WeatherFromAPI;

namespace TestTasks
{
    class Program
    {
        public static async Task Main()
        {
            //Below are examples of usage. However, it is not guaranteed that your implementation will be tested on those examples.            
            var stringProcessor = new StringProcessor();
            string str = File.ReadAllText("./CharCounting/StringExample.txt");

            var charCount = stringProcessor.GetCharCount(str, new char[] { 'l', 'r', 'm' });
            foreach (var (x, y) in charCount)
            {
                Console.WriteLine(x + " " + y);
            }

            var commodityRepository = new CommodityRepository();
            commodityRepository.GetImportTariff("Natural honey");
            commodityRepository.GetExportTariff("Iron/steel scrap not sorted or graded");

            Console.WriteLine("Natural honey import tariff: " + commodityRepository.GetImportTariff("Natural honey"));
            Console.WriteLine("Natural honey export tariff: " + commodityRepository.GetExportTariff("Natural honey"));
            Console.WriteLine("Iron/steel scrap not sorted or graded export tariff: " + commodityRepository.GetExportTariff("Iron/steel scrap not sorted or graded"));
            Console.WriteLine("Iron/steel scrap not sorted or graded import tariff: " + commodityRepository.GetImportTariff("Iron/steel scrap not sorted or graded"));

            var weatherManager = new WeatherManager(new System.Net.Http.HttpClient());
            var comparisonResult = await weatherManager.CompareWeather("kyiv,ua", "lviv,ua", 4);
            Console.WriteLine(comparisonResult.WarmerDaysCount);
            Console.WriteLine(comparisonResult.RainierDaysCount);
        }
    }
}
