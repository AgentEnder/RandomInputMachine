using System;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.Printing;

using System.Net.Http;
//using System.Net.Http;

namespace RandomInputMachine
{
    public partial class Form1 : Form
    {
        #region preloading

        int coreNumb = Environment.ProcessorCount;
        
        //Text To Console
        TextStreamWriter textToConsole = null;
        //Random setup
        public static Random _random = new Random();
        //speech Synth
        public static SpeechSynthesizer synth = new SpeechSynthesizer();
        public static Thread speechThread = new Thread(new ThreadStart(speak));
        public static bool speechThreadStopped = true;
        //Keyboard thread
        public static Thread keyboardInputThread = new Thread(new ThreadStart(RandomKeyboardInputFunc));
        public static bool keyboardInputThreadStopped = true;
        public static bool keyboardOnlyLetters =  true;
        //Mouse thread
        public static Thread mouseMovementThread = new Thread(new ThreadStart(mouseMovementFunc));
        public static bool mouseMovementThreadStopped = true;
        public static string mouseMovement = "small";
        public static bool mouseMovementIsRandom = false;
        //Sound thread
        public static SoundPlayer _soundPlayer = new SoundPlayer();
        public static Thread soundThread = new Thread(new ThreadStart(soundFunc));
        public static bool soundThreadStopped = true;
        //Cpu Thread
        public static Thread cpuBurnThread = new Thread(new ThreadStart(cpuBurnFunc));
        public static bool cpuBurnThreadStopped = true;
        //Diagnostics
        public static Thread diagThread = new Thread(new ThreadStart(diagFunc));
        public static bool diagThreadStopped = true;
        public static bool cpuDiag = false;
        public static bool memDiag = false;
        public static bool threadDiag = false;
        public Form2 passwordGenerator = new Form2();
        public logIn log = new logIn();
        #endregion
        public Form1()
        {
            InitializeComponent();
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Setup Window, and TextToConsole
            this.Text = "Random Input Machine";
            textToConsole = new TextStreamWriter(textBox1);
            textBox1.ScrollBars = ScrollBars.Vertical;
            Console.SetOut(textToConsole);
            textBox1.ReadOnly = true;
            //Setup Appdata
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string RIMDirectory = Path.Combine(appdata,"RIM");
            if (!Directory.Exists(RIMDirectory))
            {
                Directory.CreateDirectory(RIMDirectory);
                Console.WriteLine("AppData Folder Created at: {0}", RIMDirectory);
            }
            else
            {
                Console.WriteLine("AppData Folder Found at: {0}", RIMDirectory);
            }
            
            //Start Threads
            speechThread.Start();
            soundThread.Start();
            keyboardInputThread.Start();
            mouseMovementThread.Start();
            cpuBurnThread.Start();
            diagThread.Start();
            //Disable Cpu Burner
            cpuBurnThread.Abort();
            checkBox3.Enabled = false;
            //richTextBox
            richTextBox1.DetectUrls = true;
        }
#region GuiInput
        /// <summary>
        /// CheckBox's
        /// </summary>
        /// 
        public static int numbChecked = 0;
        public static void checkDisabler(){
            
        }
            
