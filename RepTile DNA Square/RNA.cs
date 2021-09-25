using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RepTile_DNA_Square
{
    //Plan: map all holons from chosen strand to the L-system according to the formulae Tony made for 
    //different side lengths found in file HTDNA.l
    /// <summary>
    /// Converts DNA strand to L-system instructions.
    /// </summary>
    class LsysSq
    {
        private int Ls;
        private string RNA;
        private string RNAsq2 = "G+g|g+G+G+g|g+G+G+g|g+G+G+g|g+G+GG";
        private string RNAsq3 = "G+gg|gg+G+g|g+G+G+gg|gg+G+g|g+G+G+gg|gg+G+g|g+G+G+gg|gg+G+g|g+G+GGG";  //altered
        private string RNAsq4 = "G+gggg|gggg+G+gg|gg+GG+G+gggg|gggg+G+gg|gg+GG+G+gggg|gggg+G+gg|gg+GG+G+gggg|gggg+G+gg|gg+GG+GGGG";
        private string RNAsq5 = "G+ggggg|ggggg+G+ggggg|ggggg+GGG+G+ggggg|ggggg+G+ggggg|ggggg+GGG+G+ggggg|ggggg+G+ggggg|ggggg+GGG+G+ggggg|ggggg+G+ggggg|ggggg+GGG+GGGGG";
        private string RNAsq6 = "G+gggggg|gggggg+G+gggggg|gggggg+G+ggg|ggg+GGG+G+gggggg|gggggg+G+gggggg|gggggg+G+ggg|ggg+GGG+G+gggggg|gggggg+G+gggggg|gggggg+G+ggg|ggg+GGG+G+gggggg|gggggg+G+gggggg|gggggg+G+ggg|ggg+GGG+GGGGGG";
        private int[] Dist;
        readonly int[] Distsq2 = new int[] { 1, 9, 10, 2, 3, 11, 12, 4, 5, 13, 14, 6, 7, 15, 16, 8 };
        readonly int[] Distsq3 = new int[] { 1, 13, 15, 16, 14, 2, 17, 18, 3, 4, 19, 21, 22, 20, 5, 23, 24, 6, 7, 25, 27, 28, 26, 8, 29, 30, 9, 10, 31, 33, 34, 32, 11, 35, 36, 12 };
        readonly int[] Distsq4 = new int[] { 1, 17, 19, 21, 23, 24, 22, 20, 18, 2, 25, 27, 28, 26, 3, 4, 5, 29, 31, 33, 35, 36, 34, 32, 30, 6, 37, 39, 40, 38, 7, 8, 9, 41, 43, 45, 47, 48, 46, 44, 42, 10, 49, 51, 52, 50, 11, 12, 13, 53, 55, 57, 59, 60, 58, 56, 54, 14, 61, 63, 64, 62, 15, 16 };
        //readonly int[] Distsq5 = new int[] { }; //Why not order the holon numbering according to the    Lsystem patterns in the beginning?
        //readonly int[] Distsq5 = new int[] {1, };
        private int[,] Seed;
        readonly int[,] Seedsq2 = new int[,]
        {
            {1,2,9,10},
            {3,4,11,12},
            {5,6,13,14},
            {7,8,15,16},
        };
        readonly int[,] Seedsq3 = new int[,]
        {
            { 1,2,3,13,14,15,16,17,18},
            { 4,5,6,19,20,21,22,23,24},
            { 7,8,9,25,26,27,28,29,30},
            { 10,11,12,31,32,33,34,35,36}
        };
        readonly int[,] Seedsq4 = new int[,]
        {
            {1,2,3,4,17,18,19,20,21,22,23,24,25,26,27,28},
            {5,6,7,8,29,30,31,32,33,34,35,36,37,38,39,40},
            {9,10,11,12,41,42,43,44,45,46,47,48,49,50,51,52},
            {13,14,15,16,53,54,55,56,57,58,59,60,61,62,63,64}
        };
        readonly int[,] Seedsq5 = new int[,]
        {
            {1,2,3,4,5,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40},
            {6,7,8,9,10,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60},
            {11,12,13,14,15,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80},
            {16,17,18,19,20,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100}
        };
        readonly int[,] Seedsq6 = new int[,]
        {
            {1,2,3,4,5,6,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54},
            {7,8,9,10,11,12,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84},
            {13,14,15,16,17,18,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114},
            {19,20,21,22,23,24,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144}
        };
        public int counter;

        public LsysSq() { } //

        /// <summary>
        /// consider removal.  I developed this with a loop in Program.cs in mind.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string ToLsys(string line) //converts line from file to array and sends on to toLsys
        {
            string[] linestr;
            linestr = line.Split(','); //convert string to string[]
            int[] strand = new int[linestr.Length];
            for(int i = 0; i < strand.Length; i++) 
                strand[i] = Int32.Parse(linestr[i]);
            ToLsys(strand);
            return RNA;
        }
        public string ToLsys(int[] strand) //output or return L-system script from input strand
        {
            int l = strand.Length;
            for (int i = 1; i * i <= l; i++) this.Ls = i; //find square root

            int lr;
            switch (Ls) //Switch must GUARANTEE lr will have a value.
            {
                case 2:
                    //this.Seed = this.Seedsq2;
                    lr = this.RNAsq2.Length;
                    RNA = RNAsq2;
                    this.Dist = this.Distsq2;
                    break;
                case 3:
                    //this.Seed = this.Seedsq3;
                    lr = this.RNAsq3.Length;
                    RNA = RNAsq3;
                    this.Dist = this.Distsq3;
                    break;
                case 4:
                    //this.Seed = this.Seedsq4;
                    lr = this.RNAsq4.Length;
                    RNA = RNAsq4;
                    this.Dist = this.Distsq4;
                    break;
                //case 5:
                //    //this.Seed = this.Seedsq5;
                //    rna = this.RNAsq5;
                //    this.Dist = this.Distsq5;
                //    break;
                //case 6:
                //    //this.Seed = this.Seedsq6;
                //    rna = this.RNAsq6;
                //    this.Dist = this.Distsq6;
                //    break;
                default:
                    lr = 0;
                    break;
            }
            char[] rna = new char[lr]; //temp holds RNA script
            bool v = testStrand(strand);
            if (v == false) throw new Exception("Input strand is invalid.");

            StringBuilder sb = new StringBuilder(rna.Length);
            int r = 0; //only used to track Dist's position
            bool m = false; //match found this round?
            for (int i = 0; i < this.RNA.Length; i++) //moves through all potential holons in Dist & RNA and marks the holons present in the pattern (changes gs to fs in RNA)
            {
                //if (this.RNA[r] == '+' || this.RNA[r] == '|') r++;
                while(this.RNA[i] == '+' || this.RNA[i] == '|') 
                {
                    sb.Append(RNA[i]);
                    i++;
                }
                for (int k = 0; /*strand[k] != this.Dist[i] &&*/ k < strand.Length; k++) //runs until match is found or the end is reached
                {
                    if (strand[k] == this.Dist[r]) //if match is found, append f
                    {
                        if (RNA[i] == 'g') sb.Append("f");
                        if (RNA[i] == 'G') sb.Append("F");
                        r++;
                        m = true;
                        break;
                    }
                }
                if (m == false)
                {
                    if (RNA[i] == 'g') sb.Append("g"); //if no match is found, append g
                    else sb.Append("G");
                    r++;
                }
                if (r == Dist.Length - 1) //RNA is longer than dist, mind the extra G's.
                {
                    i++;
                    while (i < RNA.Length)
                    {
                        sb.Append(RNA[i]);
                        i++;
                    }
                    break;
                }
                m = false;
            }
            RNA = sb.ToString();
            return RNA;
        }
        public void ToLfile(int[,] sList, string outPath)
        {
            int lfn = 1; //.l file number
            int iter = 1; //iteration
            this.counter = 1;
            int fln = sList[0, 0]; //number of lines
            int len = sList[0, 1]; //line length
            int[] strand = new int[len];
            string rna;
            StringBuilder sb = new StringBuilder();
            int sr;
            switch(strand.Length)
            {
                case 4: { sr = 2; break; }
                case 9: { sr = 3; break; }
                case 16: { sr = 4; break; }
                case 25: { sr = 5; break; }
                case 36: { sr = 6; break; }
                default: throw new Exception("Invalid number of Holons.");
            }
            string path = outPath + "sq" + sr + "." + lfn + ".l";
            Console.WriteLine("len = " + len + ", fln = " + fln);
            string header = "_README {" + Environment.NewLine +
                            "  Angle 4" + Environment.NewLine +
                            "  Axiom f" + Environment.NewLine +
                            "  f=f " + Environment.NewLine +
                            "}";
            sb.AppendLine(header);
            for (int i = 1; i < fln + 1; i++)
            {
                if (iter == 2000) //switch file path every 2000 entries
                {
                    lfn++;
                    File.WriteAllText(path, sb.ToString()); //must write THEN update path
                    path = outPath + "sq" + sr + "." + lfn + ".l";                    //update file path

                    iter = 0; //reset iteration
                }
                //move sList[,] row to strand[]
                for (int k = 0; k < len; k++)
                { strand[k] = sList[i, k];}
                
                //convert strand to RNA
                rna = ToLsys(strand); //Lsys string will always be the same length, assuming all strands have the same Ls, so it's fine
                sb.AppendLine(ToLentry(rna));

                iter++;
                counter++;
                //Console.WriteLine("counter = "+counter + " , iter =" + iter);
                if (lfn == 5) break;
            }
            File.WriteAllText(path, sb.ToString());

            //close the file

        }
        /// <summary>
        /// prepares Lsys script for entry in .l file
        /// </summary>
        /// <returns></returns>
        public string ToLentry(string rna)
        {
            int l = rna.Length;
            string entry;
            switch (l)
            {
                case 34:
                    {
                        entry = "sq2DNA" + counter + " {  " + Environment.NewLine +
                            "  Angle 4 " + Environment.NewLine + 
                            "  Axiom c9f+c47f+c12f+c14f" + Environment.NewLine + "  f=" +
                            rna + Environment.NewLine + "  g=gg" + Environment.NewLine +
                            "}" + Environment.NewLine;
                        return entry;
                    }
                case 67:
                    {
                        entry = "sq3DNA" + counter + " {  ; Anthony Hanmer 17/11/2002" + Environment.NewLine +
                            "  Angle 4 ; Key to all 3x3 Square HTs" + Environment.NewLine +
                            "  Axiom c9f+c47f+c12f+c14f" + Environment.NewLine + "  f=" +
                            rna + Environment.NewLine + "  g=ggg" + Environment.NewLine +
                            "}" + Environment.NewLine;
                        Console.WriteLine(counter);

                        return entry;
                    }
                case 96:
                    {
                        entry = "sq4DNA" + counter + " {  ; Anthony Hanmer 17/11/2002" + Environment.NewLine +
                            "  Angle 4 ; Key to all 4x4 Square HTs" + Environment.NewLine +
                            "  Axiom c9f+c47f+c12f+c14f" + Environment.NewLine + "  f=" +
                            rna + Environment.NewLine + "  g=gggg" + Environment.NewLine +
                            "}" + Environment.NewLine;
                        return entry;
                    }
                case 133:
                    {
                        entry = "sq5DNA" + counter + " {  ; Anthony Hanmer 17/11/2002" + Environment.NewLine +
                            "  Angle 4 ; Key to all 5x5 Square HTs" + Environment.NewLine +
                            "  Axiom c9f+c47f+c12f+c14f" + Environment.NewLine + "  f=" +
                            rna + Environment.NewLine + "  g=ggggg" + Environment.NewLine +
                            "}" + Environment.NewLine;
                        return entry;
                    }
                case 190:
                    {
                        entry = "sq6DNA" + counter + " {  ; Anthony Hanmer 17/11/2002" + Environment.NewLine +
                            "  Angle 4 ; Key to all 6x6 Square HTs" + Environment.NewLine +
                            "  Axiom c9f+c47f+c12f+c14f" + Environment.NewLine + "  f=" +
                            rna + Environment.NewLine + "  g=gggggg" + Environment.NewLine +
                            "}" + Environment.NewLine;
                        return entry;
                    }
                default:
                    throw new Exception("Error, invalid Lsys string.");
            }
        }
        public int sideLength
        {
            get
            {
                return this.Ls;
            }
            set
            {
                if (value < 2 || value > 6) throw new Exception("input out of range, please set a side length from 2 to 6");
                this.Ls = value;
            }
        }
        /// <summary>
        /// Creates & returns an int[] array from a single line of a 2d int[,] array
        /// !Still needs testing!
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public int[] LineSelect(int[,] List, int row) //should I make this generic? I'll just finish it first. I need the functionality.
        { //importing the entire list every time seems excessive. massive waste of ram usage???
            //eh, maybe I'll create it anyway with a disclaimer not to use it in long loops
            int l = List.Length;
            int[] line = new int[l];
            for (int i = 0; i < l; i++)
                line[i] = List[row, i];
            for (int i = 0; i < l; i++)
                Console.Write(line[i] + ",");
            return line;
        }
        public void LsSelect(int[] strand) //test what Ls class 
        {
            int l = 1;
            for (int i = 0; strand[i] != 0; i++) l++;
            if (l == 4) this.Ls  = 2;
            if (l == 9) this.Ls  = 3;
            if (l == 16) this.Ls  = 4;
            if (l == 25) this.Ls  = 5;
            if (l == 36) this.Ls  = 6;
            else Console.WriteLine("Invalid strand detected. Incorrect number of holons.");
        }
        public bool testStrand(int[] strand) //strand clan/holon order/format valid?
        {
            int l = strand.Length;
            bool v = false;
            if (l == 4 || l == 9 || l == 16 || l == 25 || l == 36) v = true;
            else
            {
                Console.WriteLine("Invalid strand detected. Incorrect number of holons.");
                return v;
            }
            //compare to Seed table. Does each strand index have an equivalent member in that clan?
            bool c = false;
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < 4; j++) //strand entry MUST match ONE clan member
                {
                    switch (l)
                    {
                        case 4:
                            if (strand[i] == this.Seedsq2[j, i]) c = true;
                            break;
                        case 9:
                            if (strand[i] == this.Seedsq3[j, i]) c = true;
                            break;
                        case 16:
                            if (strand[i] == this.Seedsq4[j, i]) c = true;
                            break;
                        case 25:
                            if (strand[i] == this.Seedsq5[j, i]) c = true;
                            break;
                        case 36:
                            if (strand[i] == this.Seedsq6[j, i]) c = true;
                            break;
                    }
                }
                if (c == false)
                {
                    Console.WriteLine("Invalid Strand. Incorrect Holon entry detected at strand[" + i + "].");                    
                    return false;   //ends loops returns Invalid state
                }
                else c = false;               //if this entry is valid, reset c to false
            }
            return true;
        }
        public void SeedPrint(int Ls )
        {
            this.Ls  = Ls ;
            int l = this.Ls  * this.Ls ;
            int[,] Seed;  //can declare and assign arrays separately

            switch (this.Ls )
            {
                case 2:
                    Seed = this.Seedsq2;
                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write("{");
                        for (int i = 0; i < l; i++)
                        {
                            Console.Write(Seed[j, i]);
                            if (i < l - 1) Console.Write(",");
                        }
                        Console.WriteLine("}");
                    }
                    break;
                case 3:
                    Seed = this.Seedsq3;
                    for(int j = 0; j < 4; j++)
                    {
                        Console.Write("{");
                        for (int i = 0; i < l; i++)
                        {
                            Console.Write(Seed[j, i]);
                            if (i < l - 1) Console.Write(",");
                        }
                        Console.WriteLine("}");
                    }
                    break;
                case 4:
                    Seed = this.Seedsq4;
                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write("{");
                        for (int i = 0; i < l; i++)
                        {
                            Console.Write(Seed[j, i]);
                            if (i < l - 1) Console.Write(",");
                        }
                        Console.WriteLine("}");
                    }
                    break;
                case 5:
                    Seed = this.Seedsq5;
                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write("{");
                        for (int i = 0; i < l; i++)
                        {
                            Console.Write(Seed[j, i]);
                            if (i < l - 1) Console.Write(",");
                        }
                        Console.WriteLine("}");
                    }
                    break;
                case 6:
                    Seed = this.Seedsq6;
                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write("{");
                        for (int i = 0; i < l; i++)
                        {
                            Console.Write(Seed[j, i]);
                            if (i < l - 1) Console.Write(",");
                        }
                        Console.WriteLine("}");
                    }
                    break;
            }
        }


    }
}
