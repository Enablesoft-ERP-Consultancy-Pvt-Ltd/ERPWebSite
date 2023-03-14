<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmOrderPlanning.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Order_FrmOrderPlanning"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function Validate() {
            if (document.getElementById("<%=DDCompanyName.ClientID %>").value == "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=DDCompanyName.ClientID %>").focus();
                return false;
            }

            else if (document.getElementById("<%=DDCustomerCode.ClientID %>").value == "0") {
                alert("Pls Select Customer Code");
                document.getElementById("<%=DDCustomerCode.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDOrderNo.ClientID %>").value == "0") {
                alert("Pls Select Order No");
                document.getElementById("<%=DDOrderNo.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDProcessName.ClientID %>").value == "0") {
                alert("Pls Select Process Name");
                document.getElementById("<%=DDProcessName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TXTQty.ClientID %>").value == "" || document.getElementById("<%=TXTQty.ClientID %>").value == "0") {
                alert("Quantity Cann't be blank Or Zero");
                document.getElementById("<%=TXTQty.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TxtProcessReqDate.ClientID %>").value == "") {
                alert("Pls Select ProcessReqDate");
                document.getElementById("<%=TxtProcessReqDate.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
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
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table width="70%">
                <tr>
                    <td colspan="6" align="center">
                        <asp:CheckBox ID="ChkEditOrder" runat="server" Text="Add Photo" AutoPostBack="True"
                            OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />
                    </td>
                </tr>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="LblCompanyName" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label><b
                            style="color: Red"> &nbsp; *</b>
                        <br />
                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblCustomerCode" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label><b
                            style="color: Red"> &nbsp; *</b>
                        <br />
                        <asp:DropDownList ID="DDCustomerCode" runat="server" Width="200px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblOrderNo" runat="server" Text="Order No" CssClass="labelbold"></asp:Label><b
                            style="color: Red"> &nbsp; *</b>
                        <br />
                        <asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblProcessName" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label><b
                            style="color: Red"> &nbsp; *</b>
                        <br />
                        <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" CssClass="dropdown"
                            AutoPostBack="True" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label3" runat="server" Text="Qty" CssClass="labelbold"></asp:Label><b
                            style="color: Red"> &nbsp; *</b>
                        <asp:TextBox ID="TXTQty" runat="server" Width="88px" onkeypress="return isNumber(event);"
                            BackColor="beige" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblDate" runat="server" Text="Process Req Date" CssClass="labelbold"></asp:Label><b
                            style="color: Red"> &nbsp; *</b>
                        <br />
                        <asp:TextBox ID="TxtProcessReqDate" runat="server" Format="dd-MMM-yyyy" Width="120px"
                            BackColor="beige" CssClass="textb" OnTextChanged="TxtProcessReqDate_TextChanged"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtProcessReqDate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="Label1" runat="server" Text="Order Date :-" CssClass="labelbold"></asp:Label>
                        <asp:Label ID="LblOrderDate" runat="server" Text="" CssClass="labelbold"></asp:Label>
                    </td>
                    <td colspan="2" class="tdstyle">
                        <asp:Label ID="Label2" runat="server" Text="Stock at PH Date :-" CssClass="labelbold"></asp:Label>
                        <asp:Label ID="LblOrderReqDate" runat="server" Text="" CssClass="labelbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div style="width: 700px; height: 200px; overflow: auto">
                            <asp:GridView ID="GDNew" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                Width="100%" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="Description"></asp:BoundField>
                                    <asp:TemplateField HeaderText="PHOTO">
                                        <ItemTemplate>
                                            <%--<asp:Image ID="Image1" runat="server" Height="50px" ImageUrl='<%# "~/ImageHandler.ashx?ID=" + Eval("orderdetailid")+"&img=4"%>'
                                                Width="100px" />--%>
                                            <asp:Image ID="Image" runat="server" ImageUrl='<%# Bind("photo") %>' Height="70px"
                                                Width="100px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add Photo">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlDetails1" runat="server" NavigateUrl='<%# "../Carpet/AddPhotoRefImage.aspx?orderid=" + Eval("id") + "&orderdetailid=" + Eval("orderdetailid")%>'
                                                Target="_blank" Text="Add Photo" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                        <div>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Total Qty =&nbsp;
                            <asp:Label ID="lblqty" runat="server" Font-Bold="true" ForeColor="RED" Text=""></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Remarks
                        <asp:TextBox ID="txtremark" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="Some IMportent Fields are Missing......."
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblmsg" runat="server" Font-Bold="true" ForeColor="RED" Text="" Visible="false"></asp:Label>
                    </td>
                    <td colspan="3">
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                            OnClientClick="return Validate()" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                            OnClientClick="return CloseForm();" />
                        <asp:Button ID="refreshPhotoRefImage" runat="server" Text="" BackColor="White" BorderWidth="0px"
                            ForeColor="White" Height="0px" Width="0px" OnClick="refreshPhotoRefImage_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div style="width: 600px; height: 200px; overflow: auto">
                            <asp:GridView ID="DG" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                Width="100%" OnRowDataBound="DG_RowDataBound" OnSelectedIndexChanged="DG_SelectedIndexChanged"
                                CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="OrderNo" HeaderText="Order No">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProcessName" HeaderText="Process">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Date" HeaderText="Req.Date">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Qty" DataField="Qty">
                                        <HeaderStyle />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="FinalDate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="FinalDate" Text='<%# Bind("FinalDate") %>' runat="server" Format="dd-MMM-yyyy"
                                                Width="100px" CssClass="textb"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtProcessReqDate">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("FinalStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Remark" HeaderText="Plan.Remark" />
                                    <asp:BoundField DataField="depRemark" HeaderText="Dep.Remark" />
                                    <asp:BoundField DataField="status" HeaderText="Status">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                OnClientClick="return confirm('Do You Want To Delete ?')" Text="Del"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                    <td colspan="3">
                        <div style="width: 100%; height: 200px; overflow: auto;">
                            <asp:GridView ID="DGShowConsumption" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory">
                                        <HeaderStyle />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                        <HeaderStyle />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                        <HeaderStyle />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                        <asp:HiddenField ID="HiddenID" runat="server" />
                    </td>
                </tr>
                <tr align="center">
                    <td colspan="4">
                        <asp:Button CssClass="buttonnorm" ID="BtnFinalsave" Text="Click For Final Planning"
                            runat="server" OnClick="BtnFinalsave_Click" OnClientClick="return confirm('Do You Want To Save For Final planning ?')" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button CssClass="buttonnorm" ID="btnExcelExport" Text="Print Internal Fabric Sheet"
                            Visible="false" runat="server" OnClick="btnExcelExport_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red" Visible="false"></asp:Label>
                        <asp:Label ID="Lblsave" runat="server" CssClass="labelbold" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="trgridconsumption" visible="false">
                    <td colspan="5">
                        <div style="height: 300px; overflow: auto">
                            <asp:GridView ID="DGConsumption" Width="100%" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                                CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <%--<asp:BoundField DataField="Category" HeaderText="Category">
                                <HeaderStyle Width="75px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Item" HeaderText="Item">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>--%>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderText="Unit">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>
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
