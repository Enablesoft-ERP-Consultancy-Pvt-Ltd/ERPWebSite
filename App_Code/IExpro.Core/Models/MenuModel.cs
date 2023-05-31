using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Models
{
    public class MenuModel
    {
        public int MenuId { get; set; }
        public int? ParentId { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string MenuUrl { get; set; }
        public string Description { get; set; }
        public float OrderNo { get; set; }
        public IEnumerable<MenuModel> MenuList { get; set; }

    }
}
