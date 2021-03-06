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
    
    public partial class CultivationStep_History
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CultivationStep_History()
        {
            this.MaterialNeeded_History = new HashSet<MaterialNeeded_History>();
        }
    
        public int Id { get; set; }
        public Nullable<int> CropId { get; set; }
        public string StepName { get; set; }
        public string VideoURL { get; set; }
        public string ImageURL { get; set; }
        public string LocalVideoURL { get; set; }
        public string LocalImageURL { get; set; }
        public Nullable<bool> Completed { get; set; }
        public Nullable<bool> VideoViewed { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public string StepDescription { get; set; }
        public string TitleAudio { get; set; }
        public string DescriptionAudio { get; set; }
        public string DescAudioLocal { get; set; }
        public string TitleAudioLocal { get; set; }
        public string MediaType { get; set; }
        public Nullable<int> CultivationCropId { get; set; }
        public Nullable<System.Guid> CultivationUID { get; set; }
    
        public virtual CropCultivation_History CropCultivation_History { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialNeeded_History> MaterialNeeded_History { get; set; }
    }
}
