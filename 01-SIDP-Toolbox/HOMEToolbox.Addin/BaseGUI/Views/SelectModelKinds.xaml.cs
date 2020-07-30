using EA;
using HOMEToolbox.BaseGUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Toolbox.Core.Helpers;
using Toolbox.Core.Interfaces;

namespace HOMEToolbox.BaseGUI
{
    /// <summary>
    /// Interaktionslogik für SelectModelKinds.xaml
    /// </summary>
    public partial class SelectModelKinds : Window
    {
        public IToolboxInfo Toolbox { get; set; }
        private Logger Logger => Toolbox.Logger;
        private AssemblyInfo Info => Toolbox.Info;
        private Repository Repository => Toolbox.AddIn.App.Repository;


        public SelectModelKinds()
        {      
            InitializeComponent();
        }

        public void initCreateDiagramWindow(string parameter,IToolboxInfo Toolbox)
        {

            SelectModelKindsViewModel viewModel = new SelectModelKindsViewModel();

            viewModel.Toolbox = Toolbox;
            viewModel.Repository = Toolbox.AddIn.App.Repository;
            viewModel.buildViewModel(parameter);
            
            viewModel.getPossibleDiagrams();
            this.DataContext = viewModel;
            viewModel.RequestClose += delegate (object sender, EventArgs args)
            {
                this.Close();
            };
        }
    }
}
