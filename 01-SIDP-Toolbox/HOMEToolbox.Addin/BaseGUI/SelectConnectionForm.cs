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

namespace HOMEToolbox.BaseGUI
{
    public partial class SelectConnectionForm : Form
    {
        public Repository repository { get; set; }
        public Diagram rootDiagram{ get; set; }
        public Element firstElement { get; set; }
        public Element secondElement { get; set; }

        public string interfaceDefinition { get; set; }
        public string informationItem { get; set; }
        public bool firstelementProvider { get; set; }

        public SelectConnectionForm()
        {
            InitializeComponent();
        }

        private void ConnectionForm_Load(object sender, EventArgs e)
        {
            firstelementProvider = true;
            groupBox3.Text = firstElement.Name;
            groupBox4.Text = secondElement.Name;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pictureBox2.Image = global::HOMEToolbox.Properties.Resources.Provided_right;
                pictureBox1.Image = global::HOMEToolbox.Properties.Resources.Required_right;
                firstelementProvider = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                pictureBox2.Image = global::HOMEToolbox.Properties.Resources.Required_left;
                pictureBox1.Image = global::HOMEToolbox.Properties.Resources.Provided_left;
                firstelementProvider = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            interfaceDefinition = textBox1.Text;
            informationItem = textBox2.Text;

            if(!string.IsNullOrEmpty(interfaceDefinition) && !string.IsNullOrEmpty(informationItem))
            {
                
                Connector newAssoc = firstElement.Connectors.AddNew("", "Association");
                newAssoc.Stereotype = "Association";
                newAssoc.SupplierID = secondElement.ElementID;
                newAssoc.Direction = "Unspecified";
                newAssoc.Update();
                

                Package useCase = repository.GetPackageByID(firstElement.PackageID);
                Package rootModel = repository.Models.GetAt(0);

                Package informationLayer = FindPackage(rootModel, "RAMI Information Layer");
                Package communicationLayer = FindPackage(rootModel, "RAMI Communication Layer");

                Diagram informationDiagram = FindDiagram(informationLayer, useCase.Name);
                Diagram communicationDiagram = FindDiagram(communicationLayer, useCase.Name);

                drawCommunication(interfaceDefinition, communicationDiagram);
                drawInformation(informationItem, informationDiagram);

                repository.ReloadDiagram(rootDiagram.DiagramID);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please specifiy the Interface and Data sent!");
            }
        }

        private Package FindPackage(Package root, string packagename)
        {
            Package temp = null;
            foreach (Package package in root.Packages)
            {
                if (package.Name == packagename)
                {
                    temp = package;
                }
            }

            return temp;
        }

        private Diagram FindDiagram(Package root, string diagramname)
        {
            Diagram temp = null;
            foreach (Package package in root.Packages)
            {
                if (package.Name == diagramname)
                {
                    foreach (Diagram diagram in package.Diagrams)
                    {
                        if (diagram.Name == diagramname)
                        {
                            temp = diagram;
                        }
                    }
                }
            }

            return temp;
        }

        private void drawCommunication(string interfaceItem, Diagram diagram)
        {
            DiagramObject newObject = null;

            if (firstelementProvider)
            {
                Element firstPort = firstElement.Elements.AddNew("", "Port");
                firstPort.Stereotype = "Service Point";
                firstPort.Update();

                Element secondPort = secondElement.Elements.AddNew("", "Port");
                secondPort.Stereotype = "Request Point";
                secondPort.Update();

                Element firstInterface = firstPort.Elements.AddNew("", "ProvidedInterface");
                firstInterface.Update();

                Element secondInterface = secondPort.Elements.AddNew("", "RequiredInterface");
                secondInterface.Update();

                Connector newAssembly = firstInterface.Connectors.AddNew("", "Assembly");
                newAssembly.Stereotype = "Assembly";
                newAssembly.SupplierID = secondInterface.ElementID;
                newAssembly.Direction = "Source -> Destination";
                newAssembly.Update();

                newObject = diagram.DiagramObjects.AddNew("", "");
                newObject.ElementID = firstPort.ElementID;
                newObject.Update();

                newObject = diagram.DiagramObjects.AddNew("", "");
                newObject.ElementID = secondPort.ElementID;
                newObject.Update();
            }
            else
            {
                Element firstPort = firstElement.Elements.AddNew("", "Port");
                firstPort.Stereotype = "Request Point";
                firstPort.Update();

                Element secondPort = secondElement.Elements.AddNew("", "Port");
                secondPort.Stereotype = "Service Point";
                secondPort.Update();

                Element firstInterface = firstPort.Elements.AddNew("", "RequiredInterface");
                firstInterface.Update();

                Element secondInterface = secondPort.Elements.AddNew("", "ProvidedInterface");
                secondInterface.Update();

                Connector newAssembly = firstInterface.Connectors.AddNew("", "Assembly");
                newAssembly.Stereotype = "Assembly";
                newAssembly.SupplierID = secondInterface.ElementID;
                newAssembly.Direction = "Destination -> Source";
                newAssembly.Update();

                newObject = diagram.DiagramObjects.AddNew("", "");
                newObject.ElementID = firstPort.ElementID;
                newObject.Update();

                newObject = diagram.DiagramObjects.AddNew("", "");
                newObject.ElementID = secondPort.ElementID;
                newObject.Update();
            }
            Element newInterface = repository.GetPackageByID(diagram.PackageID).Elements.AddNew(interfaceItem, "Interface");
            newInterface.Update();

            newObject = diagram.DiagramObjects.AddNew("", "");
            newObject.ElementID = newInterface.ElementID;
            newObject.Update();

            repository.ReloadDiagram(diagram.DiagramID);
        }

        private void drawInformation(string informationName, Diagram diagram)
        {
            Connector newInfoflow = firstElement.Connectors.AddNew("", "InformationFlow");
            newInfoflow.Stereotype = "Information Object Flow";
            newInfoflow.SupplierID = secondElement.ElementID;
            if (firstelementProvider)
            {
                newInfoflow.Direction = "Source -> Destination";
            }
            else
            {
                newInfoflow.Direction = "Destination -> Source";
            }
            newInfoflow.Update();

            repository.ReloadDiagram(diagram.DiagramID);

            Element newInfoitem = repository.GetPackageByID(diagram.PackageID).Elements.AddNew(informationName, "Information Object");
            newInfoitem.Update();

            newInfoflow.ConveyedItems.AddNew(newInfoitem.ElementGUID, null);
            newInfoflow.Update();

            DiagramObject newObject = null;
            newObject = diagram.DiagramObjects.AddNew("", "");
            newObject.ElementID = newInfoitem.ElementID;
            newObject.Update();

            repository.ReloadDiagram(diagram.DiagramID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
