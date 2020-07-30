using EA;
using HOMEToolbox.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Toolbox.Core.Interfaces;
using Toolbox.Core.Helpers;

using MessageBox = System.Windows.MessageBox;


namespace HOMEToolbox.Logic {
    class EAModelAccess {

        private static EAModelAccess instance = null;

        public IToolboxInfo Toolbox { get; set; }
        private Logger Logger => Toolbox.Logger;
        private AssemblyInfo Info => Toolbox.Info;
        private Repository repository => Toolbox.AddIn.App.Repository;

        private string rootNode = "SIDP-System";
        private Dictionary<string, string> views = new Dictionary<string, string>{
            { "process", "Processmodel" },
            { "requirements", "Requirements" },
            { "cim", "CIM" },
            { "pim", "PIM" },
            { "psm", "PSM" },
            { "psi", "PSI" }
        };

        private Dictionary<string, string> diagrams = new Dictionary<string, string>{
            { "requirements", "Requirements" }
        };

        private EAModelAccess() { }

        public static EAModelAccess getInstance() {
            if (EAModelAccess.instance == null) {
                EAModelAccess.instance = new EAModelAccess();
            }
            return EAModelAccess.instance;
        }


        /*************************************************************
         * Get the Root-Package or Create it
         *************************************************************/
        public Package getRoot() {
            Package root = null;
            try {
                root = repository.Models.GetByName(this.rootNode);
            } catch (Exception ex) {
                Logger.Error($"Exception at {Info.GetCurrentMethod()}: {ex.Message} ... Root-Package not found! Creating a new one!;");
                root = repository.Models.AddNew(this.rootNode, ObjectType.otPackage.ToString());
            }
            root.Update();
            repository.Models.Refresh();
            return root;
        }


        /*************************************************************
         * Create the Package-Structure
         *************************************************************/
        public void createStructure() {
            Package root = this.getRoot();
            int counter = 1;
            foreach (KeyValuePair<string, string> view in this.views) {
                Package viewpoint;
                viewpoint = ModelElementLogic.SelectOrCreatePackage(root, view.Value);
                viewpoint.TreePos = counter++;
                viewpoint.Update();
                root.Packages.Refresh();
            }
            repository.RefreshModelView(root.PackageID);
        }

        /*************************************************************
         * Get TextAnnotatins from the BPMN-Processes
         *************************************************************/
        public Dictionary<string, Element> getProcessTextAnnotations() {

            // Get the Package we work in
            Package root = this.getRoot();
            Package node = ModelElementLogic.SelectOrCreatePackage(root, this.views["process"]);

            Dictionary<string, Element> textAnnotations = new Dictionary<string, Element>();

            foreach(Element element in node.Elements) {
                if( element.Stereotype == "TextAnnotation") {
                    textAnnotations.Add(element.ElementGUID, element);
                }
            }

            return textAnnotations;
        }

        /*************************************************************
         * Create Requirements from TextAnnotations of the BPMN-Process
         *************************************************************/
        public Dictionary<string, Element> createRequirements(Dictionary<string,Element> textAnnotations) {

            // Get the Package we work in
            Package root = this.getRoot();
            Package node = ModelElementLogic.SelectOrCreatePackage(root, this.views["requirements"]);

            Diagram diagram = ModelElementLogic.SelectOrCreateDiagram(node, this.diagrams["requirements"], true, "Requirements");

            // Reference Dictionary so we can trace Requirements to TextAnnotations
            Dictionary<string, Element> requirements = new Dictionary<string, Element>();

            int counter = 1;
            foreach (KeyValuePair<string,Element> textAnnotation in textAnnotations) {
                Element requirement = ModelElementLogic.SelectOrCreateElement(node, textAnnotation.Value.Notes, "Requirement");
                requirements.Add(textAnnotation.Key, requirement);
                requirement.TreePos = counter++;
                requirement.Update();
            }

            // Refresh Everything
            node.Elements.Refresh();
            root.Packages.Refresh();
            repository.RefreshModelView(root.PackageID);

            return requirements;
        }


