//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrickleUpPortal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LiveStock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LiveStock()
        {
            this.LiveStockBreeds = new HashSet<LiveStockBreed>();
            this.LiveStock_Steps = new HashSet<LiveStock_Steps>();
        }
    
        public int Id { get; set; }
        public string StockName { get; set; }
        public string ImageURL { get; set; }
        public string AudioURL { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> ActiveBy { get; set; }
        public Nullable<System.DateTime> ActiveOn { get; set; }
        public Nullable<bool> Active { get; set; }
        public string AliasName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LiveStockBreed> LiveStockBreeds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LiveStock_Steps> LiveStock_Steps { get; set; }
    }
}
