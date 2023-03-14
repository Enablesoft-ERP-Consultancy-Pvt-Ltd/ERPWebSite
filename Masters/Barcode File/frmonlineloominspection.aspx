<%@ Page Title="Online Loom Inspection" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmonlineloominspection.aspx.cs" Inherits="Masters_Barcode_File_frmonlineloominspection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:FileUpload ID="fileupload" runat="server" />
                <asp:RegularExpressionValidator ID="regpdf" Text="Please upload only Text files"
                    ForeColor="Red" ErrorMessage="Please upload only Text files" ControlToValidate="fileupload"
                    ValidationExpression="^.*\.(txt|text)$" runat="server" />
            </td>
            <td>
                <asp:Button ID="btnImport" runat="server" Text="Import Scanner Data" CssClass="buttonnorm"
                    OnClick="btnImport_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblmsg" runat="server" CssClass="labelbold" Text="" ForeColor="Red"
                    Font-Size="Small" />
            </td>
        </tr>
    </table>
</asp:Content>
