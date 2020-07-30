using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using EA;
using Forms = System.Windows.Forms;
using HOMEToolbox.BaseGUI;
using HOMEToolbox.Logic;

using Toolbox.Core;
using Toolbox.Core.Helpers;
using Toolbox.Core.Interfaces;
using Toolbox.Core.Model;

using Resources = global::HOMEToolbox.Properties.Resources;
using Toolbox.Core.Logic;
using System.Security.Permissions;

namespace HOMEToolbox
{
    [Guid("04AC9F0A-A841-446A-A436-EC035F3CC565"),
        ComVisible(true)]
    public class Addin : MarshalByRefObject, IToolboxAddin, IToolboxInfo, IStepThroughDockableHost
    {
        private readonly CommandHandlerLogic _commandHandler;

        private readonly AddinBase _core;
        
        public Addin()
        {
            _core = new AddinBase(this);
            Info = new AssemblyInfo(GetType().Assembly);
            _commandHandler = new CommandHandlerLogic(this);
            Logger.LogFileName = "hometoolbox_addin.log";
        }

        #region IToolboxInfo

        public IToolboxAddin AddIn => this;

        public IStepThroughDockableHost DockableHost => this;

        public Logger Logger { get; set; } = new Logger();

        public StabilityTypes Stability { get; set; }

        public AssemblyInfo Info { get; set; }

        public string MdgTechnologyPath => "";

        public string ReferenceDataPath => "";

        public string AddinShortName { get; } = "HOME";


        #endregion

        #region IStepThroughDockableHost Members

        public bool IsItemEnabled(string command)
        {
            return _commandHandler.IsCommandEnabled(command);
        }

        public bool IsItemVisible(string command)
        {
            return _commandHandler.IsCommandVisible(command);
        }

        public void ExecuteCommand(string command, string parameter)
        {
            _commandHandler.ExecuteCommand(command, parameter);
        }

        public string DockableTitle { get; } = Resources.DockableTitle;
        public string DockableConfigPath => Path.Combine(Info.AssemblyDirectory, GetAddInSetting("StepThroughConfig", "HOMEStepThrough.xml"));

        public IStepThroughDockable Dockable { get; set; } = null;

        #endregion //IStepthroughDockableHost Members

        #region IToolboxAddin Members

        public App App
        {
            get { return _core.App; }
            set { _core.App = value; }
        }

        public IToolboxCore Core => _core;

        public event EventHandler ActiveRepositoryChanged
        {
            add { _core.ActiveRepositoryChanged += value; }
            remove { _core.ActiveRepositoryChanged -= value; }
        }

        public event EventHandler ActiveDiagramChanged
        {
            add { _core.ActiveDiagramChanged += value; }
            remove { _core.ActiveDiagramChanged -= value; }
        }

        public string GetAddInSetting(string setting, string defaultValue)
        {
            return ((IToolboxAddin)_core).GetAddInSetting(setting, defaultValue);
        }

        #region EA Event Handlers

        private List<string> menuFunctions = new List<string>() { Resources.MenuShowStepThrough, Resources.MenuTransformProcess, Resources.MenuAbout };

        //Called when user Click Add-Ins Menu item from within EA.
        //Populates the Menu with our desired selections.
        public object EA_GetMenuItems(Repository repository, string location, string menuName)
        {
            var aPackage = repository.GetTreeSelectedPackage();
            object selectedItem;
            var itemType = repository.GetTreeSelectedItem(out selectedItem);

            if (menuName == "")
                return Resources.MenuHeader;

            if (menuName != Resources.MenuHeader)
                return "";

            var arDyn = new List<string>();
            
            foreach (string menuItem in menuFunctions)
            {
                if (_commandHandler.IsCommandVisible(menuItem))
                    arDyn.AddRange(new[] { menuItem });
            }

            Logger.Debug($">>> {Info.GetCurrentMethod()}: {arDyn}");

            return arDyn.ToArray();
        }

        //Called once Menu has been opened to see what menu items are active.
        public void EA_GetMenuState(Repository repository, string location, string menuName, string itemName, ref bool isEnabled, ref bool isChecked)
        {
            Logger.Debug($">>> {Info.GetCurrentMethod()} Menu: {itemName}");

            isEnabled = _commandHandler.IsCommandEnabled(itemName);
        }

        //Called when user makes a selection in the menu.
        //This is your main exit point to the rest of your Add-in
        public void EA_MenuClick(Repository repository, string location, string menuName, string itemName)
        {
            Logger.Info($">>> {Info.GetCurrentMethod()} Menu: {itemName}");

            _commandHandler.ExecuteCommand(itemName);
        }

        public void EA_OnPostInitialized(Repository repository)
        {
            _core.RegisterStepThroughDockable(this);

            ((IToolboxAddin)_core).EA_OnPostInitialized(repository);
        }

        public string EA_Connect(Repository repository)
        {
            return ((IToolboxAddin)_core).EA_Connect(repository);
        }

        public object EA_OnInitializeTechnologies(Repository repository)
        {
            return ((IToolboxAddin)_core).EA_OnInitializeTechnologies(repository);
        }

        public void EA_FileOpen(Repository repository)
        {
            ((IToolboxAddin)_core).EA_FileOpen(repository);
        }

        public void EA_FileNew(Repository repository)
        {
            ((IToolboxAddin)_core).EA_FileNew(repository);
        }

        public void EA_FileClose(Repository repository)
        {
            ((IToolboxAddin)_core).EA_FileClose(repository);
        }

        public void EA_OnPostOpenDiagram(Repository repository, int diagramId)
        {
            ((IToolboxAddin)_core).EA_OnPostOpenDiagram(repository, diagramId);
        }

        public void EA_OnPostCloseDiagram(Repository repository, int diagramId)
        {
            ((IToolboxAddin)_core).EA_OnPostCloseDiagram(repository, diagramId);
        }

        public bool EA_OnPostNewDiagram(Repository repository, EventProperties info)
        {
            return ((IToolboxAddin)_core).EA_OnPostNewDiagram(repository, info);
        }

        public void EA_OnTabChanged(Repository repository, string tabName, int diagramId)
        {
            ((IToolboxAddin)_core).EA_OnTabChanged(repository, tabName, diagramId);
        }

        public void EA_Disconnect()
        {
            ((IToolboxAddin)_core).EA_Disconnect();
        }

        #endregion //EA Event Handlers

        #endregion //IToolboxAddin Members
		
        #region Remoting Lifetime

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion //Remoting Lifetime
    }
}