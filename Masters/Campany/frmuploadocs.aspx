<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmuploadocs.aspx.cs" Inherits="Masters_Campany_frmuploadocs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 0% 20% 0% 20%">
        <table border="1" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblemp" Text="Employee Name" CssClass="labelbold" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblempname" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Employee Code" CssClass="labelbold" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblempcode" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbldoctype" Text="DocumentType" runat="server" CssClass="labelbold" />
                </td>
                <td>
                    <asp:DropDownList ID="DDDoctype" CssClass="dropdown" Width="200px" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="req" runat="server" ControlToValidate="DDDoctype"
                        InitialValue="0" SetFocusOnError="true" ErrorMessage="*" ForeColor="Red" ValidationGroup="q"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:FileUpload ID="Docuploads" runat="server" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg or .pdf files are allowed!"
                        ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp|pdf)$" ControlToValidate="Docuploads"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnupload" CssClass="buttonnorm" Text="Upload" runat="server" OnClick="btnupload_Click"
                        ValidationGroup="q" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="btndownload" Text="download" runat="server" OnClick="btndownload_Click"
                        ValidationGroup="q" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblmsg" CssClass="labelbold" Text="" runat="server" ForeColor="Red" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
