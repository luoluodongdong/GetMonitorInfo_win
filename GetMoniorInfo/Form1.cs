using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetMoniorInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            GetMonitorInfo gmi = new GetMonitorInfo();
            List<string> info = gmi.getMoinitorInfo();
            listBox1.Items.Clear();
            if(info.Count > 0)
            {
                foreach(string item in info)
                {
                    listBox1.Items.Add(item);
                }
            }
        }
    }
}
