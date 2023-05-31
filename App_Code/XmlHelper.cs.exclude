using System.IO;
using System.Xml.Xsl;
using System.Xml;
using System;
using System.Web;

namespace IExpro.Core.Common
{
    public static class XmlHelper
    {
     

        public static string UnescapeXMLValue(this string xmlString)
        {
            if (xmlString == null)
                throw new ArgumentNullException("xmlString");


            return xmlString.Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");
        }

        public static string EscapeXMLValue(this string xmlString)
        {

            if (xmlString == null)
                throw new ArgumentNullException("xmlString");


            return xmlString.Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;").Replace(">", "&gt;").Replace("<", "&lt;");
        }








        public static string XmlWriterFunction(string xml, XsltArgumentList arguments, string xslt)
        {
            //XslCompiledTransform transform = new XslCompiledTransform();
            //using (XmlReader reader = XmlReader.Create(new StringReader(xslt)))
            //{
            //    transform.Load(reader);
            //}





            // Creating XSLCompiled object
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xslt);

            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                transform.Transform(reader, arguments, results);
            }
            return results.ToString();
        }






    }
}
