<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmOrderAssignOrder.aspx.cs" Inherits="Masters_Order_FrmOrderAssignOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function Preview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validate() {
            if (document.getElementById("<%=DDCompany.ClientID %>").value == "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=DDCompany.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDcustomer.ClientID %>").value == "0") {
                alert("Pls Select Customer Name");
                document.getElementById("<%=DDcustomer.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDOrder.ClientID %>").value == "0") {
                alert("Pls Select From Order No.");
                document.getElementById("<%=DDOrder.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDOrderto.ClientID %>").value == "0") {
                alert("Pls Select To Order No.");
                document.getElementById("<%=DDOrder.ClientID %>").focus();
                return false;
            }
            else {
                var isValid = false;
                var gridView = document.getElementById('<%= GVOrderStockAssign.ClientID %>');
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
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        //        function DoCalculation(text1, lblordercon, lblActualStock, lblOtherAssigned) {
        //            text1 = document.getElementById(text1);
        //            lblordercon = document.getElementById(lblordercon);
        //            lblActualStock = document.getElementById(lblActualStock);
        //            lblOtherAssigned = document.getElementById(lblOtherAssigned);
        //            Lbl1.innerHTML = parseFloat(Txt1.value) + parseFloat(Txt2.value);
        //            var tot = parseFloat(text1.value) + parseFloat(lblOtherAssigned.innerHTML);
        //        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblComp" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                            Width="250px" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblCustname" runat="server" CssClass="labelbold" Text="Customer"></asp:Label>
                        <%--<asp:CheckBox ID="ChkEdit" runat="server" Text="Check For Edit" Visible="true" OnCheckedChanged="ChkEdit_CheckedChanged"
                            AutoPostBack="True" />--%>
                        <br />
                        <asp:DropDownList ID="DDcustomer" runat="server" AutoPostBack="True" CssClass="dropdown"
                            Width="250px" OnSelectedIndexChanged="DDcustomer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblOrder" runat="server" CssClass="labelbold" Text="From Order"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDOrder" runat="server" CssClass="dropdown" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbltoorder" runat="server" CssClass="labelbold" Text="To Order"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDOrderto" runat="server" CssClass="dropdown" Width="200px"
                            AutoPostBack="True" OnSelectedIndexChanged="DDOrderto_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="3">
                        <div style="overflow: auto; width: 100%; height: 350px;">
                            <asp:GridView ID="GVOrderStockAssign" runat="server" AutoGenerateColumns="false"
                                CssClass="grid-view" Width="972px" DataKeyNames="FINISHEDID">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkbox" runat="server" />
                                            <%-- <asp:CheckBox ID="CheckBox1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="caterory" HeaderText="Category Name">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="item" HeaderText="Item">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="description" HeaderText="Description">
                                        <HeaderStyle Width="160px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%-- <asp:BoundField DataField="IssQty" HeaderText="IssueQty">
                                            <HeaderStyle Width="60px" HorizontalAlign="Center"/>
                                             <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>    --%>
                                    <asp:BoundField DataField="ordercon" HeaderText="OrderConspQty"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Other Order Assigned Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssignedqty" runat="server" Text='<%# getAssignedqty(DataBinder.Eval(Container.DataItem, "FINISHEDID").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ActualStockQty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrdercon" runat="server" Visible="false" Text='<%#Bind ("ordercon") %>' />
                                            <asp:Label ID="lblfinished" runat="server" Visible="false" Text='<%#Bind ("FINISHEDID") %>' />
                                            <asp:Label ID="lblActualstock" runat="server" Text='<%# getActualStock(DataBinder.Eval(Container.DataItem, "FINISHEDID").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="ActualStockQty" HeaderText="ActualStockQty">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="Assign Qty">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtAssignqty" Width="100px" align="right" runat="server" AutoPostBack="true"
                                                OnTextChanged="txtqnt1_changed" onkeypress="return isNumber(event);" Text='<%# getActualAssigned(DataBinder.Eval(Container.DataItem, "FINISHEDID").ToString()) %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Label ID="LblMessage" runat="server" align="Right" ForeColor="Red"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                            OnClientClick="return Validate();" />
                        &nbsp; &nbsp;
                        <%--<asp:Button ID="BtnPrev" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="BtnPrev_Click" />
                        &nbsp; &nbsp;--%>
                        <%--<asp:Button ID="BtnClose" Text="Close" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click" />--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
