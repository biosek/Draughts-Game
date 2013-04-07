using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Zapoctak
{
    public partial class OknoNastaveni : Form
    {
        public OknoNastaveni()
        {
            InitializeComponent();
        }
        public bool IsOpened;

        private void Form2_Load(object sender, EventArgs e)
        {
            trackBar1.Value = Settings1.Default.Radius;
            trackBar2.Value = (int)Settings1.Default.Rychlost;
            checkBox1.Checked = Settings1.Default.Oznaceni;
            checkBox2.Checked = Settings1.Default.Zvuky;
            IsOpened = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings1.Default.Oznaceni = checkBox1.Checked;
            Settings1.Default.Radius = trackBar1.Value;
            Settings1.Default.Rychlost = (float)trackBar2.Value;
            Settings1.Default.Zvuky = checkBox2.Checked;
            Settings1.Default.Save();
            Settings1.Default.Reload();
            IsOpened = false;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IsOpened = false;
            this.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }
    }
}
