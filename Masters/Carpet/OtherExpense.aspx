<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OtherExpense.aspx.cs" Inherits="Masters_Carpet_OtherExpense" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
        }
        function OpenCharge() {
            window.open('Charge.aspx', '', 'width=350px,Height=300px');
        }
        function formclose() {
            window.close();
        }
        function validate() {
            var doc, msg;
            doc = document.forms[0];
            msg = "";
            if (doc.ddcomapnyname.value == "0")
            { msg = "Select Company Name "; }
            else if (doc.ddcustomercode.value == "0") {
                msg = "select customer Code";
            }
            else if (doc.ddCategoryName.value == "0") {
                msg = "select CategoryName";
            }
            else if (doc.ddItemName.value == "0") {
                msg = "select Item Name";
            }
            else if (doc.ddchangename.value == "0") {
                msg = "select Charge Name";
            }
            else if (doc.txtpercentage.value == "0") {
                msg = "select Percentage";
            }
            if (msg == "")
            { return true; }
            else {
                alert(msg);
                return false;
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table width="80%">
            <tr>
                <td id="TDCompanyName" runat="server" visible="false" class="tdstyle">
                    Company Name
                    <br />
                    <asp:DropDownList ID="ddcomapnyname" runat="server" Width="150px" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddcomapnyname"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="TDCustomercode" runat="server" visible="false" class="tdstyle">
                    Customer Code
                    <br />
                    <asp:DropDownList ID="ddcustomercode" runat="server" Width="150px" CssClass="dropdown"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddcustomercode"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    Item Code
                    <br />
                    <asp:TextBox ID="TxtProdCode" runat="server" CssClass="textb" Width="150px" AutoPostBack="True"
                        OnTextChanged="TxtProdCode_TextChanged"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                        UseContextKey="True">
                    </cc1:AutoCompleteExtender>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblcategoryname" runat="server" Text="Category Name"></asp:Label>
                    &nbsp;<br />
                    <asp:DropDownList ID="ddCategoryName" runat="server" OnSelectedIndexChanged="ddcategoryname_SelectedIndexChanged"
                        Width="150px" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddCategoryName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddItemName" runat="server" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                        Width="150px" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddItemName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td>
                    <asp:Button ID="refresh" runat="server" Text=".." BackColor="White" BorderColor="White"
                        BorderWidth="0px" ForeColor="Black" OnClick="refresh_Click" />
                </td>
            </tr>
            <tr>
                <td id="Quality" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblqualityname" runat="server" Text="Quality "></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddQuality" runat="server" Width="150px" TabIndex="12" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddQuality"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Design" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddDesign" runat="server" Width="150px" TabIndex="13" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddDesign"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Color" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddColor" runat="server" Width="100px" TabIndex="14" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddColor"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Shape" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddShape" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                        TabIndex="15" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddShape"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Size" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblsizename" runat="server" Text="Size"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddSize" runat="server" Width="100px" TabIndex="16" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddSize"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Shade" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblshadename" runat="server" Text="Shade"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddShade"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    ChangeName &nbsp;<asp:Button ID="btnopen" runat="server" Height="20px" Text="...."
                        CssClass="buttonnormal1" OnClientClick="return OpenCharge()" />
                    <br />
                    <asp:DropDownList ID="ddchangename" runat="server" Width="130px" CssClass="dropdown"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddchangename"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <%-- <td class="tdstyle">
                    Percentage
                    <br />
                    <asp:TextBox ID="txtpercentage" runat="server" Width="70px" CssClass="textb"></asp:TextBox>
                </td>--%>
                <td>
                    <br />
                    <asp:Button ID="btnsave" runat="server" Text="Save" Visible="false" CssClass="buttonnorm"
                        OnClientClick="return confirm('Do you want to save data?')" OnClick="btnsave_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="left">
                    <asp:Button ID="Button1" runat="server" Text="New" CssClass="buttonnorm" OnClick="BtnNew_Click" />
                    <asp:Button ID="Button2" runat="server" Text="Preview" CssClass="buttonnorm" OnClientClick="return report()" />
                    <asp:Button ID="Button3" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return formclose()" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                    <asp:GridView ID="dgotherexpense" runat="server" CssClass="grid-view" OnRowCreated="dgotherexpense_RowCreated"
                        DataKeyNames="SRNO" OnRowCancelingEdit="dgotherexpense_RowCancelingEdit" AutoGenerateColumns="false"
                        OnRowEditing="dgotherexpense_RowEditing" OnRowUpdating="dgotherexpense_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="SRNO">
                                <ItemTemplate>
                                    <asp:Label ID="LblSrno" runat="server" Text='<%# Bind("SRNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QUALITY">
                                <ItemTemplate>
                                    <asp:Label ID="LblQlty" runat="server" Text='<%# Bind("QUALITY") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="LblQlty1" runat="server" Text='<%# Bind("QUALITY") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DESIGN">
                                <ItemTemplate>
                                    <asp:Label ID="LblDesign" runat="server" Text='<%# Bind("DESIGN") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="LblDesign1" runat="server" Text='<%# Bind("DESIGN") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="COLOR">
                                <ItemTemplate>
                                    <asp:Label ID="LblCOLOR" runat="server" Text='<%# Bind("COLOR") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="LblCOLOR1" runat="server" Text='<%# Bind("COLOR") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SHAPE">
                                <ItemTemplate>
                                    <asp:Label ID="LblShape" runat="server" Text='<%# Bind("SHAPE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="LblShape1" runat="server" Text='<%# Bind("SHAPE") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SIZE">
                                <ItemTemplate>
                                    <asp:Label ID="LblSize" runat="server" Text='<%# Bind("SIZE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="LblSize1" runat="server" Text='<%# Bind("SIZE") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CHARGENAME">
                                <ItemTemplate>
                                    <asp:Label ID="LblChg" runat="server" Text='<%# Bind("CHARGENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="LblChg1" runat="server" Text='<%# Bind("CHARGENAME") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PERCENTAGE">
                                <ItemTemplate>
                                    <asp:Label ID="LblPercentage" runat="server" Text='<%# Bind("PERCENTAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtPercentage" runat="server" Width="80px" onkeypress="return isNumber(event);"
                                        Text='<%# Eval("PERCENTAGE") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </td>
            </tr>
            <%-- <tr>
               
                <td colspan="4" align="right">
                    <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClick="BtnNew_Click" />
                    <asp:Button ID="bntpriview" runat="server" Text="Preview" CssClass="buttonnorm"
                        OnClientClick="return report()" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return formclose()" />
                </td>
            </tr>--%>
        </table>
    </div>
    </form>
</body>
</html>
