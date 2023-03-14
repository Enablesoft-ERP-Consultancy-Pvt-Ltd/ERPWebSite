<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmItemStatus.aspx.cs" Inherits="Masters_Carpet_FrmItemStatus" %>

<%@ Register Src="../../UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function Validate() {
            if (document.getElementById("<%=DDItemCategory.ClientID %>").value == "0") {
                alert("Pls Select Catagory Name");
                document.getElementById("<%=DDItemCategory.ClientID %>").focus();
                return false;
            }
            else {
                var isValid = false;
                var gridView = document.getElementById('<%= DGOrderDetail.ClientID %>');
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
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lblcategoryname" runat="server" Text="Item Category" CssClass="labelbold"></asp:Label>
                <%--<b style="color: Red">&nbsp;&nbsp;&nbsp; *</b>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnadditemcategory" runat="server" CssClass="buttonsmall" OnClientClick="return AddItemCategory()"
                    Text="ADD" TabIndex="12" />
                <br />--%>
                <asp:DropDownList ID="DDItemCategory" AutoPostBack="true" runat="server" Width="200px"
                    OnSelectedIndexChanged="DDItemCategory_SelectedIndexChanged" CssClass="dropdown"
                    ValidationGroup="f1" TabIndex="11">
                </asp:DropDownList>
                <%-- <asp:Button CssClass="refreshcategory" ID="refreshcategory" runat="server" Text=""
                    BorderWidth="0px" Height="1px" Width="1px" BackColor="White" BorderColor="White"
                    BorderStyle="None" ForeColor="White" OnClick="refreshcategory_Click" />--%>
                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDItemCategory"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
            <td>
                <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                <%-- <b style="color: Red">*</b>
                <asp:Button ID="BtnAdd0" runat="server" CssClass="buttonsmall" OnClientClick="return AddItum()"
                    Text="ADD" TabIndex="14" />
                <br />--%>
                <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="200px" runat="server"
                    AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged"
                    ValidationGroup="f1" TabIndex="13">
                </asp:DropDownList>
                <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDItemName"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
                <%--<asp:Button CssClass="buttonnorm" ID="BtnRefreshItem" runat="server" Text="" BorderWidth="0px"
                    Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                    ForeColor="White" OnClick="BtnRefreshItem_Click" />--%>
            </td>
            <td id="TDQuality" runat="server" visible="false">
                <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                <%-- <b style="color: Red">*</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmall" OnClientClick="return AddQuality()"
                    Text="ADD" TabIndex="16" />--%>
                <br />
                <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" AutoPostBack="True"
                    Width="200px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged" TabIndex="15">
                </asp:DropDownList>
                <%--<asp:Button CssClass="refreshquality" ID="refreshquality" runat="server" Text=""
                    BorderWidth="0px" Height="1px" Width="1px" BackColor="White" BorderColor="White"
                    BorderStyle="None" ForeColor="White" OnClick="refreshquality_Click" />--%>
                <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDQuality"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
            <td id="TdDESIGN" runat="server" class="tdstyle">
                <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <%-- <asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmall" OnClientClick="return AddDesign()"
                    Text="ADD" TabIndex="20" />--%>
                <br />
                <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" AutoPostBack="True"
                    Width="200px" TabIndex="19" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                </asp:DropDownList>
                <%--<asp:Button CssClass="refreshdesign" ID="refreshdesign" runat="server" Text="" BorderWidth="0px"
                    Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                    ForeColor="White" OnClick="refreshdesign_Click" />--%>
                <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDDesign"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
        </tr>
        <tr>
            <td id="TDColor" runat="server" visible="false">
                <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmall" OnClientClick="return AddColor()"
                    Text="ADD" TabIndex="22" />--%>
                <br />
                <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" AutoPostBack="True"
                    Width="200px" TabIndex="21" OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                </asp:DropDownList>
                <%--<asp:Button CssClass="refreshcolor" ID="refreshcolor" runat="server" Text="" BorderWidth="0px"
                    Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                    ForeColor="White" OnClick="refreshcolor_Click" />--%>
                <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDColor"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
            <td id="TDShape" runat="server" visible="false" class="tdstyle">
                <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmall" OnClientClick="return AddShape()"
                    Text="ADD" TabIndex="26" />--%>
                <br />
                <asp:DropDownList CssClass="dropdown" ID="DDShape" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="DDShape_SelectedIndexChanged" Width="200px" TabIndex="25">
                </asp:DropDownList>
                <%--<asp:Button CssClass="buttonnorm" ID="refreshshape" runat="server" Text="" BorderWidth="0px"
                    Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                    ForeColor="White" OnClick="refreshshape_Click" />--%>
                <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDShape"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
            <td id="TDSize" runat="server" class="tdstyle">
                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnAddSize" runat="server" CssClass="buttonsmall" OnClientClick="return AddSize()"
                    Text="ADD" TabIndex="28" />--%>
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
                <%--&nbsp;<asp:Button ID="btnaddshadecolor" runat="server" CssClass="buttonsmall" OnClientClick="return AddShadecolor()"
                    Text="ADD" TabIndex="24" />
                <br />--%>
                <asp:DropDownList CssClass="dropdown" ID="ddshadecolor" runat="server" AutoPostBack="True"
                    Width="200px" TabIndex="23" OnSelectedIndexChanged="ddshadecolor_SelectedIndexChanged">
                </asp:DropDownList>
                <%--<asp:Button CssClass="buttonnorm" ID="refreshshade" runat="server" Text="" BorderWidth="0px"
                    Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                    ForeColor="White" OnClick="refreshshade_Click" />--%>
                <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddshadecolor"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <span class="labelbold">Check Orders</span> &nbsp;&nbsp;
                <asp:DropDownList CssClass="dropdown" ID="DDStatus" runat="server" Width="100px"
                    OnSelectedIndexChanged="DDStatus_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;<span class="labelbold">Change Status</span> &nbsp;&nbsp;
                <asp:DropDownList CssClass="dropdown" ID="ddststatuschange" runat="server" Width="100px">
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr align="right">
            <td runat="server" colspan="4">
                <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Change Status" runat="server"
                    OnClick="BtnSave_Click" OnClientClick="return Validate();" />
            </td>
        </tr>
        <tr align="center" id="trdrig" runat="server">
            <td align="center" colspan="4">
                <div style="width: 100%; height: 400px; overflow: auto">
                    <asp:GridView ID="DGOrderDetail" runat="server" DataKeyNames="finishedid" AutoGenerateColumns="False"
                        CssClass="grid-view">
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
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
