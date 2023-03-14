<%@ Page Title="GATEIN ORDERNO WISE REPORTS" Language="C#" AutoEventWireup="true" CodeFile="FrmGateInOrderNoWiseReport.aspx.cs"
    Inherits="Masters_ReportForms_FrmGateInOrderNoWiseReport" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validate() {
            var message = "";
           
                if (document.getElementById('<%= DDCustCode.ClientID %>').value <= 0) {
                    message = message + "Please select customer code..\n";
                } 
          
            if (message != "") {
                alert(message);
                return false;
            }
        }
    </script>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 90%; height: 100%;">
                <tr style="width: 100%">
                    <td style="width: 80px">
                    </td>
                    <td>
                        <div style="width: 250px; max-height: 100%; float: left; border-style:none; border-width:thin">
                            <table>
                                <tr>
                                    <td>&nbsp;</td>                                    
                                </tr>  
                            </table>
                        </div>
                        <div style="float: left; width: 350px; max-height: 400px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>                              
                                <tr id="TRCustomerCode" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCustCode" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TROrderNo" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDOrderNo" runat="server" Width="250px" AutoPostBack="true"
                                            CssClass="dropdown" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>                                
                               
                                <tr id="TRcheckdate" runat="server" visible="true">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold"
                                            OnCheckedChanged="ChkForDate_CheckedChanged" AutoPostBack="true" />                                       
                                    </td>
                                </tr>                             

                                <tr id="TRMonthyear" runat="server" visible="false">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblmonth" Text="From Date" CssClass="labelbold" runat="server" />
                                                    <br />
                                                    <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                                    <asp:CalendarExtender ID="calfromdate" runat="server" TargetControlID="txtfromdate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" Text="To Date" CssClass="labelbold" runat="server" />
                                                    <br />
                                                    <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>                                   
                                    <td colspan="2" align="right">
                                        <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                            OnClientClick="return Validate();" CssClass="buttonnorm" />
                                        <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="labelbold"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
