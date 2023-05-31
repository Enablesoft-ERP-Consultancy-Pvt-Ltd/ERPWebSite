namespace IExpro.Core.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NewUserDetail")]
    public partial class NewUserDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(60)]
        public string UserName { get; set; }

        [StringLength(60)]
        public string Designation { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(60)]
        public string LoginName { get; set; }

        [StringLength(100)]
        public string PassWord { get; set; }

        public int? DeptId { get; set; }

        public int? CompanyId { get; set; }

        public int? VarUserid { get; set; }

        public int? LoginFlag { get; set; }

        public int? UserType { get; set; }

        public byte? canedit { get; set; }

        public int? IsVisible { get; set; }

        public int? canbackdateentry { get; set; }

        public int? canseeDevelopmentcons { get; set; }

        public int? canseeProductioncons { get; set; }
    }
}
