<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddQuality.aspx.cs" Inherits="Masters_Carpet_AddQuality"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            if (window.opener.document.getElementById('CPH_Form_refreshquality')) {
                window.opener.document.getElementById('CPH_Form_refreshquality').click();
                self.close();
            }
            else if (window.opener.document.getElementById('refreshquality')) {
                window.opener.document.getElementById('refreshquality').click();
                self.close();
            }
            else if (window.opener.document.getElementById('CPH_Form_refreshqualitydr')) {
                window.opener.document.getElementById('CPH_Form_refreshqualitydr').click();
                self.close();
            }
        }
        function addpriview() {
            //document.getElementById('BtnPreview').click();
            window.open('../../ReportViewer.aspx');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-left: 15%; margin-right: 15%; height: 430px">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table style="width: 50;">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="txtid" runat="server" Width="0px" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="lblcategory" runat="server" Text="Category" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcategory" CssClass="dropdown" runat="server" Width="160px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlcategory"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="lblitemname" runat="server" Text="Item&nbsp; Name" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="DDMasterQulaty" runat="server" Width="160px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDMasterQulaty_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDMasterQulaty"
                                            ErrorMessage="*" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDMasterQulaty"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="lblqualityname" runat="server" Text="QualityName" CssClass="labelbold"></asp:Label>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="txtquality" Text="" runat="server" Width="155px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Design Name"
                                            ControlToValidate="txtquality" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDLossLabel" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label1" runat="server" Text=" LOSS" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td id="TDLossText" runat="server" visible="false">
                                        <asp:TextBox ID="TxtLoss" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="  HS Code" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txthscode" runat="server" Width="155px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblQUALITYCODE" runat="server" Text="QUALITY CODE" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtQualityCode" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                            Width="159px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="TRRate" runat ="server" visible="false" >
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Material Rate" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtRate" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                            Width="159px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Trinstruction" runat="server" visible="false">
                                    <td>
                                        <span class="labelbold">INSTRUCTIONS</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtinstruction" CssClass="textb" TextMode="MultiLine" Width="300px"
                                            Height="100px" runat="server" />
                                    </td>
                                </tr>
                                <tr id="TrQualityRemark" runat="server" visible="false">
                                    <td>
                                        <span class="labelbold">REMARKS</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" CssClass="textb" TextMode="MultiLine" Width="300px" Height="100px"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="TDDGMonthName" runat="server" visible="false">
                            <div id="gride" runat="server" style="max-height: 350px; overflow: auto">
                                <asp:GridView ID="DGMonthName" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    EmptyDataText="No. Records found." OnRowDataBound="DGMonthName_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Month Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonthName" Text='<%#Bind("MonthName") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Loss%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLossPercentage" Text='<%#Bind("LossPercentage") %>' Width="80px"
                                                    runat="server" Font-Size="Small" Font-Bold="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonthID" Text='<%#Bind("MonthID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnsave" runat="server" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="m" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="btnclose" runat="server" Text="Close"
                                OnClientClick="return CloseForm()" />
                            &nbsp;<asp:Button CssClass="buttonnorm  preview_width" ID="BtnPreview" runat="server"
                                Text="Preview" OnClick="BtnPreview_Click" />
                            <%--  &nbsp;<asp:Button CssClass="buttonnorm  preview_width" ID="Button1" runat="server"
                                Text="Preview" OnClientClick="return addpriview();" />--%>
                            &nbsp;<asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
                <table style="margin-left: 60px">
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div style="overflow: auto; max-height: 400px; width: 500px">
                                <asp:GridView ID="gdQuality" runat="server" AllowPaging="True" PageSize="15" DataKeyNames="Sr_No"
                                    OnPageIndexChanging="gdQuality_PageIndexChanging" OnRowDataBound="gdQuality_RowDataBound"
                                    OnSelectedIndexChanged="gdQuality_SelectedIndexChanged" CaptionAlign="Left" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
