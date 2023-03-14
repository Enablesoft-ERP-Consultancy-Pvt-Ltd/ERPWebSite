<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" CodeFile="FrmReOrderLevel.aspx.cs" Inherits="Masters_RawMaterial_FrmReOrderLevel" %>

<%@ Register Src="../../UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function logout() {
            window.location.href = "../../Login.aspx";
        }
        function AddItum() {
            var a3 = document.getElementById("<%=TxtFinishedid.ClientID %>").value;
            window.open('../Carpet/AddItemName.aspx?' + a3, '', 'Height=400px,width=500px');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }
        function AddItemCode() {
            var a3 = document.getElementById("<%=TxtFinishedid.ClientID %>").value;
            window.open('../Carpet/AddItemCode.aspx?' + a3);
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
        }
        function AddItemCategory() {
            window.open('../Carpet/AddItemCategory.aspx', '', 'Height=500px,width=700px');
        }
        function AddQuality() {
            var a3 = document.getElementById("<%=TxtFinishedid.ClientID %>").value;
            window.open('../Carpet/AddQuality.aspx?' + a3, '', 'Height=500px,width=500px');
        }
        function AddDesign() {
            window.open('../Carpet/AddDesign.aspx', '', 'Height=500px,width=500px');
        }
        function AddColor() {
            window.open('../Carpet/AddColor.aspx', '', 'Height=500px,width=500px');
        }
        function AddShadecolor() {
            window.open('../Carpet/AddShadeColor.aspx', '', 'Height=500px,width=500px');
        }
        function AddShape() {
            window.open('../Carpet/AddShape.aspx', '', 'Height=400px,width=500px');
        }
        function AddSize() {
            var e = document.getElementById("<%=DDShape.ClientID %>");
            var shapeid = e.options[e.selectedIndex].value;
            if (document.getElementById("<%=HDF1.ClientID %>").value == "7") {
                window.open('../Carpet/frmSizeForLocal.aspx?shapeid=' + shapeid + '', '', 'Height=400px,width=600px');
                return false;
            }
            else {
                window.open('../Carpet/AddSize.aspx?shapeid=' + shapeid + '', '', 'Height=500px,width=1000px');
            }
        }
        function Validate() {
            if (document.getElementById("<%=DDItemCategory.ClientID %>").value == "0") {
                alert("Pls Select Catagory Name");
                document.getElementById("<%=DDItemCategory.ClientID %>").focus();
                return false;
            }
            else {
                var isValid = false;
                var gridView = document.getElementById("<%=DGOrderDetail.ClientID %>");
                for (var i = 1; i < gridView.rows.length; i++) {
                    var inputs = gridView.rows[i].getElementsByTagName('input');
                    if (inputs != null) {
                        if (inputs[0].type == "checkbox") {
                            if (inputs[0].checked) {
                                isValid = true;
                                return confirm('Do You Want To Save?')
                            }
                        }
                    }
                }
                alert("Please select atleast one checkbox");
                return false;
            }
        }
    </script>
    <table style="width: 100%" border="1">
        <tr>
            <td colspan="2">
                <table style="width: 100%; height: auto">
                    <tr>
                        <td>
                            <asp:Label ID="lblcategoryname" runat="server" Text="Item Category" CssClass="labelbold"></asp:Label>
                            <b style="color: Red">&nbsp;&nbsp;&nbsp; *</b>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnadditemcategory" runat="server" CssClass="buttonsmalls" OnClientClick="return AddItemCategory()"
                                Text="ADD" TabIndex="12" />
                            <br />
                            <asp:DropDownList ID="DDItemCategory" AutoPostBack="true" runat="server" Width="200px"
                                OnSelectedIndexChanged="DDItemCategory_SelectedIndexChanged" CssClass="dropdown"
                                ValidationGroup="f1" TabIndex="11">
                            </asp:DropDownList>
                            <asp:Button CssClass="refreshcategory" ID="refreshcategory" runat="server" Text=""
                                BorderWidth="0px" Height="1px" Width="1px" BackColor="White" BorderColor="White"
                                BorderStyle="None" ForeColor="White" OnClick="refreshcategory_Click" />
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDItemCategory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <b style="color: Red">*</b>
                            <asp:Button ID="BtnAdd0" runat="server" CssClass="buttonsmalls" OnClientClick="return AddItum()"
                                Text="ADD" TabIndex="14" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="200px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged"
                                ValidationGroup="f1" TabIndex="13">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDItemName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                            <asp:Button CssClass="buttonnorm" ID="BtnRefreshItem" runat="server" Text="" BorderWidth="0px"
                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                ForeColor="White" OnClick="BtnRefreshItem_Click" />
                        </td>
                        <td id="TDQuality" runat="server" visible="false">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <b style="color: Red">*</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmalls" OnClientClick="return AddQuality()"
                                Text="ADD" TabIndex="16" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" AutoPostBack="True"
                                Width="200px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged" TabIndex="15">
                            </asp:DropDownList>
                            <asp:Button CssClass="refreshquality" ID="refreshquality" runat="server" Text=""
                                BorderWidth="0px" Height="1px" Width="1px" BackColor="White" BorderColor="White"
                                BorderStyle="None" ForeColor="White" OnClick="refreshquality_Click" />
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TdDESIGN" runat="server" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmalls" OnClientClick="return AddDesign()"
                                Text="ADD" TabIndex="20" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" AutoPostBack="True"
                                Width="200px" TabIndex="19" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button CssClass="refreshdesign" ID="refreshdesign" runat="server" Text="" BorderWidth="0px"
                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                ForeColor="White" OnClick="refreshdesign_Click" />
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDDesign"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td id="TDColor" runat="server" visible="false">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmalls" OnClientClick="return AddColor()"
                                Text="ADD" TabIndex="22" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" AutoPostBack="True"
                                Width="200px" TabIndex="21" OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button CssClass="refreshcolor" ID="refreshcolor" runat="server" Text="" BorderWidth="0px"
                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                ForeColor="White" OnClick="refreshcolor_Click" />
                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDColor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDShape" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmalls" OnClientClick="return AddShape()"
                                Text="ADD" TabIndex="26" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDShape" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged" Width="200px" TabIndex="25">
                            </asp:DropDownList>
                            <asp:Button CssClass="buttonnorm" ID="refreshshape" runat="server" Text="" BorderWidth="0px"
                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                ForeColor="White" OnClick="refreshshape_Click" />
                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDShape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDSize" runat="server" class="tdstyle">
                            <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnAddSize" runat="server" CssClass="buttonsmalls" OnClientClick="return AddSize()"
                                Text="ADD" TabIndex="28" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDSize" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDSize_SelectedIndexChanged" Width="200px" TabIndex="27">
                            </asp:DropDownList>
                            <%--<asp:Button CssClass="buttonnorm" ID="BtnRefreshSize" runat="server" Text="" BorderWidth="0px"
                    Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                    ForeColor="White" OnClick="BtnRefreshSize_Click" />--%>
                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDSize"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDShadeColor" runat="server" visible="false">
                            <asp:Label ID="LblShadeColor" runat="server" Text="SHADE COLOR" CssClass="labelbold"></asp:Label>
                            &nbsp;<asp:Button ID="btnaddshadecolor" runat="server" CssClass="buttonsmalls" OnClientClick="return AddShadecolor()"
                                Text="ADD" TabIndex="24" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="ddshadecolor" runat="server" AutoPostBack="True"
                                Width="200px" TabIndex="23" OnSelectedIndexChanged="ddshadecolor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button CssClass="buttonnorm" ID="refreshshade" runat="server" Text="" BorderWidth="0px"
                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                ForeColor="White" OnClick="refreshshade_Click" />
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddshadecolor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td id="Td1" runat="server">
                            <asp:Label ID="Label1" runat="server" Text=" Min. Stock Qty" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox CssClass="textb" ID="TxtQuantity" runat="server" BackColor="beige" Width="100px"
                                TabIndex="30"></asp:TextBox><b style="color: Red">*</b>
                        </td>
                        <td runat="server" visible="false">
                            <span class="labelbold">Unit</span> &nbsp;&nbsp;&nbsp;
                            <asp:DropDownList CssClass="dropdown" ID="DDOrderUnit" runat="server" AutoPostBack="True"
                                Width="100px" TabIndex="6">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtFinishedid" ForeColor="White" Width="10px" BorderStyle="None"
                                runat="server"></asp:TextBox>
                            <asp:TextBox ID="TxtProdCode" ForeColor="White" Width="10px" BorderStyle="None" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TXTOURCODE" ForeColor="White" Width="10px" BorderStyle="None" runat="server"></asp:TextBox>
                        </td>
                        <td align="center" runat="server">
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                OnClientClick="return Validate()" TabIndex="10" />
                        </td>
                        <tr>
                            <td>
                                <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text=""
                                    Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </tr>
                    <tr align="center" id="trdrig" runat="server">
                        <td align="center" colspan="5">
                            <div style="width: 100%; height: 400px; overflow: auto">
                                <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                    CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chkbox" runat="server" />
                                                <%-- <asp:CheckBox ID="CheckBox1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DESCRIPTION">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldiscription" runat="server" Text='<%# Bind("Description") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION"  />--%>
                                        <asp:TemplateField HeaderText="Min.Stock Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txtqty" runat="server" CssClass="textb" Width="90px" Text='<%# Bind("Qty") %>'
                                                    TabIndex="39"></asp:TextBox>
                                                <asp:Label ID="lblfinished" runat="server" Visible="false" Text='<%# Bind("finishedid") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="HDF1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
