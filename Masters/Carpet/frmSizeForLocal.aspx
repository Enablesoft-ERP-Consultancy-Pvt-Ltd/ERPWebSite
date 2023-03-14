<%@ Page Title="Size" Language="C#" AutoEventWireup="true" CodeFile="frmSizeForLocal.aspx.cs"
    Inherits="frmSizeForLocal" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            this.close();
        }
        function priview() {
            window.open("../../ReportViewer.aspx");
        }
        function Validate() {
            if (document.getElementById('ddunit').value == "0") {
                alert("Pls Select Unit..");
                document.getElementById('ddunit').focus();
                return false;

            }
            else if (document.getElementById('ddshape').value == "0") {
                alert("Pls Select Shape..");
                document.getElementById('ddshape').focus();
                return false;

            }
            else if (document.getElementById('TxtSizeType').value == "") {
                alert("Pls Enter Size Type..");
                document.getElementById('TxtSizeType').focus();
                return false;

            }
            else if (document.getElementById('TxtLocalSize').value == "") {
                alert("Pls Enter Size...");
                document.getElementById('TxtLocalSize').focus();
                return false;

            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 450px">
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtsize" runat="server" CssClass="textb" Visible="false" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtid" runat="server" Visible="false" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Unit Name
                            <asp:DropDownList ID="ddunit" runat="server" OnSelectedIndexChanged="ddunit_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown" Width="80px" TabIndex="1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddunit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblshapeyname" runat="server" Text="Shape"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddshape" runat="server" AutoPostBack="True" Width="80px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddshape_SelectedIndexChanged" TabIndex="2">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddshape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            SizeType
                            <asp:TextBox ID="TxtSizeType" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="3"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Size
                            <asp:TextBox ID="TxtLocalSize" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr4" runat="server" visible="false">
                        <td>
                            <asp:Label ID="LblExportFormat" runat="server" Text="Export Format" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr id="TRSize" visible="false" runat="server">
                        <td class="tdstyle">
                            Ft.Width
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthFt" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" TabIndex="3"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Ft.Length
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthFt" runat="server" CssClass="textb" OnTextChanged="txtlengthFt_TextChanged"
                                AutoPostBack="True" Width="80px" TabIndex="4"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Ft.Height
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightFt" runat="server" CssClass="textb" OnTextChanged="txtheightFt_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Ft.Area
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaFt" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Ft.Volume
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolFt" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="TDMtrWidth" visible="false" runat="server">
                        <td class="tdstyle">
                            Mtr.Width
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthMtr" runat="server" CssClass="textb" OnTextChanged="txtwidthMtr_TextChanged"
                                AutoPostBack="True" Width="80px" TabIndex="5"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Mtr.Length
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthMtr" runat="server" CssClass="textb" OnTextChanged="txtlengthMtr_TextChanged"
                                AutoPostBack="True" Width="80px" TabIndex="6"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Mtr.Height
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightMtr" runat="server" CssClass="textb" OnTextChanged="txtheightMtr_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Mtr.Area
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Mtr.Volume
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="BtnCalCulate" CssClass="buttonnorm" runat="server" Text="Calculate"
                                OnClick="BtnCalCulate_Click" TabIndex="7" />
                        </td>
                    </tr>
                    <tr id="TDInch" visible="false" runat="server">
                        <td class="tdstyle">
                            Inch.Width
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtwidthInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Inch.Length
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtlengthInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Inch.Height
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtheightInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Inch.Area
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaInch" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Inch.Volume
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolInch" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server" visible="false">
                        <td>
                            <asp:Label ID="LblProductionFormat" runat="server" Text="Production Format" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr id="Tr2" runat="server" visible="false">
                        <td class="tdstyle">
                            Width Ft
                        </td>
                        <td>
                            <asp:TextBox ID="TxtWidthProdFt" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="8" OnTextChanged="TxtWidthProdFt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Length Ft
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLengthProdFt" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="9" OnTextChanged="TxtLengthProdFt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Area in Sq.Yd.
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAreaProdSqYD" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr3" runat="server" visible="false">
                        <td class="tdstyle">
                            Width Cm
                        </td>
                        <td>
                            <asp:TextBox ID="TxtWidthCm" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="10" OnTextChanged="TxtWidthCm_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Length Cm
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLengthCm" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="11" OnTextChanged="TxtLengthCm_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Area Sq. Mt.
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAreaProdSqMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10" runat="server" align="right">
                            <asp:Button ID="btnClear0" CssClass="buttonnorm" Text="New" runat="server" Width="56px"
                                OnClick="btnClear_Click" TabIndex="13" />
                            <asp:Button ID="btnSave" CssClass="buttonnorm" runat="server" OnClick="btnSave_Click"
                                Text="Save" Width="56px" TabIndex="12" OnClientClick="return Validate()" />
                            <asp:Button ID="btnclose0" CssClass="buttonnorm" Text="Close" runat="server" Width="48px"
                                OnClientClick="return CloseForm();" OnClick="btnclose0_Click" TabIndex="14" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" Visible="False" OnClick="btndelete_Click" TabIndex="15" />
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" OnClientClick="return priview();"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr id="TDlblMessage" runat="server">
                        <td colspan="3">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="td1" runat="server" colspan="6">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div id="Gride" style="overflow: scroll; width: 600px; height: 250px" align="center">
                                <asp:GridView ID="gdSize" runat="server" OnRowDataBound="gdSize_RowDataBound" OnSelectedIndexChanged="gdSize_SelectedIndexChanged"
                                    DataKeyNames="Sr_No" CssClass="grid-view" OnRowCreated="gdSize_RowCreated">
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
