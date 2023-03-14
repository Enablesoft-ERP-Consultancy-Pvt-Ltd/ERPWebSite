<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmBuyerItemWiseRecipeMaster.aspx.cs"
    Inherits="Masters_Carpet_FrmBuyerItemWiseRecipeMaster" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function Validate() {
            if (document.getElementById('CPH_Form_ddBuyerCode')) {
                if (document.getElementById("CPH_Form_ddBuyerCode").selectedIndex == 0) {
                    alert("Please Select Buyer Code....");
                    document.getElementById("CPH_Form_ddBuyerCode").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddCategory')) {
                if (document.getElementById("CPH_Form_ddCategory").selectedIndex == 0) {
                    alert("Please Select Cateogory....");
                    document.getElementById("CPH_Form_ddCategory").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddItemname')) {
                if (document.getElementById("CPH_Form_ddItemname").selectedIndex == 0) {
                    alert("Please Select Item Name....");
                    document.getElementById("CPH_Form_ddItemname").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDRecipeName')) {
                if (document.getElementById("CPH_Form_DDRecipeName").selectedIndex == 0) {
                    alert("Please Select Recipe Name....");
                    document.getElementById("CPH_Form_DDRecipeName").focus();
                    return false;
                }
            }
            return confirm('Do You Want To Save?');
        }
    </script>
    <div id="1" style="height: auto" align="left">
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="labled" Text="BUYER CODE" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="ddBuyerCode" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddBuyerCode_SelectedIndexChanged" Width="200px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddBuyerCode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCategoryName" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="200px" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddCategory"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblItemName" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddItemname" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="200px" OnSelectedIndexChanged="ddItemname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddItemname"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="ql" runat="server" class="tdstyle">
                                        <asp:Label ID="lblQualityName" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddQuality" runat="server" CssClass="dropdown" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="dsn" runat="server" class="tdstyle">
                                        <asp:Label ID="lblDesignName" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddDesign" runat="server" CssClass="dropdown" Width="200px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddDesign"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblRecipeName" runat="server" Text="Recipe Name" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDRecipeName" runat="server" CssClass="dropdown" Width="200px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDRecipeName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" runat="server">
                                        <div style="width: 100%; max-height: 400px; overflow: auto">
                                            <asp:GridView ID="DGRecipeRateMaster" runat="server" AutoGenerateColumns="False"
                                                DataKeyNames="ID" CssClass="grid-views" OnRowDeleting="DGRecipeRateMaster_RowDeleting"
                                                OnRowDataBound="DGRecipeRateMaster_RowDataBound" Width="800px">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="CustomerCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomerCode" Text='<%#Bind("CustomerCode") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CategoryName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCategoryName" Text='<%#Bind("CategoryName") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ItemName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemName" Text='<%#Bind("ItemName") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="QualityName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="DesignName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesignName" Text='<%#Bind("DesignName") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RecipeName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRecipeName" Text='<%#Bind("RecipeName") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                Text="DEL" OnClientClick="return confirm('Do You Want To Delete Data?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErr" runat="server" CssClass="labelbold" ForeColor="Red" Font-Size="Small"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return reloadPage();"
                                            Width="60px" CssClass="buttonsmalls" />
                                        <asp:Button ID="BtnSave" runat="server" Text="Save" Width="60px" CssClass="buttonsmalls"
                                            OnClick="BtnSave_Click" OnClientClick="return Validate()" />
                                        <asp:Button ID="BtnClose" runat="server" Text="Close" Width="60px" OnClientClick="return CloseForm();"
                                            CssClass="buttonsmalls" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
