using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HOMEToolbox.Model
{
    class TransformParams
    {
        public bool Initialized { get; private set; } = false;

        public int Level { get; set; }
        public string Viewpoint { get; set; }
        public string ConnectorStereotype { get; set; }

        private static readonly Dictionary<string, string> ViewParameterToPackageName = new Dictionary<string, string>
        {
            {"r", HOMEConstants.ViewRequirements},
            {"f", HOMEConstants.ViewFunctionalVP},
            {"l", HOMEConstants.ViewLogicalVP},
            {"t", HOMEConstants.ViewTechnical},
        };

        public TransformParams(string parameter)
        {
            var pattern = @"^r(\d)([rflt])";
            var match = Regex.Match(parameter, pattern);
            if (!match.Success)
                return;
            int level;
            if (!int.TryParse(match.Groups[1].ToString(), out level))
                return;
            //Take over sub parameters
            Level = level;
            Viewpoint = ViewParameterToPackageName[match.Groups[2].ToString()];

            Initialized = true;

        }
    }
}
