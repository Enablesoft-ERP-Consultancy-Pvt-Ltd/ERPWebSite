namespace IExpro.Core.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblXSLTClientMapping")]
    public partial class tblXSLTClientMapping
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int XSLTId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClientId { get; set; }

        public int? UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte UserType { get; set; }

        public virtual tblXSLTDetail tblXSLTDetail { get; set; }
    }
}
