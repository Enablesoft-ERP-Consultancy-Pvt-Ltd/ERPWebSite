<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmUPCNO.aspx.cs" Inherits="DefineItemCode"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            self.close();
        }
        function dblclick() {
            window.document.getElementById('btnItemcode').click();
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
            if (answer) {
                window.open('AddItemCategory.aspx', '', 'width=500px,Height=500px');
            }
        }
        function Additem() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var varcode = document.getElementById('ddcategory').value;
                window.open('AddItemName.aspx?Category=' + varcode + '', '', 'width=550px,Height=500px');
            }
        }
        function Addquality() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var varcode = document.getElementById('ddcategory').value;
                var varcode1 = document.getElementById('dditemname').value;
                window.open('AddQuality.aspx?Category=' + varcode + '&Item=' + varcode1 + '', '', 'width=701px,Height=501px');
            }
        }
        function Adddesign() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddDesign.aspx', '', 'width=601px,Height=401px');
            }
        }

        function Addcolor() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddColor.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
            }
        }
        function Addshape() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddShape.aspx', '', 'width=901px,Height=401px');
            }
        }
        function Addsize() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddSize.aspx', '', 'width=1000px,Height=401px');
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
                <table>
                    <tr id="Tr5">
                        <td class="tdstyle">
                            Customer Code<br />
                            <asp:DropDownList ID="ddCustomerCode" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddCustomerCode"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name"></asp:Label>&nbsp;
                            &nbsp;
                            <asp:Button ID="btnaddcategory" runat="server" CssClass="buttonsmall" OnClientClick="return Addcategory();"
                                Height="16px" Text=".." />
                            <br />
                            <asp:DropDownList ID="ddcategory" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddcategory_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddcategory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name "></asp:Label>&nbsp;
                            &nbsp;
                            <asp:Button ID="btnadditem" runat="server" CssClass="buttonsmall" OnClientClick="return Additem();"
                                Height="15px" Text=".." />
                            <br />
                            <asp:DropDownList ID="dditemname" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="dditemname_SelectedIndexChanged" CssClass="dropdown"
                                TabIndex="1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="dditemname"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="ql" runat="server" class="tdstyle">
                            <asp:Button ID="refreshquality" runat="server" Height="0px" Width="0px" BackColor="White"
                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshquality_Click" />
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>&nbsp;
                            &nbsp;
                            <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmall" OnClientClick="return Addquality();"
                                Height="15px" Text=".." />
                            <br />
                            <asp:DropDownList ID="dquality" runat="server" Width="150px" TabIndex="2" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="dquality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtProdCode" runat="server" Visible="false"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Button ID="refreshcategory" runat="server" Height="0px" OnClick="refreshcategory_Click"
                                Text="." Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px"
                                ForeColor="White" />
                        </td>
                        <td align="right">
                            <asp:Button ID="refreshitem" runat="server" Height="0px" Width="0px" BackColor="White"
                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshitem_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td id="dsn" runat="server" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>&nbsp; &nbsp;
                            &nbsp;<asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmall" OnClientClick="return Adddesign();"
                                Height="15px" Text=".." /><asp:Button ID="refreshdesign" runat="server" Height="0px"
                                    Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px" ForeColor="White"
                                    OnClick="refreshdesign_Click" /><br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="150px" TabIndex="3" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="dddesign"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="clr" runat="server" class="tdstyle">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>&nbsp; &nbsp;
                            &nbsp;<asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmall" OnClientClick="return Addcolor();"
                                Height="15px" Text=".." /><asp:Button ID="refreshcolor" runat="server" Height="0px"
                                    Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px" ForeColor="White"
                                    OnClick="refreshcolor_Click" /><br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="120px" TabIndex="4" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddcolor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="shp" runat="server" class="tdstyle">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>&nbsp; &nbsp;
                            &nbsp;
                            <asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmall" OnClientClick="return Addshape();"
                                Height="15px" Text=".." /><asp:Button ID="refreshshape" runat="server" Height="0px"
                                    Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px" ForeColor="White"
                                    OnClick="refreshshape_Click" /><br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                TabIndex="5" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddshape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Shd" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshadename" runat="server" Text="Shade"></asp:Label>
                            &nbsp; &nbsp;
                            <asp:Button ID="refreshshade" runat="server" Height="0px" Width="0px" BackColor="White"
                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshshade_Click" />&nbsp;<asp:Button
                                    ID="btnaddshade" runat="server" CssClass="buttonsmall" OnClientClick="return AddShade();"
                                    Height="15px" Text=".." /><br />
                            <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddShade"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="sz" runat="server" class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="Size"></asp:Label>
                            <asp:CheckBox ID="chkbox" runat="server" AutoPostBack="True" OnCheckedChanged="chkbox_CheckedChanged"
                                TabIndex="6" />Chk Mtr&nbsp; &nbsp;
                            <asp:Button ID="btnaddsize" runat="server" CssClass="buttonsmall" OnClientClick="return Addsize();"
                                Height="15px" Text=".." /><asp:Button ID="refreshsize" runat="server" Height="0px"
                                    Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px" ForeColor="White"
                                    OnClick="refreshsize_Click" />
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="120px" TabIndex="7" OnSelectedIndexChanged="ddsize_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddsize"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Button ID="btnItemcode0" runat="server" BackColor="White" BorderColor="White"
                                BorderWidth="0px" ForeColor="White" Height="0px" Width="0px" />
                            UPC NO.
                            <br />
                            <asp:TextBox ID="txtUPCNO" runat="server" Width="100px" CssClass="textb" TabIndex="8"
                                onKeyDown="KeyDownHandler('btnItemcode0');" AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <tr>
                    <td align="right" colspan="5">
                        <asp:Button ID="btnnew" CssClass="buttonnorm" runat="server" Text="New" OnClick="btnnew_Click"
                            TabIndex="9" />
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return confirm('Do you want to save data?')"
                            CssClass="buttonnorm" TabIndex="10" />
                        <asp:Button ID="close" runat="server" Text="Close" OnClientClick="return CloseForm()"
                            CssClass="buttonnorm" TabIndex="11" />
                    </td>
                </tr>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="Gvdefineitem" runat="server" DataKeyNames="Sr_No" OnSelectedIndexChanged="Gvdefineitem_SelectedIndexChanged"
                                AllowPaging="True" PageSize="6" OnRowDataBound="Gvdefineitem_RowDataBound" OnPageIndexChanging="Gvdefineitem_PageIndexChanging"
                                CssClass="grid-view" OnRowCreated="Gvdefineitem_RowCreated">
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
