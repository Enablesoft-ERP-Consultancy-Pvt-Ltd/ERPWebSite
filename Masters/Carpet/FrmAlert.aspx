<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAlert.aspx.cs" Inherits="Masters_Carpet_FrmAlert"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript"></script>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <link rel="Stylesheet" href="../../Styles/thickbox.css" type="text/css" media="screen" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr id="zzz" runat="server">
            <td>
                <table width="100%" border="1">
                    <tr style="width: 100%" align="center">
                        <td style="height: 66px; width: 800px" align="center">
                            <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" Width="800px"
                                Height="66px" />
                        </td>
                        <td style="background-color: #0080C0;" width="100px" valign="bottom">
                            <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                            <span style="color: Black; margin-left: 30px; font-family: Arial; font-size: xx-large">
                                <strong><em><i><font size="4" face="GEORGIA">
                                    <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em></strong></span>
                            <br />
                            <i><font size="2" face="GEORGIA">
                                <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font></i>
                        </td>
                    </tr>
                    <tr bgcolor="#999999">
                        <td class="style1">
                            <uc1:ucmenu ID="ucmenu1" runat="server" />
                            <asp:Label ID="LblFrmName" ForeColor="White" Font-Bold="true" Font-Size="Large" runat="server"
                                CssClass="labelbold"></asp:Label>
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                        </td>
                        <td width="25%">
                            <asp:UpdatePanel ID="up" runat="server">
                                <ContentTemplate>
                                    <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                                        Text="Logout" OnClick="BtnLogout_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr id="trVarFlag6" runat="server" visible="false">
                                    <td align="center">
                                        <div id="divusername" runat="server">
                                            User Name&nbsp;&nbsp;
                                            <asp:DropDownList ID="DDUserName" CssClass="dropdown" Width="250px" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDUserName_SelectedIndexChanged"
                                                TabIndex="1">
                                            </asp:DropDownList>
                                            <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pending Work</b>
                                        </div>
                                        <div style="margin-top: 5px; width: 1100px; height: 450px; overflow: scroll">
                                            <asp:GridView ID="DGToDoManagment" runat="server" OnRowDataBound="DGToDoManagment_RowDataBound"
                                                DataKeyNames="SrNo" Width="1080px" AutoGenerateColumns="False" CssClass="grid-view"
                                                OnRowCreated="DGToDoManagment_RowCreated" OnRowDeleting="DGToDoManagment_RowDeleting">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                Text="Click For Done" OnClientClick="return confirm('Do you want to update job status ?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="UserName" HeaderText="User Name">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WorkToDo" HeaderText="Work To Do">
                                                        <HeaderStyle Width="350px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="300px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                        <HeaderStyle Width="300px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="300px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PriorityLevel" HeaderText="Priority Level">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DueDate" HeaderText="DueDate">
                                                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LateByDays" HeaderText="LateBy Days">
                                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="JobStatus" HeaderText="JobStatus">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVarFlag7" runat="server" visible="false">
                                    <td>
                                        <div id="div2" runat="server" align="center">
                                            <b>Sample To Send</b>
                                        </div>
                                        <div style="margin-top: 5px; width: 1100px; height: 500px; overflow: scroll">
                                            <asp:GridView ID="DGToDoSampling" runat="server" AllowPaging="true" PageSize="6"
                                                OnRowDataBound="DGToDoSampling_RowDataBound" OnRowCancelingEdit="DGToDoSampling_RowCancelingEdit"
                                                OnRowUpdating="DGToDoSampling_RowUpdating" OnRowEditing="DGToDoSampling_RowEditing"
                                                Width="1800px" DataKeyNames="SrNo" AutoGenerateColumns="False" CssClass="grid-view"
                                                OnRowCreated="DGToDoSampling_RowCreated" OnPageIndexChanging="DGToDoSampling_PageIndexChanging">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <PagerStyle CssClass="PagerStyle" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                                                Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="True" CommandName="Update"
                                                                OnClientClick="return confirm('Do You Want To update?')" Text="Update"></asp:LinkButton>
                                                            &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                Text="Cancel"></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CourierNo">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TxtDGCourierNo" Width="100" Text='<%# Bind("CourierNo") %>' runat="server">
                                                            </asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("CourierNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TrackingNo">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TxtDGTrackingNo" Width="100" runat="server" Text='<%# Bind("TrackingNo") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("TrackingNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PriorityLevel" HeaderText="PriorityLevel" ReadOnly="True">
                                                        <HeaderStyle Width="70px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BuyerCode" HeaderText="B.Code" ReadOnly="True">
                                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DNo" HeaderText="DNo" ReadOnly="True">
                                                        <HeaderStyle Width="75px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="75px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DName" HeaderText="DName" ReadOnly="True">
                                                        <HeaderStyle Width="75px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="75px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Technique" HeaderText="Technique" ReadOnly="True">
                                                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="90px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Size" HeaderText="Size" ReadOnly="True">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RawMaterialComposition" HeaderText="RawMaterialComposition"
                                                        ReadOnly="True">
                                                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="QualityOfRaw" HeaderText="QualityOfRaw" ReadOnly="True">
                                                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WashUnWash" HeaderText="Wash UnWash" ReadOnly="True">
                                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PileHeight" HeaderText="PileHeight" ReadOnly="True">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PlWt" HeaderText="PileWt" ReadOnly="True">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TWt" HeaderText="Tot.Wt" ReadOnly="True">
                                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DispDate" HeaderText="DispDate" ReadOnly="True">
                                                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="90px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StatusOfDispatch" HeaderText="StatusOf Dispatch" ReadOnly="True">
                                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ColorNoS" HeaderText="ColorNoS" ReadOnly="True">
                                                        <HeaderStyle Width="200px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="200px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remark" HeaderText="Remark" ReadOnly="True">
                                                        <HeaderStyle Width="200px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="200px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVarFlag1" runat="server" visible="false">
                                    <td>
                                        <div style="margin-left: 150px; margin-top: 50px; width: 800px; height: 250px; overflow: scroll">
                                            <asp:GridView ID="DGForProduction" runat="server" AutoGenerateColumns="False" OnRowDataBound="DGForProduction_RowDataBound">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderNo" HeaderText="OrderNo">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderDate" HeaderText="OrderDate">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ProdReqDate" HeaderText="ProdDate">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ItemName" HeaderText="ItemName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="QualityName" HeaderText="QualityName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DesignName" HeaderText="DesignName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ColorName" HeaderText="ColorName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="125px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Shape" HeaderText="Shape">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Size" HeaderText="Size">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                        <div style="margin-left: 200px; margin-top: 50px; width: 600px; height: 250px; overflow: scroll">
                                            <asp:GridView ID="DGForProductionPending" runat="server" AutoGenerateColumns="False"
                                                OnRowDataBound="DGForProductionPending_RowDataBound">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:BoundField DataField="CompanyName" HeaderText="CompanyName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EmpName" HeaderText="Emp Name">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IssueOrderId" HeaderText="Order Slip No">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssignDate" HeaderText="AssignDate">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ReqDate" HeaderText="ReqDate">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVarFlag2" runat="server" visible="false">
                                    <td>
                                        <div style="margin-left: 350px; margin-top: 50px; width: 400px; height: 250px; overflow: scroll">
                                            <asp:GridView ID="DGOrderProcessProgram" runat="server" AutoGenerateColumns="False"
                                                OnRowDataBound="DGOrderProcessProgram_RowDataBound" ShowFooter="true">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderNo" HeaderText="OrderNo">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderDate" HeaderText="OrderDate">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVarFlag3" runat="server" visible="false">
                                    <td>
                                        <div style="margin-left: 350px; margin-top: 50px; width: 600px; height: 250px; overflow: scroll">
                                            <asp:GridView ID="DGPurchaseApproval" runat="server" AutoGenerateColumns="False"
                                                OnRowDataBound="DGPurchaseApproval_RowDataBound">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:BoundField DataField="CompanyName" HeaderText="CompanyName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DepartmentName" HeaderText="DepartmentName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartyName" HeaderText="PartyName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PIndentNo" HeaderText="Indent No">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Date" HeaderText="Date">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVarFlag4" runat="server" visible="false">
                                    <td>
                                        <div style="margin-left: 250px; margin-top: 50px; width: 800px; height: 250px; overflow: scroll">
                                            <asp:GridView ID="DGPurchaseOrder" runat="server" AutoGenerateColumns="False" OnRowDataBound="DGPurchaseOrder_RowDataBound">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:BoundField DataField="CompanyName" HeaderText="CompanyName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartyName" HeaderText="PartyName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PIndentNo" HeaderText="Indent No">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Date" HeaderText="Date">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ApprovalDate" HeaderText="ApprovalDate">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ApprovedBy" HeaderText="ApprovedBy">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ApprovalNo" HeaderText="ApprovalNo">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                        <div style="margin-left: 250px; margin-top: 50px; width: 800px; height: 250px; overflow: scroll">
                                            <asp:GridView ID="DGPurchaseOrderPending" runat="server" AutoGenerateColumns="False"
                                                OnRowDataBound="DGPurchaseOrderPending_RowDataBound">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:BoundField DataField="CompanyName" HeaderText="CompanyName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EmpName" HeaderText="PartyName">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ChallanNo" HeaderText="Po No">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Date" HeaderText="Date">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DueDate" HeaderText="DueDate">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVarFlag5" runat="server" visible="false">
                                    <td>
                                        <div style="margin-left: 350px; margin-top: 50px; width: 400px; height: 250px; overflow: scroll">
                                            <asp:GridView ID="Dgplaning" runat="server" AutoGenerateColumns="False" OnRowDataBound="DGOrderProcessProgram_RowDataBound">
                                                <HeaderStyle CssClass="gvheader" />
                                                <AlternatingRowStyle CssClass="gvalt" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <%--<asp:BoundField DataField="CustomerCode" HeaderText="CustomerCode">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                        </asp:BoundField>--%>
                                                    <asp:BoundField DataField="OrderNo" HeaderText="OrderNo">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OrderDate" HeaderText="OrderDate">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" OnClick="BtnRefresh_Click"
                                            Visible="false" CssClass="buttonnorm" />
                                        <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                            Visible="true" CssClass="buttonnorm" />
                                        &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm"
                                            OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 95%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
