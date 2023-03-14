<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmPurchaseRevisedDate.aspx.cs" Inherits="Masters_Purchase_FrmPurchaseRevisedDate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="mm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function validate() {
            if (document.getElementById("<%=ddCatagory.ClientID %>").value <= "0") {
                alert("Pls Select Category Name");
                document.getElementById("<%=ddCatagory.ClientID %>").focus();
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
        function MouseHover() {
            $find("pnlModal_ModalPopupExtender").show();
        }

    </script>
    <table width="100%">
        <tr>
            <td>
                <span class="labelbold">Department</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                    TabIndex="13" AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="labelbold">Vendor</span>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="DdEmp" runat="server" Width="150px" CssClass="dropdown" TabIndex="13"
                    AutoPostBack="True" OnSelectedIndexChanged="DDEmp_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClientClick="return validate();"
                    OnClick="BtnSave_Click" TabIndex="10" />
            </td>
        </tr>
        <tr align="center" id="trdrig" runat="server">
            <td align="center">
                <div style="width: 100%; height: 280px; overflow: auto">
                    <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                        CssClass="grid-view" DataKeyNames="PindentIssueid" OnRowCommand="DGOrderDetail_RowCommand">
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
                            <asp:TemplateField HeaderText="OrderNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderno" runat="server" Text='<%# Bind("Orderno") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P.O No">
                                <ItemTemplate>
                                    <asp:Label ID="lblpono" runat="server" Visible="false" Text='<%# Bind("PindentIssueid") %>' />
                                    <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>' />
                                    <asp:LinkButton ID="linkpono" runat="server" ToolTip="Click to View details" CausesValidation="False"
                                        CommandName="Show" Text='<%# Bind("PindentIssueid") %>' CommandArgument='<%#Eval("PindentIssueid") %>'></asp:LinkButton>
                                    <mm1:ModalPopupExtender ID="pnlModal_ModalPopupExtender" runat="server" PopupControlID="pnlModal"
                                        TargetControlID="linkpono" DynamicServicePath="" Enabled="false" BackgroundCssClass="modalBackground">
                                    </mm1:ModalPopupExtender>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="duedate" HeaderText="Fabric Delivery Date" />
                            <asp:TemplateField HeaderText="Revised Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txtreviseddate" runat="server" CssClass="textb" Width="90px" TabIndex="39"
                                        Text='<%# getDate(DataBinder.Eval(Container.DataItem, "PindentIssueid").ToString()) %>'></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="Txtreviseddate">
                                    </asp:CalendarExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Revised Remark">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtRemark" TextMode="MultiLine" runat="server" CssClass="textb"
                                        Width="150px" TabIndex="39" Text='<%# getRemark(DataBinder.Eval(Container.DataItem, "PindentIssueid").ToString()) %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Complete Status">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkpend" runat="server" />
                                    <%-- <asp:CheckBox ID="CheckBox1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Remark Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txtlastreviseddate" runat="server" CssClass="textb" Width="90px"
                                        Enabled="false" TabIndex="39" Text='<%# getlastDate(DataBinder.Eval(Container.DataItem, "PindentIssueid").ToString()) %>'></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="Txtlastreviseddate">
                                    </asp:CalendarExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <asp:Panel ID="popUpPanel" runat="server" CssClass="confirm-dialog">
                    <div class="inner">
                        <h2>
                            Detail</h2>
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="close" />
                        <div class="base">
                            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="false"
                                CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="description" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="350px" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="350px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Ordered Qty">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Receive Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfinishedid" runat="server" Text='<%# Bind("ITEM_FINISHED_ID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("PindentIssueid") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblRecqty" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "PindentIssueid").ToString(),DataBinder.Eval(Container.DataItem, "ITEM_FINISHED_ID").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pending Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPendqty" runat="server" Text='<%# getpending(DataBinder.Eval(Container.DataItem, "Qty").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hnremark" runat="server" />
                <asp:HiddenField ID="hnrecqty" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
