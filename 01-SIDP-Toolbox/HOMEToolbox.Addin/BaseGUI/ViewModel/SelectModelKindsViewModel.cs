using EA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HOMEToolbox.Logic;
using HOMEToolbox.Model;
using Toolbox.Core.Helpers;
using System.Windows.Input;
using System.Collections.ObjectModel;
using HOMEToolbox.BaseGUI.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Toolbox.Core.Interfaces;
using HOMEToolbox.Properties;
using System.IO;
using System.Xml.Linq;
using Toolbox.Core.Logic;

namespace HOMEToolbox.BaseGUI.ViewModel
{
    class SelectModelKindsViewModel : ObservableObject
    {
        Package packageToCreateDiagram=null;

        TransformParams transformBaseParams;

        public IToolboxInfo Toolbox { get; set; }
        public Repository Repository { get; set; }


        private Logger Logger => Toolbox.Logger;
        private AssemblyInfo Info => Toolbox.Info;

        public event EventHandler RequestClose;

        public ObservableCollection<RAMIDiagram> _diagramsPossible { get; set; }
        public ObservableCollection<RAMIDiagram> _diagramsToCreate { get; set; }
        public ObservableCollection<RAMIDiagram> _selectedDiagrams { get; set; }

 

        public ICommand SelectAddDiagramSingle { get; private set; }
        public ICommand SelectRemoveDiagramSingle { get; private set; }
        public ICommand SelectAddDiagramAll { get; private set; }
        public ICommand SelectRemoveDiagramAll { get; private set; }
        public ICommand CreateDiagrams { get; private set; }
        public ICommand CancleCreateDiagram { get; private set; }

        public void buildViewModel(string parameter)
        {
            transformBaseParams = new TransformParams(parameter);

            CoreElementLogic.LevelIdRegEx = "^R(\\d)";
            packageToCreateDiagram = CoreElementLogic.FindViewInLevel(Repository, transformBaseParams.Level, transformBaseParams.Viewpoint);

            if (packageToCreateDiagram==null)
            {
                //Not Implemented yet --> show Info Dialog to Users
                throw new System.NullReferenceException("Cannot create Model Package and Diagram. Level " + transformBaseParams.Level + " and " + transformBaseParams.Viewpoint + " not created in Project Structure!");
            }
            

        }


        private void createNewDiagrams(string modelName, string diagramName, string diagramType, string diagramStyleEx, Package package)
        {
            //Create new root package in first model at this time
            //TODO: Package selection and maybe guided creation?
            Package rootPackage = ModelElementLogic.SelectOrCreatePackage(package, $"{modelName}", HOMEConstants.StereoPackModel);
            if (rootPackage == null)
                return;
            ModelElementLogic.CreateDiagram(rootPackage, diagramName, diagramType, diagramStyleEx);
        }


        public void getPossibleDiagrams()
        {

            var addinDirectory = Toolbox.Info.AssemblyDirectory;
            var diagramTypesFilePath = Path.Combine(addinDirectory, HOMEConstants.XMLFileModelKinds);

            var diagramTypesXML = XDocument.Load(diagramTypesFilePath);

            XElement diagramTypes = XElement.Load($"{diagramTypesFilePath}");

            IEnumerable<XElement> choosenView = from element in diagramTypes.Elements(HOMEConstants.XMLFileViewTag)
                                                   where (string)element.Attribute(HOMEConstants.XMLFileViewNameAttribute) == transformBaseParams.Viewpoint
                                                   select element;

            IEnumerable<XElement> models = choosenView.Descendants(HOMEConstants.XMLFileModelTag);

            foreach (var model in models)
            {
                string _modelName = model.Attribute(HOMEConstants.XMLFileModelNameAttribute).Value;
                IEnumerable<XElement> modelKinds = model.Descendants(HOMEConstants.XMLFileModelKindsTag);

                foreach (var modelKind in modelKinds)
                {
                    
                    string _defaultName = modelKind.Attribute(HOMEConstants.XMLFileAttDefaultName).Value;
                    string _parameter = modelKind.Attribute(HOMEConstants.XMLFileAttParameter).Value;
                    TransformModelKindParams transformModelKindParams = new TransformModelKindParams(_parameter);
                    RAMIDiagram diagram = RAMIDiagram.CreateNewDiagramObject(_defaultName, transformModelKindParams.DestDiagramType, transformModelKindParams.DestDiagramStyleEx, _modelName);
                    diagram.PropertyChanged += AddToSelected;
                    _diagramsPossible.Add(diagram);
                }
            }
        }

