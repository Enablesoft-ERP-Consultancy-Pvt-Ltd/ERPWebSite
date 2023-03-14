<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmStockNoOutByInvoicePackingNew.aspx.cs" Inherits="Masters_Packing_FrmStockNoOutByInvoicePackingNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }
        function keypress() {

            var cmb = 0;
            var TxtBales = getNum(parseFloat($("#TxtBales").val()));
            if (TxtBales <= 0) {
                alert("Please enter No. of bales!!");
                this.value = "";
                return false;
            }
            //Assign the total to label
            //.toFixed() method will roundoff the final sum to 2 decimal places

            var L = getNum(parseFloat($("#txtlengthBale").val()));
            var W = getNum(parseFloat($("#txtwidthBale").val()));
            var H = getNum(parseFloat($("#txtheightbale").val()));
            var cbm = ((L * W * H / parseFloat(1000000))) * TxtBales;
            cbm = cbm == "Infinity" ? 0 : cbm;
            document.getElementById("txtcbmbale").value = (getNum(parseFloat(cbm)).toFixed(2));
        }
        function NewForm() {
            window.location.href = "FrmStockNoOutByInvoicePackingNew.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function AddCollection() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {

                var left = (screen.width / 2) - (500 / 2);
                var top = (screen.height / 2) - (370 / 2);

                window.open('FrmAddCollection.aspx', 'ADD collection', 'width=500px, height=370px, top=' + top + ', left=' + left);
                //window.open('FrmAddCollection.aspx', '', 'width=950px,Height=500px');
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }

        }
      
    </script>
    <style type="text/css">
        .style1
        {
            width: 238px;
        }
        .style2
        {
            width: 218px;
        }
    </style>
</head>
<body>
 <script type="text/javascript">
  function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnStockNo.ClientID %>').click();
            }
        }
    </script>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
                <%--style="background-image:url(Images/header.jpg)" --%>
                <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
            </td>
            <td style="background-color: #0080C0;" width="100px" valign="bottom">
                <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                <asp:Label ID="LblCompanyName" runat="server" Text="" ForeColor="White" Font-Bold="true"
                    CssClass="labelbold" Style="font-style: italic" Font-Size="Small"></asp:Label>
                <br />
                <i><font size="2" face="GEORGIA">
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font>
                </i>
            </td>
        </tr>
        <tr bgcolor="#999999">
            <td width="75%">
                <uc1:ucmenu ID="ucmenu1" runat="server" />
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td width="25%">
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                            Text="Logout" OnClick="BtnLogout_Click" Style="cursor: pointer; text-decoration: underline;
                            font-weight: bold" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td height="inherit" valign="top" class="style1" colspan="2">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                             <%--   <tr id="TRSearchInvoiceNo" runat="server" visible="false">
                                    <td colspan="2">
                                        <asp:TextBox ID="TxtSearchInvoiceNo" CssClass="textb" Width="240px" placeholder="Type here Invoice No. to Search"
                                            runat="server" AutoPostBack="true" OnTextChanged="TxtSearchInvoiceNo_TextChanged" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label1" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label2" class="tdstyle" runat="server" Text=" Customer Code" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                            OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        &nbsp;<%--<asp:Button ID="btnaddcontinent" runat="server" Text="&#43;" CssClass="buttonsmall"
                                            Style="margin-top: 0px" ToolTip="Click For Add New Collection" OnClientClick="return AddCollection();" />--%><cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCustomerCode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                   <%-- <td class="tdstyle">
                                        <asp:Label ID="Label3" class="tdstyle" runat="server" Text=" Consignee" CssClass="labelbold"></asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:CheckBox ID="ChkForEdit" runat="server" CssClass="checkboxbold" Text="For Edit"
                                            AutoPostBack="True" OnCheckedChanged="ChkForEdit_CheckedChanged" />
                                        <br />
                                       <asp:DropDownList ID="DDConsignee" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDConsignee"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>--%>
                                    <td id="TDDDInvoiceNo" runat="server" visible="true" class="tdstyle">
                                        <asp:Label ID="Label4" class="tdstyle" runat="server" Text="   Invoice No" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddInvoiceNo" runat="server" Width="180px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddInvoiceNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddInvoiceNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>                                
                                
                                </tr>
                              
                            </table>
                           
                            <table>                                
                                <tr>
                                    <td colspan="7" class="tdstyle">
                                        <asp:Label ID="lblStockCarpetNo" runat="server" Text="Enter Stock No" class="labelbold"></asp:Label>
                                  <%-- <asp:TextBox ID="TxtStockNo" runat="server" Width="200px" CssClass="textb" TabIndex="8"
                                        onKeyDown="KeyDownHandler('btnStockNo');" AutoPostBack="True" Height="30px"></asp:TextBox>
                                   <td>
                                        <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="TxtStockNo_TextChanged" />
                                    </td>--%>


                                    <asp:TextBox ID="TxtStockNo" CssClass="textb" Height="40px" Width="250px" runat="server"
                                                onKeypress="KeyDownHandler(event);" />
                                            <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="TxtStockNo_TextChanged" />
                                    </td>
                                </tr>
                            </table>
                            
                            <table>
                              
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblErrorMessage" runat="server" Font-Size="Small" CssClass="labelbold"
                                                ForeColor="Red"></asp:Label>
                                        </td>
                                        <td align="right">                                           
                                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do you want to save data?')"
                                                CssClass="buttonnorm" Visible="false" />
                                            &nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                                CssClass="buttonnorm" />
                                            &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                                CssClass="buttonnorm" />
                                        </td>
                                    </tr>
                                  <%--  <tr>
                                        
                                        <td valign="top">
                                            <div style="width: 200px; height: 200px; overflow: scroll">
                                                <asp:GridView ID="DGStock" Width="150px" runat="server" DataKeyNames="StockNo" AutoGenerateColumns="False"
                                                    CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <Columns>

                                                      <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="StockNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTStockNo" Text='<%#Bind("TSTOCKNO") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemFinishedId" Text='<%#Bind("Item_Finished_Id") %>' runat="server" />
                                                     <asp:Label ID="lblStockNo" Text='<%#Bind("StockNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chkbox" runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="20px" />
                                                        </asp:TemplateField>
                                                      <asp:BoundField DataField="StockNo" HeaderText="StockNo" />
                                                       <asp:TemplateField HeaderText="Pack Status" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtPack" runat="server" Visible="false" Text='<%# Bind("Pack") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>--%>
                                  
                                    <tr>
                                       <%-- <asp:HiddenField ID="hnpackingid" runat="server" />
                                        <asp:HiddenField ID="hnid" runat="server" />
                                        <asp:HiddenField ID="hnfinished" runat="server" />--%>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hnsampletype" runat="server" Value="1" />
                        </ContentTemplate>
                         <Triggers>
           <%-- <asp:PostBackTrigger ControlID="btnshow" />--%>
        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved. Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
