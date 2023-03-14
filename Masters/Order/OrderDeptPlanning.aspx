<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderDeptPlanning.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Order_OrderDeptPlanning"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function validator() {
            if (document.getElementById("<%=tdname.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDNAME').value <= "0") {
                    alert("Please Select Quality Name....!");
                    document.getElementById("CPH_Form_DDNAME").focus();
                    return false;
                }
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table width="400px">
                <tr>
                    <td class="tdstyle">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="LblOrderNo" runat="server" Text="Order No" CssClass="labelbold"></asp:Label><b
                                        style="color: Red"></b>
                                    <br />
                                    <asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td runat="server" id="tdname">
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDNAME" runat="server" Width="100px">
                                        <asp:ListItem Value="0">-Select-</asp:ListItem>
                                        <asp:ListItem Value="1">Nt</asp:ListItem>
                                        <asp:ListItem Value="2">Mh</asp:ListItem>
                                        <asp:ListItem Value="3">Har</asp:ListItem>
                                        <asp:ListItem Value="4">NM</asp:ListItem>
                                        <asp:ListItem Value="5">NH</asp:ListItem>
                                        <asp:ListItem Value="6">HM</asp:ListItem>
                                        <asp:ListItem Value="7">NHM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <div style="width: auto; height: 200px; overflow: auto">
                            <asp:GridView ID="DG" runat="server" AutoGenerateColumns="False" OnRowDataBound="DG_RowDataBound"
                                CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="Description" HeaderText="OrderDescription">
                                        <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                    <td id="tdpurchase" runat="server">
                        <div style="width: auto; height: 200px; overflow: auto">
                            <asp:GridView ID="DGPurchase" runat="server" AutoGenerateColumns="False" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="Description" HeaderText="OrderDescription">
                                        <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr align="left">
                    <td colspan="2">
                        <div style="width: auto; height: 200px; overflow: auto">
                            <asp:GridView ID="DGDeptPlan" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                Width="600px" CssClass="grid-view">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="OrderNo" HeaderText="Order No">
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProcessName" HeaderText="Process">
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Date" HeaderText="Req.Date">
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Qty" DataField="Qty">
                                        <HeaderStyle Width="75px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="FinalDate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtProcessReqDate" Text='<%# Bind("FinalDate") %>' runat="server"
                                                Format="dd-MMM-yyyy" Width="120px" Enabled="false" CssClass="textb"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtProcessReqDate">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="Lblprocessid" runat="server" Text='<%# Bind("Processid") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="remark" HeaderText="PlanRemark"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:TextBox ID="Txtremark" Text='<%# Bind("depremark") %>' runat="server" Width="120px"
                                                CssClass="textb"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr align="right">
                    <td>
                        <asp:Button CssClass="buttonnorm" ID="BtnSave" Visible="false" Text="Save" runat="server"
                            OnClick="BtnSave_Click" OnClientClick="return validator();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red" Visible="false"></asp:Label>
                        <asp:Label ID="Lblsave" runat="server" CssClass="labelbold" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
