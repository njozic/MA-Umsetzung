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
using HOMEToolbox.Functions.ProjectCreation;


namespace HOMEToolbox.Functions.CreateStructure
{
    public partial class CreateStructureForm : Form
    {
        public IToolboxInfo Toolbox { get; set; }
        public Repository Repository;
        private Logger Logger => Toolbox.Logger;


        public CreateStructureForm(IToolboxInfo toolbox, Repository repository)
        {
            Repository = repository;
            Toolbox = toolbox;

            InitializeComponent();
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

        private void handleOKButtonState()
        {
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked)
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

            this.Close();
        }




    }
}
