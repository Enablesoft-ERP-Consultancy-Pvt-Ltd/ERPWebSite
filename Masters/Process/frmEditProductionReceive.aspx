<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEditProductionReceive.aspx.cs"
    Inherits="Masters_Process_frmEditProductionReceive" MasterPageFile="~/ERPmaster.master"
    Title="EDIT PRODUCTION RECEIVE" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div style="width: 890px">
                <div style="width: 800px; height: auto">
                    <div style="background-color: #DEB887; height: 39px; padding-top: 2px; padding-left: 20px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblfoliono" runat="server" Text="Enter Folio No." CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfolioNo" CssClass="textb" runat="server" Width="195px" Height="30px"
                                        OnTextChanged="txtfolioNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 880px; height: 257px; overflow: auto; margin-top: 20px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblerrMsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="DGReceiveDetail" runat="server" AutoGenerateColumns="False" OnRowUpdating="DGReceiveDetail_RowUpdating"
                                        OnRowDeleting="DGReceiveDetail_RowDeleting">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDate" runat="server" Width="90px" CssClass="textb" Text='<%# Bind("ReceiveDate") %>'
                                                        Enabled="false"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calenderext1" runat="server" TargetControlID="txtDate"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="StockNo">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtStockNo" runat="server" Width="100px" CssClass="textb" Enabled="false"
                                                        Text='<%#Bind("TStockNo") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="FolioNo" DataField="FolioNo" />
                                            <asp:BoundField DataField="Item_Name" HeaderText="Item" />
                                            <asp:BoundField DataField="ColorName" HeaderText="Colour" />
                                            <asp:TemplateField HeaderText="W/Y Ply">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtWyPly" runat="server" Width="70px" CssClass="textb" Text='<%#Bind("Wyply") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="C/Y Ply">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCyPly" runat="server" Width="70px" CssClass="textb" Text='<%#Bind("CyPly") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtWeight" runat="server" Width="50px" CssClass="textb" Text='<%#Bind("Weight") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Width">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtWidth" runat="server" Width="70px" CssClass="textb" Text='<%# Bind("Width") %>'
                                                        AutoPostBack="true" OnTextChanged="txtWidth_TextChanged"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Length">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtlength" runat="server" Width="70px" CssClass="textb" Text='<%# Bind("Length") %>'
                                                        AutoPostBack="true" OnTextChanged="txtLength_TextChanged"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Warp/10 Cm." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%-- <asp:CheckBox ID="ChkWarp" runat="server" />--%>
                                                    <asp:TextBox ID="txtWarp" runat="server" Width="70px" CssClass="textb" Text='<%#Bind("Warp") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weft/10 Cm." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--<asp:CheckBox ID="ChkWeft" runat="server" />--%>
                                                    <asp:TextBox ID="txtWeft" runat="server" Width="70px" CssClass="textb" Text='<%#Bind("Weft") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Straightness" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%-- <asp:CheckBox ID="Chkstraightness" runat="server" />--%>
                                                    <asp:TextBox ID="txtStraightness" runat="server" Width="70px" CssClass="textb" Text='<%#Bind("Strainghtness") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Design" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--<asp:CheckBox ID="ChkDesign" runat="server" />--%>
                                                    <asp:TextBox ID="txtDesign" runat="server" Width="70px" CssClass="textb" Text='<%#Bind("Design") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OBA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--<asp:CheckBox ID="ChkOBA" runat="server" />--%>
                                                    <asp:TextBox ID="txtOBA" runat="server" Width="70px" CssClass="textb" Text='<%#Bind("OBA") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Stamp" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDateStamp" runat="server" Width="70px" CssClass="textb" Enabled="false"
                                                        Text='<%#Bind("Date_Stamp") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtremarks" runat="server" Width="250px" TextMode="MultiLine" CssClass="textb"
                                                        Text='<%#Bind("StockNoRemarks") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemFinishedid" runat="server" CssClass="textb" Text='<%# Bind("Item_Finished_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssueOrderId" runat="server" CssClass="textb" Text='<%# Bind("IssueOrderId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssueDetailId" runat="server" CssClass="textb" Text='<%#Bind("issue_Detail_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProcess_Rec_Id" runat="server" CssClass="textb" Text='<%# Bind("Process_Rec_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProcess_Rec_Detail_Id" runat="server" CssClass="textb" Text='<%#Bind("Process_Rec_Detail_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCaltype" runat="server" CssClass="textb" Text='<%# Bind("CalType") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnitId" runat="server" CssClass="textb" Text='<%# Bind("UnitId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArea" runat="server" CssClass="textb" Text='<%# Bind("Area") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFlagFixorWeight" runat="server" CssClass="textb" Text='<%# Bind("FlagFixOrWeight") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                            <%--<asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderid" runat="server" CssClass="textb" Text='<%# Bind("Orderid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                            <%-- <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" CssClass="textb" Text='<%# Bind("Rate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" CssClass="textb" Text='<%# Bind("Qty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCommission" runat="server" CssClass="textb" Text='<%# Bind("Comm") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="buttonnorm" Width="100px"
                                                        CausesValidation="false" CommandName="Update" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="buttonnorm" Width="100px"
                                                        CausesValidation="false" CommandName="Delete" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Wrap="False" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 880px; overflow: auto; margin-top: 20px">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnexport" CssClass="buttonnorm" Text="Export To Excel" runat="server"
                                        OnClick="btnexport_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
