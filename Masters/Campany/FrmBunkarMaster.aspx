<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmBunkarMaster.aspx.cs"
    Inherits="Masters_Campany_FrmBunkarMaster" EnableEventValidation="false" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <style type="text/css">
        #newPreview
        {
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }
        .textbox
        {
        }
    </style>
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "FrmBunkarMaster.aspx";
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }
        
    </script>
    <%--Page Design--%>
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table width="100%" border="1">
                <tr>
                    <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                        <div id="1" style="height: auto" align="left">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtBunkarId" runat="server" Style="display: none"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle" colspan="2">
                                        <asp:Label ID="Label7" Text="Contractor Name" runat="server" CssClass="label" />
                                    </td>
                                    <td class="style6">
                                        <asp:DropDownList CssClass="dropdown" ID="DDContractorName" runat="server" Width="200px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDContractorName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDContractorName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label8" Text="Bunkar Name" runat="server" CssClass="label" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBunkarName" runat="server" CssClass="textb" Width="200px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label9" Text="Father Name" runat="server" CssClass="label" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFatherName" runat="server" CssClass="textb" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle" colspan="2">
                                        <asp:Label ID="Label4" Text="Address" runat="server" CssClass="label" />
                                    </td>
                                    <td class="style6">
                                        <asp:TextBox CssClass="textb" ID="txtAddress" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label5" Text="Mobile No" runat="server" CssClass="label" />
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="txtMobileNo" runat="server" Width="200px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label6" Text="Joining Date" runat="server" CssClass="label" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtJoiningDate" CssClass="textb" Width="100px" runat="server" />
                                        <asp:CalendarExtender ID="calfrom" TargetControlID="txtJoiningDate" runat="server"
                                            Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ChkBunkarBlackList" runat="server" Text="Bunkar Black List" CssClass="checkboxnormal" />
                    </td>
                    <td class="style6">
                        <asp:Label ID="lblErr1" runat="server" CssClass="errormsg" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblerr" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="text-align: right" colspan="5">
                        <asp:Button ID="btnnew0" runat="server" CssClass="buttonnorm" OnClientClick="NewForm();"
                            TabIndex="31" Text="New" Width="70px" />
                        <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CssClass="buttonnorm"
                            ValidationGroup="s" OnClientClick="return confirm('Do you want to save data?')"
                            Width="70px" />
                        <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" OnClick="BtnPreview_Click"
                            Text="Preview" Visible="false" />
                        <asp:Button ID="Button1" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();"
                            TabIndex="33" Text="Close" Width="70px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <div style="width: 1000px; height: 200px; overflow: auto">
                            <asp:GridView ID="GVBunkarMaster" runat="server" Width="864px" CellPadding="4" PageSize="50"
                                ForeColor="#333333" DataKeyNames="BMID" AllowPaging="true" OnPageIndexChanging="GVBunkarMaster_PageIndexChanging"
                                OnRowDataBound="GVBunkarMaster_RowDataBound" OnSelectedIndexChanged="GVBunkarMaster_SelectedIndexChanged"
                                TabIndex="33" AutoGenerateColumns="False" OnRowDeleting="GVBunkarMaster_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <PagerSettings NextPageText="Next" PreviousPageText="Prev" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Contractor Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContractorName" Text='<%#Bind("ContractorName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bunkar Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBunkarName" Text='<%#Bind("BunkarName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Father Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBunkarFatherName" Text='<%#Bind("BunkarFatherName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bunkar Address">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBunkarAddress" Text='<%#Bind("BunkarAddress") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bunkar Mobile No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBunkarMobileNo" Text='<%#Bind("BunkarMobileNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Joining Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBunkarJoiningDate" Text='<%#Bind("BunkarJoiningDate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBunkarStatus" Text='<%#Bind("BunkarStatus") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            </div> </td> </tr> </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
