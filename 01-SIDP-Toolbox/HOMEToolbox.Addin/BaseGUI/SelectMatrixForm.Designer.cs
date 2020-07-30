namespace HOMEToolbox.BaseGUI
{
    partial class SelectMatrixForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupPlaneTypes = new System.Windows.Forms.GroupBox();
            this.rbAsset = new System.Windows.Forms.RadioButton();
            this.rbIntegration = new System.Windows.Forms.RadioButton();
            this.rbCommunication = new System.Windows.Forms.RadioButton();
            this.rbInformation = new System.Windows.Forms.RadioButton();
            this.rbFunction = new System.Windows.Forms.RadioButton();
            this.rbBusiness = new System.Windows.Forms.RadioButton();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupPlaneTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPlaneTypes
            // 
            this.groupPlaneTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPlaneTypes.Controls.Add(this.rbAsset);
            this.groupPlaneTypes.Controls.Add(this.rbIntegration);
            this.groupPlaneTypes.Controls.Add(this.rbCommunication);
            this.groupPlaneTypes.Controls.Add(this.rbInformation);
            this.groupPlaneTypes.Controls.Add(this.rbFunction);
            this.groupPlaneTypes.Controls.Add(this.rbBusiness);
            this.groupPlaneTypes.Location = new System.Drawing.Point(13, 13);
            this.groupPlaneTypes.Name = "groupPlaneTypes";
            this.groupPlaneTypes.Size = new System.Drawing.Size(259, 161);
            this.groupPlaneTypes.TabIndex = 0;
            this.groupPlaneTypes.TabStop = false;
            this.groupPlaneTypes.Text = "Select your desired plane type:";
            // 
            // rbAsset
            // 
            this.rbAsset.AutoSize = true;
            this.rbAsset.Location = new System.Drawing.Point(7, 139);
            this.rbAsset.Name = "rbAsset";
            this.rbAsset.Size = new System.Drawing.Size(80, 17);
            this.rbAsset.TabIndex = 5;
            this.rbAsset.Text = "Asset Layer";
            this.rbAsset.UseVisualStyleBackColor = true;
            this.rbAsset.CheckedChanged += new System.EventHandler(this.rbAsset_CheckedChanged);
            // 
            // rbIntegration
            // 
            this.rbIntegration.AutoSize = true;
            this.rbIntegration.Location = new System.Drawing.Point(7, 116);
            this.rbIntegration.Name = "rbIntegration";
            this.rbIntegration.Size = new System.Drawing.Size(104, 17);
            this.rbIntegration.TabIndex = 4;
            this.rbIntegration.Text = "Integration Layer";
            this.rbIntegration.UseVisualStyleBackColor = true;
            this.rbIntegration.CheckedChanged += new System.EventHandler(this.rbIntegration_CheckedChanged);
            // 
            // rbCommunication
            // 
            this.rbCommunication.AutoSize = true;
            this.rbCommunication.Location = new System.Drawing.Point(7, 92);
            this.rbCommunication.Name = "rbCommunication";
            this.rbCommunication.Size = new System.Drawing.Size(126, 17);
            this.rbCommunication.TabIndex = 3;
            this.rbCommunication.Text = "Communication Layer";
            this.rbCommunication.UseVisualStyleBackColor = true;
            this.rbCommunication.CheckedChanged += new System.EventHandler(this.rbCommunication_CheckedChanged);
            // 
            // rbInformation
            // 
            this.rbInformation.AutoSize = true;
            this.rbInformation.Location = new System.Drawing.Point(7, 68);
            this.rbInformation.Name = "rbInformation";
            this.rbInformation.Size = new System.Drawing.Size(106, 17);
            this.rbInformation.TabIndex = 2;
            this.rbInformation.Text = "Information Layer";
            this.rbInformation.UseVisualStyleBackColor = true;
            this.rbInformation.CheckedChanged += new System.EventHandler(this.rbInformation_CheckedChanged);
            // 
            // rbFunction
            // 
            this.rbFunction.AutoSize = true;
            this.rbFunction.Location = new System.Drawing.Point(7, 44);
            this.rbFunction.Name = "rbFunction";
            this.rbFunction.Size = new System.Drawing.Size(95, 17);
            this.rbFunction.TabIndex = 1;
            this.rbFunction.Text = "Function Layer";
            this.rbFunction.UseVisualStyleBackColor = true;
            this.rbFunction.CheckedChanged += new System.EventHandler(this.rbFunction_CheckedChanged);
            // 
            // rbBusiness
            // 
            this.rbBusiness.AutoSize = true;
            this.rbBusiness.Checked = true;
            this.rbBusiness.Location = new System.Drawing.Point(7, 20);
            this.rbBusiness.Name = "rbBusiness";
            this.rbBusiness.Size = new System.Drawing.Size(96, 17);
            this.rbBusiness.TabIndex = 0;
            this.rbBusiness.TabStop = true;
            this.rbBusiness.Text = "Business Layer";
            this.rbBusiness.UseVisualStyleBackColor = true;
            this.rbBusiness.CheckedChanged += new System.EventHandler(this.rbBusiness_CheckedChanged);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCreate.Location = new System.Drawing.Point(197, 200);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SelectMatrixForm
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 235);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.groupPlaneTypes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelectMatrixForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Plane";
            this.Load += new System.EventHandler(this.SelectMatrixForm_Load);
            this.groupPlaneTypes.ResumeLayout(false);
            this.groupPlaneTypes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupPlaneTypes;
        private System.Windows.Forms.RadioButton rbIntegration;
        private System.Windows.Forms.RadioButton rbCommunication;
        private System.Windows.Forms.RadioButton rbInformation;
        private System.Windows.Forms.RadioButton rbFunction;
        private System.Windows.Forms.RadioButton rbBusiness;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbAsset;
    }
}