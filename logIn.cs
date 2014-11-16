using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomInputMachine
{
    public partial class logIn : Form
    {
        
        
        public logIn()
        {
            InitializeComponent();
            Thread log = new Thread(new ThreadStart(ifLoggedIn));
            log.Start();
        }

         void ifLoggedIn()
        {
            if (Program.loggedIn == true)
            {
                const string message = "You are already Logged In, do you wish to log out?";
                const string caption = "You are already Logged In";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                // If the no button was pressed ... 
                if (result == DialogResult.No)
                {
                    // cancel the closure of the form.
                    this.Close();
                }
                else
                {
                    Program.loggedIn = false;
                }
            }
            while (true)
            {
                Thread.Sleep(1000);
                if (Program.loggedIn == true)
                {
                    this.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
            if (txtUser.Text == "user")
            {
                if (txtPass.Text == "pass")
                {
                    Program.loggedIn = true;
                    textBox1.Text = "Log in Succeeded";
                    textBox1.Show();
                }
                else
                {
                    textBox1.Text = "Log In Failed";
                    textBox1.Show();
                }
            }
            else
            {
                textBox1.Text = "Log In Failed";
                textBox1.Show();
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        

        
    }
}
