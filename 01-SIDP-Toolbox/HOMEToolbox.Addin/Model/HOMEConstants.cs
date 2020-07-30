using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOMEToolbox.Model
{
    class HOMEConstants
    {
        //MDA Structure
        public const string XMLFileMDAStructure = "CreateStructure.xml";

        //Project Structure
        public const string XMLFileProjectStructure = "ProjectStructure.xml";
        public const string XMLFileRootNodeTag = "rootNode";
        public const string XMLFilePackageTag = "package";

        //Diagram Types XML File
        public const string XMLFileModelKinds = "ModelKinds.xml";
        public const string XMLFileModelTag = "Model";
        public const string XMLFileModelKindsTag = "ModelKind";
        public const string XMLFileModelNameAttribute = "name";
        public const string XMLFileViewTag = "View";
        public const string XMLFileViewNameAttribute = "name";
        public const string XMLFileAttDefaultName = "defaultName";
        public const string XMLFileAttParameter = "parameter";


        //SysML Version
        public const string SysMLVersion = "SysML1.4";



        //SPES Viewpoints
        public const string ViewRequirements = "Requirements Viewpoint";
        public const string ViewFunctionalVP = "Function Viewpoint";
        public const string ViewLogicalVP = "Logical Viewpoint";
        public const string ViewTechnical = "Technical Viewpoint";


        //Element Stereotypes
        public const string BUC = "SPES::Business Use Case";
        public const string HLUC = "SPES::High Level Use Case";
        public const string PUC = "SPES::Primary Use Case";
        public const string BusinessActor = "SPES::Business Actor";
        public const string LogicalActor = "SPES::Logical Actor";
        public const string UseCase = "UseCase";
        public const string Actor = "Actor";
        public const string Node = "Node";
        //Element Types
        public const string Requirement = "Requirement";
        public const string Interface = "Interface";

        //SysML Diagram Stereotypes

        public const string DiagSysMLBlockDefinition =  SysMLVersion + "::BlockDefinition";
        public const string DiagSysMLUseCase = SysMLVersion + "::UseCase";
        public const string DiagSysMLInternalBlock = SysMLVersion + "::InternalBlock";
        public const string DiagSysMLRequirement = SysMLVersion + "::Requirement";
        public const string DiagSysMLSequence = SysMLVersion + "::Sequence";
        public const string DiagSysMLActivity = SysMLVersion + "::Activity";
        public const string DiagSysMLStateMachine = SysMLVersion + "::StateMachine";


        //DSL Diagram Stereotypes
        public const string DiagBusiness = "SPES Diagram Profile::Business Diagram";
        public const string DiagContext = "SPES Diagram Profile::Context Diagram";
        public const string DiagFunction = "SPES Diagram Profile::Function Development Diagram";
        public const string DiagFunctionNetwork = "SPES Diagram Profile::Function Network Diagram";
        public const string DiagLogicalArch = "SPES Diagram Profile::Logical Architecture Diagram";
        public const string DiagActorMapping = "SPES Diagram Profile::Mapping Diagram";
        public const string DiagTechnicalArch = "SPES Diagram Profile::Technical Architecture Diagram";
        public const string DiagInformation = "SPES Diagram Profile::Information Diagram";
        public const string DiagEE = "SPES Diagram Profile::EE Diagram";
        public const string DiagMechanic = "SPES Diagram Profile::Mechanics Diagram";
        public const string DiagElectrics = "SPES Diagram Profile::Electrics Diagram";
        public const string DiagThermal = "SPES Diagram Profile::Thermal Diagram";


        //Connection StereoTypes
        public const string Association = "Association";
        public const string Invokes = "invokes";
        public const string Trace = "trace";
        //Connection Types
        public const string Dependency = "Dependency";
        public const string Generalization = "Generalization";
        public const string Realization = "Realisation";
        //Package Stereotypes
        public const string StereoPackModel = "model";
    }
}
