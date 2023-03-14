<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageSave.aspx.cs" Inherits="ImageSave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
<meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
<title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
     <style type="text/css">
        #newPreview
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
    </script>
    </head>
<body>
    <form id="form1" runat="server">
    <table>
     <tr>
                <td >
                        <div id="newPreview" runat="server">
                        <asp:Image ID="newPreview1" runat="server" Height="66px" Width="111px"/>
                        </div>
               </td>
                <td>       
                      <asp:FileUpload ID="compneyImage" onchange="PreviewImg(this)"  runat="server" />
                      <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" 
                      ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" 
                        ControlToValidate="compneyImage"></asp:RegularExpressionValidator>

               </td>           
               <td>
               
                   <asp:Image ID="Image1" runat="server" />
               
               </td>
               <td>
               
                   <asp:Image ID="Image2" runat="server" />
               
               </td>
               <td>
               
                   <asp:Image ID="Image3" runat="server" />
               
               </td><td>
               
                   <asp:Image ID="Image4" runat="server" />
               
               </td>

             </tr>
             <tr>
             <td>
                 <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
             </td>
             </tr>
       </table>
    </form>
</body>
</html>
