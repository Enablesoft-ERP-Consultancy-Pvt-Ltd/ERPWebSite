namespace IExpro.Core.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FormName")]
    public partial class FormName
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormName()
        {
            FormName1 = new HashSet<FormName>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MenuID { get; set; }

        [StringLength(250)]
        public string NavigateURL { get; set; }

        [StringLength(50)]
        public string DisplayName { get; set; }

        public int? ParentId { get; set; }

        [StringLength(250)]
        public string ToolTip { get; set; }

        public int? Type { get; set; }

        public int? Isvisible { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormName> FormName1 { get; set; }

        public virtual FormName FormName2 { get; set; }
    }
}
