using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOR0552.Models
{
    public class TreatmentStep
    {
        public int step { get; set; }
        public Procedure procedure { get; set; }
        public int? nextStep { get; set; }
        public Dictionary<string, int>? nextSteps { get; set; }
        public bool isCompleted { get; set; } = false;
        public DateTime? stepDate { get; set; } = null;
        public int deadlineInDays { get; set; } = 0;
        public int daysUntilDeadline { get; set; } = int.MaxValue;

    }
}
