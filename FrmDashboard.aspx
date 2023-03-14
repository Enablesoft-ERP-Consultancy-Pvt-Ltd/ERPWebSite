<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Title="Download Dashboard Data" CodeFile="FrmDashboard.aspx.cs"
    Inherits="FrmDashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="Scripts/JScript.js" type="text/jscript"></script>
    <script src="Scripts/jquery-1.4.1.js" type="text/jscript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "FrmDashboard");
        }
    </script>
    <asp:UpdatePanel ID="upnew1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <div>
                    <tr style="height: 50px">
                        <td>
                        </td>
                    </tr>
                     <tr align="center" style="height: 50px">
                        <td>
                                                    <asp:Label ID="lblmonth" Text="From Date" CssClass="labelbold" runat="server" />
                                                   
                                                    <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                                    <br />
                                                    <asp:CalendarExtender ID="calfromdate" runat="server" TargetControlID="txtfromdate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender> <br />
                                                     <asp:Label ID="Label4" Text="To Date" CssClass="labelbold" runat="server" />
                                                   
                                                    <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                                    <br />
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                               
                    </tr>
                    <tr align="center" style="height: 50px">
                        <td>
                            <asp:Button ID="BtnDownloadDashboardData" runat="server" Text="Click for Download Dashboard Data"
                                OnClick="BtnDownloadDashboardData_Click" 
                                CssClass="buttonnorm" Width="230px" />

                               <%-- <asp:Button ID="Button1" runat="server" Text="Click for Download Dashboard Data"
                                OnClick="BtnDownloadDashboardData_Click" OnClientClick="return confirm('Do you want to take data back-up its take some minutes ?')"
                                CssClass="buttonnorm" Width="175px" />--%>
                        </td>
                    </tr>
                    <tr align="center" style="width: 75%; margin-left: 10%">
                        <td>
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </div>
            </table>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="BtnDownloadDashboardData" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
