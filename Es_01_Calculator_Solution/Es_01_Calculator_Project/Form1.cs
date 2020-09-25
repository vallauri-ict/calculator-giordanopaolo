using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Es_01_Calculator_Project
{
    public partial class FormMain : Form
    {
        //private char[,] bottoni = new char[6,4];
        public struct strutturabottoni
        {
            public char Content;
            public bool mybool;
            public strutturabottoni(char Content, bool mybool)
            {
                this.Content = Content;
                this.mybool = mybool;
            }
            public override string ToString()
            {
                return Content.ToString();
            }

        };
        private strutturabottoni[,] button =
        {
            {new strutturabottoni('%',false),new strutturabottoni('ɶ',false),new strutturabottoni('c',false),new strutturabottoni('C',false) },
            {new strutturabottoni(' ',false),new strutturabottoni(' ',false),new strutturabottoni(' ',false),new strutturabottoni('÷',false) },
            {new strutturabottoni('7',false),new strutturabottoni('8',false),new strutturabottoni('9',false),new strutturabottoni('x',false) },
            {new strutturabottoni('4',false),new strutturabottoni('5',false),new strutturabottoni('6',false),new strutturabottoni('-',false) },
            {new strutturabottoni('1',false),new strutturabottoni('2',false),new strutturabottoni('3',false),new strutturabottoni('+',false) },
            {new strutturabottoni('±',false),new strutturabottoni('0',false),new strutturabottoni(',',false),new strutturabottoni('=',false) },

        };
        public FormMain()
        {
            InitializeComponent();
        }
        private RichTextBox txt;
        
        private void FormMain_Load(object sender, EventArgs e)
        {
            Makebuttons(button);
            MakeresultsBox(txt);
        }
        private void MakeresultsBox(RichTextBox txt)
        {
            txt = new RichTextBox();
            txt.ReadOnly = true;
            txt.SelectionAlignment = HorizontalAlignment.Right;
            txt.Font = new Font("Segoe UI", 22);
            txt.Width = this.Width - 16;
            txt.Height = 50;
            txt.Top = 20;
            txt.Text = "123456789";
            this.Controls.Add(txt);
        }
        private void Makebuttons(strutturabottoni[,] bottoni)
        {
            int buttonWidth = 78;
            int buttonHeight = 50;
            int posx = 0;
            int posy = 131;

            for (int i = 0; i < bottoni.GetLength(0); i++)
            {
                for (int j = 0; j < bottoni.GetLength(1); j++)
                {
                    Button newbutton = new Button();
                    newbutton.Text = bottoni[i, j].Content.ToString();
                    newbutton.Width = buttonWidth;
                    newbutton.Height = buttonHeight;
                    newbutton.Left = posx;
                    newbutton.Top = posy;
                    this.Controls.Add(newbutton);
                    posx += buttonWidth;
                }
                posx = 0;
                posy += buttonHeight;

            }
        }
    }
}
