namespace IExpro.Core.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDocumentType")]
    public partial class tblDocumentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentId { get; set; }

        [Required]
        [StringLength(200)]
        public string DocumentType { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
