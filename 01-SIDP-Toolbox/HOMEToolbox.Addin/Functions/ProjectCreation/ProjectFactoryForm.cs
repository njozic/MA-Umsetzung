using EA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolbox.Core.Helpers;
using Toolbox.Core.Interfaces;

namespace HOMEToolbox.Functions.ProjectCreation
{
    public partial class ProjectFactoryForm : Form
    {
        public IToolboxInfo Toolbox { get; set; }
        public Repository Repository;
        private Logger Logger => Toolbox.Logger;

        public ProjectFactoryForm(IToolboxInfo toolbox, Repository repository)
        {
            Repository = repository;
            Toolbox = toolbox;

            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            button1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            button1.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            handleOKButtonState();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            handleOKButtonState();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            handleOKButtonState();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            handleOKButtonState();
        }

        private void handleOKButtonState()
        {
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProjectFactory projectFactory = new ProjectFactory();
            projectFactory.Toolbox = Toolbox;
            projectFactory.readModelStructure();

            if (checkBox1.Checked)
                projectFactory.createModelStructure(0);

            if (checkBox2.Checked)
                projectFactory.createModelStructure(1);

            if (checkBox3.Checked)
                projectFactory.createModelStructure(2);

            if (checkBox4.Checked)
                projectFactory.createModelStructure((Int32)numericUpDown1.Value);

            this.Close();
        }
    }
}
