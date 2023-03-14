<%@ Page Title="JobRateDefine" Language="C#" AutoEventWireup="true" CodeFile="JobRateDefine.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_Process_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Form" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function reload() {
            window.location.href = "JobRateDefine.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
        }
    
    </script>
    <div>
        <asp:UpdatePanel ID="Updatepanal1" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="TxtId" runat="server" Visible="false"></asp:TextBox>
                <table>
                    <tr>
                        <td class="tdstyle">
                            Company Name
                        </td>
                        <td class="tdstyle">
                            Process Name
                        </td>
                        <td class="tdstyle">
                            Process Type
                        </td>
                        <td class="tdstyle">
                            Employee
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            Unit
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="DDCompany" runat="server" Width="120px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCompany"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDProcess" runat="server" Width="120px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="DDProcess_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDProcess"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDProcessType" runat="server" Width="120px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="DDProcessType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDProcessType"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDEmployeeName" runat="server" AutoPostBack="True" Width="120px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDEmployeeName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" Width="120px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDItemName" runat="server" AutoPostBack="True" Width="120px"
                                CssClass="dropdown" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDItemName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDUnit" runat="server" AutoPostBack="True" Width="120px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="DDUnit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdQuality" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality "></asp:Label>
                        </td>
                        <td id="tdDesign" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>
                            &nbsp;<asp:CheckBox ID="CHKDesign" runat="server" AutoPostBack="True" Text="All"
                                OnCheckedChanged="CHKDesign_CheckedChanged" />
                        </td>
                        <td id="tdColor" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>
                            &nbsp;<asp:CheckBox ID="CHKColor" runat="server" AutoPostBack="True" Text="All" OnCheckedChanged="CHKColor_CheckedChanged" />
                        </td>
                        <td id="tdShape" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                        </td>
                        <td id="tdSize" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblsizename" runat="server" Text="Size"></asp:Label>
                            &nbsp;<asp:CheckBox ID="CHKSize" runat="server" AutoPostBack="True" Text="All" OnCheckedChanged="CHKSize_CheckedChanged" />
                        </td>
                        <td class="tdstyle">
                            Rate
                        </td>
                    </tr>
                    <tr>
                        <td id="tdQ" runat="server" visible="false">
                            <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" Width="120px"
                                CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="tdD" runat="server" visible="false">
                            <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" Width="120px"
                                CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDDesign"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="tdC" runat="server" visible="false">
                            <asp:DropDownList ID="DDColor" runat="server" AutoPostBack="True" Width="120px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDColor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="tdSh" runat="server" visible="false">
                            <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" Width="120px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDShape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="tdsz" runat="server" visible="false" class="tdstyle">
                            <asp:DropDownList ID="DDSize" runat="server" Width="70px" AutoPostBack="True" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:CheckBox ID="ChMtr" runat="server" AutoPostBack="True" Text="Mtr" CssClass="checkboxnormal"
                                OnCheckedChanged="ChMtr_CheckedChanged" />
                            <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDSize"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRate" runat="server" Width="110px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="style2">
                            <b>
                                <asp:Label ID="LblError" ForeColor="Red" runat="server" Text=""></asp:Label></b>
                        </td>
                        <td colspan="3" align="right" class="style2">
                            <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="reload();" />
                            <asp:Button ID="BtnPreview" runat="server" OnClientClick="report();" CssClass="buttonnorm"
                                Text="Preview" Enabled="false" />
                            <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div style="width: 100%; height: 300px; overflow: scroll">
                                <asp:GridView ID="DGJobeRate" runat="server" AutoGenerateColumns="true" DataKeyNames="SrNo"
                                    OnRowDataBound="DGJobeRate_RowDataBound" OnSelectedIndexChanged="DGJobeRate_SelectedIndexChanged"
                                    CssClass="grid-view" OnRowCreated="DGJobeRate_RowCreated">
                                    <HeaderStyle CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
