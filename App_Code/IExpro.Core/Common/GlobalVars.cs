

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IExpro.Core.Common
{

    public enum DocumentType
    {
        [Description("EXPORT INVOICE")]
        EXPORTINVOICE = 1,
        [Description("TAX INVOICE")]
        TAXINVOICE = 2,
        [Description("Invoice")]
        Invoice = 3,
    }


    public enum UserType
    {
        SuperAdmin = 1,
        SuperAdminUser = 2,
        Admin = 3,
        AdminUser = 4,
        Client = 5,
        Customer = 6,
    }
    public enum ProcessStatus
    {
        Pending = 0,
        Completed = 1,
 
    }

}

