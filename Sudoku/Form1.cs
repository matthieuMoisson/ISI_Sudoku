using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        int difficulte = 20;
        static bool checkLigne(int lig, int val, int[,] grille)
        {
            for (int i = 0; i < 9; i++)
            {
                if (grille[i, lig] == val) return false;
            }
            return true;
        }

        static bool checkColone(int col, int val, int[,] grille)
        {
            for (int i = 0; i < 9; i++)
            {
                if (grille[col, i] == val) return false;
            }
            return true;
        }

        static bool checkBlock(int col, int lig, int val, int[,] grille)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (grille[col - (col % 3) + i, lig - (lig % 3) + j] == val) return false;
                }

            }
            return true;
        }

        static bool check(int col, int lig, int val, int[,] grille)
        {
            if (!checkLigne(lig, val, grille)) return false;
            if (!checkColone(col, val, grille)) return false;
            if (!checkBlock(col, lig, val, grille)) return false;
            return true;
        }
        
        static bool generateurDeGrille(int px, int py, int[,] grille)
        {
            /* 2 cas de base :
                    1) On a fini de replir le sudoku on peut donc sortir
                    2) On a pas fini on passe donc a la suivante on cherche 
                       les possibilité et on les teste toute
            */
            
            if (py > 8)
            {
                py = 0;
                px++;
            }
            //On est sortie du tableau on peut donc sortir
            if (px > 8)
            {
                return true;
            }
            else
            {
                //On a pas fini de remplir il faut donc lister toutes les possibilités :
                List<int> possibleNumbers = new List<int>();
                for (int i = 1; i <= 9; i++)
                {
                    if (check(px, py, i, grille))
                    {
                        possibleNumbers.Add(i);
                    }
                }
                if (possibleNumbers.Count != 0)
                {
                    Random rnd = new Random();
                    int[] value = possibleNumbers.OrderBy(b => rnd.Next()).ToArray();
                    foreach (int val in value)
                    {
                        grille[px, py] = val;
                        if (generateurDeGrille(px, py + 1, grille))
                        {
                            return true;
                        }
                        else
                        {
                            grille[px, py] = 0;
                        }
                    }
                }
                else
                {
                    return false; //Pas de solution on revient donc en arriere
                }
            }
            return false;
        }
        


        int[,] grilleComplete = new int[9, 9];
        int[,] grilleIncomplete = new int[9, 9];

        static int[,] cacheGrille(int Nb, int[,] g)
        {
            Random rnd = new Random();
            int[,] gBis = new int[9, 9];
            int px, py, i;
            
            for(i=0;i<=8;i++)
            {
                for(int j=0; j<=8; j++)
                {
                    gBis[i,j] = g[i, j];
                }
            }
            i = 0;
            
            while (i< Nb)
            {
                px = rnd.Next(0,9);
                py = rnd.Next(0,9);
                if (!(gBis[px, py] == 0))
                {
                    gBis[px, py] = 0;
                    i++;
                }
            }
            return gBis;
        }

        private TextBox organizeTextBox()
        {
            TextBox cas = new TextBox();
            cas.Font = new Font(cas.Font.FontFamily, 14);
            cas.TextAlign = HorizontalAlignment.Center;
            
            return cas;
        }

        public void affichage(int[,] grill)
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    TextBox cas = organizeTextBox();
                    cas.Font = new Font(cas.Font.FontFamily, 14);
                    cas.TextAlign = HorizontalAlignment.Center;
                    if ((grilleIncomplete[i, j] != 0))
                    {
                        cas.BackColor = Color.DarkGray;
                        cas.ForeColor = Color.Black;
                        cas.ReadOnly = true;
                        cas.Text = Convert.ToString(grilleIncomplete[i, j]);
                    }
                    else
                    {
                        cas.Text = "";
                    }
                    tableLayoutPanel1.Controls.Add(cas, i, j);
                }
            }
        }

        public Form1()
        {
            
            InitializeComponent();
            generateurDeGrille(0,0, grilleComplete);
            grilleIncomplete=cacheGrille(difficulte, grilleComplete);
            affichage(grilleIncomplete);
            tableLayoutPanel2.BackColor = Color.Black;
            tableLayoutPanel3.BackColor = Color.Black;
            tableLayoutPanel4.BackColor = Color.Black;
            tableLayoutPanel5.BackColor = Color.Black;
            tableLayoutPanel6.BackColor = Color.Black;
            tableLayoutPanel7.BackColor = Color.Black;
            tableLayoutPanel8.BackColor = Color.Black;
            tableLayoutPanel9.BackColor = Color.Black;
            tableLayoutPanel10.BackColor = Color.Black;
            tableLayoutPanel11.BackColor = Color.Black;
            tableLayoutPanel12.BackColor = Color.Black;
        }

        private TextBox resetTextBox(TextBox c)
        {
            c.BackColor = Color.White;
            c.Text = "";
            c.Font = new Font(c.Font.FontFamily, 14);
            c.TextAlign = HorizontalAlignment.Center;
            c.ForeColor = Color.Black;

            return c;
        }

        private void viderTab(int[,] tab)
        {
            for(int i = 0; i<=8; i++)
            {
                for(int j = 0; j<=8; j++)
                {
                    tab[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Buton 1 pour réaliser une nouvelle partie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            viderTab(grilleComplete);
            generateurDeGrille(0, 0, grilleComplete);
            difficulte = (int)numericUpDown1.Value;
            if(difficulte>81)
            {
                MessageBox.Show("Nombre de case a caché trop grand pour les nuls comme toi sa sera par default 20", "Erreur", MessageBoxButtons.OK);
                difficulte = 20;
                numericUpDown1.Value = difficulte;
            }
            grilleIncomplete = cacheGrille(difficulte, grilleComplete);
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    TextBox cas = (TextBox)tableLayoutPanel1.GetControlFromPosition(i,j);
                    cas = resetTextBox(cas);
                    if ((grilleIncomplete[i, j] != 0))
                    {
                        cas.BackColor = Color.DarkGray;
                        cas.ReadOnly = true;
                        cas.Text = Convert.ToString(grilleIncomplete[i, j]);
                    }
                    else
                    {
                        cas.Text = "";
                        cas.ReadOnly = false;
                    }
                }
            }
        }

        /// <summary>
        /// Boutton 2 pour corriger la grille
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void button2_Click(object sender, EventArgs e)
        {
            int valeur=1;
            bool valide = true;
            //On parcours l'ensemble de la grille si on voie une valeur fausse on mais en rouge
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    TextBox cas = (TextBox)tableLayoutPanel1.GetControlFromPosition(i,j);
                    if (cas.Text != "1" && cas.Text != "2" && cas.Text != "3" && cas.Text != "4" && cas.Text != "5" && cas.Text != "6" && cas.Text != "8" && cas.Text != "9" && cas.Text != "7")
                    {
                        cas.Text = "";
                        valide = false;
                    }

                    if (cas.Text != "")
                    {
                        valeur = int.Parse(cas.Text);
                        cas.ForeColor = System.Drawing.Color.Black;
                    }
                    if (valeur != grilleComplete[i, j])
                    {
                        cas.ForeColor = System.Drawing.Color.Red;
                        valide = false;
                    }
                    if (cas.Text == "")
                    {
                        valide = false;
                        cas.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            if (valide)
            {
                MessageBox.Show("Vous avez completé avec succées la grille", "Félicitation", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("La grille comporte des erreures ou est incomplete", "Attention", MessageBoxButtons.OK);
            } 
        }

        /// <summary>
        /// Button indice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            bool valide = true;
            Random rnd = new Random();
            int px, py;
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    TextBox cas = (TextBox)tableLayoutPanel1.GetControlFromPosition(i, j);
                    if (cas.Text != "1" && cas.Text != "2" && cas.Text != "3" && cas.Text != "4" && cas.Text != "5" && cas.Text != "6" && cas.Text != "8" && cas.Text != "9" && cas.Text != "7" && valide)
                    {
                        valide = false;
                    }
                }
            }
            while (!valide)
            {
                px = rnd.Next(0, 9);
                py = rnd.Next(0, 9);
                TextBox cas = (TextBox)tableLayoutPanel1.GetControlFromPosition(px, py);
                if (cas.Text != "1" && cas.Text != "2" && cas.Text != "3" && cas.Text != "4" && cas.Text != "5" && cas.Text != "6" && cas.Text != "8" && cas.Text != "9" && cas.Text != "7")
                {
                    valide = !valide;
                    cas.Text = Convert.ToString(grilleComplete[px, py]);
                    cas.ForeColor = System.Drawing.Color.Green;
                }
            }

        }
    }

        
}

