using System;
using System.Collections.Generic;
using System.Text;
using System.IO;  //allows reading/writing to/from a text file

namespace RepTile_DNA_Square
{
    class DNA
    {
        private int Ls;     //length of sides
        private int Ns; //number of sides
        private int Ht;
        private int He;
        private int Hi;
        private int Sc; //clan spacing, or distange to cosymmetrical holon
        private int Nc;
        private int[,] Seed;  //now I can easily access the table without writing to a file
        public DNA(int N, int L)
        {
            this.Ls = L;
            this.Ns = N;
            this.Ht = Ls * Ls * Ns;  //Total holons = length of side squared times number of sides
            this.He = Ls * Ns;
            this.Hi = this.Ht - this.He;
            this.Sc = this.Hi / this.Ns;
            this.Nc = Ls + Sc;

            Console.WriteLine("Ht = " + Ht);
            Console.WriteLine("He = " + He);
            Console.WriteLine("Hi = " + Hi);
            Console.WriteLine("Sc = " + Sc);

            
            this.Seed = new int[Ns, Nc]; //seed table includes all holon values
            //make Seed dial/clan table
            int row = 0;
            int col = 0;
            for (row = 0; row < Ns; row++) ////generate He values.
            {
                Seed[row, 0] = row * Ls + 1; //gen Seed[row,0] values. //only works with Ls=4!!! >:(
                for (col = 1; col < Ls; col++) Seed[row, col] = Seed[row, col - 1] + 1; //other He values

                Seed[row, col] = Ns * Ls + 1 + row * Sc; //establish 1st Hi
                while (col + 1 < Nc)  //gen other Hi's in row.  I chose while() because the last for() finishes with the correct col value
                {
                    col++;
                    Seed[row, col] = Seed[row, col - 1] + 1;
                }
            }
        }
        public void SpinOut() //algorithm that outputs all possible strands to .txt
        {
            int[] indexes = new int[Nc];
            for (int i = 0; i < Nc; i++) indexes[i] = 0;
            int n = Nc;
            for (int i = 0; i < n; i++) indexes[i] = 0;

            StringBuilder sb = new StringBuilder(); //need to modify to write to .txt every several hundred k.
            //string txtPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DNA().txt");
            string txtPath = Path.Combine("E:/For_Tony", "DNAsq4.txt");
            File.WriteAllText(txtPath, "");
            float count = 0;
            int pwr = 0;
            int millions = 0;
            int nth = 1; //used to count number of individual powers in the millions loop

            while (true)
            {
                // store current combination
                for (int i = 0; i < Nc; i++)
                {
                    sb.Append(Seed[indexes[i], i]);
                    if (i < Nc - 1) sb.Append(",");
                }
                sb.AppendLine();
                
                int next = Nc - 1;
                while (next >= 0 && (indexes[next] + 1 >= 4))//size of any column is 4/*Seed[next,4]*//*arr[next].Count*/)) //HERE is my problem with applying this alg.  I don't know what arr[next].Count means or accomplishes.
                {
                    next--;
                }

                if (next < 0)
                    break;

                indexes[next]++;

                for (int i = next + 1; i < n; i++)
                    indexes[i] = 0;
                count++;

                //manage count and offload memory to txt, add power 6 first time and 1 every 10th iteration hereafter.
                if (count % 1000000 == 0)
                {
                    millions++;
                    File.AppendAllText(txtPath, sb.ToString()); //dump memory constructively
                    sb.Clear();                                 //reset stringbuilder.
                    if (millions == 1) pwr += 6;
                    if (millions % 10 == 0 && millions != 0)
                    {
                        if (nth % 10 == 0) 
                        {
                            pwr++; //only gains a power once whenever new zero is added
                            nth = 1;
                        }
                        nth++; //DON'T mess with n, it's used elsewhere in the loop!!!
                    }
                    if (millions % 100 == 0 && millions != 0) Console.WriteLine("pwr = " + pwr + " , millions = " + millions);
                    count = 0;
                }
            }
            Console.WriteLine("count = " + count + " , pwr = " + pwr);

            File.AppendAllText(txtPath, sb.ToString());
            //float sigdigits = 0;
            for(int i = 0; count>=9; i++)
            {
                //if (i > 0) sigdigits /= 10;
                //sigdigits += count % 10;
                count /= 10;
                pwr += 1;
            }
            //sigdigits /= 10;
            //float digital = 
            Console.WriteLine("count = " + count /*+ sigdigits*/ + " * 10^" + pwr);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public void SpinOut(int[] indexes)  //generates remaining strands from starting point
        //{
        //    //int[] indexes = new int[Nc];
        //    //for (int i = 0; i < Nc; i++) indexes[i] = 0;
        //    int n = Nc;
        //    //for (int i = 0; i < n; i++) indexes[i] = 0;

        //    StringBuilder sb = new StringBuilder();
        //    //string txtPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DNA().txt");
        //    string txtPath = Path.Combine("E:/For_Tony", "DNAsq4.2.txt");
        //    File.WriteAllText(txtPath, "");
        //    float count = 0;
        //    //int pwr = 0;
        //    //int millions = 0;
        //    //int nth = 1;

        //    while (true)
        //    {
        //        // Print current combination
        //      //  for (int i = 0; i < Nc; i++)
        //        //{
        //        //    sb.Append(Seed[indexes[i], i]);
        //        //    if (i < Nc - 1) sb.Append(",");
        //        //}
        //        //sb.AppendLine();

        //        //int next = Nc - 1;
        //        //while (next >= 0 && (indexes[next] + 1 >= 4))//size of any column is 4/*Seed[next,4]*//*arr[next].Count*/)) //HERE is my problem with applying this alg.  I don't know what arr[next].Count means or accomplishes.
        //        //{
        //        //    next--;
        //        //}

        //        //if (next < 0)
        //        //    break;

        //        //indexes[next]++;

        //        //for (int i = next + 1; i < n; i++)
        //        //    indexes[i] = 0;
        //        //count++;

        //        //manage count and offload memory to txt, add power 6 first time and 1 every 10th iteration hereafter.
        //        if (count % 1000000 == 0)
        //        {
        //            //millions++;
        //            File.AppendAllText(txtPath, sb.ToString()); //dump memory constructively
        //            sb.Clear();                                 //reset stringbuilder.
        //            //if (millions == 1) pwr += 6;
        //            //if (millions % 10 == 0 && millions != 0)
        //            //{
        //            //    if (nth % 10 == 0)
        //            //    {
        //            //        pwr++; //only gains a power once whenever new zero is added
        //            //        n = 1;
        //            //    }
        //            //    n++;
        //            //}
        //            //if (millions % 100 == 0 && millions != 0) Console.WriteLine("pwr = " + pwr + " , millions = " + millions);
        //            count = 0;
        //        }
        //    }
        //Console.WriteLine("count = " + count + " , pwr = " + pwr);

        //    File.AppendAllText(txtPath, sb.ToString());
        //    //float sigdigits = 0;
        //    for (int i = 0; count >= 9; i++)
        //    {
        //        //if (i > 0) sigdigits /= 10;
        //        //sigdigits += count % 10;
        //        count /= 10;
        //    //    pwr += 1;
        //    }
        //    //sigdigits /= 10;
        //    //float digital = 
        ////    Console.WriteLine("count = " + count /*+ sigdigits*/ + " * 10^" + pwr);
        //}
        public void JustSpin() //tests alg without writing to file
        {
            int[] indexes = new int[Nc];
            for (int i = 0; i < Nc; i++) indexes[i] = 0;
            int n = Nc;
            for (int i = 0; i < n; i++) indexes[i] = 0;

            float count = 0;
            int pwr = 0;
            int millions = 0;
            int nth = 10; //used to count number of individual powers in the millions loop

            while (true)
            {
                int next = Nc - 1;
                while (next >= 0 && (indexes[next] + 1 >= 4))//size of any column is 4/*Seed[next,4]*//*arr[next].Count*/)) //HERE is my problem with applying this alg.  I don't know what arr[next].Count means or accomplishes.
                {
                    next--;
                }
                if (next < 0)
                    break;
                indexes[next]++;

                for (int i = next + 1; i < n; i++)
                    indexes[i] = 0;
                count++;

                //manage count and offload memory to txt, add power 6 first time and 1 every 10th iteration hereafter.
                if (count % 1000000 == 0)
                {
                    millions++;
                    if (millions == 1) pwr += 6;
                    if (millions == nth && millions != 0)
                    {
                        pwr++; //only gains a power once whenever new zero is added
                        nth*=10; 
                    }
                    if (millions % 100 == 0 && millions != 0) Console.WriteLine("pwr = " + pwr + " , millions = " + millions);
                    count = 0;
                }
            }
            Console.WriteLine("count = " + count + " , pwr = " + pwr);

            for (int i = 0; count >= 9; i++)
            {
                count /= 10;
                pwr += 1;
            }
            Console.WriteLine("count = " + count /*+ sigdigits*/ + " * 10^" + pwr);
        }

        public int[,] ListIn(string inPath)
        {
            int fln = 0; //final line number
            //bool z 
            //= File.ReadLines(inPath).Contains(',');
            //const char comma = ',';
            //while(z==true) 
            //{
            //    File.ReadLines(inPath);
            //    if (line2.Contains(","))
            //    fl++;
            //}
            StreamReader sr = new StreamReader(inPath);
            string line = sr.ReadLine();
            int l = line.Length;
            int en = 1; //List entry number
            for (int i = 0; i < l; i++)
                if (line[i] == ',') en++;
            Console.WriteLine(en);
            foreach (string line2 in File.ReadLines(inPath)) //Find final line number in file
            {
                if (line2.Contains(",")) fln++;
            }
            int[,] List = new int[fln + 1, en];  //+1 because the first line of List will be its dimensions
            List[0, 0] = fln;
            List[0, 1] = en;
            string[] str = line.Split(',');

            for (int i = 1; i < fln + 1; i++)
            {
                str = line.Split(',');
                for (int b = 0; b < str.Length; b++)
                    List[i, b] = Int32.Parse(str[b]);
                line = sr.ReadLine();
            }
            return List;
        }

        public void SeedPrint()
            {
            for (int row = 0; row < Ns; row++)
            {
                int Nc = Ls + Sc;
                Console.Write("{");
                for (int col = 0; col < Nc; col++)  //Ht/Ns = Sc
                {
                    Console.Write(Seed[row, col]);
                    if (col < Nc - 1) Console.Write(",");
                }
                Console.WriteLine("}");
            }
        }
        public void SeedOut() //outputs this.Seed to default file path
        {
            StringBuilder sb = new StringBuilder();
            string txtPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Seedsq" + Ls + ".txt");
            for (int row = 0; row < Ns; row++)
            {
                int Nc = Ls + Sc;
                sb.Append("{");
                for (int col = 0; col < Nc; col++)  
                {
                    sb.Append(Seed[row, col]);
                    if (col < Nc - 1) sb.Append(",");
                }
                sb.AppendLine("}");
            }
            File.WriteAllText(txtPath, sb.ToString());
        }
        public void SeedOut(string path) //outputs this.Seed to custom file path
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < Ns; row++)
            {
                int Nc = Ls + Sc;
                sb.Append("{");
                for (int col = 0; col < Nc; col++)  //Ht/Ns = Sc
                {
                    sb.Append(Seed[row, col]);
                    if (col < Nc - 1) sb.Append(",");
                }
                sb.AppendLine("}");
            }
            File.WriteAllText(path, sb.ToString());
        }



