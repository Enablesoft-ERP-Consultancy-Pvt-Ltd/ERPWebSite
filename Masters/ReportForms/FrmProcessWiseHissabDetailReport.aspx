<%@ Page Title="Process Wise Hissab Detail Report" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmProcessWiseHissabDetailReport.aspx.cs" Inherits="Masters_ReportForms_FrmProcessWiseHissabDetailReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
     <script type="text/javascript">
         function validatesave() {
             var Message = "";
             var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
             if (selectedindex < 0) {
                 Message = Message + "Please select Company Name!!\n";
             }
             selectedindex = $("#<%=DDProcessName.ClientID %>").attr('selectedIndex');
             if (selectedindex <= 0) {
                 Message = Message + "Please Select Process Name. !!\n";
             }
             selectedindex = $("#<%=DDEmployerName.ClientID %>").attr('selectedIndex');
             if (selectedindex <= 0) {
                 Message = Message + "Please select Employee Name. !!\n";
             }
             selectedindex = $("#<%=DDHissabSlipNo.ClientID %>").attr('selectedIndex');
             if (selectedindex <= 0) {
                 Message = Message + "Please select Hissab SlipNo. !!\n";
             }
             if (Message == "") {
                 return true;
             }
             else {
                 alert(Message);
                 return false;
             }
         }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" style="width: 50%">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" Text="Company Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="200px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Label ID="Label4" Text=" Process Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDProcessName" runat="server" TargetControlID="DDProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                    <td>
                             <asp:Label ID="Label3" Text="Emp Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDEmployerName" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDEmployerName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDEmployerName" runat="server" TargetControlID="DDEmployerName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>

                     <tr>
                    <td>
                             <asp:Label ID="Label5" Text="Hissab SlipNo" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDHissabSlipNo" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>                            
                        </td>
                    </tr>

                   <%-- <tr>
                        <td>
                            <asp:Label ID="lblfromdate" Text="From Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromdate" CssClass="textb" runat="server" />
                            <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txtfromdate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="To Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txttodate" CssClass="textb" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2" align="right">
                            <%--<asp:CheckBox ID="chkExportForExcel" runat="server" CssClass="checkboxbold" Text=" Export Excel Report"  Visible="false"/>--%>
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnpreview_Click" OnClientClick="return validatesave();"/>
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
