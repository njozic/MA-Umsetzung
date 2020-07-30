using EA;
using HOMEToolbox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core.Helpers;

namespace HOMEToolbox.Logic
{
    public class ModelElementLogic {

        private static Dictionary<int, Element> _elementCache = null;
        private static Dictionary<string, Element> _elementGUIDCache = null;

        public static Element GetElementByGuid(Repository repository, string guid) {
            Element elem = null;

            if (_elementGUIDCache == null)
                _elementGUIDCache = new Dictionary<string, Element>();

            if (_elementGUIDCache.TryGetValue(guid, out elem))
                return elem;

            elem = repository.GetElementByGuid(guid);
            _elementGUIDCache.Add(guid, elem);

            return elem;
        }

        public static Element GetElementById(Repository repository, int elementId) {
            Element elem = null;

            if (_elementCache == null)
                _elementCache = new Dictionary<int, Element>();

            if (_elementCache.TryGetValue(elementId, out elem))
                return elem;

            elem = repository.GetElementByID(elementId);
            _elementCache.Add(elementId, elem);

            return elem;
        }

        public static void ClearElementCache() {
            _elementCache.Clear();
            _elementCache = null;
        }

        /// <summary>
        /// Selects an existing (sub-)package or creates a new one
        /// </summary>
        /// <param name="root">Provided Root-Package or Model</param>
        /// <param name="packageName">(New) Package Name</param>
        /// <param name="stereotype">Stereotype (if needed)</param>
        /// <returns>Existing or newly created Package</returns>
        public static Package SelectOrCreatePackage(Package root, string packageName, string stereotype="")
        {
            var package = root.Packages.Cast<Package>().FirstOrDefault(p => p.Name == packageName) ?? root.Packages.AddNew(packageName, "Nothing");
            package?.Update();
            if (!string.IsNullOrEmpty(stereotype))
            {
                package.StereotypeEx = stereotype;
                package?.Update();
            }
            root.Packages.Refresh();
            root.Update();
            return package;
        }

        /// <summary>
        /// Selects an existing diagram or creates a new one (if desired)
        /// </summary>
        /// <param name="root">Provided Root-Package</param>
        /// <param name="diagramName">(New) Diagram Name</param>
        /// <param name="createIfNotExists">Flag if a Diagram should be created if not found</param>
        /// <param name="diagramType">New Diagram Type</param>
        /// <returns>Existing or newly created Diagram</returns>
        public static Diagram SelectOrCreateDiagram(Package root, string diagramName, bool createIfNotExists, string diagramType)
        {
            //Look for existing Diagram first...
            Diagram diagram = root.Diagrams.Cast<Diagram>().FirstOrDefault(p => p.Name == diagramName) ??
                                    //...and create a new if not existing but desired
                                    (createIfNotExists ? root.Diagrams.AddNew(diagramName, diagramType) : null);
            diagram?.Update();
            root.Diagrams.Refresh();
            root.Update();
            return diagram;
        }

        /// <summary>
        /// Selects an existing Element or creates a new one
        /// </summary>
        /// <param name="root">Provided Root-Package</param>
        /// <param name="name">(New) Element Name</param>
        /// <param name="elementType">Type of the Element/param>
        /// <returns>Existing or newly created Element</returns>
        public static Element SelectOrCreateElement(Package root, string name, string elementType) {
            Element e = root.Elements.Cast<Element>().FirstOrDefault(p => p.Name == name && p.Type == elementType) ?? root.Elements.AddNew(name, elementType);
            e?.Update();
            root.Elements.Refresh();
            root.Update();
            return e;
        }

        public static Element SelectOrCreateElement(Diagram diagram, string name, string elementType) {
            Element e = new Element();

            return e;
        }

        /// <summary>
        /// Creates a new diagram
        /// </summary>
        /// <param name="root">Provided Root-Package</param>
        /// <param name="diagramName">Diagram Name</param>
        /// <param name="diagramType">New Diagram Type</param>
        /// <returns>Existing or newly created Diagram</returns>
        public static Diagram CreateDiagram(Package root, string diagramName, string type, string styleEx){

            Diagram diagram = root.Diagrams.AddNew(diagramName, type);
            diagram?.Update();
            diagram.StyleEx = styleEx;
            diagram?.Update();
            root.Diagrams.Refresh();
            root.Update();
            return diagram;
        }


        /// <summary>
        /// Adds an element to a diagram with prior checking if already present
        /// </summary>
        /// <param name="destDiagram">Provided Diagram (may be null)</param>
        /// <param name="element">Provided Element (may be null)</param>
        public static void AddToDiagram(Diagram destDiagram, Element element) {
            //Check if current diagram already contains this element
            //...unfortunatelly a manual check as no API call is available at this time
            if (destDiagram == null || element == null || destDiagram.DiagramObjects.Cast<DiagramObject>().Any(diagObject => diagObject.ElementID == element.ElementID))
                return;

            //If not, create a new link
            DiagramObject diagramObj = (DiagramObject)destDiagram.DiagramObjects.AddNew(element.Name, element.Type);
            diagramObj.ElementID = element.ElementID;
            diagramObj.Update();
            destDiagram.DiagramObjects.Refresh();
            destDiagram.Update();
        }
    }

}
