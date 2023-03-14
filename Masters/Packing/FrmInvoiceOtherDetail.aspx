<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoiceOtherDetail.aspx.cs"
    Inherits="Masters_Packing_FrmInvoiceOtherDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            self.close();
        }
        function AddDescriptionOfGood() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddDescriptionGoods.aspx', '', 'width=700px,Height=500px');
            }
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 74%">
                    <tr>
                        <td>
                            <asp:Label ID="LblTerms" runat="server" Text="Terms" Font-Bold="True" Enabled="false"
                                ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtInvoiceId" runat="server" BorderWidth="0px" ForeColor="White"
                                Height="0px" Width="0px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblGoods" runat="server" class="tdstyle" Text="Description of Good"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDGoods" runat="server" Width="100%" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDGoods"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="BtnAddGoods" runat="server" Text="..." Width="30px" Height="22px"
                                OnClientClick="return AddDescriptionOfGood();" CssClass="buttonsmall" />
                        </td>
                        <td>
                            <asp:Button ID="BtnRefreshDescriptionOfGood" runat="server" Height="0px" Text="."
                                Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px" ForeColor="White"
                                OnClick="BtnRefreshDescriptionOfGood_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" class="tdstyle" runat="server" Text="Contents"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtContents" runat="server" CssClass="textboxremark" TextMode="MultiLine"
                                Width="250px" Text="Woollen Yarn : 80% Cotton Yarn : 20%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label10" class="tdstyle" runat="server" Text="ACD Per Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtACDPerson" CssClass="textb" runat="server" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:RadioButton ID="RDDeclaration1" runat="server" Text="Declaration1" AutoPostBack="True"
                                Font-Bold="True" OnCheckedChanged="RDDeclaration1_CheckedChanged" />
                            <br />
                            <br />
                            <br />
                            <asp:RadioButton ID="RDDeclaration2" runat="server" Text="Declaration2" AutoPostBack="True"
                                Font-Bold="True" OnCheckedChanged="RDDeclaration2_CheckedChanged" />
                        </td>
                        <td colspan="2" class="tdstyle">
                            Declaration
                            <br />
                            <asp:TextBox ID="TxtDeclaration" runat="server" CssClass="textboxremark" Height="100px"
                                TextMode="MultiLine" Width="250px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Notify
                            <br />
                            <asp:TextBox ID="TxtNotify" runat="server" Height="100px" CssClass="textboxremark"
                                TextMode="MultiLine" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <%-- <tr>
    <td >
        <asp:Label ID="Label2" class="tdstyle" runat="server" Text="Other Ref"></asp:Label></td>
    <td> 
        <asp:TextBox ID="TxtOtherRef" runat="server" Width ="250px"></asp:TextBox>
    </td>
    <td>
        <asp:Label ID="Label11" class="tdstyle" runat="server" Text="SDF No"></asp:Label></td>
    <td>
        <asp:TextBox ID="TxtSDFNo" runat="server" Width="150px"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td>
        <asp:Label ID="Label3" class="tdstyle" runat="server" Text="Drawback Or White S/Bill"></asp:Label></td>
    <td>
        <asp:TextBox ID="TxtDBackSwhiteBill" runat="server" Width ="250px"></asp:TextBox>
    </td>

    <td>
        <asp:Label ID="Label12" class="tdstyle" runat="server" Text="GR No"></asp:Label></td>
    <td>
        <asp:TextBox ID="TxtGRNo" runat="server" Width="150px"></asp:TextBox>
    </td>
    
    </tr>
    <tr>
    
    <td>
        <asp:Label ID="Label4" class="tdstyle" runat="server" Text="Single Contry Declaration Name" Width="200px"></asp:Label></td>
    <td>
        <asp:TextBox ID="TxtPersonName" runat="server" Width ="250px"></asp:TextBox>
    </td>
    <td>
        <asp:Label ID="Label15" class="tdstyle" runat="server" Text="TR/RR No. And Date"></asp:Label></td>
    <td>
        <asp:TextBox ID="TxtTRNo" runat="server" Width="150px"></asp:TextBox></td>
    </td>
    </tr>
    <tr>
    <td >
        <asp:Label ID="Label5" class="tdstyle" runat="server" Text="Special Shipping Instr"></asp:Label></td>
    <td>
        <asp:TextBox ID="TxtSplInstr" runat="server" Width="250px"></asp:TextBox>
    </td>
    <td>
        <asp:Label ID="Label14" class="tdstyle" runat="server" Text="US $/Euro Ex. Rate"></asp:Label></td>
        <td>
    <asp:TextBox ID="TxtExchRate" runat="server" Width="150px"></asp:TextBox></td>
    </tr>
    <tr>
    <td >
        <asp:Label ID="Label6" class="tdstyle" runat="server" Text="Invoice Covering"></asp:Label>
        &nbsp;&nbsp;
        
    </td>
    <td>
        <asp:TextBox ID="txtInvoiceCvrng" runat="server" Height="50px" 
                 TextMode="MultiLine" Width="250px"></asp:TextBox></td>
                 <td>
        <asp:Label ID="Label13" class="tdstyle" runat="server" Text="Vessel Agent"></asp:Label></td>
    <td>
        <asp:TextBox ID="TxtBLAgencyInfo" runat="server"  Height="50px" Width="150px"></asp:TextBox>
    </td>
    <td>
        <asp:TextBox ID="TxtVesselAgent" runat="server"  Height="50px" Width="150px"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td >
        <asp:Label ID="Label7" class="tdstyle" runat="server" Text="Knots"></asp:Label></td> 
    <td>       
        <asp:TextBox ID="TxtKnots" runat="server" Width ="250px"></asp:TextBox>
    </td>
    <td>
        <asp:Label ID="Label16" class="tdstyle" runat="server" Text="Document In"></asp:Label></td>
    <td>
        <asp:DropDownList ID="DDDocType" runat="server" Width ="150px" 
            CssClass="dropdown">
            <asp:ListItem Value="1">Shipment</asp:ListItem>
            <asp:ListItem Value="2">InHand</asp:ListItem>
            <asp:ListItem Value="3">Collection</asp:ListItem>
            <asp:ListItem Value="4">Purchase</asp:ListItem>
            <asp:ListItem Value="5">Negotiation</asp:ListItem>
        </asp:DropDownList>
    </td>
    </tr>
    <tr>
    <td >
        <asp:Label ID="Label8" class="tdstyle" runat="server" Text="Order No"></asp:Label></td> 
    <td>
        <asp:TextBox ID="TxtOrderNo" runat="server" Width ="250px"></asp:TextBox>
    </td>
    <td>
        <asp:Label ID="Label17" class="tdstyle" runat="server" Text="Bank Ref. And Date"></asp:Label></td>
    <td>
        <asp:TextBox ID="TxtDocRef" runat="server" Width="150px"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td >
        <asp:Label ID="Label9" class="tdstyle" runat="server" Text="Total Order No"></asp:Label></td> 
    <td>
        <asp:TextBox ID="TxtTOrderNo" runat="server" Width ="250px"></asp:TextBox>
    </td>
    <td>
        <asp:Label ID="Label18" class="tdstyle" runat="server" Text="Proforma Ref. And Date" Width="135px"></asp:Label></td>
    <td>
        <asp:TextBox ID="txtPrfmaRef" runat="server" Width="150px"></asp:TextBox>
    </td>
    </tr>--%>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="ErrorMessage" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="buttonnorm" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" />
                            <asp:Button CssClass="refreshcolor" ID="refreshcolor" runat="server" Text="" BorderWidth="0px"
                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                ForeColor="White" OnClick="refreshcolor_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
