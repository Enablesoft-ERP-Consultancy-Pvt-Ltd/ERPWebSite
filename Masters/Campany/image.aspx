<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true" CodeFile="image.aspx.cs" Inherits="Masters_Campany_image" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" Runat="Server">
    <script language="JavaScript" type="text/JavaScript">
    function preview(thisImg, thisObj) {
        alert("Only jpg, jpeg and gif file are allowed.");
        var filename = "";
        var nothing = "";
        filename = "file:\/\/" + thisObj.value;

        var fileExtension
        filename.substring(filename.lastIndexOf(".") + 1);
        if (fileExtension == "jpg" || fileExtension == "jpeg" |fileExtension == "gif")
        { thisImg.src = "file:\/\/" + thisObj.value; }

        else
        { alert("Only jpg, jpeg and gif file are allowed."); }

    }
</script>
    <asp:FileUpload ID="FileUpload1" runat="server" ONchange="preview(myimga,upfile);"/>
    <img src="../../Images/3_compney.gif" name="myimga" width=100 height=150 border=0>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" Runat="Server">
</asp:Content>