        /*************************************************************
         * Get the ServiceTasks from the BPMN-Processes
         *************************************************************/
        public Dictionary<string,Element> getProcessServiceTasks() {

            // Get the Package we work in
            Package root = this.getRoot();
            Package node = ModelElementLogic.SelectOrCreatePackage(root, this.views["process"]);

            Dictionary<string, Element> usecases = new Dictionary<string, Element>();
            
            foreach (Element element in node.Elements) {
                if (element.Type == "Activity") {
                    string taskType = element.TaggedValues.Cast<TaggedValue>().FirstOrDefault(tv => tv.Name == "taskType").Value;
                    string activityType = element.TaggedValues.Cast<TaggedValue>().FirstOrDefault(tv => tv.Name == "activityType").Value;

                    if( activityType == "Task" && taskType == "Service") {
                        usecases.Add(element.ElementGUID, element);
                    }
                }
            }
            return usecases;
        }

        public Dictionary<string, Element> createUseCases(Dictionary<string, Element> serviceTasks) {
            
            // Get the Package we work in
            Package root = this.getRoot();
            Package node = ModelElementLogic.SelectOrCreatePackage(root, this.views["cim"]);

            // Reference Dictionary so we can trace UseCases to ServiceTasks
            Dictionary<string, Element> usecases = new Dictionary<string, Element>();

            int counter = 1;
            foreach (KeyValuePair<string, Element> svericeTask in serviceTasks) {
                Element usecase = ModelElementLogic.SelectOrCreateElement(node, svericeTask.Value.Name, "UseCase");
                usecases.Add(svericeTask.Key, usecase);
                usecase.TreePos = counter++;
                usecase.Update();
            }

            // Refresh Everything
            node.Elements.Refresh();
            root.Packages.Refresh();
            repository.RefreshModelView(root.PackageID);

            return usecases;
        }

        /*************************************************************
         * Get the Artifacts from the BPMN-Processes
         *************************************************************/
        public Dictionary<string, Element> getProcessArtifacts() {

            // Get the Package we work in
            Package root = this.getRoot();
            Package node = ModelElementLogic.SelectOrCreatePackage(root, this.views["process"]);

            Dictionary<string, Element> artifacts = new Dictionary<string, Element>();

            foreach (Element element in node.Elements) {
                if (element.Type == "Artifact") {
                    artifacts.Add(element.ElementGUID, element);
                }
            }
            return artifacts;
        }

        public Dictionary<string,Element> createActors(Dictionary<string,Element> artifacts, Dictionary<string, Element> usecases) {

            // Get the Package we work in
            Package root = this.getRoot();
            Package node = ModelElementLogic.SelectOrCreatePackage(root, this.views["cim"]);

            // Reference Dictionary so we can trace Actors to Artifacts
            Dictionary<string, Element> actors = new Dictionary<string, Element>();

            // Dictionary to check artifact Connections
            int counter = 1;
            foreach (KeyValuePair<string, Element> artifact in artifacts) {
                List<Element> connections = new List<Element>();
                int artifact_connections = 0;
                foreach (Connector connector in artifact.Value.Connectors) {
                    Element connectedElement = ModelElementLogic.GetElementById(repository, connector.SupplierID);
                    if (connectedElement == null) {
                        continue;
                    }
                    if (connectedElement.Type == "Artifact") {
                        artifact_connections++;
                    }
                    connections.Add(connectedElement);
                }
                // Only create Actors from Artifacts if they have Connections to other Artifacts
                if(artifact_connections > 0) {
                    Element actor = ModelElementLogic.SelectOrCreateElement(node, artifact.Value.Name, "Actor");
                    actor.Stereotype = "Component";
                    if (artifact_connections > 1) {
                        actor.Stereotype = "Control";
                    }
                    actors.Add(artifact.Key, actor);
                    actor.TreePos = counter++;
                    actor.Update();
                }
            }

            // refresh everything
            node.Elements.Refresh();
            root.Packages.Refresh();
            repository.RefreshModelView(root.PackageID);

            return actors;
        }

        public Dictionary<string,Element> getCimActors() {
            Package cim = ModelElementLogic.SelectOrCreatePackage(this.getRoot(), "CIM");


            Dictionary<string, Element> actors = new Dictionary<string, Element>();
            foreach (Element element in cim.Elements) {
                if (element.Type == "Actor") {
                    actors.Add(element.ElementGUID, element);
                }
            }
            return actors;
        }
    }
}
