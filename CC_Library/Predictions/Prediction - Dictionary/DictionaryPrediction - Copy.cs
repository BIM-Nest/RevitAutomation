﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CC_Library.Datatypes;
/*
/// <summary>
/// This system is specifically for creating and using a vocabulary.
/// For this reason, it doesnt need all of the delegates that a typical network would.
/// </summary>
namespace CC_Library.Predictions
{
    public class DictionaryNetwork
    {
        private const int RunSize = 8;
        private static List<string> Words = Enum.GetNames(typeof(Dict)).ToList();
        private static int WordCount = Enum.GetNames(typeof(Dict)).GetLength(0);
        public NeuralNetwork Network { get; }

        public DictionaryNetwork(WriteToCMDLine write)
        {
            Network = Datatype.Dictionary.LoadNetwork(write);
        }
        internal static void SamplePropogate
            (
            string line,
           int lineno,
           DictionaryNetwork d,
           Alpha a,
           LocalContext lctxt,
           GlobalContext gctxt,
           Accuracy acc,
           WriteToCMDLine write
           )
        {
            string input = line;
            AlphaMem am = new AlphaMem();

            List<double[]> Results = new List<double[]>();
            Results.Add(a.Forward(input, lctxt, gctxt, am, write));

            for (int k = 0; k < d.Network.Layers.Count(); k++)
            {
                Results.Add(d.Network.Layers[k].Output(Results.Last()));
            }

            int choice = Results.Last().ToList().IndexOf(Results.Last().Max());
            int correct = GetResult(input);

            double[] res = new double[WordCount];
            res[correct] = 1;
            var result = CategoricalCrossEntropy.Forward(Results.Last(), res);

            acc.Add(input, lineno, Results.Last()[correct], result.Sum(), choice);
            var DValues = res;

            for (int l = d.Network.Layers.Count() - 1; l >= 0; l--)
            {
                DValues = d.Network.Layers[l].DActivation(DValues, Results[l + 1]);
                d.Network.Layers[l].DBiases(DValues);
                d.Network.Layers[l].DWeights(DValues, Results[l]);
                DValues = d.Network.Layers[l].DInputs(DValues);
            }
            a.Backward(input, DValues, lctxt, gctxt, am, write);
        }

        internal static void Propogate
            (string filepath,
            WriteToCMDLine write)
        {
            DictionaryNetwork dict = new DictionaryNetwork(write);
            Alpha a = new Alpha(write);
            LocalContext lctxt = new LocalContext(Datatype.Dictionary, write);
            GlobalContext gctxt = new GlobalContext(Datatype.Dictionary, write);
            Random random = new Random();
            string[] Lines = Words.ToArray();

            int[] numbs = new int[Lines.Count()];
            for (int i = 0; i < numbs.Count(); i++)
                numbs[i] = i;

            int samples = 0;
            int cycles = 0;
            int epochs = 0;
            var acc = new Accuracy(Lines);

            while (samples < 3000000)
            {
                var numbers = numbs.OrderBy(x => random.Next()).ToList();

                for (int i = 0; i < Lines.Count() - (RunSize + 1); i += RunSize)
                {
                    Parallel.For(0, RunSize, j =>
                    {
                        if (i + j < numbers.Count())
                        {
                            SamplePropogate(Lines[numbers[i + j]], numbers[i + j], dict, a, lctxt, gctxt, acc, write);
                            samples++;
                        }
                    });
                    if (cycles > 1)
                    {
                        dict.Network.Update(RunSize, 0.1);
                        //a.Location.Update(RunSize, 0.00001);
                        lctxt.Network.Update(RunSize, 0.1);
                        gctxt.Network.Update(RunSize, 0.1);
                    }
                    write("Samples Run : " + samples + " : Cycles : " + cycles + " : Epochs : " + epochs);
                    acc.Show(write);
                    write("");

                    cycles++;
                }
                epochs++;
                if (acc.CheckError() && epochs > 1)
                {
                    dict.Network.Save();
                    //a.Location.Save();
                    lctxt.Save();
                    gctxt.Save();
                    write("Network Saved");
                }
                acc.Reset();
            }
        }
        private static string WriteNull(string s) { return null; }
        private static int GetResult(string p)
        {
            return Words.IndexOf(p);
        }
    }
}*/