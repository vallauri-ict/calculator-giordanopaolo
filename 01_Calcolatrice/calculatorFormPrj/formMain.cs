﻿using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace calculatorFormPrj
{
    public partial class formMain : Form
    {
        struct ButtonStruct
        {
            public char Content;
            public bool IsBold;
            public bool IsNumber;
            public bool IsOperator;
            public bool IsDecimalSeparator;
            public bool IsEqualSign;
            public bool IsPlusMinusSign;
            public bool IsSpecial;///le strutture si utilizzano per avere più variabili
            public ButtonStruct(char content, bool isBold, bool isNumber = false, bool isDecimalSeparator = false, bool isPlusMinusSign = false, bool isOperator = false, bool isEqualSign = false, bool isSpecial = false)
            {
                this.Content = content;
                this.IsBold = isBold;
                this.IsNumber = isNumber;
                this.IsDecimalSeparator = isDecimalSeparator;
                this.IsPlusMinusSign = isPlusMinusSign;
                this.IsOperator = isOperator;
                this.IsEqualSign = isEqualSign;
                this.IsSpecial = isSpecial;
            }
            //per noi oggetto .tostring fa gia rappresentazione stringa del content
            public override string ToString()
            {
                return Content.ToString();
            }
        }
        private ButtonStruct[,] buttons =
        {
            {new ButtonStruct('%',false,false,false,false,true),new ButtonStruct('c',false),new ButtonStruct('C',false),new ButtonStruct('<',false)},
            {new ButtonStruct('¼',false,false,false,false,false,false,true),new ButtonStruct('²',false,false,false,false,true),new ButtonStruct('r',false),new ButtonStruct('/',false,false,false,false,true)},
            {new ButtonStruct('7',true,true),new ButtonStruct('8',true,true),new ButtonStruct('9',true,true),new ButtonStruct('x',false,false,false,false,true)},
            {new ButtonStruct('4',true,true),new ButtonStruct('5',true,true),new ButtonStruct('6',true,true),new ButtonStruct('-',false,false,false,false,true)},
            {new ButtonStruct('1',true,true),new ButtonStruct('2',true,true),new ButtonStruct('3',true,true),new ButtonStruct('+',false,false,false,false,true)},
            {new ButtonStruct('±',false,false,false,true),new ButtonStruct('0',true,true),new ButtonStruct(',',false,false,true),new ButtonStruct('=',false,false,false,false,true,true)}
        };
        private RichTextBox resultBox;
        private Font baseFont = new Font("Segoe UI", 22, FontStyle.Bold);//font di default
        private const char ASCIIZERO = '\x0000';
        private double operand1, operand2, result;
        private char lastOperator;
        private ButtonStruct lastButtonClicked;
        public formMain()
        {
            InitializeComponent();
        }
        private void formMain_Load(object sender, EventArgs e)
        {
            MakeResultBox();
            MakeButtons(buttons);
        }
        private void manageOperators(ButtonStruct bs)
        {
            if (lastOperator == ASCIIZERO)//valore di default
            {
                operand1 = double.Parse(resultBox.Text);
                lastOperator = bs.Content;
            }
            else
            {
                if (lastButtonClicked.IsOperator && !lastButtonClicked.IsEqualSign)
                    lastOperator = bs.Content;
                else
                {
                    if (!lastButtonClicked.IsEqualSign)
                    {
                        operand2 = double.Parse(resultBox.Text);
                    }
                    switch (lastOperator)
                    {
                        case '+':
                            result = operand1 + operand2;
                            break;
                        case '-':
                            result = operand1 - operand2;
                            break;
                        case 'x':
                            result = operand1 * operand2;
                            break;
                        case '/':
                            result = operand1 / operand2;
                            break;
                        default:
                            break;
                    }
                    operand1 = result;
                    if (!bs.IsEqualSign)
                    {
                        lastOperator = bs.Content;
                        operand2 = 0;
                    }
                    if (lastOperator == '%')
                        result = result / 100;//se è una percentuale moltiplica per il secondo operatore diviso 100 esempi: 20 * 25% = 5     50 * 50% = 25
                    resultBox.Text = getFormattedNumber(result);
                }
            }
        }
        private void MakeResultBox()
        {
            resultBox = new RichTextBox();
            resultBox.Font = baseFont;
            resultBox.SelectionAlignment = HorizontalAlignment.Right;
            resultBox.Width = this.Width - 16;
            resultBox.Height = 50;
            resultBox.Top = 40;
            resultBox.ReadOnly = true;
            resultBox.Text = "0";
            resultBox.TabStop = false;//per rimuovere cursore che lampeggia, perchè result box non prende il fuoco
            resultBox.TextChanged += ResultBox_TextChanged;
            this.Controls.Add(resultBox);
        }
        private void MakeButtons(ButtonStruct[,] buttons)
        {
            int buttonWidth = 82, buttonHeight = 60;
            int posX = 0, posY = 101;
            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    Button newButton = new Button();
                    newButton.Font = new Font("Segoe UI", 16);
                    ButtonStruct bs = buttons[i, j];
                    newButton.Text = bs.Content.ToString();
                    if (bs.IsBold)
                        newButton.Font = new Font(newButton.Font, FontStyle.Bold);
                    newButton.Width = buttonWidth;
                    newButton.Height = buttonHeight;
                    newButton.Left = posX;
                    newButton.Top = posY;
                    newButton.Tag = bs;//proprietà tag serve per taggare pulsante
                    newButton.Click += Button_Click;
                    this.Controls.Add(newButton);
                    posX += buttonWidth;
                }
                posX = 0;
                posY += buttonHeight;
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            //qualsiasi controllo del netframework deriva sempre da object//se voglio utilizzare proprietà specifiche dei bottoni devo effettuare casting
            Button clickedButton = (Button)sender;//effettuo casting del sender
            ButtonStruct bs = (ButtonStruct)clickedButton.Tag;//casting perchè oggetto
            if (bs.IsNumber)
            {
                if (lastButtonClicked.IsEqualSign)
                    clearAll();
                if (resultBox.Text == "0" || lastButtonClicked.IsOperator)
                    resultBox.Text = "";
                if (resultBox.Text.Length < 17)
                    resultBox.Text += clickedButton.Text;
            }
            else
            {
                if (bs.IsDecimalSeparator)
                {
                    if (!resultBox.Text.Contains(bs.Content.ToString()))
                        resultBox.Text += clickedButton.Text;
                }
                if (bs.IsPlusMinusSign && resultBox.Text != "0")//-0 non esiste
                {
                    if (!resultBox.Text.Contains("-"))
                        resultBox.Text = "-" + resultBox.Text;
                    else   resultBox.Text = resultBox.Text.Substring(1);
                }
                else
                {
                    switch (bs.Content)
                    {
                        case 'C':
                            clearAll();// pulisco tutto
                            break;
                        case 'c':
                            resultBox.Text = "0";//pulisco solo il testo scritto
                            break;
                        case '<':
                            if (resultBox.Text.Length != 0)
                                resultBox.Text = resultBox.Text.Remove(resultBox.Text.Length - 1);
                            if (resultBox.Text.Length == 0 || resultBox.Text == "-0" || resultBox.Text == "-")
                                resultBox.Text = "0";
                            break;
                        case '¼':
                            double s = 1/Convert.ToDouble(resultBox.Text) ;
                            resultBox.Text = s.ToString();
                            break;
                        case '²':
                            double p = Math.Pow(Convert.ToDouble(resultBox.Text), 2);
                            resultBox.Text = p.ToString();
                            break;
                        case 'r':
                            double r = Math.Sqrt(Convert.ToDouble(resultBox.Text));
                            resultBox.Text = r.ToString();
                            break;
                        default:
                            if (bs.IsOperator)
                                manageOperators(bs);
                            break;
                    }
                }
            }
            lastButtonClicked = bs;
        }
        private void clearAll(double numberToWrite = 0)
        {
            operand1 = 0;
            operand2 = 0;
            result = 0;
            lastOperator = ASCIIZERO;
            resultBox.Text = getFormattedNumber(numberToWrite);
        }
        private void ResultBox_TextChanged(object sender, EventArgs e)
        {

            int delta = 17 - resultBox.Text.Length;
            if (delta % 2 == 0)
            {
                float newSize = baseFont.Size + delta;
                if (newSize > 8 && newSize < 23)
                    resultBox.Font = new Font(baseFont.FontFamily, newSize, FontStyle.Bold);
            }
        }
        private string getFormattedNumber(double number)
        {
            char decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            return number.ToString("N16").TrimEnd('0').TrimEnd(decimalSeparator);
        }
    }
}