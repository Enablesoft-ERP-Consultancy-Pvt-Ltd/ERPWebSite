<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddSize.aspx.cs" Inherits="Masters_Carpet_AddSize"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AddSize</title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            if (window.opener.document.getElementById('CPH_Form_refreshsize2')) {
                window.opener.document.getElementById('CPH_Form_refreshsize2').click();
                self.close();
            }
            else if (window.opener.document.getElementById('refreshsize')) {
                window.opener.document.getElementById('refreshsize').click();
                self.close();
            }
            else if (window.opener.document.getElementById('CPH_Form_BtnRefreshSize')) {
                window.opener.document.getElementById('CPH_Form_BtnRefreshSize').click();
                self.close();
            }
            else if (window.opener.document.getElementById('BtnRefreshSize')) {
                window.opener.document.getElementById('BtnRefreshSize').click();
                self.close();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server">
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
                        <td class="tdstyle">
                            <asp:Label Text="Unit Name" runat="server" ID="lblwe" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddunit" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddunit_SelectedIndexChanged"
                                AutoPostBack="True" Width="80px" TabIndex="1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddunit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblshapeyname" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
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
                            <asp:Label ID="lblSizeCode" runat="server" Text="Size Code" Font-Bold="true"></asp:Label>
                        </td>
                        <td class="tdstyle">
                            <asp:TextBox ID="txtSizeCode" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="BtnUpdateCode" CssClass="buttonnorm" runat="server" OnClientClick="return confirm('Do you want to Update data?')"
                                OnClick="BtnUpdateCode_Click" Text="Update Code" Width="90px" Visible="false" />
                        </td>
                    </tr>
                    <tr id="Tr4" runat="server" visible="false">
                        <td colspan="3">
                            <asp:Label ID="LblExportFormat" runat="server" Text="Export Format" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Ft.Width" runat="server" ID="Label1" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthFt" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px" TabIndex="3"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Ft.Length" runat="server" ID="Label2" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthFt" runat="server" CssClass="textb" OnTextChanged="txtlengthFt_TextChanged"
                                AutoPostBack="True" Width="80px" TabIndex="4"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Ft.Height" runat="server" ID="Label3" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightFt" runat="server" CssClass="textb" OnTextChanged="txtheightFt_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Ft.Area" runat="server" ID="Label4" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaFt" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Ft.Volume" runat="server" ID="Label5" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolFt" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Cm.Width" runat="server" ID="lblMtrWidth" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthMtr" runat="server" CssClass="textb" OnTextChanged="txtwidthMtr_TextChanged"
                                AutoPostBack="True" Width="80px" TabIndex="5"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Cm.Length" runat="server" ID="lblmtrlength" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthMtr" runat="server" CssClass="textb" OnTextChanged="txtlengthMtr_TextChanged"
                                AutoPostBack="True" Width="80px" TabIndex="6"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Cm.Height" runat="server" ID="lblmtrheight" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightMtr" runat="server" CssClass="textb" OnTextChanged="txtheightMtr_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Mtr.Area" runat="server" ID="Label9" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Mtr.Volume" runat="server" ID="Label10" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="BtnCalCulate" runat="server" Text="Calculate" CssClass="buttonnorm"
                                OnClick="BtnCalCulate_Click" TabIndex="7" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Inch.Width" runat="server" ID="Label11" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtwidthInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Inch.Length" runat="server" ID="Label12" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtlengthInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Inch.Height" runat="server" ID="Label13" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtheightInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Inch.Area" runat="server" ID="Label14" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaInch" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Inch.Volume" runat="server" ID="Label15" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolInch" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server" visible="false">
                        <td colspan="3">
                            <asp:Label ID="LblProductionFormat" runat="server" Text="Production Format" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr id="Tr2" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label Text="Width Ft" runat="server" ID="Label16" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtWidthProdFt" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="8" OnTextChanged="TxtWidthProdFt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Length Ft" runat="server" ID="Label17" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLengthProdFt" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="9" OnTextChanged="TxtLengthProdFt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Area in Sq.Yd." runat="server" ID="Label18" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAreaProdSqYD" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr3" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label Text=" Width Cm" runat="server" ID="Label19" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtWidthCm" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="10" OnTextChanged="TxtWidthCm_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Length Cm" runat="server" ID="Label20" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLengthCm" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" TabIndex="11" OnTextChanged="TxtLengthCm_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Area Sq. Mt." runat="server" ID="Label21" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAreaProdSqMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td id="Td1" colspan="10" runat="server" align="right">
                            <asp:Button ID="btnClear0" Text="New" CssClass="buttonnorm" runat="server" Width="56px"
                                OnClick="btnClear_Click" TabIndex="13" />
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnSave_Click" Text="Save" Width="56px" TabIndex="12" />
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" Width="48px"
                                OnClientClick="return CloseForm();" TabIndex="14" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" Visible="False" OnClick="btndelete_Click" TabIndex="15" />
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" OnClientClick="return priview();"
                                CssClass="buttonnorm preview_width" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="td2" runat="server" colspan="10">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div id="Gride" style="overflow: auto; width: 900px; height: 250px">
                                <asp:GridView ID="gdSize" runat="server" OnRowDataBound="gdSize_RowDataBound" OnSelectedIndexChanged="gdSize_SelectedIndexChanged"
                                    DataKeyNames="Sr_No" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
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
