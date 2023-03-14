<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmShowFinishingDetail.aspx.cs"
    Inherits="Masters_Process_frmShowFinishingDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div style="float: left; width: 800px; position: relative;">
        <div style="width: 500px; height: 21px; position: absolute; top: 0px; left: -4px;
            padding-left: 10px; font: normal bold 14px Arial, Helvetica, sans-serif; padding-top: 13px;
            color: #00b7ff;">
            DETAILS
        </div>
    </div>
    <div style="width: 1000px; padding: 40px 10px 0px 8px; text-align: justify; font-size: 12px;">
        <table width="98%">
            <tr style="background-color: Teal; color: White">
                <td style="width: 100px; font-size: 15px; height: 50px">
                    JOB
                </td>
                <td style="width: 200px; font-size: 15px">
                    EMPLOYEE
                </td>
                <td style="width: 50px; font-size: 15px">
                    PO
                </td>
                <td style="width: 100px; font-size: 15px">
                    ORDERDATE
                </td>
                <td style="width: 100px; font-size: 15px">
                    REQBYDATE
                </td>
                <td style="width: 95px; font-size: 15px">
                    ORDERQTY
                </td>
                <td style="width: 70px; font-size: 15px">
                    RECQTY
                </td>
                <td style="width: 100px; font-size: 15px">
                    PENDINGQTY
                </td>
                <td style="width: 75px; font-size: 15px">
                    LATEBY
                </td>
            </tr>
        </table>
    </div>
    <asp:Timer ID="Timer1" runat="server" OnTick="gettickvalue" Interval="120000">
    </asp:Timer>
    <asp:UpdatePanel ID="BannerPanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
            <div style="position: relative; width: 1000px; padding: 10px 10px 0px 8px; text-align: justify;
                font-size: 12px;">
                <asp:Table ID="tblMenu" runat="server" CellSpacing="0" CellPadding="0" Width="98%"
                    Style="display: table; border-collapse: separate; border-color: Gray;" GridLines="Both">
                </asp:Table>
                <marquee scrollamount="1" direction="up" height="400px">       
              <asp:Table id="tblrecord" runat="server" cellspacing="0" cellpadding="0" width="98%" 
            style="display:table;border-collapse: separate; border-color: Gray;height:200px" GridLines="Both">            
        </asp:Table>
     
        </marquee>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
