<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportViewer.aspx.cs" Inherits="Default2"
    EnableEventValidation="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 <%--   <script type="text/javascript">
        function Print() {
            var dvReport = document.getElementById("report");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            }
            else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }
    </script>--%>
    
    <script type="text/javascript" src="crystalreportviewers13/js/crviewer/crv.js"></script>
    <script type="text/javascript">
        function SetBrowserMode() {
            engine = null;
            if (window.navigator.appName == "Microsoft Internet Explorer") {
                // This is an IE browser. What mode is the engine in?
                if (document.documentMode) // IE8 or later
                    engine = document.documentMode;
                else // IE 5-7
                {
                    engine = 5; // Assume quirks mode unless proven otherwise
                    if (document.compatMode) {
                        if (document.compatMode == "CSS1Compat")
                            engine = 7; // standards mode
                    }
                    // There is no test for IE6 standards mode because that mode  
                    // was replaced by IE7 standards mode; there is no emulation.
                }
                // the engine variable now contains the document compatibility mode.
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="report" runat="server">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
            HasToggleGroupTreeButton="False" ToolPanelView="None" />
        <br />
        <asp:Label ID="LblPath" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="LblFormula" runat="server" Text=""></asp:Label>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
