<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmDyeingProgramReport.aspx.cs"
    Inherits="Masters_ReportForms_FrmDyeingProgramReport" MasterPageFile="~/ERPmaster.master"
    Title="Dyeing Program Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {

            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 100%; margin-left: 250px">
                    <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="70%">
                        <div style="padding: 0px 0px 0px 20px">
                            <table style="height: 150px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCompany" Text="CompanyName" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" Width="350px" CssClass="dropdown"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Tdselectdate" runat="server">
                                        <asp:CheckBox ID="ChkselectDate" Text="Select Date" runat="server" CssClass="checkboxbold" />
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                                    <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td id="Tdtodatelabel" runat="server">
                                                    <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                                                </td>
                                                <td id="Tdtodate" runat="server">
                                                    <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                                    <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCustomerCode" Text="Customer Code" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddcustomer" runat="server" AutoPostBack="True" Width="350px"
                                            CssClass="dropdown" OnSelectedIndexChanged="ddcustomer_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddcustomer"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </asp:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="tdstyle">
                                        <div style="height: 150px; width: 100%; overflow: scroll">
                                            <asp:Label ID="lblchekboxlist" runat="server" Text="LocalOrder/OrderNo" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:CheckBoxList ID="chekboxlist" runat="server" AutoPostBack="True" Width="600px"
                                                CssClass="checkboxnormal" OnSelectedIndexChanged="chekboxlist_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                            OnClick="BtnPreview_Click" />
                                        &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                            OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