        //#Enabled
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (numbChecked > 4)
                {
                    checkBox1.Checked = false;
                    return;
                }
                speechThreadStopped = false;
                numbChecked += 1;
                if (speechThread.IsAlive == false)
                {
                    speechThread = new Thread(new ThreadStart(speak));
                    speechThread.Start();
                }
            }
            else
            {
                numbChecked -= 1;
                speechThreadStopped = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                if (numbChecked > 4)
                {
                    checkBox4.Checked = false;
                    return;
                }
                diagThreadStopped = false;
                numbChecked += 1;
                memDiag = true;
                cpuDiag = true;
                if (diagThread.IsAlive == false)
                {
                    diagThread = new Thread(new ThreadStart(diagFunc));
                    diagThread.Start();
                }
            }
            else
            {
                numbChecked -= 1;
                memDiag = false;
                if (checkBox3.Checked == false)
                {
                    cpuDiag = false;
                    diagThreadStopped = true;
                }
            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                if (numbChecked > 4)
                {
                    checkBox5.Checked = false;
                    return;
                }
                mouseMovementThreadStopped = false;
                numbChecked += 1;
                if (mouseMovementThread.IsAlive == false)
                {
                    mouseMovementThread = new Thread(new ThreadStart(mouseMovementFunc));
                    mouseMovementThread.Start();
                }
            }
            else
            {
                numbChecked -= 1;
                mouseMovementThreadStopped = true;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (numbChecked > 4)
                {
                    checkBox2.Checked = false;
                    return;
                }
                keyboardInputThreadStopped = false;
                numbChecked += 1;
                if (keyboardInputThread.IsAlive == false)
                {
                    keyboardInputThread = new Thread(new ThreadStart(RandomKeyboardInputFunc));
                    keyboardInputThread.Start();
                }
            }
            else
            {
                numbChecked -= 1;
                keyboardInputThreadStopped = true;
            }
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                if (numbChecked > 4)
                {
                    checkBox6.Checked = false;
                    return;
                }
                soundThreadStopped = false;
                numbChecked += 1;
                if (soundThread.IsAlive == false)
                {
                    soundThread = new Thread(new ThreadStart(soundFunc));
                    soundThread.Start();
                }
            }
            else
            {
                numbChecked -= 1;
                soundThreadStopped = true;
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                
                cpuBurnThreadStopped = false;
                numbChecked += 1;
                
                if (cpuBurnThread.IsAlive == false)
                {
                    cpuBurnThread = new Thread(new ThreadStart(soundFunc));
                    cpuBurnThreadStopped = false;
                    cpuBurnThread.Start();
                }
            }
            else
            {
                cpuBurnThreadStopped = true;
                numbChecked -= 1;
                if (checkBox4.Checked == false)
                {
                    diagThreadStopped = true;
                    cpuDiag = false;
                }
            }
        }

        /*
        /// <summary>
        /// Console Input Texbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        */private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*if (e.KeyChar == (char)Keys.Enter)
            {
                textBox2.Text
            }*/
        }
