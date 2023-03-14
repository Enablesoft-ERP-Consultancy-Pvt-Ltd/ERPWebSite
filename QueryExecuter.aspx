<%@ Page Title="ExecuteQuery" Language="C#" AutoEventWireup="true" CodeFile="QueryExecuter.aspx.cs"
    Inherits="frmColor" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login</title>
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../Styles/Style.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
</head>
<body bgcolor="#edf3fe">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="hbhb" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table width="1000px">
                    <tr>
                        <td>
                            Query / Command<br />
                            <asp:TextBox ID="TxtQuery" Width="100%" TextMode="MultiLine" Height="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Button ID="Button1" Text="Click" runat="server" CssClass="checkboxbold" 
                                onclick="Button1_Click" Visible="false" />
                            <asp:Label ID="LblErrorMessage" Font-Bold="true" ForeColor="Red" runat="server" Text=""></asp:Label>
                            &nbsp;
                            <asp:Button ID="btnSave" Text="Click" runat="server" CssClass="checkboxbold" OnClick="btnSave_Click" />
                            &nbsp;<asp:CheckBox ID="ChkQuery" runat="server" Text="Chk for ExecuteDataSet" CssClass="checkboxbold" />
                            &nbsp;<asp:Button ID="btnClear" Text="Close" runat="server" CssClass="checkboxbold"
                                OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="gride" runat="server" style="width: 1000px; overflow: scroll">
                                <asp:GridView ID="gdcolor" runat="server" ForeColor="#333333" GridLines="None">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
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
