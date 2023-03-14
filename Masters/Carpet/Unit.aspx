<%@ Page Title="Unit" Language="C#" AutoEventWireup="true" CodeFile="Unit.aspx.cs"
    Inherits="Masters_Campany_Unit" MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div style="height: 480px">
                <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                <table style="width: 50%; margin: auto">
                    <tr align="left">
                        <td class="tdstyle">
                            <asp:Label Text="Unit Type" runat="server" ID="labelb" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddUnit" runat="server" CssClass="dropdown" Width="43%">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddUnit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <caption>
                            <br />
                        </caption>
                    </tr>
                    <caption>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <tr align="left">
                            <td class="tdstyle">
                                <asp:Label ID="label1" runat="server" Font-Bold="true" Text=" Unit" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtUnit" runat="server" CssClass="textb" ValidationGroup="f1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUnit"
                                    ErrorMessage="Please Enter  Unit Name" ForeColor="Red" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <td>
                                    &nbsp;
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="LblError" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gdUnit" runat="server" AllowPaging="True" CellPadding="4" CssClass="grid-views"
                                    DataKeyNames="Sr_No" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gdUnit_PageIndexChanging" OnRowCreated="gdUnit_RowCreated"
                                    OnRowDataBound="gdUnit_RowDataBound" OnSelectedIndexChanged="gdUnit_SelectedIndexChanged"
                                    PageSize="6" Width="280px">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <Columns>
                                        <asp:BoundField DataField="Sr_No" HeaderText="SrNo">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UnitName" HeaderText="UnitName">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UnitType" HeaderText="UnitType">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    OnClientClick="return confirm('Do you want to save data?')" Text="Save" ValidationGroup="f1" />
                                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                    Text="Close" />
                            </td>
                        </tr>
                    </caption>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
