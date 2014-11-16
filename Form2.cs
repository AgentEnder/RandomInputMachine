using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace RandomInputMachine
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public static Random _random = Form1._random;
        public static int defaultSeed;
        private void Form2_Load(object sender, EventArgs e)
        {
            checkedListBox1.SetItemChecked(0, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";    
            while(numericUpDown1.Value > textBox2.Text.Length)
            {
                char let;
                let = randomChar(checkedListBox1.GetItemChecked(0), checkedListBox1.GetItemChecked(1), checkedListBox1.GetItemChecked(2), checkedListBox1.GetItemChecked(3));
                    
                textBox2.AppendText(let.ToString());       
            }
        }
        public static char randomChar(bool lowerLetters, bool upperLetters, bool numbers, bool symbols)
        {
            int num;
            int lns;
            char let = '~';
            string specialSymbols = "+-,./*=_!@#$%^&()";
            while (let == '~'){
                lns = _random.Next(0,3);
                switch (lns){
                    case 0: 
                        if (lowerLetters || upperLetters)
                        {
                            num = Form1._random.Next(0, 26);
                            let = (char)('a' + num);
                            if (upperLetters)
                            {
                                if (!lowerLetters)
                                {
                                    let = let.ToString().ToUpper()[0];
                                }
                                else
                                {
                                    if (_random.Next(0, 10) > 5)
                                    {
                                        let = let.ToString().ToUpper()[0];
                                    }
                                }
                            }
                            return (let);
                        }
                        break;
                    case 1:
                        if (numbers)
                        {
                            num = _random.Next(0, 10);
                            let = num.ToString()[0];
                        }
                        break;
                    case 2:
                        if (symbols)
                        {
                            num = _random.Next(0, specialSymbols.Length + 1);
                            let = specialSymbols[num];
                        }
                        break;
                    
                }
            }
            
            return (let);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

       
    }
}
