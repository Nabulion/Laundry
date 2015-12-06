//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UserLaundry
{
    using System;
    using System.Collections.Generic;
    
    public partial class MachineProgram
    {
        public MachineProgram()
        {
            this.StartedWashCosts = new HashSet<StartedWashCost>();
        }
    
        public int id { get; set; }
        public string programType { get; set; }
        public Nullable<int> Machine { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> programTime { get; set; }
    
        public virtual Machine Machine1 { get; set; }
        public virtual ICollection<StartedWashCost> StartedWashCosts { get; set; }

        public override string ToString()
        {
            return programType + " price " + price + " duration " + programTime + " minutes";
        }
    }
}
