<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmtestinvoiceexcelprint.aspx.cs" Inherits="Masters_ReportForms_frmtestinvoiceexcelprint" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <div>
        <asp:Button ID="btnprint" Text="Print" runat="server" OnClick="btnprint_Click" />
    </div>
</asp:Content>
