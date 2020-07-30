using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOMEToolbox.BaseGUI.Model
{
    public class RAMIDiagram 
    {
        // Private Constructor
        private RAMIDiagram(string name, string type, string styleEx, string relatedModel)
        {
            diagramName = name;
            diagramType = type;
            diagramRelatedModel = relatedModel;
            diagramStyleEx = styleEx;
        }

        //Diagram Create Factory
        public static RAMIDiagram CreateNewDiagramObject(string name,string type, string styleEx, string relatedModel)
        {
            return new RAMIDiagram(name,type, styleEx, relatedModel);
        }

        public string diagramName { get; set; }
        public string diagramType { get; set; }
        public string diagramRelatedModel { get; set; }
        public string diagramStyleEx { get; set; }
        public bool isSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string DiagramName
        {
            get { return diagramName; }

            set
            {
                if (value != this.diagramName)
                {
                    if (value != null)
                    {
                        this.diagramName = value;
                        NotifyPropertyChanged("DiagramName");
                    }
                }
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }

            set
            {
                if (value != isSelected)
                {

                    isSelected = value;
                    NotifyPropertyChanged("IsSelected");

                }
            }
        }

    }
}
