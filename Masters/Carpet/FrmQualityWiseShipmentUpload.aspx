<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmQualityWiseShipmentUpload.aspx.cs"
    Inherits="Masters_Carpet_FrmQualityWiseShipmentUpload" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function addpriview() {
            window.open("../../ReportViewer.aspx");
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%;">
        <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="False"></asp:TextBox>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table style="width: 50;">
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddBuyerCode" runat="server" CssClass="dropdown" Width="165px" AutoPostBack="true" OnSelectedIndexChanged="ddBuyerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategory" runat="server" Text="CATEGORY" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged" Width="165px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" PromptCssClass="labelbold"
                                PromptPosition="Bottom" TargetControlID="ddlcategory" ViewStateMode="Disabled">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="ITEM NAME" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDMasterQuality" runat="server" Width="165px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDMasterQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDMasterQuality"
                                ErrorMessage="*" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDMasterQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblQualityName" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" Width="165px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DDQuality"
                                ErrorMessage="*" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtDescription" runat="server" Width="200px" Height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Description"
                                ControlToValidate="txtDescription" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" runat="server" Text="Shipment" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtShipment" runat="server" Width="200px" Height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter Shipment"
                                ControlToValidate="txtShipment" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" runat="server" Text="Upload" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtUpload" runat="server" Width="200px" Height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Upload"
                                ControlToValidate="txtUpload" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                     <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="Net Wt (Kg)" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtNetWt" runat="server" Width="200px"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Upload"
                                ControlToValidate="txtUpload" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnsave" runat="server" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="m" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="btnclose" runat="server" Text="Close"
                                OnClientClick="return CloseForm()" />
                            <%--&nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="Button1" runat="server"
                                Text="Preview" OnClick="Button1_Click" />--%>
                            <%--&nbsp;<asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />--%>
                        </td>
                    </tr>
                </table>
                <table style="margin-left: 80px">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div style="overflow: auto; max-height: 400px; width: 500px">
                                <asp:GridView ID="GDQualityShipment" runat="server" AllowPaging="True" PageSize="15" DataKeyNames="id" AutoGenerateColumns="false"
                                    OnPageIndexChanging="GDQualityShipment_PageIndexChanging" OnRowDataBound="GDQualityShipment_RowDataBound"
                                    OnSelectedIndexChanged="GDQualityShipment_SelectedIndexChanged" CaptionAlign="Left" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                         <asp:TemplateField HeaderText="Customer Code" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerCode" Text='<%#Bind("CustomerCode") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Quality Type" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityType" Text='<%#Bind("Item_Name") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Quality" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Description" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQDescription" Text='<%#Bind("QDescription") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Shipment" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQShipment" Text='<%#Bind("QShipment") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Upload" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQUpload" Text='<%#Bind("QUpload") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Net Wt(kg)" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQNetWtInKg" Text='<%#Bind("QNetWtInKg") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" Text='<%#Bind("id") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                       
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
