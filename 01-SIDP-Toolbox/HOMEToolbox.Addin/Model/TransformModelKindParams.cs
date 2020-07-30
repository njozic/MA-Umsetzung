using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace HOMEToolbox.Model
{
    class TransformModelKindParams
    {

        public bool Initialized { get; private set; } = false;

        public string DestDiagramType { get; set; }
        public string DestDiagramStyleEx { get; set; }

        public TransformModelKindParams(string parameter)
        {
            var subParams = parameter.Split(';');
            if (subParams.Length < 2)
                return;

            DestDiagramType = subParams[0];
            DestDiagramStyleEx = subParams[1];

            Initialized = true;
        }

    }
}

