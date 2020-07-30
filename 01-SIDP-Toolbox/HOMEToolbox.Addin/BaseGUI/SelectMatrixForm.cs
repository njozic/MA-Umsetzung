using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EA;
using Toolbox.Core.Model;
using Toolbox.Core.Logic;


namespace HOMEToolbox.BaseGUI
{
    public partial class SelectMatrixForm : Form
    {
        public string MatrixType { get; private set; } = "RAMI_Business_Layer";

        public SelectMatrixForm()
        {
            InitializeComponent();
        }

        private void SelectMatrixForm_Load(object sender, EventArgs e)
        {

        }

        private void rbBusiness_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBusiness.Checked)
                MatrixType = "RAMI_Business_Layer";
        }

        private void rbFunction_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFunction.Checked)
                MatrixType = "RAMI_Function_Layer";
        }

        private void rbInformation_CheckedChanged(object sender, EventArgs e)
        {
            if (rbInformation.Checked)
                MatrixType = "RAMI_Information_Layer";
        }

        private void rbCommunication_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCommunication.Checked)
                MatrixType = "RAMI_Communication_Layer";
        }

        private void rbIntegration_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIntegration.Checked)
                MatrixType = "RAMI_Integration_Layer";
        }

        private void rbAsset_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAsset.Checked)
                MatrixType = "RAMI_Asset_Layer";
        }
    }
}
