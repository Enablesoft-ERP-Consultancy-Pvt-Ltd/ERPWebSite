using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IExpro.Core.Common
{





    public static class CommonHelper
    {
        public static string GetURI()
        {
            var urlBuilder = new UriBuilder(HttpContext.Current.Request.Url.AbsoluteUri) { Path = HttpContext.Current.Request.ApplicationPath, Query = null, Fragment = null };
            return urlBuilder.Uri.ToString();
        }
    }
}
