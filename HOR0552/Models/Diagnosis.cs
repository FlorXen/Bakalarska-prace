using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOR0552.Models
{
    public class Diagnosis
    {
        public string diagnosisId { get; set; }
        public string name { get; set; }
        public List<TreatmentStep> treatmentPlan { get; set; }
        public DateTime? startDate { get; set; }
        public int? currentStepNum { get; set; } = 1;

        public override string ToString()
        {
            //var treatmentPlanString = treatmentPlan != null ? string.Join(", ", treatmentPlan.Select(tp => tp.procedure.name)) : "No treatment plan";
            //return $"Diagnosis ID: {diagnosisId}, Name: {name}, Start Date: {startDate?.ToString("yyyy-MM-dd") ?? "N/A"}, Treatment Plan: {treatmentPlanString}";
            return $"{name}";
        }
    }
}
