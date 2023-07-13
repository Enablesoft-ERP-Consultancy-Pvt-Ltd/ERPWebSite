<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefineItemCode.aspx.cs" Inherits="Masters_Carpet_DefineItemCode"
    Title="Define Item Code" EnableEventValidation="false" ViewStateMode="Enabled" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
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
        function CloseForm() {
            self.close();
        }
        function dblclick() {
            window.document.getElementById('btnItemcode').click();
        }
        function doConfirm() {
            var r = confirm("Do you want to add?");
            document.getElementById("hnsst").value = r;
            if (r == true) {
                return true;
            }
            else {

                return false;
            }

        }
        function KeyDownHandler(btn) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                //btn.click();
                window.document.getElementById(btn).click();
            }
        }
        function RefreshCombo() {
            window.document.getElementById('').click();
        }
        function Addcategory() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (500 / 2);
            var top = (screen.height / 2) - (500 / 2);

            if (answer) {
                window.open('AddItemCategory.aspx', '', 'width=500px,Height=500px,top=' + top + ',left=' + left);
            }
        }
        function Additem() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (550 / 2);
            var top = (screen.height / 2) - (500 / 2);

            if (answer) {
                var varcode = document.getElementById('ddcategory').value;
                window.open('AddItemName.aspx?Category=' + varcode + '', '', 'width=550px,Height=500px,top=' + top + ',left=' + left);
            }
        }
        function Addquality() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (701 / 2);
            var top = (screen.height / 2) - (501 / 2);

            if (answer) {
                var varcode = document.getElementById('ddcategory').value;
                var varcode1 = document.getElementById('dditemname').value;
                window.open('AddQuality.aspx?Category=' + varcode + '&Item=' + varcode1 + '', '', 'width=701px,Height=501px,top=' + top + ',left=' + left);
            }
        }
        function AddOtherparameter() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var varcode = document.getElementById('ddcategory').value;
                var varcode1 = document.getElementById('dditemname').value;
                window.open('AddQuality.aspx?Category=' + varcode + '&Item=' + varcode1 + '', '', 'width=701px,Height=570px');
            }
        }
        function Adddesign() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (601 / 2);
            var top = (screen.height / 2) - (450 / 2);

            if (answer) {
                window.open('AddDesign.aspx', '', 'width=601px,Height=450px,top=' + top + ',left=' + left);
            }
        }
        function Addcolor() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (501 / 2);
            var top = (screen.height / 2) - (501 / 2);

            if (answer) {
                window.open('AddColor.aspx', '', 'width=501px,Height=501px,top=' + top + ',left=' + left, 'resizeable=yes');
            }
        }
        function Addshape() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (400 / 2);
            var top = (screen.height / 2) - (300 / 2);

            if (answer) {
                window.open('AddShape.aspx', '', 'width=400px,Height=300px,top=' + top + ',left=' + left);
            }
        }
        function Addsize() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddSize.aspx', '', 'width=1000px,Height=401px');
            }
        }
        function AddQualitysize() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('FrmNewSize.aspx', '', 'width=1000px,Height=401px');
            }
        }
        function AddShade() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddShadeColor.aspx', '', 'width=901px,Height=401px');
            }
        }   
    </script>
    <title></title>
    <style type="text/css">
        .textbox
        {
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%" border="1">
                    <tr id="zzz" runat="server">
                        <td>
                            <table width="100%">
                                <tr style="width: 100%" align="center" id="trheader" runat="server">
                                    <td height="66px" align="center">
                                        <%--style="background-image:url(Images/header.jpg)" --%>
                                        <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                                        <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
                                    </td>
                                    <td style="background-color: #0080C0;" width="100px" valign="bottom">
                                        <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                                        <span style="color: Black; margin-left: 30px; font-family: Arial; font-size: xx-large">
                                            <strong><em><i><font size="4" face="GEORGIA">
                                                <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em></strong></span>
                                        <br />
                                        <i><font size="2" face="GEORGIA">
                                            <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font></i>
                                    </td>
                                </tr>
                                <tr bgcolor="#999999">
                                    <td class="style1">
                                        <uc2:ucmenu ID="ucmenu1" runat="server" />
                                        <%--<asp:ScriptManager ID="ScriptManager2" runat="server">
                            </asp:ScriptManager>--%>
                                    </td>
                                    <td width="25%">
                                        <asp:UpdatePanel ID="up" runat="server">
                                            <ContentTemplate>
                                                <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                                                    Text="Logout" OnClick="BtnLogout_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table style="margin-left: 200px; padding: 10px">
                    <tr id="TrItemcode" runat="server">
                        <td class="tdstyle" id="tdItemCode" runat="server">
                            <asp:Button ID="btnItemcode0" runat="server" Style="display: none" OnClick="txtitemcode_TextChanged" />
                            <asp:Label ID="lblProductCode" runat="server" Text="Item Code" Font-Bold="true" ></asp:Label>   
                            &nbsp;
                            <asp:TextBox ID="txtitemcode" runat="server" Width="200px" CssClass="textb" TabIndex="8"
                                onKeyDown="KeyDownHandler('btnItemcode0');" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle" id="TDUPCNo" runat="server" visible="false">      
                        <asp:Label ID="lblUPCNo" runat="server" Text="UPC No" Font-Bold="true"></asp:Label>                     
                             &nbsp;
                            <asp:TextBox ID="txtUpcNo" runat="server" Width="200px" CssClass="textb" TabIndex="9"
                                onKeyDown="KeyDownHandler('btnItemcode0');" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle" id="tdSearchDesign" runat="server">
                            <span class="labelbold">Design Name</span> &nbsp;
                            <asp:TextBox ID="txtsearchdesign" CssClass="textb" runat="server" placeholder="Type design"
                                AutoPostBack="true" OnTextChanged="txtsearchdesign_TextChanged" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Button ID="refreshcategory" runat="server" OnClick="refreshcategory_Click" Style="display: none" />
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="ddcategory" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddcategory_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:Button ID="btnaddcategory" runat="server" CssClass="buttonsmall" OnClientClick="return Addcategory();"
                                Text="&#43;" />
                        </td>
                        <td>
                            <asp:Button ID="refreshitem" runat="server" Style="display: none" OnClick="refreshitem_Click" />
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name " CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="dditemname" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="dditemname_SelectedIndexChanged" CssClass="dropdown"
                                TabIndex="1">
                            </asp:DropDownList>
                            <asp:Button ID="btnadditem" runat="server" CssClass="buttonsmall" OnClientClick="return Additem();"
                                Text="&#43;" />
                        </td>
                        <td id="dateforsize" runat="server" class="tdstyle" visible="false">
                            <asp:Label ID="Label2" runat="server" Text="Date for Size" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="tbSizeDate" runat="server" CssClass="textb" Width="150px" ReadOnly="false"
                                AutoPostBack="true" OnTextChanged="tbSizeDate_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="tbSizeDate">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbSizeDate"
                                ErrorMessage="Please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td id="ql" runat="server" class="tdstyle">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshquality" runat="server" Style="display: none" OnClick="refreshquality_Click" /><br />
                            <asp:DropDownList ID="dquality" runat="server" Width="150px" TabIndex="2" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmall" OnClientClick="return Addquality();"
                                Text="&#43;" />
                        </td>
                        <td id="dsn" runat="server" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshdesign" runat="server" Style="display: none" OnClick="refreshdesign_Click" /><br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="150px" TabIndex="3" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmall" OnClientClick="return Adddesign();"
                                Text="&#43;" />
                        </td>
                        <td id="clr" runat="server" class="tdstyle">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshcolor" runat="server" Style="display: none" OnClick="refreshcolor_Click" /><br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="120px" TabIndex="4" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmall" OnClientClick="return Addcolor();"
                                Text="&#43;" />
                        </td>
                        <td id="Shd" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshadename" runat="server" Text="Shade" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshshade" runat="server" Style="display: none" OnClick="refreshshade_Click" />&nbsp;<br />
                            <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:Button ID="btnaddshade" runat="server" CssClass="buttonsmall" OnClientClick="return AddShade();"
                                Text="&#43;" />
                        </td>
                        <td id="shp" runat="server" class="tdstyle">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshshape" runat="server" Style="display: none" OnClick="refreshshape_Click" /><br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                TabIndex="5" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmall" OnClientClick="return Addshape();"
                                Text="&#43;" />
                        </td>
                        <td id="sz" runat="server" class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                            <asp:CheckBox ID="chkbox" runat="server" AutoPostBack="True" OnCheckedChanged="chkbox_CheckedChanged"
                                TabIndex="6" Visible="false" /><%--Chk Mtr--%>
                            <asp:Button ID="refreshsize" runat="server" Style="display: none" OnClick="refreshsize_Click" />
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="120px" TabIndex="7" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:Button ID="btnaddsize" runat="server" CssClass="buttonsmall" OnClientClick="return Addsize();"
                                Text="&#43;" />
                            <asp:Button ID="btnAddQualitySize" runat="server" CssClass="buttonsmall" OnClientClick="return AddQualitysize();"
                                Visible="false" Text="&#43;" />
                        </td>
                    </tr>
                    <tr id="TRSKUNo" runat="server" visible="false">
                        <td colspan="1" class="tdstyle">
                            <span class="labelbold">SkU No</span>
                            <asp:TextBox ID="txtsuk_no" runat="server" Width="100px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td colspan="1" class="tdstyle">
                            <span class="labelbold">Weight</span> &nbsp;&nbsp;<asp:TextBox ID="TxtWeight" runat="server"
                                Width="100px" CssClass="textb" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle" colspan="3">
                            <asp:Label ID="lblDescp" runat="server" Text="Description" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="Txtdesc" runat="server" TextMode="MultiLine" Width="200" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td colspan="1" class="tdstyle">
                <span class="labelbold">Photo</span>
            </td>
            <td colspan="1">
                <div id="newPreview" runat="server">
                    <asp:Image ID="newPreview1" runat="server" Height="66px" Width="111px" />
                </div>
            </td>
            <td colspan="1">
                <asp:FileUpload ID="PhotoImage" onchange="PreviewImg(this)" ViewStateMode="Enabled"
                    runat="server" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                    ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="PhotoImage"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr id="trweight" runat="server" visible="false">
            <td class="tdstyle">
                <asp:Label ID="lblweight1" runat="server" Text=""></asp:Label>
                <br>
                <asp:TextBox ID="Txtweight1" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
            </td>
            <td colspan="1" class="tdstyle">
                <asp:Label ID="lblweight2" runat="server" Text=""></asp:Label>
                <br />
                <asp:TextBox ID="Txtweight2" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
            </td>
            <td class="tdstyle">
                <asp:Label ID="lblweight3" runat="server" Text=""></asp:Label>
                <br />
                <asp:TextBox ID="Txtweight3" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td align="left" colspan="4">
                        <asp:Button ID="btnnew" runat="server" Text="New" OnClick="btnnew_Click" TabIndex="9"
                            CssClass="buttonnorm" />
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return confirm('Do you want to save data?')"
                            CssClass="buttonnorm" TabIndex="10" />
                        <asp:Button ID="close" runat="server" Text="Close" CssClass="buttonnorm" TabIndex="11"
                            OnClick="close_Click" />
                        <asp:Button ID="BtnRef" runat="server" Text="Ref." CssClass="buttonnorm" OnClick="BtnRef_Click" />
                        <asp:Button ID="BtnDelete" runat="server" Text="Delete" OnClick="BtnDelete_Click"
                            OnClientClick="return confirm('Do You Want To Deleted ?')" Visible="false" CssClass="buttonnorm" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 100%; height: 200px; overflow: auto;">
                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="Gvdefineitem" runat="server" DataKeyNames="Sr_No" OnSelectedIndexChanged="Gvdefineitem_SelectedIndexChanged"
                                OnRowDataBound="Gvdefineitem_RowDataBound" AutoGenerateColumns="false" CssClass="grid-views"
                                EmptyDataText="No. Records found." OnRowCommand="Gvdefineitem_RowCommand">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />
                                    <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY" />
                                    <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME" />
                                    <asp:BoundField DataField="Quality" HeaderText="Quality" />
                                    <asp:BoundField DataField="Design" HeaderText="Design" />
                                    <asp:BoundField DataField="Color" HeaderText="Color" />
                                    <asp:BoundField DataField="Shape" HeaderText="Shape" />
                                    <asp:BoundField DataField="Size" HeaderText="Size" />
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
                                    <asp:BoundField DataField="ProdCode" HeaderText="ProdCode" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="BTNAddParameter" runat="server" Text="Add Parameter" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                CommandName="Add" OnClientClick="return doConfirm();"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnaddrate" runat="server" Text="Add Rate" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        CommandName="AddRate" OnClientClick="return doConfirm();"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                            <br />
                        </div>
                    </td>
                    <asp:HiddenField ID="hncomp" runat="server" />
                    <asp:HiddenField ID="hnsst" runat="server" />
                    <asp:HiddenField ID="hnItemFinishedId" runat="server" />
                </tr>
                <tr id="TRDesignWithQuality" runat="server" visible="false">
                    <td>
                        &nbsp;
                        <div style="width: 100%; height: 200px; overflow: auto;">
                            <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="GVDesignWithQuality" runat="server" AutoGenerateColumns="false"
                                CssClass="grid-views" EmptyDataText="No. Records found.">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="DesignName" HeaderText="Design_Name" />
                                    <asp:BoundField DataField="QualityName" HeaderText="Quality_Name" />
                                    <asp:BoundField DataField="Item_Name" HeaderText="Item_Name" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                            <br />
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
