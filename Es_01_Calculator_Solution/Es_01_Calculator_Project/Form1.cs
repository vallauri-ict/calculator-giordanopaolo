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
            public bool IsNumber;
            public bool issepar;
            public bool ispls;
            public strutturabottoni(char Content, bool mybool, bool IsNumber = true, bool issepar = false, bool ispls = false)
            {
                this.Content = Content;
                this.mybool = mybool;
                this.IsNumber = IsNumber;
                this.issepar = issepar;
                this.ispls = ispls;
            }
        public override string ToString()
            {
                return Content.ToString();
            }

        };
        private strutturabottoni[,] button =
        {
            {new strutturabottoni('%',false,false),new strutturabottoni('ɶ',false,false),new strutturabottoni('c',false,false),new strutturabottoni('C',false,false) },
            {new strutturabottoni(' ',false,false),new strutturabottoni(' ',false,false),new strutturabottoni(' ',false,false),new strutturabottoni('÷',false,false) },
            {new strutturabottoni('7',true),new strutturabottoni('8',true),new strutturabottoni('9',true),new strutturabottoni('x',false) },
            {new strutturabottoni('4',true),new strutturabottoni('5',true),new strutturabottoni('6',true),new strutturabottoni('-',false) },
            {new strutturabottoni('1',true),new strutturabottoni('2',true),new strutturabottoni('3',true),new strutturabottoni('+',false) },
            {new strutturabottoni('±',false,false,false,true),new strutturabottoni('0',true),new strutturabottoni(',',false,false,true),new strutturabottoni('=',false,false) },

        };
        public FormMain()
        {
            InitializeComponent();
        }
        private RichTextBox txt;
        
        private void FormMain_Load(object sender, EventArgs e)
        {
            Makebuttons(button);
            MakeresultsBox();
        }
        private void MakeresultsBox()
        {
            txt = new RichTextBox();
            txt.ReadOnly = true;
            txt.SelectionAlignment = HorizontalAlignment.Right;
            txt.Font = new Font("Segoe UI", 22,FontStyle.Bold);
            txt.Width = this.Width - 16;
            txt.Height = 50;
            txt.Top = 20;
            txt.Text = "0";
            this.Controls.Add(txt);
            txt.TextChanged += resulstBox_TextCHang;
        }
        private void resulstBox_TextCHang(object sender, EventArgs e)
        {
            int num = txt.Text.Length;
            bool minus = txt.Text.Contains('-')?true:false;
            if (minus)
            {
                if (num > 16)
                    txt.Text = txt.Text.Remove(txt.Text.Length - 1);
                else if (num > 13)
                    txt.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                else if (num > 6)
                    txt.Font = new Font("Segoe UI", 17, FontStyle.Bold);
            }
            else
            {
                if (num > 15)
                    txt.Text = txt.Text.Remove(txt.Text.Length - 1);
                else if (num > 13)
                    txt.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                else if (num > 6)
                    txt.Font = new Font("Segoe UI", 17, FontStyle.Bold);
            }
            




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
                    strutturabottoni bs = bottoni[i, j];
                    Button newbutton = new Button();
                    newbutton.Text = bottoni[i, j].Content.ToString();
                    newbutton.Width = buttonWidth;
                    newbutton.Height = buttonHeight;
                    newbutton.Left = posx;
                    newbutton.Top = posy;
                    newbutton.Tag = bs;
                    newbutton.Click += ButtonClick;
                    this.Controls.Add(newbutton);
                    posx += buttonWidth;
                }
                posx = 0;
                posy += buttonHeight;

            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button thisbutton = (Button)sender;//casto perchè sono sicuro che sara un bottone per poi usare sempre e solo thisbutton
                                               // MessageBox.Show("Button: " + thisbutton.Text);
            strutturabottoni bs = (strutturabottoni)thisbutton.Tag;
            
            if (bs.IsNumber)
            {
                if (txt.Text == "0")
                    txt.Text = "";
                txt.Text += thisbutton.Text;
            }
            else
            {
                if (bs.issepar)
                {
                    if(!txt.Text.Contains(bs.Content))
                    {
                        txt.Text += thisbutton.Text;
                    }
                }
                else if(bs.ispls)
                {
                    if (txt.Text != "0") 
                    {
                        if (!txt.Text.Contains("-"))

                            txt.Text = "-" + txt.Text;
                        else txt.Text = txt.Text.Substring(1);
                    }
                    
                }
                else
                {
                    switch(bs.Content)
                    {
                        case 'c':
                            txt.Text = "0";
                            break;
                        case 'C':
                            if (txt.Text.Length == 1 || txt.Text == "-0,")
                                txt.Text = "0";
                            else
                                txt.Text = txt.Text.Remove(txt.Text.Length - 1);
                            break;
                        default: break;
                    }
                        
                }
            }
                



        }
    }
}
