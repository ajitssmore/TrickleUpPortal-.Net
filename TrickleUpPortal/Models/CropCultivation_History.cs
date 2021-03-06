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
    
    public partial class CropCultivation_History
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CropCultivation_History()
        {
            this.CultivationStep_History = new HashSet<CultivationStep_History>();
        }
    
        public int Id { get; set; }
        public Nullable<float> LandArea { get; set; }
        public string LandType { get; set; }
        public string CropName { get; set; }
        public Nullable<int> CropId { get; set; }
        public string FilePath { get; set; }
        public Nullable<bool> Ready { get; set; }
        public Nullable<bool> Complete { get; set; }
        public Nullable<float> Total_Income { get; set; }
        public Nullable<int> Production_kg { get; set; }
        public int CultivationHistoryId { get; set; }
        public Nullable<System.Guid> CultivationUID { get; set; }
    
        public virtual Cultivation_History Cultivation_History { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CultivationStep_History> CultivationStep_History { get; set; }
    }
}
