using IExpro.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace IExpro.Core.Models
{
    public class SelectList
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }


    public abstract class BaseEntity
    {
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

     
    }

    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        public virtual T Id { get; set; }
    }
}