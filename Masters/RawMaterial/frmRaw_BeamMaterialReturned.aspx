<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmRaw_BeamMaterialReturned.aspx.cs"
    Inherits="Masters_RawMaterial_frmRaw_BeamMaterialReturned" MasterPageFile="~/ERPmaster.master"
    Title="RAW_BEAM RETURNED" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmRaw_BeamMaterialReturned.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            if (document.getElementById('CPH_Form_DDjob').selectedIndex <= "0") {
                alert('Plz Select Job...')
                document.getElementById('CPH_Form_DDjob').focus()
                return false;
            }

        }
        function EnterNumericonly(evt) {
            var charcode = (evt.which) ? evt.which : event.keycode;
            if (charcode >= 31 && (charcode < 48 || charcode > 57)) {
                alert('Enter numeric value only...')
                return false;
            }
        }
        function EnterNumericWithdot(evt) {
            var charcode = (evt.which) ? evt.which : event.keycode;
            if (charcode != 46 && charcode > 31 && (charcode < 48 || charcode > 57)) {
                alert('Enter numeric value only')
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 800px; height: 500px;">
                    <div style="background-color: #DEB887; width: 771px; height: 45px; margin: 10px 0px;
                        padding: 23px 0px 0px 15px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFolioNo" runat="server" Text="Enter Folio No" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFolioNo" runat="server" CssClass="textb" Width="150px" Height="23px"
                                        OnTextChanged="txtFolioNo_TextChanged" onkeypress="return EnterNumericonly(event);"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblFolioMsg" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="background-color: #DEB887; width: 791px; height: Auto;">
                        <div style="background-color: #DEB887; width: 787px; height: Auto; padding: 10px 0px 0px 4px;">
                            <table width="775px">
                                <tr>
                                    <td>
                                        <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                            Width="774px" OnRowCommand="DGDetail_RowCommand">
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
                                                <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="IssuedQty">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssuedQty" runat="server" Text='<%#Bind("IssuedQty") %>' Width="75px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Received Qty">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceivedQty" runat="server" Text='<%#Bind("ReceivedQty") %>' Width="75px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IssueOrderid" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CompanyId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCompanyId" runat="server" Text='<%#Bind("CompanyId") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Godown" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGodownId" runat="server" Text='<%#Bind("GodownId") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CategoryId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategoryId" runat="server" Text='<%#Bind("CategoryId") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FinishedId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFinishedId" runat="server" Text='<%#Bind("FinishedId") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="UnitId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitId" runat="server" Text='<%#Bind("UnitId") %>'>></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="LotNo/BatchNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLotNo" runat="server" Text='<%#Bind("LotNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TagNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTagno" runat="server" Text='<%#Bind("TagNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BeamNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBeamNo" runat="server" Text='<%#Bind("BeamNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Return Qty">
                                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtreturnQty" runat="server" BackColor="Yellow" Width="75px" CssClass="textb"
                                                            onkeypress="return EnterNumericWithdot(event);" ToolTip="Enter return Qty"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbeamtype" Text='<%#Bind("Beamtype") %>' runat="server" />
                                                        <asp:Label ID="lblitemid" Text='<%#Bind("Itemid") %>' runat="server" />
                                                        <asp:Label ID="lblflagsize" Text='<%#Bind("flagsize") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkSave" runat="server" CausesValidation="False" CommandName="Save"
                                                            Text="Save" Font-Size="15px" OnClientClick="return confirm('Do you want to save Data?');"
                                                            ToolTip="Click to Save"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table width="775px">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnNew" runat="server" Text="New" CssClass="buttonnorm" Width="42px"
                                        OnClientClick="return NewForm();" />&nbsp;
                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonnorm" Width="52px"
                                        OnClientClick="return CloseForm();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
