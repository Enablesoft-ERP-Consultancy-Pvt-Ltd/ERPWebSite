<%@ Page Title="Import Emp Salary Data" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmImportEmpSalaryData.aspx.cs" Inherits="Masters_Payroll_FrmImportEmpSalaryData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmImportAttendanceData.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }      
        }
    </script>
    <%-- <style type="text/css">
        .overlay
        {
        position: fixed;
        z-index: 999;
        height: 100%;
        width: 100%;
        top: 0;
        background-color: Black;
        filter: alpha(opacity=60);
        opacity: 0.6;
        -moz-opacity: 0.8;
        }
        </style>--%>
    <style type="text/css">
        .overlay
        {
            position: fixed;
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.6;
            height: 100%;
            width: 100%;
            z-index: 1000;
            -moz-opacity: 0.6;
            top: 0;
        }
    </style>
    <script type="text/javascript">
        function showProgress() {
            var updateProgress = $get("<%= UpdateProgress.ClientID %>");
            updateProgress.style.display = "block";
        }
    </script>
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <%-- <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>--%>
            <table border="1" style="width: 100%">
                <tr>
                    <td id="TDDelete" runat="server" visible="true">
                        <table border="1" style="width: 40%">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkDelete" Text="For Delete" CssClass="labelbold" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkDelete_CheckedChanged" />
                                </td>
                                <td id="TDCompanyName" runat="server" visible="false">
                                    <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                    <asp:DropDownList ID="DDCompanyName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                        Width="300px">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDCompanyUnit" runat="server" visible="false">
                                    <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="UnitName"></asp:Label>
                                    <asp:DropDownList ID="DDCompanyUnit" runat="server" AutoPostBack="True" CssClass="dropdown"
                                        OnSelectedIndexChanged="DDCompanyUnit_SelectedIndexChanged" Width="300px">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDreceiveno" runat="server" visible="false">
                                    <asp:Label ID="lblReceiveNo" Text="ReceiveNo" runat="server" CssClass="labelbold" />
                                    <asp:DropDownList ID="DDreceiveNo" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDDeleteBtn" runat="server" visible="false">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="buttonnorm" Text="Delete" OnClick="btnDelete_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <div style="float: left; width: 450px; margin: 0% 10% 3% 20%">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRProcessName" runat="server">
                                        <td>
                                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Unit Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDProcessUnit" runat="server" CssClass="dropdown" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfilename" Text="File Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fileupload" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                            <span style="margin-left: 80px;">
                                                <asp:Button ID="btnimport" runat="server" CssClass="buttonnorm" Text="Import Salary Data"
                                                    OnClientClick="showProgress()" OnClick="btnimport_Click" /></span>
                                            <%--   <span style="margin-left: 80px;">
                                                <asp:Button ID="Button1" runat="server" CssClass="buttonnorm" Text="Import Salary Data" OnClientClick="showProgress()" 
                                                    OnClick="btnimport_Click" /></span>--%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnid" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnimport" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="upd">
        <ProgressTemplate>
            <div class="overlay">
                <div style="z-index: 1000; margin-left: 450px; margin-top: 280px; opacity: 1; -moz-opacity: 1;">
                    <%--<div style="position:fixed; top:280px; left:450px; background-color:Black; z-index:99; opacity:0.8; -moz-opacity: 0.8; filter:alpha(opacity=80); min-height:100%; width:100%">--%>
                    <img alt="" src="../../Images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
