<%@ Page Title="Godown Master" Language="C#" AutoEventWireup="true" CodeFile="GodownMaster.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Campany_Term" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%; margin-top: 10px; height: 590px">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Width="0px" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Godown Name" runat="server" ID="label12" Font-Bold="true" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtGodawnName" runat="server" CssClass="textb" Width="300px">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Term"
                                ControlToValidate="txtGodawnName" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="ChkForCompanyGodown" runat="server" Text="For Company Godown" />
                        </td>
                        <td class="tdstyle" id="TDProcessEmployeeName" runat="server" visible="false">
                            <div style="height: 150px; width: 300px; overflow: scroll">
                                <asp:Label ID="LblProcessEmployeName" runat="server" Text="Process Employe Name"
                                    CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:CheckBoxList ID="ChkBoxListProcessEmployeName" runat="server" Width="400px"
                                    CssClass="checkboxnormal">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
                &nbsp;
                <table>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="LblErr" runat="server" Font-Bold="False" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="DGGodownMaster" runat="server" Width="280px" AllowPaging="True"
                                CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="DGGodownMaster_PageIndexChanging"
                                OnRowDataBound="DGGodownMaster_RowDataBound" AutoGenerateColumns="False" OnSelectedIndexChanged="DGGodownMaster_SelectedIndexChanged"
                                CssClass="grid-views" OnRowCreated="DGGodownMaster_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SrNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" Text='<%#Bind("SrNo") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Godown Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGodownName" Text='<%#Bind("GodownName") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CompanyGodown" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompanyGodown" Text='<%#Bind("CompanyGodown") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" Style="margin-left: 180px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();"
                                OnClick="btnclose_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
