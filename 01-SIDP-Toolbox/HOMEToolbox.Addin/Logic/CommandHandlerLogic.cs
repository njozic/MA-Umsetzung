using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using EA;
using Toolbox.Core;
using Toolbox.Core.Events;
using Toolbox.Core.Helpers;
using Toolbox.Core.Interfaces;
using Toolbox.Core.Logic;
using HOMEToolbox.BaseGUI;
using StabilityTypes = Toolbox.Core.Model.StabilityTypes;
using MessageBox = System.Windows.MessageBox;
using Resources = global::HOMEToolbox.Properties.Resources;
using Toolbox.Core.Model;
using HOMEToolbox.Functions.CreateStructure;
using HOMEToolbox.Functions.ProjectCreation;


namespace HOMEToolbox.Logic {
    public class CommandHandlerLogic {

        private EAModelAccess access;

        #region Members

        private readonly IDispatcher _dispatcher;
        private IToolboxInfo Toolbox { get; set; }
        private readonly IToolboxCore Core;

        //private readonly Dictionary<string, string> _diagExplainChapters = new Dictionary<string, string>
        //{
        //    {"RAMI Layers::RAMI Business Layer", "ExplainBusinessLayer"},
        //    {"RAMI Layers::RAMI Function Layer", "ExplainFunctionLayer"},
        //    {"RAMI Layers::RAMI Information Layer", "ExplainInformationLayer"},
        //    {"RAMI Layers::RAMI Communication Layer", "ExplainCommunicationLayer"},
        //    {"RAMI Layers::RAMI Integration Layer", "ExplainIntegrationLayer"},
        //    {"RAMI Layers::RAMI Asset Layer", "ExplainAssetLayer"}
        //};

        private readonly Dictionary<string, string> _diagExplainChapters = new Dictionary<string, string>
        {
            {"HOME Layers::HOME CIM Layer", "ExplainCIM"}

        };

        private class CommandItem {
            internal readonly Action<string> Handler;
            internal readonly Func<bool> Enabled;
            internal readonly Func<bool> Visible;

            internal CommandItem(Action<string> handler, Func<bool> enabled = null, Func<bool> visible = null) {
                Handler = handler;
                Enabled = enabled;
                Visible = visible;
            }
        }


        private readonly Dictionary<string, CommandItem> _commandHandlers = new Dictionary<string, CommandItem>();

        #endregion //Members

        public CommandHandlerLogic(IToolboxInfo toolbox) {
            _dispatcher = new DispatcherWrapper();

            Toolbox = toolbox;
            Core = Toolbox.AddIn.Core;

            //Subscribe to Repository Events
            Toolbox.AddIn.ActiveDiagramChanged += OnActiveDiagramChanged;
            Toolbox.AddIn.ActiveRepositoryChanged += OnActiveRepositoryChanged;

            RegisterCommandHandlers();

            access = EAModelAccess.getInstance();
            access.Toolbox = Toolbox;
        }

        public CommandHandlerLogic(Dictionary<string, string> diagExplainChapters) {
            _diagExplainChapters = diagExplainChapters;
        }

        public bool IsCommandEnabled(string command) {
            CommandItem commandItem;
            if (!_commandHandlers.TryGetValue(command, out commandItem))
                return true;
            return commandItem.Enabled == null || commandItem.Enabled();
        }

        public bool IsCommandVisible(string command) {
            CommandItem commandItem;
            if (!_commandHandlers.TryGetValue(command, out commandItem))
                return true;
            return commandItem.Visible == null || commandItem.Visible();
        }

        public void ExecuteCommand(string command, string parameter = "") {
            CommandItem commandItem;
            if (_commandHandlers.TryGetValue(command, out commandItem))
                commandItem.Handler(parameter);
        }

        #region Helper Members

        private Repository Repository => Toolbox.AddIn.App.Repository;

        private bool BatchAppend {
            get { return Repository.BatchAppend; }
            set { Repository.BatchAppend = value; }
        }

