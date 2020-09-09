using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Pricing
{
    public partial class Form2 : Form
    {
        Timer formClose = new Timer();
        public Form2()
        {
            InitializeComponent();
            Form1 ne = new Form1();
            //ne.Read_Main();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            formClose.Interval = 1000;
            formClose.Tick += new EventHandler(formClose_Tick);
            formClose.Start();
        }

        void formClose_Tick(object sender, EventArgs e)
        {
            formClose.Stop();
            formClose.Tick -= new EventHandler(formClose_Tick);
            this.Close();
        }

        }
    }


