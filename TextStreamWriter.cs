using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace RandomInputMachine
{
    class TextStreamWriter : TextWriter
    {
        TextBox textBox1 = null;
 
        public TextStreamWriter(TextBox output)
        {
            textBox1 = output;
        }
 
        public override void Write(char value)
        {
            base.Write(value);
            textBox1.AppendText(value.ToString()); // When character data is written, append it to the text box.
        }
 
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

    }

    
}
