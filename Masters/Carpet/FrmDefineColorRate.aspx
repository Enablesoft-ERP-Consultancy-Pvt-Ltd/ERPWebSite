<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmDefineColorRate.aspx.cs"
    Inherits="FrmDefineColorRate" MasterPageFile="~/ERPmaster.master" Title="Define Shade Rate"
    EnableEventValidation="false" %>

<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmDefineColorRate.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 60) {
                alert("Please Enter Only Numeric Value:");
                return false;
            }
            return true;
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <table style="width: 80%; margin-top: auto">
                    <tr>
                        <td>
                            <td align="right">
                                <asp:Label Text="Category" runat="server" ID="lblcategory" CssClass="labelbold" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddcategory" Width="200px" Height="20px" AutoPostBack="true"
                                    CssClass="dropdown" OnSelectedIndexChanged="ddcategory_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ErrorMessage="Please select Category " ControlToValidate="ddcategory"
                                    runat="server" ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                            </td>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Label ID="lblitem" runat="server" Text="Item Name" CssClass="labelbold" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlitem" runat="server" Height="20px" Width="200px" AutoPostBack="true"
                                        CssClass="dropdown" OnSelectedIndexChanged="ddlitem_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Please select Item Name "
                                        ControlToValidate="ddcategory" runat="server" ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Label ID="lblquality" runat="server" Text="Quality Name" CssClass="labelbold" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddquality" runat="server" Height="20px" Width="200px" AutoPostBack="true"
                                        CssClass="dropdown" OnSelectedIndexChanged="ddquality_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Please select Quality Name "
                                        ControlToValidate="ddcategory" runat="server" ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Label ID="lblshaded" runat="server" Text="Shade Color Name" CssClass="labelbold" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddshadecolor" runat="server" Height="20px" Width="200px" AutoPostBack="true"
                                        CssClass="dropdown" OnSelectedIndexChanged="ddshadecolor_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Please select Shade Color Name "
                                        ControlToValidate="ddcategory" runat="server" ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Label ID="lblrate" runat="server" Text="Rate" CssClass="labelbold" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtrate" CssClass="textb" onkeypress="return isNumberKey(event)" />
                                    <asp:RequiredFieldValidator ErrorMessage="Please Enter Rate" ControlToValidate="txtrate"
                                        runat="server" ID="rfv" ForeColor="red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <caption>
                                &nbsp;
                                <tr>
                                    <td align="right" colspan="4">
                                        <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                            Text="Save" Width="70px" />
                                        <asp:ValidationSummary ID="vs" runat="server" HeaderText="Following error occurs:"
                                            ShowMessageBox="true" ShowSummary="false" />
                                        <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" OnClientClick="return  NewForm();"
                                            Text="New" ValidationGroup="l" Width="70px" />
                                        <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" ValidationGroup="l"
                                            Width="70px" OnClientClick="return  CloseForm();" />
                                    </td>
                                </tr>
                </table>
                <table style="margin: 5%; width: 70%">
                    <tr>
                        <td style="height: 40%; width: auto">
                        </td>
                        <td>
                            <asp:Label ID="lblmsg" runat="server" ForeColor="red" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <div style="overflow: auto; width: 100%; height: 250px">
                                <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" EmptyDataText="No records has been added."
                                    OnRowEditing="gv_RowEditing" OnRowCancelingEdit="gv_RowCancelingEdit" OnRowUpdating="gv_RowUpdating"
                                    DataKeyNames="ID" OnRowDeleting="gv_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SrNo.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QualityName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityname" Text='<%#Bind("qualityname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shadecolor">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShadecolorName" Text='<%#Bind("ShadecolorName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" Text='<%#Eval("Rate") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <%#Eval("Rate") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtrate" Text='<%#Eval("Rate") %>' runat="server" Width="70px" BackColor="Yellow" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dateadded">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateadded" Text='<%#Bind("Dateadded") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityId" Text='<%#Bind("Quality_Id") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblshadecolorid" Text='<%#Bind("ShadedColor_id") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowEditButton="True" ValidationGroup="aa" />
                                        <asp:CommandField ShowDeleteButton="True" ValidationGroup="aa" />
                                    </Columns>
                                    <HeaderStyle BackColor="#8B7B8B" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <%--<tr>
                                <td>
                                    <asp:Label ID="lblmsg" runat="server" />
                                </td>
                            </tr>--%>
                    </tr> </caption> </td> </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
