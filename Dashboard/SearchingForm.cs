using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard
{
    public partial class SearchingForm : Form
    {
        public SearchingForm()
        {
            InitializeComponent();
        }

        private void Homebutton1_Click(object sender, EventArgs e)
        {
            panel3.Top = Homebutton1.Top;
            panel3.Height = Homebutton1.Height;

        }

        private void Complainbutton2_Click(object sender, EventArgs e)
        {
            panel3.Top = Complainbutton2.Top;
            panel3.Height = Complainbutton2.Height;
        }

        private void Accusedbutton3_Click(object sender, EventArgs e)
        {
            panel3.Top = Accusedbutton3.Top;
            panel3.Height = Accusedbutton3.Height;
        }

        private void Searchbutton4_Click(object sender, EventArgs e)
        {
            panel3.Top = Searchbutton4.Top;
            panel3.Height = Searchbutton4.Height;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