        public void ComboGen1() //my 1st working combo generator. Does not cover all possibilities and uses a 1d array that is confusing to work with.
        {
            //Here is where we generate the strands to a StringBuilder instance and output it to a text file all at once
            StringBuilder stringbuilder = new StringBuilder();
            string txtPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RepTile DNA 2.txt");
            int clans = Ls + (Hi / Ns);
            Console.WriteLine("clans = " + clans);
            int[] strand = new int[clans]; //in case Ls==4, clans==16

            //build 1st strand
            for (int i = 0; i < Ls; i++) strand[i] = i + 1; //if Ls==4, indexes 0-3 hold values 1-4
            int n = Ls;
            for (int i = Ls * Ns; i < Ls * Ns + Sc; i++)      //if Ls==4, strand[4] = 16 (5th slot); final value is i=19 with strand[16]=32.
            {
                strand[n] = i + 1;
                n += 1;
            }

            //Print 1st strand
            Console.WriteLine("");
            //            for (int y = 0; y < clans; y++) stringbuilder.AppendLine(Convert.ToString(strand[y]));
            //            File.AppendAllText(txtPath, stringbuilder.ToString());  //Append 1st line to file
            for (int y = 0; y < clans; y++)
            {
                if (y < clans - 1) Console.Write(Convert.ToString(strand[y]) + ",");
                else Console.Write(Convert.ToString(strand[y]));
            }
            Console.WriteLine("");

            //spin the bike lock!
            n = 0;
            int j;
            //            while (complete == false)       //DNA combo generation loops
            //          {
            for (int e = 0; e < Ns - 1; e++) //switch He dials
            {
                for (int e2 = 0; e2 < Ns - 1; e2++)  //spin He dials
                {
                    for (j = Ns; j < clans; j++)   //j starts at 4(index 5), meant to skip He dials
                    {
                        for (int j2 = 0; j2 < Ns - 1; j2++) //individual Hi dial spin, jump by Sc(12) for Ns(4) iterations
                        {
                            strand[j] += Sc;       //starts at 5th index, or strand[4+0], 1st cycle: 17 -> 29 -> 41 -> 53
                                                   //                           for (int y = 0; y < clans; y++) stringbuilder.AppendLine(Convert.ToString(strand[y])); //Creates one DNA strand every time a dial spins
                            for (int y = 0; y < clans; y++)
                            {
                                if (y < clans - 1) Console.Write(Convert.ToString(strand[y]) + ",");
                                else Console.Write(Convert.ToString(strand[y]));
                            }
                            Console.WriteLine("");
                        }
                    }
                    strand[e] += Ns;
                    for (int y = 0; y < clans; y++) stringbuilder.AppendLine(Convert.ToString(strand[y])); //Creates one DNA strand every time a dial spins
                    for (int y = 0; y < clans; y++)
                    {
                        if (y < clans - 1) Console.Write(Convert.ToString(strand[y]) + ",");
                        else Console.Write(Convert.ToString(strand[y]));
                    }
                    Console.WriteLine("");
                    File.AppendAllText(txtPath, stringbuilder.ToString());
                }

            }
        }            //                if (He1 == 9)      //loop terminator
                //            }
                

