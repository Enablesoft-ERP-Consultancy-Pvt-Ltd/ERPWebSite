<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmProductionReceive.aspx.cs"
    Inherits="Masters_Process_frmProductionReceive" MasterPageFile="~/ERPmaster.master"
    Title="PRODUCTION RECEIVE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "frmProductionReceive.aspx";
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td style="width: 70%" valign="top">
                        <table style="width: 100%; background-color: #deb887">
                            <tr>
                                <td style="width: 30%">
                                    <span class="labelbold">Unit</span>
                                    <br />
                                    <asp:DropDownList ID="ddUnits" runat="server" Width="95%" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 30%">
                                    <span class="labelbold">Enter Weaver ID No.</span>
                                    <br />
                                    <asp:TextBox ID="txtWeaverIdNo" runat="server" Width="95%" Height="20px" CssClass="textb"
                                        AutoPostBack="true" OnTextChanged="txtWeaverIdNo_TextChanged"></asp:TextBox>
                                </td>
                                <td style="width: 40%">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="listWeaverName" runat="server" Width="95%" Height="150px" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btnDelete" Text="Delete" runat="server" Width="100px" OnClick="btnDelete_Click"
                                                    CssClass="buttonnorm" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnshowDetail" runat="server" Text="Click For Show Detail" Width="150px"
                                        CssClass="buttonnorm" OnClick="btnshowDetail_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 30%" valign="top">
                        <div runat="server" id="divmultipleconstruction" visible="false" style="width: 95%;
                            max-height: 300px; overflow: auto">
                            <asp:GridView ID="Dgmultiplecons" runat="server" CssClass="grid-views" AutoGenerateColumns="false">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shade Color">
                                        <ItemTemplate>
                                            <asp:Label ID="lblshadecolor" Text='<%#Bind("SHADECOLORNAME") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No. of Weft/10CM">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtnoofweft_Shade" Width="100px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <div>
                <div id="main" style="width: 100%;">
                    <table width="100%">
                        <tr>
                            <td style="width: 70%" valign="top">
                            </td>
                        </tr>
                    </table>
                    <div id="divGrid" runat="server" style="display: block">
                        <table width="100%">
                            <tr>
                                <td>
                                    <div style="width: 1000px; max-height: 150px; overflow: auto;">
                                        <asp:GridView ID="DGReceiveDetail" runat="server" AutoGenerateColumns="False" Width="100%">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDate" runat="server" Width="90px" CssClass="textb" Text='<%# Bind("RecDate") %>'></asp:TextBox>
                                                        <asp:CalendarExtender ID="calenderext1" runat="server" TargetControlID="txtDate"
                                                            Format="dd-MMM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="StockNo">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStockNo" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="FolioNo" DataField="FolioNo" />
                                                <asp:BoundField DataField="Item" HeaderText="Item" />
                                                <asp:BoundField DataField="Colour" HeaderText="Colour" />
                                                <asp:TemplateField HeaderText="W/Y Ply">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWyPly" runat="server" Width="70px" CssClass="textb"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="C/Y Ply">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCyPly" runat="server" Width="70px" CssClass="textb"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Weight">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWeight" runat="server" Width="50px" CssClass="textb"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtDateStamp" runat="server" Width="70px" CssClass="textb" Text='<%#Bind("Date_Stamp") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtremarks" runat="server" Width="250px" TextMode="MultiLine" CssClass="textb"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemFinishedid" runat="server" CssClass="textb" Text='<%# Bind("ItemFinishedid") %>'></asp:Label>
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
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFlagFixorWeight" runat="server" CssClass="textb" Text='<%# Bind("FlagFixOrWeight") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderid" runat="server" CssClass="textb" Text='<%# Bind("Orderid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
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
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle Wrap="False" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <br />
                                    <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                        TabIndex="23" Text="Save" OnClick="BtnSave_Click" Width="75px" />
                                    &nbsp;<asp:Button ID="BtnNew" runat="Server" CssClass="buttonnorm" OnClientClick="return reloadPage();"
                                        TabIndex="24" Text="New" Width="75px" />
                                    &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" TabIndex="26"
                                        Text="Preview" Visible="false" OnClick="BtnPreview_Click" Width="75px" />
                                    &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                        TabIndex="27" Text="Close" Width="75px" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
