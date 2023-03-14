<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPhotoRefImage1.aspx.cs"
    Inherits="Masters_Carpet_AddPhotoRefImage" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript"></script>
    <style type="text/css">
        #newPreview
        {
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }
        #DivReferenceImage
        {
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }
    </style>
    <script language="javascript" type="text/javascript">
        function PreviewImg(imgFile) {
            var newPreview = document.getElementById("newPreview");
            document.getElementById("newPreview").value = "";
            newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            newPreview.style.width = "111px";
            newPreview.style.height = "66px";
            var control = document.getElementById("newPreview1");
            control.style.visibility = "hidden";
        }
        function PreviewReferenceImage(imgFile) {
            var newPreviewRef = document.getElementById("DivReferenceImage");
            document.getElementById("DivReferenceImage").value = "";
            newPreviewRef.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            newPreviewRef.style.width = "111px";
            newPreviewRef.style.height = "66px";
            var controlRef = document.getElementById("ImageReferenceImage");
            controlRef.style.visibility = "hidden";
        }
        function CloseForm() {
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table style="margin-left: 100px;">
            <tr>
                <td>
                    Photo
                </td>
                <td colspan="2">
                    <div id="newPreview" runat="server">
                        <asp:Image ID="newPreview1" runat="server" Height="66px" Width="111px" />
                    </div>
                </td>
                <td colspan="2">
                    <asp:FileUpload ID="PhotoImage" onchange="PreviewImg(this)" ViewStateMode="Enabled"
                        runat="server" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                        ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="PhotoImage"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="trReferenceImage" runat="server" visible="false">
                <td>
                    Ref Image
                </td>
                <td colspan="2">
                    <div id="DivReferenceImage" runat="server">
                        <asp:Image ID="ImageReferenceImage" runat="server" Height="66px" Width="111px" />
                    </div>
                </td>
                <td colspan="2">
                    <asp:FileUpload ID="FileReferenceImage" onchange="PreviewReferenceImage(this)" runat="server" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                        ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="FileReferenceImage"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="BTNSAVE" runat="server" Text="SAVE" CssClass="buttonnorm" OnClick="BTNSAVE_Click" />
                </td>
                <td align="left">
                    <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm()" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
