<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmRawMaterialIssueForAnisa.aspx.cs"
    Inherits="Masters_RawMaterial_frmRawMaterialIssueForAnisa" MasterPageFile="~/ERPmaster.master"
    Title="Raw Material Issue" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "frmRawMaterialIssueForAnisa.aspx";
        }
        function Preview() {
            window.open('../../reportViewer1.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 950px">
                <div style="width: 800px; height: auto">
                    <div style="background-color: #DEB887; height: 60px; height: auto; padding-top: 5px;
                        padding-left: 20px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblfoliono" runat="server" Text="Enter Folio No." CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfolioNo" runat="server" Width="195px" Height="30px" OnTextChanged="txtfolioNo_TextChanged"
                                        AutoPostBack="true" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkforCone" Text="Check For Cone Issue" runat="server" ForeColor="Red"
                                        CssClass="checkboxbold" OnCheckedChanged="chkforCone_CheckedChanged" AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="width: 500px; height: 89px; overflow: auto; margin-top: 10px">
                    <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="IssueDetailId"
                        Width="479px">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <Columns>
                            <asp:BoundField DataField="Category" HeaderText="CATEGORY" Visible="false">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="125px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Articles" HeaderText="ARTICLES">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Colour" HeaderText="COLOUR">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Size" HeaderText="SIZE">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Length" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtlength" runat="server" Text='<%#Bind("Length") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Width" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtWidth" runat="server" Text='<%#Bind("Width") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Qty" HeaderText="Qty">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Rate" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRate" runat="server" Text='<%#Bind("Rate") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Area" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtArea" runat="server" Text='<%#Bind("Area") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("AMount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OrderId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderId" runat="server" Text='<%#Bind("OrderId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ItemFInishedid" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemFinishedId" runat="server" Text='<%#Bind("Item_Finished_Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnitName" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnits" runat="server" Text='<%#Bind("Units") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <div style="margin-top: 15px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblErrmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" id="TdDgConsumption" runat="server" style="display: none">
                                <div style="height: 400px; overflow: auto; width: 890px">
                                    <asp:GridView ID="DGConsumption" runat="server" AutoGenerateColumns="False" DataKeyNames="IFinishedid"
                                        OnRowDataBound="DGConsumption_RowDataBound" OnRowCommand="DGConsumption_RowCommand">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Item Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("ITEM_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="250px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ConsmpQTY" HeaderText="Consmp Qty">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IssuedQty" HeaderText="Issued Qty">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Pend Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendQty" runat="server" Text='<%#Bind("PendQty") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Godown" Visible="false">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddgodown" runat="server" Width="150px" OnSelectedIndexChanged="ddgodown_onSelectedindexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LotNo/BatchNo">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlotNo" runat="server" Width="150px" OnSelectedIndexChanged="ddlotnoDgConsumption_onSelectedindexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtStockQty" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issue Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtIssueQty" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkSave" runat="server" CausesValidation="False" CommandName="Save"
                                                        Text="Save"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" id="TdDGConsumptionConeType" runat="server" style="display: none">
                                <div style="height: 400PX; overflow: auto; width: 950px">
                                    <asp:GridView ID="DGConsumptionConeType" runat="server" AutoGenerateColumns="False"
                                        DataKeyNames="IFinishedid" CssClass="grid-view" OnRowDataBound="DGConsumptionConeType_RowDataBound"
                                        OnRowCommand="DGConsumptionConeType_RowCommand">
                                        <HeaderStyle CssClass="gvheaders" Wrap="False" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ConsmpQTY" HeaderText="Consmp Qty">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IssuedQty" HeaderText="Issued Qty">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PendQty" HeaderText="Pend Qty">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Godown" Visible="false">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddgodown" runat="server" Width="130px" OnSelectedIndexChanged="ddgodown_onSelectedindexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LotNo/BatchNo">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlotNo" runat="server" Width="130px" OnSelectedIndexChanged="ddlotnoDgConsumptionConeType_onSelectedindexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtStockQty" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ConeType">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDConeType" runat="server" Width="130px">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No. Of Cones">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNoofCones" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issue Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtIssueQty" runat="server" Width="75px" CssClass="textb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkSave" runat="server" CausesValidation="False" CommandName="Save"
                                                        Text="Save"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblConsumption" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" runat="server" id="TDPreview" visible="false">
                                <asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                    CssClass="buttonnorm" Width="70px" />
                                &nbsp;<asp:Button ID="BtnPreview" runat="server" Text="Preview" Visible="true" CssClass="buttonnorm"
                                    Width="90px" OnClick="BtnPreview_Click" />
                                &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                    CssClass="buttonnorm" Width="80px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
