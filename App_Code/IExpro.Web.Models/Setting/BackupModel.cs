using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BackupModel
/// </summary>
/// 
namespace IExpro.Web.Pages
{
    public class BackupModel
    {
        public int BackupId { get; set; }
        public string DatabaseName { get; set; }
        public string BackupPath { get; set; }


    }
}