        private AssemblyInfo Info => Toolbox.Info;
        private Logger Logger => Toolbox.Logger;

        private bool IsRepositoryOpen { get; set; } = true;

        public bool IsProjectOpen() {
            try {
                var c = Toolbox.AddIn.App.Repository.Models;
                return IsRepositoryOpen;
            } catch (Exception) {
                return true;
                //return false;
            }
        }

        private bool IsCurrentDiagramOpen { get; set; } = false;
        //Sets the state of the menu depending if there is an open diagram or not
        public bool IsDiagramOpen() {
            Diagram diag = null;
            try {
                diag = Toolbox.AddIn.App.Repository.GetCurrentDiagram();
            } catch {
                diag = null;
            }

            return diag != null && IsCurrentDiagramOpen;
        }

        private bool IsVisible { get; set; } = true;

        //Sets visibility of menu depending on level of stability 
        private bool CheckMinStability(StabilityTypes minStability = StabilityTypes.Stable) {
            var stability = Toolbox.Stability;

            if (minStability >= stability) {
                return IsVisible;
            } else {
                return false;
            }
        }

        private bool IsAlpha() {
            return CheckMinStability(StabilityTypes.Alpha);
        }

        private bool IsBeta() {
            return CheckMinStability(StabilityTypes.Beta);
        }

        private bool IsStable() {
            return CheckMinStability();
        }


        #endregion //Helper Members

        #region Repository Event Handlers

        private void OnActiveDiagramChanged(object sender, EventArgs eventArgs) {
            var e = eventArgs as ActiveDiagramChangedEventArgs;
            if (e == null)
                return;

            IsCurrentDiagramOpen = e.IsOpen;

            //Only process with opening Description items if the diagram is actually open
            if (!IsCurrentDiagramOpen)
                return;

            var repo = Toolbox.AddIn.App.Repository;
            var package = repo.GetPackageByID(e.Diagram.PackageID);
            int level;
            string view = string.Empty;
            CoreElementLogic.LevelIdRegEx = "^R(\\d)";
            CoreElementLogic.FindTopLevelPackage(repo, package, out level, ref view);

            //Try to decode diagram name and if successful, send it to Dockable
            if (level < 0 || string.IsNullOrEmpty(view))
                return;

            string val;

            if (level == 0) {
                //Try to decode diagram name and if successful, send it to Dockable
                if (!_diagExplainChapters.TryGetValue(e.Diagram.MetaType, out val))
                    return;
            } else {
                if (level > 3)
                    level = 3;

                val = string.Format("r{0}{1}Desc", level, view.ToLower()[0]);
            }

            if (Toolbox.DockableHost.Dockable != null) {
                Toolbox.DockableHost.Dockable.ShowExplanationChapter = val;
                Toolbox.AddIn.Core.ActivateDockableWindow();
            }
        }

        private void OnActiveRepositoryChanged(object sender, EventArgs eventArgs) {
            var e = eventArgs as ActiveRepositoryChangedEventArgs;
            if (e != null) {
                IsRepositoryOpen = e.IsOpen;
            }
        }

        #endregion

        #region Command Handlers

        private void RegisterCommandHandlers() {
            //Menu Handlers
            _commandHandlers.Add(Resources.MenuShowStepThrough, new CommandItem(OnShowStepThrough, null, IsStable));
            _commandHandlers.Add(Resources.MenuTransformProcess, new CommandItem(OnMenuTransformProcess, IsProjectOpen, IsAlpha));
            _commandHandlers.Add(Resources.MenuAbout, new CommandItem(OnAbout));

            //Dockable Command Handlers
            _commandHandlers.Add("GenMDAStruc", new CommandItem(OnGenMDAStruc, IsProjectOpen, IsStable));

            // Toolbox Command Handlers
            _commandHandlers.Add("GenerateRequirements", new CommandItem(OnGenerateRequirements, IsProjectOpen, IsStable));
            _commandHandlers.Add("GenerateUseCases", new CommandItem(OnGenerateUseCases, IsProjectOpen, IsStable));
            _commandHandlers.Add("GenerateSIDP", new CommandItem(OnGenerateSIDP, IsProjectOpen, IsStable));
            _commandHandlers.Add("GenerateComponents", new CommandItem(OnGenerateComponents, IsProjectOpen, IsStable));
            _commandHandlers.Add("GenerateClasses", new CommandItem(OnGenerateClasses, IsProjectOpen, IsStable));
        }

