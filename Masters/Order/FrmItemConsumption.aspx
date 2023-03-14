<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmItemConsumption.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Order_FrmItemConsumption" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table width="60%">
                <tr>
                    <td align="center" colspan="4">
                        <b style="color: Blue">Rate Unit&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b>
                        <asp:RadioButton ID="rdoUnitWise" Text="UNIT WISE" ForeColor="Blue" CssClass="radiobuttonnormal"
                            runat="server" GroupName="OrderType" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoPcWise" Text="PC WISE" CssClass="radiobuttonnormal" ForeColor="Blue"
                            runat="server" GroupName="OrderType" />
                    </td>
                </tr>
                <tr>
                    <td id="TDItemCode" runat="server" class="tdstyle">
                        <asp:Label ID="LblItemCode" runat="server" Text="Item Code"></asp:Label>
                        <br />
                        <asp:TextBox ID="TxtProdCode" runat="server" CssClass="textb" Width="150px" AutoPostBack="True"
                            OnTextChanged="TxtProdCode_TextChanged" TabIndex="29"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" EnableCaching="true"
                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                            UseContextKey="True">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblProcessName" runat="server" Text="Process Name"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddProcessName" CssClass="dropdown" runat="server" Width="150px"
                            AutoPostBack="True" TabIndex="30">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblOrderUnit" runat="server" Text="Order Unit"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddOrderUnit" CssClass="dropdown" runat="server" Width="150px"
                            TabIndex="30">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="lblCategoryname" runat="server" Text="Category Name"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddCategoryName" CssClass="dropdown" runat="server" Width="150px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddCategoryName_SelectedIndexChanged"
                            TabIndex="30">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="lblItemname" runat="server" Text="Item Name"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddItemName" runat="server" Width="150px" AutoPostBack="True"
                            CssClass="dropdown" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged"
                            TabIndex="31">
                        </asp:DropDownList>
                    </td>
                    <td id="Quality" runat="server" visible="false" colspan="2" class="tdstyle">
                        <asp:Label ID="lblQualityname" runat="server" Text="Quality"></asp:Label>
                        &nbsp;<br />
                        <asp:DropDownList ID="ddQuality" runat="server" Width="150px" TabIndex="32" AutoPostBack="True"
                            CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td id="Design" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblDesignname" runat="server" Text="Design"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddDesign" runat="server" Width="150px" TabIndex="33" AutoPostBack="True"
                            CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td id="Color" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblColorname" runat="server" Text="Color"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddColor" runat="server" Width="150px" TabIndex="34" AutoPostBack="True"
                            CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td id="Shape" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblShapename" runat="server" Text="Shape"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddShape" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                            TabIndex="35" OnSelectedIndexChanged="ddShape_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td id="Size" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblSizename" runat="server" Text="Size"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddSize" runat="server" Width="150px" TabIndex="36" AutoPostBack="True"
                            CssClass="dropdown" OnSelectedIndexChanged="ddSize_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td id="Shade" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblShade" runat="server" Text="Shade"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddShade" runat="server" Width="150px" TabIndex="37" CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblQty" runat="server" Text="Qty"></asp:Label>
                        <br />
                        <asp:TextBox ID="TxtQty" CssClass="textb" runat="server" Width="100px" AutoPostBack="True"
                            OnTextChanged="TxtQty_TextChanged"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblArea" runat="server" Text="Area"></asp:Label>
                        <br />
                        <asp:TextBox ID="TxtArea" CssClass="textb" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                            OnClientClick="return confirm('Do you want to save data?')" ValidationGroup="f1"
                            TabIndex="48" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                            OnClientClick="return CloseForm();" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnReport" runat="server" Text="Preview"
                            OnClick="BtnReport_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div style="width: 100%; height: 200px; overflow: scroll">
                            <asp:GridView ID="DGDetail" Width="100%" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                                OnRowDataBound="DGDetail_RowDataBound" OnRowDeleting="DGDetail_RowDeleting" CssClass="grid-view"
                                OnRowCreated="DGDetail_RowCreated">
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="Category" HeaderText="Category">
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ItemName" HeaderText="ItemName">
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Left" Width="400px" />
                                        <ItemStyle HorizontalAlign="Left" Width="400px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Area" HeaderText="Area">
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Del" OnClientClick="return confirm('Do You Want To Delete ?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ConsmptionID" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
