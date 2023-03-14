<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmQCParaMeterName.aspx.cs"
    Inherits="Masters_Carpet_FrmQCParaMeterName" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.opener.document.getElementById('CPH_Form_BtnAddParameterReferce').click();
            self.close();
        }
        function validate() {
            if (document.getElementById("<%=txtParameterName.ClientID %>").value == "") {
                alert("Parameter Name Can't Be Blank");
                document.getElementById("<%=txtParameterName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TxtSerialNumber.ClientID %>").value == "") {
                alert("SerialNumber Can't Be Blank");
                document.getElementById("<%=TxtSerialNumber.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TxtShortName.ClientID %>").value == "") {
                alert("ShortName Name Can't Be Blank");
                document.getElementById("<%=TxtShortName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtSpecified.ClientID %>").value == "") {
                alert("Specified Can't Be Blank");
                document.getElementById("<%=txtSpecified.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtMethod.ClientID %>").value == "") {
                alert("Method Can't Be Blank");
                document.getElementById("<%=txtMethod.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="Parameter Name" CssClass="labelbold"></asp:Label>
                            <b style="color: Red">&nbsp;&nbsp; *&nbsp;&nbsp; </b>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtParameterName" runat="server" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text="Serial Number" CssClass="labelbold"></asp:Label>
                            <b style="color: Red">&nbsp;&nbsp; *&nbsp;&nbsp; </b>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="TxtSerialNumber" runat="server" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" runat="server" Text="Short Name" CssClass="labelbold"></asp:Label>
                            <b style="color: Red">&nbsp;&nbsp; *&nbsp;&nbsp; </b>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="TxtShortName" runat="server" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" runat="server" Text="Specified" CssClass="labelbold"></asp:Label>
                            <b style="color: Red">&nbsp;&nbsp; *&nbsp;&nbsp; </b>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtSpecified" runat="server" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" runat="server" Text="Method" CssClass="labelbold"></asp:Label>
                            <b style="color: Red">&nbsp;&nbsp; *&nbsp;&nbsp; </b>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtMethod" runat="server" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                        <asp:TextBox ID="txtid" runat="server" Visible="false"></asp:TextBox>
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return validate()"
                                CssClass="buttonnorm" />
                            <asp:Button ID="close" runat="server" Text="Close" OnClientClick="return CloseForm()"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="width: 200%; height: 200px; overflow: scroll;">
                                <asp:GridView ID="DG" runat="server" DataKeyNames="ParaID" AutoGenerateColumns="False"
                                    OnRowDataBound="DG_RowDataBound" OnRowDeleting="DG_RowDeleting" CssClass="grid-views" 
                                    OnSelectedIndexChanged="DG_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="ParameterName" HeaderText="Parameter Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SerialNumber" HeaderText="SR No">
                                            <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ShortName" HeaderText="Short Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Specified" HeaderText="Specified">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Method" HeaderText="Method">
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Delete" OnClientClick="return confirm('Do You Want To Delete Data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="SelectedRowStyle" />
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