        private void OnShowStepThrough(string parameter) {
            //Show Dockable (if hidden)
            Repository.ShowAddinWindow(Resources.DockableTitle);
            Logger.Info($">>> {Toolbox.Info.GetCurrentMethod()} ");
        }


        private void OnMenuTransformProcess(string parameter) {
            try {
                BatchAppend = true;
                //CoreElementLogic.LevelIdRegEx = "^R(\\d)";
                //Toolbox.AddIn.Core.TransformItems(new TransformBaseParams(parameter), CrawlStrategy.CurrentLevel, "^R(\\d)");

            } catch (Exception ex) {
                Logger.Error($"!! Exception at {Info.GetCurrentMethod()}: {ex.Message} !!");
            } finally {
                BatchAppend = false;
                Logger.Info($">>> {Info.GetCurrentMethod()} ");
            }
        }

        private void OnAbout(string parameter) {
            var anAbout = new Toolbox.Core.BaseGUI.AboutForm("HOME Toolbox",
                                                        "The HOME Toolbox is Smart Home Utility which helps building SMART HOME Models from BPMN Processes.",
                                                        $"Version: {Info.AssemblyVersion}",
                                                        "niko.jozic@fh-salzburg.ac.at",
                                                        "Niko Jozic",
                                                         Resources.HOME_Icon);
            anAbout.ShowDialog();
        }

        private void OnGenMDAStruc(string parameter) {
            try {
                BatchAppend = true;

                access.createStructure();

            } catch (Exception ex) {
                Logger.Error($"!! Exception at {Info.GetCurrentMethod()}: {ex.Message} !!");
            } finally {
                BatchAppend = false;
                Logger.Info($">>> {Info.GetCurrentMethod()} ");
            }

        }


        // Eventhandler zum Transformieren der Requirements
        private void OnGenerateRequirements(string parameter) {
            // Nodes aus denen gelesen bzw. in die geschrieben wird
            Package root = access.getRoot();
            Package processNode = ModelElementLogic.SelectOrCreatePackage(root, "Processmodel");
            Package requirementsNode = ModelElementLogic.SelectOrCreatePackage(root, "Requirements");

            // Diagram in das die Requirements eingefügt werden
            Diagram diagram = ModelElementLogic.SelectOrCreateDiagram(requirementsNode, "Requirements", true, "Requirements");

            int counter = 1;
            // Alle Elemente des Prozess-Models in einer Schleife Durchlaufen
            foreach (Element element in processNode.Elements){
                // Transformations-Regel:
                // Textannotationen werden in Requirements überführt
                if (element.Stereotype == "TextAnnotation"){
                    // Erstellen des Reqirements:
                    // Die Notiz der Textannotation wird zum Namen des Requirements
                    Element requirement = ModelElementLogic.SelectOrCreateElement(requirementsNode, element.Notes, "Requirement");
                    // Position des Requirements im Projektbaum setzen
                    requirement.TreePos = counter++;
                    requirement.Update();

                    // Requirement in das Diagram linken
                    ModelElementLogic.AddToDiagram(diagram, requirement);
                }
            }

            // Model Aktualisieren
            requirementsNode.Elements.Refresh();
            root.Packages.Refresh();
            Repository.RefreshModelView(root.PackageID);
        }

