//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mehrsan.Dal.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Graph
    {
        public long Id { get; set; }
        public long SrcWordId { get; set; }
        public long DstWordId { get; set; }
    
        public virtual Word Word { get; set; }
        public virtual Word Word1 { get; set; }
    }
}
