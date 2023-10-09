<%@ Page Language="C#" Title="FinisherJobRate" AutoEventWireup="true" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" CodeFile="FrmFinisherJobRate.aspx.cs" Inherits="Masters_Carpet_FrmFinisherJobRate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmFinisherJobRate.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; float: left; background-color: #E3E3E3">
                <div style="width: 100%; float: left; background-color: #E3E3E3">
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label21" Text="Branch Name" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:DropDownList ID="DDBranchName" AutoPostBack="true" runat="server" Width="150px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDBranchName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label9" Text="Job Type" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:DropDownList ID="DDJobType" AutoPostBack="true" runat="server" Width="150px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDJobType_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%-- <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDJobType"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                                <%-- <asp:TextBox ID="TxtRecDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtRecDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>--%>
                            </td>
                            <td id="TDCustomercode" runat="server">
                                <asp:Label ID="Label5" Text="Customer Code" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:DropDownList ID="DDCustomerCode" AutoPostBack="true" runat="server" Width="150px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%-- <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCustomerCode"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                            </td>
                            <td class="tdstyle" id="TDEmployeeName" runat="server" visible="false">
                                <asp:Label ID="Label19" Text="Employee Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDEmployeeName" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--<cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDQualityType"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label6" Text="Quality Type" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:DropDownList ID="DDQualityType" runat="server" AutoPostBack="true" CssClass="dropdown"
                                    Width="150px" OnSelectedIndexChanged="DDQualityType_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--<cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDQualityType"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label7" Text="Quality" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="true" Width="150px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%-- <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label8" Text="Design" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="true" Width="150px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--  <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="DDDesign"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" Text="Color" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDColor" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="tdShape" runat="server">
                                <asp:Label ID="Label13" Text="Shape" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDShape" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label2" Text="Size" runat="server" CssClass="labelbold" />
                                <asp:CheckBox ID="chkForFtSize" runat="server" Text="For Ft" Font-Bold="true" AutoPostBack="true"
                                    OnCheckedChanged="chkForFtSize_CheckedChanged" />
                                <asp:CheckBox ID="ChkForInchSize" runat="server" Text="For Inch" Font-Bold="true"
                                    Visible="false" AutoPostBack="true" OnCheckedChanged="ChkForInchSize_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="DDSize" AutoPostBack="true" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                                <%--<cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDSize"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label3" Text="Calc Option" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:DropDownList ID="DDCalcOption" runat="server" AutoPostBack="true" CssClass="dropdown"
                                    Width="150px" OnSelectedIndexChanged="DDCalcOption_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--<cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDCalcOption"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                            </td>
                            <td class="tdstyle" runat="server" id="TDRateFtMtr" visible="false">
                                <asp:Label ID="Label15" Text="Unit" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDUnit" runat="server" Width="90px">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" runat="server" id="TDratelocation" visible="false">
                                <asp:Label ID="Label14" Text="Rate Location" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDratelocation" runat="server" CssClass="dropdown" Width="150px">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    <asp:ListItem Value="0">InHouse</asp:ListItem>
                                    <asp:ListItem Value="1">OutSide</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label4" Text="Rate" runat="server" CssClass="labelbold" /><span style="color: Red">*</span>
                                <br />
                                <asp:TextBox ID="txtRate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            </td>
                            <td class="tdstyle" id="TDRate2" runat="server">
                                <asp:Label ID="Label12" Text="Rate2" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtRate2" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label10" Text="Re Iss.Rate" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtReIssRate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            </td>
                             <td class="tdstyle" id="TDBonusRate" runat="server" visible="false">
                                <asp:Label ID="Label20" Text="Bonu Rate" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtBonusRate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            </td>

                            <td class="tdstyle">
                                <asp:Label ID="Label11" Text="Effective Date" runat="server" CssClass="labelbold" /><span
                                    style="color: Red">*</span>
                                <br />
                                <asp:TextBox ID="txtEffectiveDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEffectiveDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label16" Text="Remark" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtRemark" runat="server" Width="450px" CssClass="textb"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <table width="80%">
                    <tr>
                        <td colspan="6" align="right">
                            <asp:Label ID="llMessageBox" runat="server" Text="" ForeColor="Red"></asp:Label>
                            &nbsp
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"
                                OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" />
                            <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                Width="90px" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <div style="height: 250px; overflow: auto; width: 100%;">
                                <asp:GridView ID="GVFinisherJobRate" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    OnSelectedIndexChanged="GVFinisherJobRate_SelectedIndexChanged" DataKeyNames="RateId"
                                    OnRowDataBound="GVFinisherJobRate_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityType" runat="server" Text='<%#Bind("QualityType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityName" runat="server" Text='<%#Bind("QualityName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="400px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Design" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblColor" runat="server" Text='<%#Bind("ColorName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shape" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShape" runat="server" Text='<%#Bind("Shape") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubJobType" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubJobType" runat="server" Text='<%#Bind("JobType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cust Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcustcode" runat="server" Text='<%#Bind("customercode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CalcOption">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCalcOption" runat="server" Text='<%#Bind("CalcName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate2">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate2" runat="server" Text='<%#Bind("Rate2") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#Bind("EffectiveDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ReIssRate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReIssRate" runat="server" Text='<%#Bind("ReIssRate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRateId" runat="server" Text='<%# Bind("RateId") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblratelocation" runat="server" Text='<%# Bind("Ratelocation") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitName" runat="server" Text='<%# Bind("UnitName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EmpCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Bind("EmpCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Bonus" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBonus" runat="server" Text='<%# Bind("Bonus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkdel" runat="server" Text="Del" OnClientClick="return confirm('Do you Want to delete this row?')"
                                                    OnClick="lnkdelClick"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="8">
                            <table>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label17" Text="From Date" runat="server" CssClass="labelbold" /><span
                                            style="color: Red">*</span>
                                        <br />
                                        <asp:TextBox ID="txtFromDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFromDate" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label18" Text="To Date" runat="server" CssClass="labelbold" /><span
                                            style="color: Red">*</span>
                                        <br />
                                        <asp:TextBox ID="txtToDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtToDate" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <br />
                                        <asp:Button CssClass="buttonnorm" ID="BtnPreviewBetweenDate" runat="server" Text="Preview"
                                            OnClick="BtnPreviewBetweenDate_Click" Width="90px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnRateId" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
