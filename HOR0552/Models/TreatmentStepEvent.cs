using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOR0552.Models
{
    class TreatmentStepEvent
    {
        public TreatmentStep step { get; set; } = null;
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public string doctor { get; set; } = "";
        public string location { get; set; } = "";
    }

}
