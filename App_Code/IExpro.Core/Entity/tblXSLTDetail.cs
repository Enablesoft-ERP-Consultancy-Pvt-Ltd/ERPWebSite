namespace IExpro.Core.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblXSLTDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblXSLTDetail()
        {
            tblXSLTClientMappings = new HashSet<tblXSLTClientMapping>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int XSLTId { get; set; }

        public int? DocumentType { get; set; }

        public string XSLTText { get; set; }

        [StringLength(200)]
        public string XSLTSubject { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblXSLTClientMapping> tblXSLTClientMappings { get; set; }
    }
}
