<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_receive_process_next.aspx.cs"
    Inherits="Masters_Process_frm_receive_process_next" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frm_receive_process_next.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(btn) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById(btn).click();
            }
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function YourFunctionName(msg) {
            var txt = msg;
            alert(txt);
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {
                        if (inputlist[i].disabled) {
                        }
                        else {
                            inputlist[i].checked = true;
                        }

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td width="100%" colspan="2">
                    <table width="100%">
                        <tr style="width: 100%" align="center">
                            <td height="66px" align="center">
                                <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
                            </td>
                            <td style="background-color: #0080C0;" width="100px" valign="bottom">
                                <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                                <span style="color: Black; margin-left: 30px; font-family: Arial; font-size: xx-large">
                                    <strong><em><i><font size="4" face="GEORGIA">
                                        <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em>
                                    </strong></span>
                                <br />
                                <i><font size="2" face="GEORGIA">
                                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font>
                                </i>
                            </td>
                        </tr>
                        <tr bgcolor="#999999">
                            <td width="75%">
                                <uc1:ucmenu ID="ucmenu1" runat="server" />
                                <asp:ScriptManager ID="ScriptManager2" runat="server">
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
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div>
                                <table width="80%">
                                    <tr id="Tr1" runat="server">
                                        <td id="TDChallanNo" runat="server" visible="false" class="tdstyle">
                                            <asp:Label ID="lbl" Text="Challan No" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox CssClass="textb" ID="TxtEditChallanNo" runat="server" Width="100px"
                                                AutoPostBack="True" OnTextChanged="TxtEditChallanNo_TextChanged"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label3" Text="  Company Name" runat="server" CssClass="labelbold" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b>
                                            <br />
                                            <asp:DropDownList ID="ddCompName" runat="server" CssClass="dropdown" Width="200px"
                                                OnSelectedIndexChanged="ddCompName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCompName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label4" Text="Process" runat="server" CssClass="labelbold" />
                                            &nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                            <asp:DropDownList ID="ddprocess" runat="server" CssClass="dropdown" Width="150px"
                                                OnSelectedIndexChanged="ddprocess_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddprocess"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label5" Text=" Emp/Contractor" runat="server" CssClass="labelbold" />
                                            &nbsp;&nbsp; <b style="color: Red">*</b>&nbsp;&nbsp;
                                            <asp:CheckBox ID="ChkForEdit" runat="server" CssClass="checkboxbold" Text="For Edit"
                                                OnCheckedChanged="ChkForEdit_CheckedChanged" AutoPostBack="True" />
                                            <br />
                                            <asp:DropDownList ID="ddemp" runat="server" CssClass="dropdown" Width="200px" OnSelectedIndexChanged="ddemp_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddemp"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td id="TDDDChallanNo" runat="server" visible="false" class="tdstyle">
                                            <asp:Label ID="Label6" Text=" Challan No" runat="server" CssClass="labelbold" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                            <asp:DropDownList ID="DDChallanNo" runat="server" Width="100px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDChallanNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label7" Text="P OrderNo" runat="server" CssClass="labelbold" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                            <asp:DropDownList ID="ddorderno" runat="server" CssClass="dropdown" Width="100px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddorderno"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label8" Text="Cal Type" runat="server" CssClass="labelbold" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                                Enabled="false">
                                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                                <asp:ListItem Value="4">L-2</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label9" Text=" Unit" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDUnit" runat="server" Width="100px" AutoPostBack="True"
                                                Enabled="false">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="LSEDDUnit" runat="server" TargetControlID="DDUnit" ViewStateMode="Disabled"
                                                PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td id="Td1" runat="server" class="tdstyle">
                                            <asp:Label ID="Label10" Text="   Rec.Date" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox CssClass="textb" ID="TxtreceiveDate" runat="server" Width="90px" AutoPostBack="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtreceiveDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td id="Td2" runat="server" class="tdstyle">
                                            <asp:Label ID="Label11" Text="Challan No" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox CssClass="textb" ReadOnly="true" ID="TxtChallanNo" runat="server" Width="90px"
                                                AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcategoryname" class="tdstyle" runat="server" Text="Item Category"
                                                CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="ddcattype" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddcattype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddcattype"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="dditem" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="True"
                                                OnSelectedIndexChanged="dditem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="dditem"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td runat="server" id="tdquality" visible="false">
                                            <asp:Label ID="lblqualityname" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="ddquality" runat="server" CssClass="dropdown" Width="150px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddquality_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddquality"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td runat="server" id="tddesign" visible="false">
                                            <asp:Label ID="lbldesignname" class="tdstyle" runat="server" Text="Design" CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="ddldesig" runat="server" CssClass="dropdown" Width="150px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddldesig_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddldesig"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td id="tdcolor" runat="server" visible="false">
                                            <asp:Label ID="lblcolorname" class="tdstyle" runat="server" Text="Color" CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="ddcolour" runat="server" CssClass="dropdown" Width="100px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddcolour_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddcolour"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td runat="server" id="tdshape" visible="false">
                                            <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape"></asp:Label><br />
                                            <asp:DropDownList ID="ddshape" runat="server" CssClass="dropdown" Width="100px" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddshape"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td runat="server" id="tdsize" visible="false" class="tdstyle">
                                            <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddsize" runat="server" CssClass="dropdown" Width="100px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddsize"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td id="tdshadecolor" runat="server" visible="false">
                                            <asp:Label ID="lblshadecolor" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label>
                                            &nbsp;<br />
                                            <asp:DropDownList ID="ddlshade" runat="server" Width="100px" CssClass="dropdown"
                                                OnSelectedIndexChanged="ddlshade_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchddlshade" runat="server" TargetControlID="ddlshade"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TDsrno" runat="server" visible="false">
                                            <asp:Label ID="lblsrno" Text="Sr No." CssClass="labelbold" runat="server" />
                                            <br />
                                            <asp:DropDownList ID="DDsrno" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDsrno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" ForeColor="Red"
                                                CssClass="labelbold"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 80%">
                                    <tr>
                                        <td>
                                            <div style="width: 100%">
                                                <div style="width: 80%; float: left">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div style="overflow: auto; max-height: 200px">
                                                                    <asp:GridView ID="mygdv" runat="server" AutoGenerateColumns="False" OnRowDeleting="mygdv_RowDeleting"
                                                                        OnRowUpdating="mygdv_RowUpdating" CssClass="grid-views" OnRowDataBound="mygdv_RowDataBound">
                                                                        <HeaderStyle CssClass="gvheaders" />
                                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="SN">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" ID="lblfinishedid" Visible="false" Text='<%#(DataBinder.Eval(Container,"DataItem.item_finished_id").ToString()) %>'></asp:Label>
                                                                                    <asp:Label runat="server" ID="lblorderid" Visible="false" Text='<%#(DataBinder.Eval(Container,"DataItem.OrderId").ToString()) %>'></asp:Label>
                                                                                    <asp:Button ID="BtnStockFil" runat="server" CssClass="buttonnorm" CommandName="Delete"
                                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="Stock No" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Category">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" ID="lblcatname" Visible="true" Text='<%#(DataBinder.Eval(Container,"DataItem.category_name").ToString()) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Item">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="Server" ID="lblitem" Visible="true" Text='<%# DataBinder.Eval(Container.DataItem, "item_name") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Description">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="Server" ID="lbldescription" Visible="true" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Qty">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="lblqty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "qty") %>'
                                                                                        Width="70px" />
                                                                                    <%--   <asp:Label runat="Server" ID="lblqty" Visible="true" Font-Bold="true" ForeColor="DarkSlateBlue"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem, "qty") %>' />--%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Area">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="Server" ID="lblarea" Visible="true" Font-Bold="true" ForeColor="DarkSlateBlue"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "area") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="BtnStockF" runat="server" CssClass="buttonnorm" CommandName="update"
                                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="All" Width="50px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="TQty">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtTQty" runat="server" Text='<%#Bind("Qty") %>' Width="70px" ReadOnly="true"
                                                                                        Enabled="false" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Sr No./OC No.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbllocalorder" Text='<%#Bind("Localorder") %>' runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="width: 20%; float: right" id="divstock" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td class="tdstyle">
                                                                <asp:Button ID="btnStockNo" runat="server" BackColor="White" BorderColor="White"
                                                                    BorderWidth="0px" ForeColor="White" Height="0px" Width="0px" OnClick="TxtStockNo_TextChanged" />
                                                                <asp:Label ID="Label12" Text=" Stock No" runat="server" CssClass="labelbold" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtStockNo" runat="server" Width="150px" CssClass="textb" onKeyDown="KeyDownHandler('btnStockNo');"
                                                                    AutoPostBack="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div style="max-height: 180px; overflow: auto">
                                                                    <asp:GridView ID="mygdstock" runat="server" AutoGenerateColumns="False" OnRowDeleting="mygdstock_RowDeleting"
                                                                        CssClass="grid-views" DataKeyNames="StockNo">
                                                                        <HeaderStyle CssClass="gvheaders" />
                                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Carpet No.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblcarpetno" runat="server" Width="150px" Text='<%#DataBinder.Eval(Container, "DataItem.tstockNO") %>'
                                                                                        Visible="true"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Rate">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtRate" Width="50px" CssClass="textb" runat="server" Text='<%# Bind("Rate") %>'
                                                                                        Enabled="true"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Save">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="Btnsave" runat="server" Width="50px" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                        CommandName="Delete" CssClass="buttonnorm" Text="Save" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Stock No." Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStockNo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.StockNo") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table width="80%">
                                    <tr>
                                        <td>
                                            <div style="max-height: 400px; overflow: auto; width: 100%">
                                                <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="process_rec_Detail_Id"
                                                    OnRowDeleting="DGDetail_RowDeleting" OnRowUpdating="DGDetail_RowUpdating" CssClass="grid-views"
                                                    OnRowCommand="DGDetail_RowCommand" OnRowEditing="DGDetail_RowEditing" 
                                                    OnRowCancelingEdit="DGDetail_RowCancelingEdit" 
                                                    onrowdatabound="DGDetail_RowDataBound">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Item">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitem" Text='<%#Bind("Item") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescription" Text='<%#Bind("Description") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Width">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblwidth" Text='<%#Bind("width") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Length">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbllength" Text='<%#Bind("length") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Size">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsize" Text='<%#Bind("Size") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblqty" Text='<%#Bind("qty") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrate" Text='<%#Bind("rate") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txteditrate" Text='<%#Bind("rate") %>' runat="server" Width="75px"
                                                                    BackColor="Yellow" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Area">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblarea" Text='<%#Bind("Area") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="StockNo">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblstockno" Text='<%#Bind("Stockno") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SrNo./Local Order No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbllocalorder" Text='<%#Bind("localorder") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkall" Text="Delete all" runat="server" onclick="return CheckAll(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkboxitem" Text="" CssClass="checkboxbold" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                    Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="BtnPenality" runat="server" Text="Penality" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    CommandName="Penality"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:CommandField EditText="Edit Rate" ShowEditButton="True" />
                                                        <asp:TemplateField HeaderText="QualityChk">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BtnStockF" runat="server" CssClass="buttonnorm" CommandName="update"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="QualityChk" />
                                                            </ItemTemplate>
                                                            <ItemStyle ForeColor="Black" HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblisissued" Text='<%#Bind("isissued") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                        <td id="qulitychk" runat="server" valign="top">
                                            <asp:GridView ID="grdqualitychk" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                                CssClass="grid-views">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SrNo">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SrNo") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ParaName">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ParaName") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ParaName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Button ID="btnqcsave" runat="server" CssClass="buttonnorm" OnClick="btnqcsave_Click"
                                                Text="Save" />
                                        </td>
                                        <td id="TDPenality" runat="server" visible="false" valign="top" align="right">
                                            <asp:GridView ID="DGPenality" runat="server" AutoGenerateColumns="False" DataKeyNames="Rec_Detail_Id"
                                                CssClass="grid-views">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Penality Amt">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtPenalityAmt" runat="server" CssClass="textb" Width="50px" Text='<%# Bind("PenalityAmt") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="P Remark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtPenalityRemark" runat="server" CssClass="textb" Width="250px"
                                                                Text='<%# Bind("PenalityRemark") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Button ID="BtnPenalitySave" runat="server" CssClass="buttonnorm" OnClick="BtnPenalitySave_Click"
                                                Text="Save" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:CheckBox ID="ChkDetailPrint" class="tdstyle" runat="server" Text="For Detail Print"
                                                CssClass="checkboxbold" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" runat="server" Text="Preview"
                                                OnClick="BtnPreview_Click" />
                                            &nbsp;<asp:Button CssClass="buttonnorm " ID="btnQcPreview" runat="server" Text="QCPreview"
                                                OnClick="btnQcPreview_Click" Width="100px" />
                                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                                                OnClientClick=" return CloseForm(); " />
                                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                                            &nbsp;<asp:Button CssClass="buttonnorm" ID="btndelete" runat="server" Text="Delete"
                                                OnClick="btndelete_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label13" Text=" Total Pcs" runat="server" CssClass="labelbold" />
                                            <asp:TextBox CssClass="textb" ID="TxtTotalPcs" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label14" Text="Total Area" runat="server" CssClass="labelbold" />
                                            <asp:TextBox CssClass="textb" ID="TxtArea" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label15" Text="Total Amount" runat="server" CssClass="labelbold" />
                                            <asp:TextBox CssClass="textb" ID="TxtAmount" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShowdata" runat="server" Text="Click to Show Data" CssClass="buttonnorm"
                                                OnClick="btnShowdata_Click" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label16" Text="Remarks" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox CssClass="textb" ID="TxtRemarks" runat="server" Width="100%" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:HiddenField ID="hn_finished" runat="server" />
                                            <asp:HiddenField ID="hnstockno" runat="server" />
                                            <asp:HiddenField ID="hnrate1" runat="server" />
                                            <asp:HiddenField ID="hnorderid" runat="server" />
                                            <asp:HiddenField ID="hn_recieve_id" runat="server" />
                                            <asp:HiddenField ID="Hn_Qty" runat="server" />
                                            <asp:HiddenField ID="Hn_TQty" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
