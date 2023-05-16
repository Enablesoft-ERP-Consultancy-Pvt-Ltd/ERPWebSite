<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportViewer2.aspx.cs" Inherits="Default23"  EnableEventValidation="false"%>



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
</head>
<body>
    <form id="form1" runat="server">
    <div id="report" runat="server">
       <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" HasToggleGroupTreeButton="False" ToolPanelView="None" />
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
