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
    
    public partial class Cultivation_Steps
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cultivation_Steps()
        {
            this.CropSteps_Material = new HashSet<CropSteps_Material>();
        }
    
        public int Id { get; set; }
        public int Crop_Id { get; set; }
        public string Step_Name { get; set; }
        public string MediaURL { get; set; }
        public string Step_Description { get; set; }
        public string Description_audio { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CropSteps_Material> CropSteps_Material { get; set; }
        public virtual Crop Crop { get; set; }
    }
}