#endregion
        /// <summary>
        /// Threads Found Below
        /// </summary>
        /// 
        
        //Speech Thread
        public static void speak()
        {
            while (true)
            {
                if (speechThreadStopped == false)
                {
                    synth.Speak("Hello");
                }
                else
                {
                    return;
                }
                Thread.Sleep(500);
            }
        }
        //Sound Thread
        public static void soundFunc()
        {
            while (true)
            {
                if (soundThreadStopped == false)
                {
                    int numb = _random.Next(0, 4);
                    SystemSound sound;
                    switch (numb){
                        case 0: 
                            sound = SystemSounds.Beep;
                            break;
                        case 1:
                            sound = SystemSounds.Exclamation;
                            break;
                        case 2:
                            sound = SystemSounds.Hand;
                            break;
                        case 3:
                            sound = SystemSounds.Question;
                                break;
                        default:
                            sound = SystemSounds.Asterisk;
                            break;
                    }
                    sound.Play();
                }
                else
                {
                    return;
                }
                Thread.Sleep(500);
            }
        }
        //Random Keyboard Thread
        public static void RandomKeyboardInputFunc()
        {
            while (true)
            {
                if (keyboardInputThreadStopped == false)
                {
                    //Console.WriteLine("Attempted Console Thread");
                    int numLetter;
                    if (keyboardOnlyLetters == false)
                    {
                        numLetter = _random.Next(1, 51);
                    }
                    else
                    {
                        numLetter = _random.Next(1, 26);
                    }
                        
                    string letter = "a";
                    switch (numLetter)
                    {
                        case 1: 
                            letter = "a";
                            break;
                        case 2:
                            letter = "b";
                            break;
                        case 3:
                            letter = "c";
                            break;
                        case 4:
                            letter = "d";
                            break;
                        case 5:
                            letter = "e";
                            break;
                        case 6:
                            letter = "f";
                            break;
                        case 7:
                            letter = "g";
                            break;
                        case 8:
                            letter = "h";
                            break;
                        case 9:
                            letter = "i";
                            break;
                        case 10:
                            letter = "j";
                            break;
                        case 11:
                            letter = "k";
                            break;
                        case 12:
                            letter = "l";
                            break;
                        case 13:
                            letter = "m";
                            break;
                        case 14:
                            letter = "n";
                            break;
                        case 15:
                            letter = "o";
                            break;
                        case 16:
                            letter = "p";
                            break;
                        case 17:
                            letter = "q";
                            break;
                        case 18:
                            letter = "r";
                            break;
                        case 19:
                            letter = "s";
                            break;
                        case 20:
                            letter = "t";
                            break;
                        case 21:
                            letter = "u";
                            break;
                        case 22:
                            letter = "v";
                            break;
                        case 23:
                            letter = "w";
                            break;
                        case 24:
                            letter = "x";
                            break;
                        case 25:
                            letter = "y";
                            break;
                        case 26:
                            letter = "z";
                            break;
                        case 27:
                            letter = "{+}";
                            break;
                        case 28:
                            letter = "=";
                            break;
                        case 29:
                            letter = "_";
                            break;
                        case 30:
                            letter = "-";
                            break;
                        case 31:
                            letter = "{)}";
                            break;
                        case 32:
                            letter = "{(}";
                            break;
                        case 33:
                            letter = "*";
                            break;
                        case 34:
                            letter = "&";
                            break;
                        case 35:
                            letter = "^";
                            break;
                        case 36:
                            letter = "%";
                            break;
                        case 37:
                            letter = "$";
                            break;
                        case 38:
                            letter = "#";
                            break;
                        case 39:
                            letter = "@";
                            break;
                        case 40:
                            letter = "!";
                            break;
                        case 41:
                            letter = "9";
                            break;
                        case 42:
                            letter = "8";
                            break;
                        case 43:
                            letter = "7";
                            break;
                        case 44:
                            letter = "6";
                            break;
                        case 45:
                            letter = "5";
                            break;
                        case 46:
                            letter = "4";
                            break;
                        case 47:
                            letter = "3";
                            break;
                        case 48:
                            letter = "2";
                            break;
                        case 49:
                            letter = "1";
                            break;
                        case 50:
                            letter = "0";
                            break;
                        default:
                            letter = Environment.NewLine;
                            break;
                    }
                    //Console.WriteLine(letter);
                    try
                    {
                        SendKeys.SendWait(letter);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    return;
                }
                Thread.Sleep(500);
            }
        }
        //Random Mouse Movement Thread
        public static void mouseMovementFunc()
        {
            Cursor cursor = new Cursor(Cursor.Current.Handle);
            string lastMouseMovementX = "right" ;
            string lastMouseMovementY = "up" ;
            int mouseMovementInt = 2;
            while (true){
                if (mouseMovement == "small")
                {
                    mouseMovementInt = 2;
                }
                else if (mouseMovement == "medium")
                {
                    mouseMovementInt = 8;
                }
                else
                {
                    mouseMovementInt = 16;
                }
                if (mouseMovementThreadStopped == false){
                    //Console.WriteLine("mouse movement attempted");
                    Thread.Sleep(20);
                    if (mouseMovementIsRandom == true){
                        int random = _random.Next(0, 3);
                        switch (random)
                        {
                            case 0:
                                lastMouseMovementX = "left";
                                break;
                            case 1:
                                lastMouseMovementX = "right";
                                break;
                            case 2:
                                lastMouseMovementY = "up";
                                break;
                            case 3:
                                lastMouseMovementY = "down";
                                break;
                        }
                    }
                        if (lastMouseMovementX == "right"){
                            Cursor.Position = new Point(Cursor.Position.X - mouseMovementInt, Cursor.Position.Y);
                            lastMouseMovementX = "left";
                        }
                        else
                        {
                            Cursor.Position = new Point(Cursor.Position.X + mouseMovementInt, Cursor.Position.Y);
                            lastMouseMovementX = "right";
                        }
                        if (lastMouseMovementY == "down")
                        {
                            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - mouseMovementInt);
                            lastMouseMovementY = "up";
                        }
                        else
                        {
                            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + mouseMovementInt);
                            lastMouseMovementY = "down";
                        }
                }
                else
                {
                    return;
                }
                Thread.Sleep(125);
            }
        }
        #region Cpu Burn Thread
        public static void cpuBurnFunc()
        {
            int tim = 0;
            while (true & tim < 500)
            {
                DateTime targetTime = DateTime.Now;
                
                //Console.WriteLine("Hello");
                Thread burnFunc1 = new Thread(new ThreadStart(cpuBurnFunc1));
                Thread burnFunc2 = new Thread(new ThreadStart(cpuBurnFunc1));
                Thread burnFunc3 = new Thread(new ThreadStart(cpuBurnFunc1));
                if (cpuBurnThreadStopped == false)
                {
                    //Thread.Sleep(500);
                    tim++;
                    //int duration = 20;
                    
                    //time = System.DateTime.Now;
                    if (targetTime == null)
                    {
                           
                    }
                    burnFunc1.Start();
                    burnFunc2.Start();
                    burnFunc3.Start();
                }
                else
                {
                    burnFunc1.Abort();
                    burnFunc2.Abort();
                    burnFunc3.Abort();
                    //return;
                }
                burnFunc1.Abort();
                burnFunc2.Abort();
                burnFunc3.Abort();
            }
        }
        public static void cpuBurnFunc1()
        {
            while (cpuBurnThreadStopped == false)
            {
                //Console.WriteLine("hello");
                synth.Volume = 10;
                synth.Speak("aegnnakjgnagnoag");
            }
        }
        public static void cpuBurnWatcher()
        {
            while (true)
            {
                if (cpuBurnThreadStopped == true)
                {
                    synth.Volume = 0;
                    synth.Speak("aegnnakjgnagnoag");
                }
            }
        }
        #endregion
        #region diagnostics
        //Diagnostics Thread's

        public static void diagFunc()
        {
            //Diagnostics Counter's
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available Mbytes");
            PerformanceCounter perfThreadCount = new PerformanceCounter("Process", "Thread Count", "_Total");
            while (true)
            {
                if (diagThreadStopped == false)
                {
                    int CpuPercentage = (int)perfCpuCount.NextValue();
                    if (cpuDiag == true)
                    {
                        Console.WriteLine("Cpu Load: {0}%", CpuPercentage);
                    }
                    int memUsage = (int)perfMemCount.NextValue();
                    if (memDiag == true)
                    {
                        Console.WriteLine("Available Memory: {0}MB", memUsage);
                    }
                    int threadNum = (int)perfThreadCount.NextValue();
                    if (threadDiag == true)
                    {
                        Console.WriteLine("Active Threads: {0}", threadNum);
                    }
                    Console.WriteLine("---------------------------------------------------");
                }
                
                Thread.Sleep(503);
            }
        }
        #endregion
        #region close the app
        /// <summary>
        /// When Closing        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            const string message =
        "Are you sure that you would like to close the form?";
            const string caption = "Form Closing";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ... 
            if (result == DialogResult.No)
            {
                // cancel the closure of the form.
                e.Cancel = true;
            }
            else
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
            }

        }
        #endregion
