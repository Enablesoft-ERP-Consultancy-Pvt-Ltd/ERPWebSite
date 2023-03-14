<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmReportStockLotNo.aspx.cs"
    Inherits="Masters_ReportForms_frmReportStockLotNo" MasterPageFile="~/ERPmaster.master"
    Title="Check Stock No With Lot No./Batch No." %>

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
                                    <%--          <table style="border: 1px solid #c4e4f2; background-color: #effaff" cellspacing="0"
                                        cellpadding="0" border="0">
                                        <tr id="Tr1" style="height: 25px" runat="server">
                                            <td>
                                                <asp:Label ID="lblFromDate" runat="server" CssClass="labelbold" Text="Enter LotNo/BatchNo"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:TextBox ID="txtLotno" runat="server" Width="100px" BackColor="Yellow" ToolTip="Click to Change Date"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="height: 25px;">
                                            <td colspan="4" align="right">
                                                <asp:Button ID="btnPrint" Text="Print" runat="server" Width="100px" CssClass="buttonnorm"
                                                    OnClick="btnPrint_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblErrormsg" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>--%>
                                    <asp:Label ID="lblLotno" runat="server" Text="Enter Lot No./Batch No." CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLotno" runat="server" BackColor="Beige" Width="100px" CssClass="textb"
                                        OnTextChanged="txtLotno_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </td>

                            </tr>
                            
                                 <tr id="TRTagNo" runat="server" visible="false">
                                <td>
                                     <asp:Label ID="Label1" runat="server" Text="Enter Tag No." CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTagNo" runat="server" BackColor="Beige" Width="100px" CssClass="textb"
                                        OnTextChanged="txtTagNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </td>
                                </tr>
                            <tr>
                            <td>
                             <asp:CheckBox ID="ChkForExcelExport" Text="Export" CssClass="checkboxbold" runat="server" Visible="false"/>
                            
                            </td>
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