        //}

        //    int i = this.Ls;
        //    //Console.WriteLine("do you want to print a chart?");
        //    //if (Console.ReadLine() == "y") HCountChart(Ns);
        //    //else Console.WriteLine("You don't scare me.  Try harder!");

        //    //the first n columns in Dna are Exterior holons.
        //    //the next Ht/4 columns are Interior holons  





        //    //Here is where we generate the strands to a StringBuilder instance and output it to a text file all at once
        //    StringBuilder stringbuilder = new StringBuilder();
        //    string txtPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RepTile DNA 1.txt");
        //    //            int He1 = 1;
        //    //            bool complete = false;
        //    int clans = Ls + (Hi / Ns);
        //    Console.WriteLine("clans = " + clans);
        //    int[] strand = new int[clans]; //in case Ls==4, clans==16

        //    //build 1st strand
        //    for (i = 0; i < Ls; i++) strand[i] = i + 1; //if Ls==4, indexes 0-3 hold values 1-4
        //    int n = Ls;
        //    for (i = Ls * Ns; i < Ls * Ns + Sc; i++)      //if Ls==4, strand[4] = 16 (5th slot); final value is i=19 with strand[16]=32.
        //    {
        //        strand[n] = i + 1;
        //        n += 1;
        //    }

        //    //Print 1st strand
        //    Console.WriteLine("");
        //    //            for (int y = 0; y < clans; y++) stringbuilder.AppendLine(Convert.ToString(strand[y]));
        //    //            File.AppendAllText(txtPath, stringbuilder.ToString());  //Append 1st line to file
        //    for (int y = 0; y < clans; y++)
        //    {
        //        if (y < clans - 1) Console.Write(Convert.ToString(strand[y]) + ",");
        //        else Console.Write(Convert.ToString(strand[y]));
        //    }
        //    Console.WriteLine("");

