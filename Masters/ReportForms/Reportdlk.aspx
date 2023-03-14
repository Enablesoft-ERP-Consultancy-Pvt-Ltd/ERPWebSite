<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="Reportdlk.aspx.cs" EnableEventValidation="false" Inherits="Masters_ReportForms_Reportdlk" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            {
                window.open('../../ReportViewer.aspx', '');
            }
        }
        function check() {
            if (document.getElementById("<%=ddCompName.ClientID %>")) {
                if (document.getElementById("<%=ddCompName.ClientID %>").value == "0") {
                    alert("Please Select Company");
                    document.getElementById("<%=ddCompName.ClientID %>").focus();
                    return false;
                }
                else if (document.getElementById("<%=TxtFRDate.ClientID %>").value == "") {
                    alert("Please Select from Date");
                    document.getElementById("<%=TxtFRDate.ClientID %>").focus();
                    return false;
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="height: 750px">
                <table>
                    <tr>
                        <td width="200px">
                        </td>
                        <td valign="top" id="tdreporttype" runat="server">
                            <div style="width: 300px; height: 170px; float: left; background-color: #e1efbb;
                                border-width: thin">
                                <asp:RadioButton ID="rdgarmentorder" Text="OrderDate Between Two Dates " runat="server"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDRawMaterial_CheckedChanged" /><br />
                                <asp:RadioButton ID="RDproductionRpt" Text="Stock At PH Month" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDpurdelivRpt_CheckedChanged" /><br />
                                <asp:RadioButton ID="RDPurchaseStatus" Text="Purchase Status" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDPurchaseStatus_CheckedChanged" />
                                <br />
                                <asp:RadioButton ID="RDJwissue" Text="JW Garment Report" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDJwissue_CheckedChanged" />
                                <br />
                                <asp:RadioButton ID="RDdpissue" Text="DP Garment Report" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDdpissue_CheckedChanged" />
                                <br />
                                <asp:RadioButton ID="Rdorderdetail" Text="Order Detail" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDorderdetail_CheckedChanged" /><br />
                                <asp:RadioButton ID="RdVendorwise" Text="Vendor Wise" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RdVendorwise_CheckedChanged" /><br />
                                <asp:RadioButton ID="Rdpurchase" Text="Purchase Order Sheet" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="Rdpurchaseorder_CheckedChanged" />
                            </div>
                        </td>
                        <td valign="top">
                            <table>
                                <tr id="Trcomp" runat="server">
                                    <td id="Tdcomp" runat="server" class="tdstyle">
                                        <span class="labelbold">Company Name</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddCompName" runat="server" Width="250px" TabIndex="1" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCompName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr runat="server" id="trcustomer" visible="false">
                                    <td id="tdcustomer" runat="server">
                                        <asp:Label ID="lblcusomername" class="tdstyle" runat="server" Text="Customer Code"
                                            CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddcustomer" runat="server" Width="250px" OnSelectedIndexChanged="ddcustomer_SelectedIndexChanged"
                                            AutoPostBack="True" TabIndex="7" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddcustomer"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="Tr3" runat="server">
                                    <td>
                                        <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Department Name"
                                            CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                            TabIndex="13" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddCatagory"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="labelbold">Order&nbsp; Status</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="DDStatus" runat="server" Width="100px"
                                            OnSelectedIndexChanged="DDStatus_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="0">Pending</asp:ListItem>
                                            <asp:ListItem Value="1">Complete</asp:ListItem>
                                            <asp:ListItem Value="2">Cancel</asp:ListItem>
                                            <asp:ListItem Value="3">ALL</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="pstatus" runat="server" visible="false">
                                    <td>
                                        <span class="labelbold">Purchase&nbsp; Status</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="ddpstatus" runat="server" Width="100px"
                                            OnSelectedIndexChanged="DDStatus_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                            <asp:ListItem Value="Complete">Complete</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="pindent" runat="server" visible="false">
                                    <td>
                                        <span class="labelbold">Indent&nbsp; Status</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="ddistatus" runat="server" Width="100px"
                                            OnSelectedIndexChanged="DDStatus_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                            <asp:ListItem Value="complete">complete</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="dpstatus" runat="server" visible="false">
                                    <td>
                                        <span class="labelbold">DP&nbsp; Status</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="dpstatus1" runat="server" Width="100px"
                                            OnSelectedIndexChanged="DDStatus_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                            <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trpptype" runat="server" visible="false">
                                    <td>
                                        <span class="labelbold">Status Of Vendor</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="ddvendorstatus" runat="server" Width="100px"
                                            OnSelectedIndexChanged="DDStatus_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="0">Purchase</asp:ListItem>
                                            <asp:ListItem Value="1">Production</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" id="trorder">
                                    <td id="tdorder" runat="server" class="tdstyle">
                                        <span class="labelbold">Order No.</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddOrderno" runat="server" Width="250px" TabIndex="8" AutoPostBack="True"
                                            CssClass="dropdown" OnSelectedIndexChanged="ddOrderno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddOrderno"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr runat="server" id="trsupply">
                                    <td id="tdsupply" runat="server">
                                        <asp:Label ID="lblemp" runat="server" class="tdstyle" Text="SUPPLIER NAME" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dsuppl" runat="server" Width="250px" TabIndex="9" CssClass="dropdown"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="dsuppl"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr runat="server" id="trfr">
                                    <td id="frdate" runat="server">
                                        <asp:Label ID="LBLFRDATE" runat="server" class="tdstyle" Text="From Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="TxtFRDate" runat="server" TabIndex="7"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtFRDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="trto" runat="server">
                                    <td runat="server" id="todate">
                                        <asp:Label ID="Label2" runat="server" class="tdstyle" Text="To Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="TxtTODate" runat="server" TabIndex="8"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtTODate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="trpusaseindent" runat="server" visible="false">
                                    <td id="td2" runat="server" class="tdstyle">
                                        <span class="labelbold">Purchase Order No</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddpurchaseint" runat="server" Width="250px" TabIndex="8" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddOrderno"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="Tr7" runat="server">
                                    <td colspan="4" align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                                        <asp:Button ID="btnNew" runat="server" Text="New" TabIndex="22" CssClass="buttonnorm"
                                            OnClientClick="return check();" OnClick="btnNew_Click" />
                                        <asp:Button ID="btnsybmit" runat="server" Text="Submit" TabIndex="22" CssClass="buttonnorm"
                                            OnClientClick="return check();" OnClick="btnsybmit_Click" />
                                        <asp:Button ID="btnpreview" runat="server" Visible="false" Text="Preview" TabIndex="22"
                                            CssClass="buttonnorm" OnClientClick="return check();" />
                                        <asp:Button ID="BtnExport" runat="server" Text="Export Excel" TabIndex="24" CssClass="buttonnorm"
                                            OnClick="BtnExport_Click" />
                                        <asp:Button CssClass="buttonnorm" ID="btnExcelExport" Text="Print Internal Fabric Sheet"
                                            runat="server" OnClick="btnExcelExport_Click" />
                                        <asp:Button CssClass="buttonnorm" ID="btnPOExport" Text="Print PO Order" Visible="false"
                                            OnClientClick="return check();" Width="160px" runat="server" OnClick="btnPOExport_Click" />
                                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                            TabIndex="23" CssClass="buttonnorm" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="width: 900px" runat="server" visible="false">
                        <td style="width: 50px" align="left">
                            <span class="labelbold">Department</span>
                        </td>
                        <td style="width: 25px" align="left">
                            <span class="labelbold">Customer Order No</span>
                        </td>
                        <td style="width: 100px" align="left">
                            <span class="labelbold">Description</span>
                        </td>
                        <td style="width: 100px" align="center">
                            <span class="labelbold">PHOTO</span>
                        </td>
                        <td style="width: 75px" align="center">
                            <span class="labelbold">JWDP</span>
                        </td>
                        <td style="width: 75px" align="center">
                            <span class="labelbold">QTY</span>
                        </td>
                        <td style="width: 75px" align="center">
                            <span class="labelbold">OrderDate</span>
                        </td>
                        <td style="width: 75px" align="center">
                            <span class="labelbold">SRCdate</span>
                        </td>
                        <td style="width: 75px" align="center">
                            <span class="labelbold">Expdate</span>
                        </td>
                        <td style="width: 75px" align="center">
                            <span class="labelbold">Del.Date</span>
                        </td>
                        <td style="width: 75px" align="center">
                            <span class="labelbold">Purchase Date</span>
                        </td>
                    </tr>
                    <tr id="trgrid2" runat="server" visible="false">
                        <td colspan="11" align="right" runat="server">
                            <div style="width: 100%; height: 400px; overflow: auto">
                                <asp:GridView ID="DGOrderDetail2" Width="100%" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="DGOrderDetail_RowDataBound" OnRowDeleting="DGOrderDetail_RowDeleting"
                                    ShowHeader="true" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="Department" HeaderText="Department">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Customer Order No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderid" runat="server" Visible="false" Text='<%# Bind("orderid") %>' />
                                                <asp:Label ID="lbljwdp" runat="server" Visible="false" Text='<%# Bind("JWDP") %>' />
                                                <asp:Label ID="lblorderdetailid" runat="server" Visible="false" Text='<%# Bind("orderdetailid") %>' />
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text='<%# Bind("CustomerOrderNo") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                            <ItemStyle HorizontalAlign="Left" Width="300px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="PHOTO">
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" Width="100px" Height="50px" runat="server" ImageUrl='<%# "~/ImageHandler.ashx?ID=" + Eval("orderdetailid")+"&img=4"%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="JWDP" HeaderText="JWDP">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="qty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="orderdate" HeaderText="OrderDate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SRCdate" HeaderText="SRCdate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Expdate" HeaderText="Expdate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Duedate" HeaderText="Del.Date">
                                            <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="purchasedate" HeaderText="Purchase Date">
                                            <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr runat="server" id="trgridconsumption" visible="false">
                        <td colspan="5" style="font-weight: bold">
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
                                        <asp:BoundField DataField="thanlength" HeaderText="thanlength">
                                            <HeaderStyle Width="50px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr runat="server" id="trgridpo" visible="false">
                        <td colspan="5" style="font-weight: bold">
                            <div style="height: 300px; overflow: auto">
                                <asp:GridView ID="DGOrderDetail3" Width="300px" runat="server" DataKeyNames="ID"
                                    AutoGenerateColumns="False" CssClass="grid-views">
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
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="OrderQty">
                                            <HeaderStyle />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="" HeaderText="RecOrderQty">
                                            <HeaderStyle />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="" HeaderText="CH.No">
                                            <HeaderStyle />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="" HeaderText="Balance Qty">
                                            <HeaderStyle />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark">
                                            <HeaderStyle />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr id="trgvdetail" runat="server" visible="false">
                        <td id="Td1" colspan="11" align="right" runat="server">
                            <div style="width: 100%; height: 400px; overflow: auto">
                                <asp:GridView ID="gvdetailorder" Width="100%" runat="server" AutoGenerateColumns="False"
                                    ShowHeader="true" CssClass="grid-views" OnRowDeleting="gvdetailorder_RowDeleting"
                                    OnRowUpdating="gvdetailorder_RowUpdating">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Deptt.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderid" runat="server" Visible="false" Text='<%# Bind("orderid") %>' />
                                                <asp:Label ID="lbljwdp" runat="server" Visible="false" Text='<%# Bind("JWDP") %>' />
                                                <asp:Label ID="lbldeptt" runat="server" Visible="true" Text='<%# getdeptt(DataBinder.Eval(Container.DataItem, "orderid").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Order No">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblcustorder" runat="server" Visible="true" Text='<%# Bind("CustomerOrderNo") %>' />--%>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Update"
                                                    Text='<%# Bind("CustomerOrderNo") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldiscription" runat="server" Visible="true" Text='<%# getdiscription(DataBinder.Eval(Container.DataItem, "orderid").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PHOTO">
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" Width="100px" Height="50px" runat="server" ImageUrl='<%# "~/ImageHandler.ashx?ID=" + Eval("orderid")+"&img=5"%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="JWDP" HeaderText="JWDP">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblvendor" runat="server" Visible="true" Text='<%# getvendor(DataBinder.Eval(Container.DataItem, "JWDP").ToString(),DataBinder.Eval(Container.DataItem, "orderid").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="orderdate" HeaderText="OrderDate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SRCdate" HeaderText="StoreSRCdate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Expdate" HeaderText="Expdate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Duedate" HeaderText="StoreDelDate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="purchasedate" HeaderText="FabricSRCDate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="qty" HeaderText="Ordered Pcs">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Received Pcs">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrecpcs" runat="server" Visible="true" Text='<%# getRecPcs(DataBinder.Eval(Container.DataItem, "orderid").ToString(),DataBinder.Eval(Container.DataItem, "JWDP").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pending Pcs">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpendpcs" runat="server" Visible="true" Text='<%# getpendPcs(DataBinder.Eval(Container.DataItem, "qty").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ordered Mtr">
                                            <ItemTemplate>
                                                <asp:Label ID="lblordermtr" runat="server" Visible="true" Text='<%# getordermtr(DataBinder.Eval(Container.DataItem, "orderid").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Receive Mtr">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrecmtr" runat="server" Visible="true" Text='<%# getrecmtr(DataBinder.Eval(Container.DataItem, "orderid").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pending Mtr">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPendmtr" runat="server" Visible="true" Text='<%# getpendmtr().ToString() %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fabric Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPstatus" runat="server" Visible="true" Text='<%# getpstatus(DataBinder.Eval(Container.DataItem, "orderid").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstatus" runat="server" Visible="true" Text='<%# getstatus(DataBinder.Eval(Container.DataItem, "status").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Production Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblprodstatus" runat="server" Visible="true" Text='<%# getprodstatus(DataBinder.Eval(Container.DataItem, "orderid").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Updated Fabric Status">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text='Detail'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hndesc" runat="server" />
                            <asp:HiddenField ID="hnrecqty" runat="server" />
                            <asp:HiddenField ID="hnordermtr" runat="server" />
                            <asp:HiddenField ID="hnrecmtr" runat="server" />
                            <asp:HiddenField ID="hnpstatus" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
