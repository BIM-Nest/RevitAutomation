﻿using System;
using System.Threading;
using CC_Library.Predictions;

namespace DataAnalysis
{
    class Program
    {
        public static void Write(string wo)
        {
            Console.WriteLine(wo);
            Thread.Sleep(100);
        }
        [STAThread]
        static void Main(string[] args)
        {
            Datasets.RunPredictions(new WriteToCMDLine(Write));
        }
    }
}