        //    //spin the bike lock!
        //    n = 0;
        //    int j;
        //    //            while (complete == false)       //DNA combo generation loops
        //    //          {
        //    for (int e = 0; e < Ns - 1; e++) //switch He dials
        //    {
        //        for (int e2 = 0; e2 < Ns - 1; e2++)  //spin He dials
        //        {
        //            for (j = Ns; j < clans; j++)   //j starts at 4(index 5), meant to skip He dials
        //            {
        //                for (int j2 = 0; j2 < Ns - 1; j2++)    //individual Hi dial spin, jump by Sc(12) for Ns(4) iterations
        //                {
        //                    strand[j] += Sc;       //starts at 5th index, or strand[4+0], 1st cycle: 17 -> 29 -> 41 -> 53
        //                                           //                                for (int y = 0; y < clans; y++) stringbuilder.AppendLine(Convert.ToString(strand[y])); //Creates one DNA strand every time a dial spins
        //                    for (int y = 0; y < clans; y++)
        //                    {
        //                        if (y < clans - 1) Console.Write(Convert.ToString(strand[y]) + ",");
        //                        else Console.Write(Convert.ToString(strand[y]));
        //                    }
        //                    Console.WriteLine("");
        //                }
        //            }
        //            strand[e] += Ns;
        //            //                        for (int y = 0; y < clans; y++) stringbuilder.AppendLine(Convert.ToString(strand[y])); //Creates one DNA strand every time a dial spins
        //            for (int y = 0; y < clans; y++)
        //            {
        //                if (y < clans - 1) Console.Write(Convert.ToString(strand[y]) + ",");
        //                else Console.Write(Convert.ToString(strand[y]));
        //            }
        //            Console.WriteLine("");
        //        }

        //    }
        //    //                if (He1 == 9)      //loop terminator
        //    //            }
        //    //            File.AppendAllText(txtPath, stringbuilder.ToString());



        //    //Scan for duplicates in the list.  My clan methodology should render this impossible.  Order is irrelevant.
        //}

        //static void HCountChart(int Ns)
        //{
        //    //generate a list of Ls to holon counts
        //    StringBuilder SB = new StringBuilder();
        //    string txtPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RepTile Holon counts.txt");
        //    int Ls, Ht, He, Hi, Sc;
        //    for (Ls = 1; Ls < 101; Ls++)
        //    {
        //        Ht = Ls * Ls * Ns;
        //        He = Ls * Ns;
        //        Hi = Ht - He;
        //        Sc = Hi / Ns;

        //        SB.AppendLine("Ls = " + Ls + ",  Ht = " + Ht + ",  He = " + He + ",  Hi = " + Hi + ",  Sc = " + Sc);
        //    }
        //    File.WriteAllText(txtPath, SB.ToString());  //overwrites file if duplicate found
    }
}
