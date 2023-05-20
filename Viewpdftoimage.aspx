<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Viewpdftoimage.aspx.cs" Inherits="_Default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript">
        function SetBrowserMode() {
            engine = null;
            if (window.navigator.appName == "Microsoft Internet Explorer") {
                if (document.documentMode) // IE8 or later
                    engine = document.documentMode;
                else // IE 5-7
                {
                    engine = 5; // Assume quirks mode unless proven otherwise
                    if (document.compatMode) {
                        if (document.compatMode == "CSS1Compat")
                            engine = 7; // standards mode
                    }
                }

            }

        }
    </script>    
    <script type="text/javascript" src="crystalreportviewers13/js/crviewer/crv.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- <div id="dvReport">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    </div>
    <br />
    <input type="button" id="btnPrint" value="Print" onclick="Print()" />
    <br />
    <asp:Label ID="LblReportName" runat="server" Text="Report Name"></asp:Label>--%>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="LblReportName" runat="server" Text="Report Name"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
