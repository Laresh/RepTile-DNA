using System;
using System.IO;  //allows reading/writing to/from a text file
using System.Text;
using System.Diagnostics;

namespace RepTile_DNA_Square
{
    
    
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("type number of sides (3 or 4)");
            //int N = Int32.Parse(Console.ReadLine());
            Console.WriteLine("type length of side");
            int L = Int32.Parse(Console.ReadLine());
            DNA dna = new DNA(4, L);
            dna.SeedPrint();
            //string path = "C:/Users/Void/Downloads/Seed[].txt";
            //dna.SeedOut(path);

            Console.WriteLine();
            //dna.SeedOut();
            //dna.SpinOut();

            ////testing alg speed 
            //Stopwatch stopwatch = new Stopwatch();
            //for (int repeat = 0; repeat < 1; ++repeat)
            //{
            //    stopwatch.Reset();
            //    stopwatch.Start();
            //    //dna.SeedOut();
            //    //dna.SpinOut();

            //    //int[] bla = new int[] { 13, 2, 15, 12, 53, 18, 43, 32, 57, 46, 35, 60, 25, 62, 63, 64 };
            //    //dna.SpinOut(bla);
            //    //dna.JustSpin();

            //    stopwatch.Stop();
            //    Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks +
            //    " mS: " + stopwatch.ElapsedMilliseconds);
            //}

            //Need to have Lsys class inherit all field values from parent
            //but, for the moment, I'll just build Lsys to process individual strands from input or file
            LsysSq lsys= new LsysSq();

            int[] test1 = new int[] { 1, 2, 15, 16 };
            int[] test2 = new int[] { 1, 2, 3, 13, 14, 15, 16, 17, 36 };
            int[] test3 = new int[] { 1, 2, 3, 4, 5, 6, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54 };
            //Console.WriteLine(lsys.testStrand(test1));
            //Console.WriteLine(lsys.testStrand(test2));
            //Console.WriteLine(lsys.testStrand(test3));

            //Console.WriteLine("type length of side");
            //int l2 = Int32.Parse(Console.ReadLine());
            //lsys.SeedPrint(l2);
            try
            {
                lsys.sideLength = 4;
                Console.WriteLine("Ls = " + lsys.sideLength);
                lsys.ToLsys(test2);
                //"E:/For_Tony", "DNAsq4.txt"
                Console.WriteLine();

            //print all converted DNA strands in file to Lsys strands
                string line;
                string RNA;
                string inPath = "C:/Users/Void/Downloads/For_Tony/DNAsq3/DNAsq3.txt";
                string outPath = "D:/Cabinet/Utilities/¬Coding/FractInt for Windows/lsystem/DNA_work_folder/HTDNA";

                ////testing alg speed 
                Stopwatch stopwatch = new Stopwatch();
                for (int repeat = 0; repeat < 1; ++repeat)
                {
                    stopwatch.Reset();
                    stopwatch.Start();

                    int[,] strandlist = dna.ListIn(inPath); //convert .txt to int[,] List
                    lsys.ToLfile(strandlist, outPath);//send List to Lsys for HTDNA.l output


                    stopwatch.Stop();
                    Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks +
                    " mS: " + stopwatch.ElapsedMilliseconds);
                }

            //Fractint CMD automation testing area!
                
                
                Stopwatch stopwatch2 = new Stopwatch();
                for (int repeat = 0; repeat < 1; ++repeat)
                {
                    stopwatch2.Reset();
                    stopwatch2.Start();

                    //feed .l file path,
                    //

                    stopwatch2.Stop();
                    Console.WriteLine("Ticks: " + stopwatch2.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);
                }

                



            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
