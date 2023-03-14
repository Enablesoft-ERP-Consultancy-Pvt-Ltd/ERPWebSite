<%@ Page Title="Dry Weight Percentage Master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="DryWeightPercentageMaster.aspx.cs" Inherits="Masters_Campany_DryWeightPercentageMaster"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closeForm() {

            var objParent = window.opener;
            if (objParent != null) {
                if (window.opener.document.getElementById('refreshdesign')) {
                    window.opener.document.getElementById('refreshdesign').click();
                    self.close();
                }

            }
            else {
                window.location.href = "../../main.aspx";
            }

        }
        function priview() {
            window.open("../../ReportViewer.aspx");
        }
        function KeyDownHandler(btn) {

            var objParent = window.opener;
            if (objParent != null) {
                btn = "usercontrol_btnsearch";
            }
            else {
                btn = "CPH_Form_usercontrol_btnsearch"
            }
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById(btn).click();
            }
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div style="margin: 1% 20% 0% 20%">
                <table>
                   <%-- <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="QualityName" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQualityName" runat="server" CssClass="textb" ValidationGroup="a"
                                Width="200px" Enabled="false"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Quality Name"
                                ControlToValidate="txtQualityName" ValidationGroup="a" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblDryWeightPercentage" runat="server" Text="Free%" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDryWeightPercentage" runat="server" CssClass="textb" ValidationGroup="a"
                                Width="200px"></asp:TextBox>                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="a" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();"
                                UseSubmitBehavior="False" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Lblerr" runat="server" ForeColor="Red"></asp:Label>
                            <div style="max-height: 500px; overflow: auto">
                                <asp:GridView ID="gdQuality" runat="server" Width="400px" AllowPaging="True" PageSize="100"
                                    AutoGenerateColumns="false" DataKeyNames="Sr_No" OnPageIndexChanging="gdQuality_PageIndexChanging"
                                    OnRowDataBound="gdQuality_RowDataBound" 
                                    OnInit="gdQuality_Init">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Free%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDryWeightPercentage" Text='<%#Bind("DryWeightPercentage") %>' runat="server" Visible="false" />
                                                <asp:TextBox ID="txtDryWeightPercentage" runat="server" Text='<%#Bind("DryWeightPercentage") %>' 
                                                OnTextChanged="txtDryWeightPercentage_TextChanged" AutoPostBack="True"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityId" Text='<%#Bind("QualityId") %>' runat="server" />
                                                <asp:Label ID="lblDWMID" Text='<%#Bind("DWMID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
