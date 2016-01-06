using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class fonction
    {
        
        public bool checkLigne(int lig, int val, int[,] grille)
        {
            for (int i = 0; i < 9; i++)
            {
                if (grille[i, lig] == val) return false;
            }
            return true;
        }
    }
}
