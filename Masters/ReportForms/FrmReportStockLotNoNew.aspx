<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmReportStockLotNoNew.aspx.cs"
    Inherits="Masters_ReportForms_FrmReportStockLotNoNew" MasterPageFile="~/ERPmaster.master"
    Title="Check Stock No With Lot No./Tag No." %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <div style="width: 800px; height: 600px; margin: 0px 0px 0px 0px">
                <div style="margin: auto; width: 500px; background-color: #DEB887">
                    <div style="width: 420px; margin-top: 20px; position: relative; float: right; top: 0px;
                        left: 0px; height: 150px; padding-top: 10px; padding-left: 10px">
                        <table style="height: 130px; background-color: #DEB887;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblLotno" runat="server" Text="Enter Lot No." CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLotno" runat="server" BackColor="Beige" Width="100px" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="TRTagNo" runat="server" visible="true">
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Enter Tag No." CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTagNo" runat="server" BackColor="Beige" Width="100px" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <td>
                                <asp:CheckBox ID="ChkForDyeingIssue" Text="For Dyeing" CssClass="checkboxbold" runat="server"
                                    OnCheckedChanged="ChkForDyeingIssue_CheckedChanged" AutoPostBack="true" Visible="true" />
                            </td>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:Button ID="btnPrint" Text="Print" runat="server" Width="100px" CssClass="buttonnorm"
                                        OnClick="btnPrint_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
