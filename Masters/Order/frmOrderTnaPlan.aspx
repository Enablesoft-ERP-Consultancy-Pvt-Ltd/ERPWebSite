<%@ Page Title="T&A PLAN" Language="C#" AutoEventWireup="true" CodeFile="frmOrderTnaPlan.aspx.cs"
    Inherits="Masters_Order_frmOrderTnaPlan" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmOrderTnaPlan.aspx";
        }
        function validate() {
            var ddcompany = document.getElementById("<%=DDCompanyName.ClientID %>");
            var ddcustomer = document.getElementById("<%=DDCustomer.ClientID %>");
            var ddorderno = document.getElementById("<%=DDOrderNo.ClientID %>");
            var msg = "";
            if (ddcompany.value == "") {
                msg = msg + 'Please select CompanyName!!!\n';
            }
            if (ddcustomer.value <= "0" || ddcustomer.value == "") {
                msg = msg + 'Please select CustomerName!!!\n';
            }
            if (ddorderno.value <= "0" || ddorderno.value == "") {
                msg = msg + 'Please select OrderNo!!!\n';
            }
            if (msg == "") {
                return true;
            }
            else {
                alert(msg);
                return false;
            }


        }
        function selectSinglecheckbox() {
            var isValid = false;
            var j = 0;
            var gridView = document.getElementById("<%=GVOrderDetail.ClientID %>");
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            j = j + 1;

                            if (j > 1) {
                                alert("Please Select Only One Item");
                                inputs[0].checked = false;
                                return false;
                            }
                        }
                    }
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="Updatepnl1" runat="server">
        <ContentTemplate>
            <div>
                <div style="margin-left: 10px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblCompName" Text="CompanyName" runat="server" CssClass="labelnormalMM" />
                                <br />
                                <asp:DropDownList ID="DDCompanyName" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblcustomer" Text="Customer Name" runat="server" CssClass="labelnormalMM" />
                                <br />
                                <asp:DropDownList ID="DDCustomer" Width="150px" CssClass="dropdown" runat="server"
                                    OnSelectedIndexChanged="DDCustomer_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblOrderNO" Text="P.O.#" runat="server" CssClass="labelnormalMM" />
                                <br />
                                <asp:DropDownList ID="DDOrderNo" Width="150px" CssClass="dropdown" runat="server"
                                    OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div style="clear: both">
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <div style="overflow: auto; max-height: 250px">
                                        <asp:GridView ID="GVOrderDetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="GVOrderDetail_RowDataBound">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkItem" CssClass="checkboxnormal" Text="" runat="server" AutoPostBack="true"
                                                            OnCheckedChanged="chkItem_CheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ItemFinishedid" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemfinishedid" Text='<%#Bind("Item_Finished_id") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="orderdetailid" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblorderdetailid" Text='<%#Bind("orderdetailid") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Item Description" DataField="ItemDescription" ItemStyle-CssClass="labelnormal" />
                                                <asp:BoundField HeaderText="Quantity" DataField="QtyRequired" ItemStyle-CssClass="labelnormal" />
                                                <asp:BoundField HeaderText="Order Date" DataField="Orderdate" ItemStyle-CssClass="labelnormal" />
                                                <asp:BoundField HeaderText="Dispatch Date" DataField="dispatchdate" ItemStyle-CssClass="labelnormal" />
                                                <asp:TemplateField HeaderText="Rawtnaorderdetailid" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRawtnaorderdetailid" Text='<%#Bind("Rawtnaorderdetailid") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-bottom: 5px">
                        <div style="background-color: #999999">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkRawMat" runat="server" Text=" Rawmaterials and  Accessories T&A"
                                            Font-Bold="true" ForeColor="White" CssClass="labelbold" OnCheckedChanged="chkRawMat_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="margin-left: 10px; margin-top: 5px; max-height: 200px; overflow: auto"
                            runat="server" id="divRawmat" visible="false">
                            <asp:GridView ID="GVRaw" runat="server" AutoGenerateColumns="False" OnRowDataBound="GVRaw_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkrawmat" CssClass="checkboxnormal" Text="" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" Text='<%#Bind("ItemName") %>' CssClass="labelnormalMM"
                                                Width="250px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Target Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtrawtargetdate" Width="90px" runat="server" CssClass="textb" ToolTip="Click to set Date"
                                                Text='<%#Bind("TargetDate") %>' />
                                            <cc1:CalendarExtender ID="cc1rawTDate" TargetControlID="txtrawtargetdate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revised Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtrawrevisedDate" Width="90px" runat="server" CssClass="textb"
                                                ToolTip="Click to set Date" Text='<%#Bind("RevisedDate") %>' />
                                            <cc1:CalendarExtender ID="cc1rawRDate" TargetControlID="txtrawrevisedDate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtrawactualdate" Width="90px" runat="server" CssClass="textb" ToolTip="Click to set Date"
                                                Text='<%#Bind("ActualDate") %>' />
                                            <cc1:CalendarExtender ID="cc1rawADate" TargetControlID="txtrawactualdate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revised Remark">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtrawremark" Width="180px" runat="server" CssClass="textb" BackColor="Yellow"
                                                Text='<%#Bind("Remark") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ItemFinishedid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfinishedid" Text='<%#Bind("Item_finished_id") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RPFinishedid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrpfinishedid" Text='<%#Bind("RPFinishedid") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div style="margin-bottom: 5px">
                        <div style="background-color: #999999">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="ChkProduction" runat="server" Text=" Production T&A" Font-Bold="true"
                                            ForeColor="White" CssClass="labelbold" OnCheckedChanged="ChkProduction_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="margin-left: 10px; margin: 5px 0px 5px 10px; max-height: 150px; overflow: auto"
                            runat="server" id="divProduction" visible="false">
                            <asp:GridView ID="GVProduction" runat="server" AutoGenerateColumns="False" OnRowDataBound="GVProduction_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkprod" CssClass="checkboxnormal" Text="" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Process Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblprocess" Text='<%#Bind("Process_Name") %>' CssClass="labelnormalMM"
                                                runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Target Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtprodtargetdate" Width="90px" runat="server" CssClass="textb"
                                                ToolTip="Click to set Date" Text='<%#Bind("TargetDate") %>' />
                                            <cc1:CalendarExtender ID="cc1prodTDate" TargetControlID="txtprodtargetdate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revised Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtprodrevisedDate" Width="90px" runat="server" CssClass="textb"
                                                ToolTip="Click to set Date" Text='<%#Bind("RevisedDate") %>' />
                                            <cc1:CalendarExtender ID="cc1prodRDate" TargetControlID="txtprodrevisedDate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtprodactualdate" Width="90px" runat="server" CssClass="textb"
                                                ToolTip="Click to set Date" Text='<%#Bind("ActualDate") %>' />
                                            <cc1:CalendarExtender ID="cc1rawADate" TargetControlID="txtprodactualdate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revised Remark">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtprodremark" Width="180px" runat="server" CssClass="textb" BackColor="Yellow"
                                                Text='<%#Bind("remark") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="processid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblprocessid" Text='<%#Bind("process_Name_id") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PTNAAprocessid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPtnaprocessid" Text='<%#Bind("PTnaProcessid") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div style="margin-bottom: 5px">
                        <div style="background-color: #999999">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkinspection" runat="server" Text=" Inspection T&A" Font-Bold="true"
                                            ForeColor="White" CssClass="labelbold" OnCheckedChanged="chkinspection_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="margin-left: 10px; margin-top: 10px; height: auto" runat="server" id="divinsp"
                            visible="false">
                            <asp:GridView ID="GVInspection" runat="server" AutoGenerateColumns="False" OnRowDataBound="GVInspection_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkinsp" CssClass="checkboxnormal" Text="" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inspection Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblinspectiontype" Text='<%#Bind("InspectionType") %>' CssClass="labelnormalMM"
                                                runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Target Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInspectiontargetdate" Width="90px" runat="server" CssClass="textb"
                                                ToolTip="Click to set Date" Text='<%#Bind("TargetDate") %>' />
                                            <cc1:CalendarExtender ID="cc1prodTDate" TargetControlID="txtInspectiontargetdate"
                                                Format="dd-MMM-yyyy" runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revised Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInspectionrevisedDate" Width="90px" runat="server" CssClass="textb"
                                                ToolTip="Click to set Date" Text='<%#Bind("RevisedDate") %>' />
                                            <cc1:CalendarExtender ID="cc1prodRDate" TargetControlID="txtInspectionrevisedDate"
                                                Format="dd-MMM-yyyy" runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInspectionactualdate" Width="90px" runat="server" CssClass="textb"
                                                ToolTip="Click to set Date" Text='<%#Bind("ActualDate") %>' />
                                            <cc1:CalendarExtender ID="cc1rawADate" TargetControlID="txtInspectionactualdate"
                                                Format="dd-MMM-yyyy" runat="server">
                                            </cc1:CalendarExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revised Remark">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInspectionremark" Width="180px" runat="server" CssClass="textb"
                                                BackColor="Yellow" Text='<%#Bind("remark") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="inspectionId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblinspection" Text='<%#Bind("Id") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="inspectionIdTna" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblinspectionIdtna" Text='<%#Bind("inspectionIdtna") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div style="width: 70%;">
                    <div style="float: right">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnnew" Text="New" CssClass="buttonnorm" runat="server" OnClientClick="NewForm();" />
                                </td>
                                <td>
                                    <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClientClick="return validate();"
                                        OnClick="btnsave_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="CloseForm();" />
                                </td>
                                <td>
                                    <asp:Button ID="btnpreview" Text="Preview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
            </Triggers>       
    </asp:UpdatePanel>
</asp:Content>