#region main menu and textbox menu
        /// <summary>
        /// Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string aboutTxt = "Random Input Machine created by Craigory Coppola, design started 11/9/14";
            const string aboutCaption = "About";
            MessageBox.Show(aboutTxt, aboutCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void onlyLettersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onlyLettersToolStripMenuItem.Checked = !onlyLettersToolStripMenuItem.Checked;
            keyboardOnlyLetters = !keyboardOnlyLetters;
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            randomToolStripMenuItem.Checked = !randomToolStripMenuItem.Checked;
            mouseMovementIsRandom = !mouseMovementIsRandom;
        }

        private void mediumMovementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediumMovementToolStripMenuItem.Checked = true;
            largeMovementToolStripMenuItem.Checked = false;
            smallMovementToolStripMenuItem.Checked = false;
            mouseMovement = "medium";
        }

        private void largeMovementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediumMovementToolStripMenuItem.Checked = false;
            largeMovementToolStripMenuItem.Checked = true;
            smallMovementToolStripMenuItem.Checked = false;
            mouseMovement = "large";
        }

        private void smallMovementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediumMovementToolStripMenuItem.Checked = false;
            largeMovementToolStripMenuItem.Checked = false;
            smallMovementToolStripMenuItem.Checked = true;
            mouseMovement = "small";
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void webBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://google.com");
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("calc");
        }
        /// <summary>
        /// TextEditorMenu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void saveAsTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTextBoxToFile(textBox1);
        } 
        #region colors
        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.ForeColor = Color.Black;
        }

        private void whiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.ForeColor = Color.White;
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.ForeColor = Color.Red;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.ForeColor = Color.Blue;
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.ForeColor = Color.Yellow;
        }

        private void blackToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.Black;
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.White;
        }

        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.Red;
        }

        private void blueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.Blue;
        }

        private void yellowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.Yellow;
        }
        private void speakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread _speach = new Thread(new ThreadStart(textboxspeach));
            _speach.Start();
        }
        #endregion
        #region context menu
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        private void contextMenuStrip1(object sender, EventArgs e)
        {

        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }
        #endregion
        #region TextBox Actions
        public void textboxspeach()
        {
            synth.Speak(richTextBox1.Text);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = String.Empty;
        }
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SaveTextBoxToFile(richTextBox1);
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTextBoxFromFile(richTextBox1);
        }
        #endregion