        public void AddToSelected(object sender, EventArgs e)
        {
            if (sender != null)
            {
                
                if (((RAMIDiagram)sender).isSelected)
                {
                    _selectedDiagrams.Clear(); //Only Single Selection allowed
                    _selectedDiagrams.Add((RAMIDiagram)sender);
                }
            }
               
        }


        public ObservableCollection<RAMIDiagram> DiagramsPossible
        {
            get { return _diagramsPossible; }

            set
            {
                _diagramsPossible=value;
                RaisePropertyChangedEvent("DiagramsPossible");
            }
        }




        public ObservableCollection<RAMIDiagram> DiagramsToCreate
        {
            get { return _diagramsToCreate; }

            set
            {
                _diagramsToCreate = value;
                RaisePropertyChangedEvent("DiagramsToCreate");
            }
        }

        public ObservableCollection<RAMIDiagram> SelectedDiagrams
        {
            get { return _selectedDiagrams; }

            set
            {
                _selectedDiagrams = value;
                RaisePropertyChangedEvent("SelectedDiagrams");
            }
        }



        /// <summary>
        /// The functionality of the methods is implemented here.
        /// </summary>
        public SelectModelKindsViewModel()
        {
            _selectedDiagrams = new ObservableCollection<RAMIDiagram>();
            _diagramsPossible = new ObservableCollection<RAMIDiagram>();
            _diagramsToCreate = new ObservableCollection<RAMIDiagram>();
            SelectAddDiagramSingle = new DelegateCommand(onSelectAddDiagramSingle);
            SelectRemoveDiagramSingle = new DelegateCommand(onSelectRemoveDiagramSingle);
            SelectAddDiagramAll = new DelegateCommand(onSelectAddDiagramAll);
            SelectRemoveDiagramAll = new DelegateCommand(onSelectRemoveDiagramAll);
            CancleCreateDiagram = new DelegateCommand(onCancleCreateDiagram);
            CreateDiagrams = new DelegateCommand(onCreateDiagrams);
        }


        private void onSelectAddDiagramSingle(object obj)
            {
                if (_selectedDiagrams != null) {
                foreach (RAMIDiagram diag in _selectedDiagrams)
                {
                    if (!(_diagramsToCreate.Contains(diag)))
                    {
                        _diagramsPossible.Remove(diag);
                        _diagramsToCreate.Add(diag);
                        clearSelction();
                    }
                }
                _selectedDiagrams.Clear();
            }
            }

            private void onSelectRemoveDiagramSingle(object obj)
            {
            if (_selectedDiagrams != null)
            {
                foreach (RAMIDiagram diag in _selectedDiagrams)
                {
                    if (!(_diagramsPossible.Contains(diag)))
                     { 
                    _diagramsPossible.Add(diag);
                    _diagramsToCreate.Remove(diag);
                    clearSelction();
                    }
                }
                _selectedDiagrams.Clear();
            }
        }

            private void onSelectAddDiagramAll(object obj)
            {
            clearSelction();
            foreach (RAMIDiagram diag in _diagramsPossible) { 
                _diagramsToCreate.Add(diag);
            }
            _diagramsPossible.Clear();
            
            }

        private void onSelectRemoveDiagramAll(object obj)
        {
            clearSelction();
            foreach (RAMIDiagram diag in _diagramsToCreate)
            {
                _diagramsPossible.Add(diag);
            }
            _diagramsToCreate.Clear();
        }

        private void onCreateDiagrams(object obj)
        {
            if (_diagramsToCreate.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show(Resources.Message_NoDiagramToCreate.ToString());
            }
            else
            {
                string viewpoint = transformBaseParams.Viewpoint;
                int level = transformBaseParams.Level;
                packageToCreateDiagram = CoreElementLogic.FindViewInLevel(Repository, transformBaseParams.Level, transformBaseParams.Viewpoint);
                foreach (RAMIDiagram diag in _diagramsToCreate)
                {
                    createNewDiagrams(diag.diagramRelatedModel, diag.diagramName, diag.diagramType,diag.diagramStyleEx,packageToCreateDiagram);
                }
                closeWindow();
            }
        }

        private void onCancleCreateDiagram(object obj)
        {
            closeWindow();
        }

        private void clearSelction()
        {
            foreach(RAMIDiagram diag in _selectedDiagrams)
            {
                diag.isSelected = false;
            }
        }

        private void closeWindow()
        {
            _selectedDiagrams.Clear();
            _diagramsPossible.Clear();
            _diagramsToCreate.Clear();
            if(RequestClose != null)
            {
                RequestClose(this, EventArgs.Empty);
            }
        }
    }
}
