using EA;
using HOMEToolbox.Logic;
using HOMEToolbox.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Toolbox.Core.Interfaces;

namespace HOMEToolbox.Functions.ProjectCreation {


    class ProjectFactory {
        public IToolboxInfo Toolbox { get; set; }
        private Repository repository => Toolbox.AddIn.App.Repository;
        private IEnumerable<XElement> rootNodes;
        private IEnumerable<XElement> viewPackages_r0;
        private IEnumerable<XElement> viewPackages;

        public ProjectFactory() {

        }

        public void readModelStructure() {

            var addinDirectory = Toolbox.Info.AssemblyDirectory;
            var projectStructureFilePath = Path.Combine(addinDirectory, HOMEConstants.XMLFileProjectStructure);

            //var projectStructureFileXML = XDocument.Load(diagramTypesFilePath);

            XElement projectStructureFileXML = XElement.Load($"{projectStructureFilePath}");

            rootNodes = from element in projectStructureFileXML.Elements(HOMEConstants.XMLFileRootNodeTag) select element;

            foreach (XElement node in rootNodes) {
                if (node.Attribute("name").Value == "R0") {
                    viewPackages_r0 = node.Elements(HOMEConstants.XMLFilePackageTag);
                } else {
                    viewPackages = node.Elements(HOMEConstants.XMLFilePackageTag);
                }

            }

        }

        public void createModelStructure(int layer, string name = "RX") {
            if (layer < 3) {
                name = "R" + layer;
            }

            foreach (XElement node in rootNodes) {
                if (node.Attribute("name").Value == name) {
                    Package root = repository.Models.AddNew("R" + layer, ObjectType.otPackage.ToString());
                    root.Update();
                    repository.Models.Refresh();

                    int counter = 1;
                    IEnumerable<XElement> currentPackages = null;

                    if (layer == 0) {
                        currentPackages = viewPackages_r0;
                    } else {
                        currentPackages = viewPackages;
                    }

                    foreach (XElement package in currentPackages) {
                        Package viewpoint = ModelElementLogic.SelectOrCreatePackage(root, package.Attribute("name").Value, package.Attribute("stereotype").Value);
                        viewpoint.TreePos = counter;
                        viewpoint.Update();
                        root.Packages.Refresh();
                        counter++;
                    }

                    repository.RefreshModelView(root.PackageID);
                    break;
                }
            }
        }
    }
}
