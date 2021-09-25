using System;
using System.Collections.Generic;
using System.Text;

namespace RepTile_DNA_Square
{
    class ComboCruncher  //this class computes all possible full combinations of n sets of equivalent length
    {
        private int n;
        private int l;
        private int[] list;
        public ComboCruncher(int n)
        {
            this.n = n;
        }
        public int[] Sets
        {
            get { return this.list; }
            set
            {
                this.l = value.Length;
                int[] list = new int[this.l*this.n];
                list = value;
            }
        }
        public string PrintSets()
        {
            StringBuilder str = new StringBuilder();
            for (int n = 1; n < this.n; n++)
            {
                str.Append("{");
                for (int i = 0; i < this.l*n; i++)
                {
                    str.Append(this.list[i]);
                    if (i != this.l * n - 1) str.Append(",");
                }
                str.AppendLine("}");
            }
            return str.ToString();
        }

    }
}
