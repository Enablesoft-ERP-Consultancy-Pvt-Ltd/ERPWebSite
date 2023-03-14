<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmpackingRegister.aspx.cs"
    Inherits="Masters_Packing_frmpackingRegister" MasterPageFile="~/ERPmaster.master"
    Title="PACKING REGISTER" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "frmpackingRegister.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            else {
                return true;
            }
        }
        function isNumberWith(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <div id="maindiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <div id="main" style="width: 950px; height: 238px;">
                        <div style="width: 800px; height: 218px; border-style: groove; background-color: #DEB887;
                            margin-left: 50px">
                            <div style="margin-left: 100px; margin-top: 80px; width: 500px">
                                <table width="500px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblunit" runat="server" Text="Unit" Font-Size="15px" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddUnits" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStokNo" runat="server" Text="Enter Stock No." Font-Size="15px"
                                                CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStockNo" runat="server" CssClass="textb" Width="150px" Height="28px"
                                                OnTextChanged="txtStockNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblArticleNo" Text="ArticleNo." Font-Size="18px" runat="server" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblArticlevalue" runat="server" CssClass="labelbold" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblitem" runat="server" Text="Item:" Font-Size="18px" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblItemDetail" runat="server" Text="" CssClass="labelbold"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="divGrid" runat="server" style="display: block">
                            <table width="860px" style="text-align: right">
                                <tr>
                                    <td>
                                        &nbsp;<asp:Button ID="BtnNew" runat="Server" CssClass="buttonnorm" OnClientClick="return reloadPage();"
                                            TabIndex="24" Text="New" />
                                        &nbsp;&nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                            TabIndex="27" Text="Close" />
                                    </td>
                                </tr>
                            </table>
                            <div style="width: 900px; height: 150px; overflow: auto;">
                                <table>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table width="880px">
                                    <tr>
                                        <td align="right">
                                            <br />
                                            <asp:GridView ID="DGPackingRegister" runat="server" AutoGenerateColumns="False" DataKeyNames="Item_Finished_id"
                                                OnRowCommand="DGPackingRegister_RowCommand">
                                                <HeaderStyle CssClass="gvheaders" Width="500px" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="RollNo">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRollNo" runat="server" CssClass="textb" Text='<%#Bind("RollNo") %>'
                                                                Width="90px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="StockNo">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtStockNo" runat="server" CssClass="textb" Enabled="false" Text='<%#Bind("TStockNo") %>'
                                                                Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Lay Flat">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDLayFlat" runat="server">
                                                                <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hand/Feet Feel">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDHandfeetfeel" runat="server">
                                                                <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Not ok" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Smell">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDSmell" runat="server">
                                                                <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Colour Variation">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDColourVariation" runat="server">
                                                                <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stain">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDStain" runat="server">
                                                                <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Moisture">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDMoisture" runat="server">
                                                                <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Not ok" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Appearance">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDTotalAppearance" runat="server">
                                                                <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Not ok" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Clarity of Design">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDClarityofDesign" runat="server">
                                                                <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Not ok" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Insect">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDInsect" runat="server">
                                                                <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Weaving Defect">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDWeavingDefect" runat="server">
                                                                <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Weight Gross">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWeightGross" runat="server" CssClass="textb" Width="75px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Weight Net">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWeightNet" runat="server" CssClass="textb" Width="75px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fringes">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDFringes" runat="server">
                                                                <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Not ok" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actual Size">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtActualSize" runat="server" CssClass="textb" Enabled="false" Text='<%#Bind("ActualSize") %>'
                                                                Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Width">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtwidth" runat="server" CssClass="textb" Text='<%#Bind("Width") %>'
                                                                Width="80px" BackColor="Yellow"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Length">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLength" runat="server" CssClass="textb" Text='<%#Bind("Length") %>'
                                                                Width="80px" BackColor="Yellow"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Binding">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DDBinding" runat="server">
                                                                <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Not ok" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date Stamp">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDateStamp" runat="server" CssClass="textb" Text='<%#Bind("Date_Stamp") %>'
                                                                Width="70px" BackColor="Yellow"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Table No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txttableno" runat="server" CssClass="textb" Width="70px" BackColor="Yellow"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnSave" runat="server" CommandName="Save" CssClass="buttonnorm"
                                                                Text="Save" Width="75px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle Wrap="False" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblmessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
