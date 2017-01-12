using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;
using System.DirectoryServices.AccountManagement;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Function to check if computer name exists in AD
        public bool DoesCptrExist(string cptrName)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain))
            {
                using (var foundUser = ComputerPrincipal.FindByIdentity(domainContext, IdentityType.Name, cptrName))
                {
                    return foundUser != null;
                }
            }
        }

        //Function to detect Spooler service status
        public void SpoolerStatus(string cptrName)
        {
            string target = cptrName;
            ServiceController svc = new ServiceController("Spooler", target);
            string svcStatus = svc.Status.ToString();
            textBox2.Text = svcStatus;
        }

        //Function to restart print spooler
        public void BounceSpooler(string cptrName)
        {
            string target = cptrName;
            ServiceController svc = new ServiceController("Spooler", target);
            string svcStatus = svc.Status.ToString();
            if (svcStatus == "Running")
            {
                svc.Stop();
                while (svcStatus != "Stopped")
                {
                    svc.Refresh();
                    svcStatus = svc.Status.ToString();
                }
                checkBox1.Checked = true;
                svc.Start();
                while (svcStatus != "Running")
                {
                    svc.Refresh();
                    svcStatus = svc.Status.ToString();
                }
                checkBox2.Checked = true;
            }
            else if (svcStatus == "Stopped")
            {
                checkBox1.Checked = true;
                svc.Start();
                while (svcStatus != "Running")
                {
                    svc.Refresh();
                    svcStatus = svc.Status.ToString();
                }
                checkBox2.Checked = true;
            }
            else
            {
                svc.Stop();
                while (svcStatus != "Stopped")
                {
                    svc.Refresh();
                    svcStatus = svc.Status.ToString();
                }
                checkBox1.Checked = true;
                svc.Start();
                while (svcStatus != "Running")
                {
                    svc.Refresh();
                    svcStatus = svc.Status.ToString();
                }
                checkBox2.Checked = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Clears all existing form values
            textBox2.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;

            //collect  the computer name
            string cptrName = textBox1.Text;

            //Verify computer name exists in AD
            bool ADCheck = DoesCptrExist(cptrName);
            if (ADCheck == true)
            {
                checkBox4.Checked = true;
            }
            else
            {
                textBox1.Text = "Please enter a valid computer name";
            }

            //Call the function that checks the spooler service. Pass in computer name
            if (checkBox4.Checked == true)
            {
                SpoolerStatus(cptrName);
            }


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Clears existing form values
            checkBox1.Checked = false;
            checkBox2.Checked = false;

            //collect  the computer name
            string cptrName = "legdets100";
            if (textBox1.Text == null)
            {
                textBox1.Text = "Please enter a valid computer name";
            }
            else
            {
                cptrName = textBox1.Text;
            }

            //Verify computer name exists in AD
            bool ADCheck = DoesCptrExist(cptrName);
            if (ADCheck == true)
            {
                checkBox4.Checked = true;
            }
            else
            {
                textBox1.Text = "Please enter a valid computer name";
            }

            //Call the function that restarts the spooler service. Pass in computer name
            if (checkBox4.Checked == true)
            {
                BounceSpooler(cptrName);
            }
        }
    }
}
