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
    
    public partial class Village
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Village()
        {
            this.VillageContacts = new HashSet<VillageContact>();
        }
    
        public int Id { get; set; }
        public string VillageName { get; set; }
        public Nullable<int> Grampanchayat { get; set; }
        public Nullable<int> District { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual District District1 { get; set; }
        public virtual Grampanchayat Grampanchayat1 { get; set; }
        public virtual State State1 { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VillageContact> VillageContacts { get; set; }
    }
}
