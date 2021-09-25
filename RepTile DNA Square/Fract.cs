using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;  //huh, it's not here

namespace RepTile_DNA_Square
{
    /// <summary>
    /// automates Fractint operations from premade list files.  Should organize methods around the idea of command scripts.
    /// todo: integrate into DNA or RNA later?
    /// </summary>
    class Fract
    {
        private string programPath;
        private string vidMode;

        public Fract(string program)
        {
            programPath = program;
        }

        //public int mode
        //{
        //    get
        //    {
        //        return vidmode;
        //    }
        //    set 
        //    {

        //    }
        //}
        public void Feed(string list)
        {

        }
        /// <summary>
        /// generate fractal image gifs for list entries
        /// </summary>
        public void ImgGen(string file, string outPath) //actually, I may not need the .l path, Fractint already knows that
        {
            char p = '"';
            string cmdtxt = "D:/Cabinet/Utilities¬Coding/" + p + "FractInt for Windows+" + p + "/fractint.exe";
            System.Diagnostics.Process.Start(cmdtxt); //open Fractint.exe
            
            //press <t>,<l>,<s>,Enter,<F6>,<D><N><A>,Enter
            //type file name, Enter
        }
    }
}