        private void OnGenerateUseCases(string parameter) {
            Package root = access.getRoot();
            Package node = ModelElementLogic.SelectOrCreatePackage(root, "CIM");
            Diagram diagram = ModelElementLogic.SelectOrCreateDiagram(node, "Anwendungsfälle", true, "UseCase");

            Dictionary<string, Element> serviceTasks = access.getProcessServiceTasks();
            Dictionary<string, Element> artifacts = access.getProcessArtifacts();

            Dictionary<string, Element> usecases = access.createUseCases(serviceTasks);
            Dictionary<string, Element> actors = access.createActors(artifacts, usecases);


            foreach (KeyValuePair<string, Element> usecase in usecases) {
                ModelElementLogic.AddToDiagram(diagram, usecase.Value);
            }
            foreach (KeyValuePair<string, Element> actor in actors) {
                ModelElementLogic.AddToDiagram(diagram, actor.Value);
            }

        }


        private void OnGenerateSIDP(string parameter) {
            Package cim = ModelElementLogic.SelectOrCreatePackage(access.getRoot(), "CIM");
            Package pim = ModelElementLogic.SelectOrCreatePackage(access.getRoot(),"PIM");
            Diagram diagram = ModelElementLogic.SelectOrCreateDiagram(pim, "System", true, "Logical");

            int counter = 1;
            foreach(Element actor in cim.Elements) {
                if( actor.Type == "Actor") {
                    // TODO: Make Selection vor sensor Blocks
                    // eg. MessageBox with Selection how to map
                    string type = "device block";
                    if (actor.Stereotype == "Control") {
                        type = "controller block";
                    }
                    Element block = ModelElementLogic.SelectOrCreateElement(pim, actor.Name, type);

                    block.TreePos = counter++;
                    block.Update();
                    ModelElementLogic.AddToDiagram(diagram, block);
                }
            }
            pim.Elements.Refresh();
            access.getRoot().Packages.Refresh();
            Repository.RefreshModelView(access.getRoot().PackageID);
        }

        private void OnGenerateComponents(string parameter) {
            Package pim = ModelElementLogic.SelectOrCreatePackage(access.getRoot(), "PIM");
            Diagram diagram = ModelElementLogic.SelectOrCreateDiagram(pim, "Komponenten", true, "Component");

            List<Element> blocks = new List<Element>();
            foreach(Element block in pim.Elements) {
                if(block.Stereotype == "device block" || block.Stereotype == "controller block") {
                    blocks.Add(block);
                }
            }

            int counter = 1;
            foreach(Element block in blocks) {
                Element component = ModelElementLogic.SelectOrCreateElement(pim, block.Name, "Component");
                component.TreePos = counter++;
                component.Update();
                ModelElementLogic.AddToDiagram(diagram, component);
            }
            pim.Elements.Refresh();
            access.getRoot().Packages.Refresh();
            Repository.RefreshModelView(access.getRoot().PackageID);
        }


        private void OnGenerateClasses(string parameter) {
            Package pim = ModelElementLogic.SelectOrCreatePackage(access.getRoot(), "PIM");
            Package psm = ModelElementLogic.SelectOrCreatePackage(access.getRoot(), "PSM");
            Diagram diagram = ModelElementLogic.SelectOrCreateDiagram(psm, "Klassen", true, "Class");

            int counter = 1;
            foreach (Element element in pim.Elements) {
                if (element.Type == "Component") {
                    Element classElement = ModelElementLogic.SelectOrCreateElement(psm, element.Name, "Class");
                    EA.Attribute attribute = classElement.Attributes.AddNew("description", "string");
                    attribute.Visibility = "private";
                    attribute.Update();
                    classElement.TreePos = counter++;
                    classElement.Update();
                    ModelElementLogic.AddToDiagram(diagram, classElement);
                }
            }
            psm.Elements.Refresh();
            access.getRoot().Packages.Refresh();
            Repository.RefreshModelView(access.getRoot().PackageID);

        }

        private void OnGenerateCode(string parameter) {
            
        }

        #endregion //Command Handlers
    }
}
