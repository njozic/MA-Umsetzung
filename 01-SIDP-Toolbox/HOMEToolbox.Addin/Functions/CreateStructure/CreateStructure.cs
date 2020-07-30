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

namespace HOMEToolbox.Functions.CreateStructure
{
    class CreateStructure
    {
        public IToolboxInfo Toolbox { get; set; }
        private Repository repository => Toolbox.AddIn.App.Repository;
        private IEnumerable<XElement> rootNodes;
        private IEnumerable<XElement> viewPackages;

        public CreateStructure()
        {
        }

        public void readXMLStructure()
        {
            var addinDirec = Toolbox.Info.AssemblyDirectory;
            var mdaStructureFilePath = Path.Combine(addinDirec, HOMEConstants.XMLFileMDAStructure);
            XElement mdaStructureFileXML = XElement.Load($"{mdaStructureFilePath}");
            rootNodes = from element in mdaStructureFileXML.Elements(HOMEConstants.XMLFileRootNodeTag) select element;
        
            foreach(XElement node in rootNodes)
            {
                if (node.Attribute("name").Value == "MDA Structure")
                {
                    viewPackages = node.Elements(HOMEConstants.XMLFilePackageTag);
                }
            }

        }

        public void createMDAStructure()
        {
            Package root = repository.Models.AddNew("MDA Structure", ObjectType.otPackage.ToString());
            root.Update();
            repository.Models.Refresh();

            int counter = 1;
            IEnumerable<XElement> currentPackages = null;
            currentPackages = viewPackages;

            foreach (XElement package in currentPackages)
            {
                Package viewpoint = ModelElementLogic.SelectOrCreatePackage(root, package.Attribute("name").Value, package.Attribute("stereotype").Value);
                viewpoint.TreePos = counter;
                viewpoint.Update();
                root.Packages.Refresh();
                counter++;
            }

            repository.RefreshModelView(root.PackageID);
 
        }


    }



}