#endregion



        private void SaveTextBoxToFile(TextBoxBase textBox){
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".rtf";
            if (sfd.ShowDialog() == DialogResult.OK)
            {

                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {

                    sw.Write(textBox.Text);

                }

            }
        }

        private void LoadTextBoxFromFile(RichTextBox textBox)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".rtf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    richTextBox1.Text = System.IO.File.ReadAllText(ofd.FileName);
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

            }
        }


        #region printing
        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, new Font("Arial", 20, FontStyle.Regular), Brushes.Black, 20, 20);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument document = new PrintDocument();
            PrintDialog dialog = new PrintDialog();
            document.PrintPage += new PrintPageEventHandler(document_PrintPage);
            dialog.Document = document;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                document.Print();
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument document = new PrintDocument();
            PrintPreviewDialog dialog = new PrintPreviewDialog();
            document.PrintPage += new PrintPageEventHandler(document_PrintPage);
            dialog.Document = document;
            dialog.ShowDialog();
        }
        #endregion
        private void passwordManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.loggedIn == true)
            {
            
            }
            else
            {
                if (log.IsDisposed == true)
                {
                    log = new logIn();
                }
                log.Show();
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.loggedIn = false;
        }

        private void logInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (log.IsDisposed == true)
            {
                log = new logIn();
            }
            log.Show();
        }

        private void passwordGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Program.loggedIn == true)
            //{
            if (passwordGenerator.IsDisposed == true)
            {
                passwordGenerator = new Form2();
            }
            passwordGenerator.Show();
        }

        private void cUsageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cUsageToolStripMenuItem.Checked = !cUsageToolStripMenuItem.Checked ;
            threadDiag = cUsageToolStripMenuItem.Checked;
        }
    }
}
