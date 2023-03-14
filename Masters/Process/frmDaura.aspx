<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmDaura.aspx.cs" Inherits="Masters_Process_frmDaura"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Title="Daura" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script  type="text/javascript">
        function ReloadForm() {
            window.location.href = "frmDaura.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <div style="width: 900px; height: auto">
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblSearch" runat="server" Text="Search" Font-Bold="true"></asp:Label></legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCompanyName" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDCompanyName" runat="server" CssClass="dropdown" Width="200px"
                                        OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblCustomerCode" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDCustomercode" runat="server" CssClass="dropdown" Width="150px"
                                        OnSelectedIndexChanged="DDCustomercode_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblOrderNo" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDOrderNo" runat="server" CssClass="dropdown" Width="150px"
                                        OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblWeaverName" runat="server" Text="Weaver" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDWeaver" runat="server" CssClass="dropdown" Width="200px"
                                        OnSelectedIndexChanged="DDWeaver_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPoNo" runat="server" Text="PO No." CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDPoNo" runat="server" CssClass="dropdown" Width="150px"
                                        OnSelectedIndexChanged="DDPoNo_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblQuality" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblDesign" runat="server" Text="DesignName" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="labelbold"></asp:Label><br />
                                    <asp:TextBox ID="txtDate" runat="server" Width="100px" BackColor="Yellow"> </asp:TextBox>
                                    <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txtDate" Format="dd-MMM-yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnShowDetail" runat="server" Text="Show Detail" CssClass="buttonnorm"
                                        Width="100px" OnClick="btnShowDetail_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div style="margin-left: 270px; width: 250px; margin-top: 10px">
                    <table width="200px" style="visibility: hidden">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RdSummary" runat="server" Text="Summary" Font-Bold="true" GroupName="m" />
                            </td>
                            <td>
                                <asp:RadioButton ID="RdDetail" runat="server" Text="Detail" Font-Bold="true" GroupName="m" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 900px">
                    <div>
                        <div style="overflow: auto; width: 860px; height: 300px; margin-left: 10px">
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lblloomDetails" runat="server" Text="Loom Summary Details" Font-Bold="true"></asp:Label></legend>
                                <asp:GridView ID="gdLoomDetail" runat="server" AutoGenerateColumns="false" Width="850px">
                                    <HeaderStyle CssClass="gvheader" Height="25px" HorizontalAlign="Center" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkitem" runat="server" Width="50px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNo" runat="server" Text='<%#Bind("PoNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weaver/Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWeaver" runat="server" Text='<%#Bind("Weaver") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("QualityName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Design">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RecQty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRecQty" runat="server" Text='<%#Bind("RecQty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BalQty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalQty" runat="server" Text='<%#Bind("BalQty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Looms">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLooms" runat="server" Text='<%#Bind("Loom") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtremark" runat="server" Width="250px" TextMode="MultiLine">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Running Looms">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRunningLoom" runat="server" Width="50px" BackColor="Yellow"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssueOrderid" runat="server" Text='<%#Bind("IssueOrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoomDetailId" runat="server" Text='<%#Bind("LoomDetailId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QualityId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityId" runat="server" Text='<%#Bind("QualityId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="Design">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesignId" runat="server" Text='<%#Bind("DesignId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="SizeId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSizeId" runat="server" Text='<%#Bind("SizeId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </div>
                        <div style="width: 860px">
                            <table width="860px">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnNew" runat="server" Text="New" CssClass="buttonnorm" Width="75px" OnClientClick="return ReloadForm();" />
                                        &nbsp;
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonnorm" Width="75px"
                                            OnClick="btnSave_Click" />
                                        &nbsp;
                                          <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonnorm" Width="75px"
                                           OnClientClick="return CloseForm();"  />
                                        &nbsp;
                                        <asp:Button ID="btnPrint" runat="server" Text="Preview" CssClass="buttonnorm" Width="75px"
                                            OnClick="btnPrint_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
