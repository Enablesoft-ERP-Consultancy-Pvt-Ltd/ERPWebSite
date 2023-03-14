<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmCreateRecipe.aspx.cs"
    Inherits="Master_Carpet_FrmCreateRecipe" Title="Create Recipe" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmGateInOfAllItems.aspx";
        }
        function closeform() {
            window.location.href = "../../main.aspx";
        }
        function AddRecipeName() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (500 / 2);
            var top = (screen.height / 2) - (500 / 2);

            var e = document.getElementById("CPH_Form_DDProcessName");
            var ProcessID = e.options[e.selectedIndex].value;

            if (answer) {
                window.open('AddRecipeName.aspx?ProcessID=' + ProcessID + '', '', 'width=500px,Height=500px,top=' + top + ',left=' + left);
            }
        }
        function ValidateSave() {
            if (document.getElementById('CPH_Form_DDRecipeName').options[document.getElementById('CPH_Form_DDRecipeName').selectedIndex].value == 0) {
                alert("Please select recipe name....!");
                document.getElementById("CPH_Form_DDRecipeName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_ddcategory').options[document.getElementById('CPH_Form_ddcategory').selectedIndex].value == 0) {
                alert("Please select category name....!");
                document.getElementById("CPH_Form_ddcategory").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_dditemname').options[document.getElementById('CPH_Form_dditemname').selectedIndex].value == 0) {
                alert("Please Select Item Name....!");
                document.getElementById("CPH_Form_dditemname").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_ql')) {
                if (document.getElementById('CPH_Form_dquality').options[document.getElementById('CPH_Form_dquality').selectedIndex].value == 0) {
                    alert("Please select quality....!");
                    document.getElementById("CPH_Form_dquality").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_dsn')) {
                if (document.getElementById('CPH_Form_dddesign').options[document.getElementById('CPH_Form_dddesign').selectedIndex].value == 0) {
                    alert("Please select design....!");
                    document.getElementById("CPH_Form_dddesign").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_clr')) {
                if (document.getElementById('CPH_Form_ddcolor').options[document.getElementById('CPH_Form_ddcolor').selectedIndex].value == 0) {
                    alert("Please select color....!");
                    document.getElementById("CPH_Form_ddcolor").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_shp')) {
                if (document.getElementById('CPH_Form_ddshape').options[document.getElementById('CPH_Form_ddshape').selectedIndex].value == 0) {
                    alert("Please select shape....!");
                    document.getElementById("CPH_Form_ddshape").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_sz')) {
                if (document.getElementById('CPH_Form_ddsize').options[document.getElementById('CPH_Form_ddsize').selectedIndex].value == 0) {
                    alert("Please select size....!");
                    document.getElementById("CPH_Form_ddsize").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_Shd')) {
                if (document.getElementById('CPH_Form_ddShade').options[document.getElementById('CPH_Form_ddShade').selectedIndex].value == 0) {
                    alert("Please select shadecolor....!");
                    document.getElementById("CPH_Form_ddShade").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDUnit').options[document.getElementById('CPH_Form_DDUnit').selectedIndex].value == 0) {
                alert("Please select unit....!");
                document.getElementById("CPH_Form_DDUnit").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtConsmp').value == "") {
                alert("Pls fill consmp qty....!");
                document.getElementById('CPH_Form_TxtConsmp').focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="RefreshRecipeName" runat="server" OnClick="RefreshRecipeName_Click"
                                Style="display: none" />
                            <asp:Label ID="Label3" runat="server" Text="Recipe Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDRecipeName" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="DDRecipeName_SelectedIndexChanged" CssClass="dropdown"
                                TabIndex="1">
                            </asp:DropDownList>
                            &nbsp;<asp:Button ID="BtnAddRecipeName" runat="server" CssClass="buttonsmall" OnClientClick="return AddRecipeName();"
                                Text="&#43;" />
                        </td>
                        <td class="tdstyle">
                            <asp:TextBox ID="TxtProdCode" runat="server" Width="100px" CssClass="textb" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="ddcategory" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddcategory_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name " CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="dditemname" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="dditemname_SelectedIndexChanged" CssClass="dropdown"
                                TabIndex="1">
                            </asp:DropDownList>
                        </td>
                        <td id="ql" runat="server" class="tdstyle">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dquality" runat="server" Width="150px" TabIndex="2" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td id="dsn" runat="server" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="150px" TabIndex="3" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="clr" runat="server" class="tdstyle">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="150px" TabIndex="4" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Shd" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshadename" runat="server" Text="Shade" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddShade" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="shp" runat="server" class="tdstyle">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                TabIndex="5" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="sz" runat="server" class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:DropDownList CssClass="dropdown" Width="50px" ID="DDsizetype" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <br />
                            <asp:DropDownList ID="DDUnit" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Consmp/sqYd" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtConsmp" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="5">
                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                            <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="buttonnorm"
                                OnClientClick="return ValidateSave();" />
                            &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm"
                                OnClientClick="return closeform();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" runat="server">
                            <div style="width: 100%; height: 200px; overflow: scroll">
                                <asp:GridView ID="DGRecipe" runat="server" DataKeyNames="ID" OnRowDataBound="DGRecipe_RowDataBound"
                                    OnRowDeleting="DGRecipe_RowDeleting" CssClass="grid-views" AutoGenerateColumns="False">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <ItemStyle Width="400px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                            <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ConsmpQty" HeaderText="Consmp Qty">
                                            <ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
