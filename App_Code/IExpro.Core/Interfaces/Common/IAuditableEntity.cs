// (c) Ingenium Technologies 2016
// Created in 2016 as an unpublished copyrighted work.  This program
// and the information contained in it is confidential and proprietary to
// Ingenium Technologies and may not be used, copied, or reproduced without the prior written
// permission of Ingenium.

//*****************************************************************************************************************
// Revision History
//*****************************************************************************************************************
//    Date        Author                     Version     Defect ID       Change Description
//*****************************************************************************************************************
// 13 Oct 16      Rakesh Gangwar              1.0                          Initial Version    

//*****************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IExpro.Core.Interfaces.Common
{

    public interface IAuditableEntity
    {
       
        byte Status { get; set; }
        long CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }


    }
}
