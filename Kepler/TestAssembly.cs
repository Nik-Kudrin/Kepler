//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kepler
{
    using System;
    using System.Collections.Generic;
    
    public partial class TestAssembly : BuildObject
    {
        public TestAssembly()
        {
            this.TestSuites = new HashSet<TestSuite>();
        }
    
        public long BuildId1 { get; set; }
    
        public virtual Build Build { get; set; }
        public virtual ICollection<TestSuite> TestSuites { get; set; }
    }
}
