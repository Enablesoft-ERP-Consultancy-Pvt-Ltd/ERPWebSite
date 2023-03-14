<%@ Page Title="Finishing Detail" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmFinishingProcess_Issue_Receive.aspx.cs" Inherits="Masters_ReportForms_FrmFinishingProcess_Issue_Receive" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnprint.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDEmpName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Weaver. !!\n";
                    }
                    selectedindex = $("#<%=DDIssueChallanNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Folio. !!\n";
                    }

                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <asp:UpdatePanel ID="upda1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div style="margin: 1% 20% 0% 20%">
                <table style="width: 100%" border="1" cellspacing="0">
                   
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label Text="Company Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label3" Text="Process Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="95%" runat="server" 
                            OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lblfoliono" Text="Enter IssueChallan No." CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:TextBox ID="txtIssueChallanNo" CssClass="textb" Width="95%" runat="server" AutoPostBack="true"
                                OnTextChanged="txtIssueChallanNo_TextChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Emp Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList CssClass="dropdown" ID="DDEmpName" Width="95%" runat="server" OnSelectedIndexChanged="DDEmpName_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label2" Text="IssueChallan No." runat="server" CssClass="labelbold" />
                        </td>
                        <td style="width: 20%; border-style: dotted">
                            <asp:DropDownList CssClass="dropdown" ID="DDIssueChallanNo" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                        <%--<td style="width: 50%; border-style: dotted">
                            <asp:CheckBox Text="Final Folio" ID="chkfinalfolio" CssClass="checkboxbold" AutoPostBack="true"
                                runat="server" OnCheckedChanged="chkfinalfolio_CheckedChanged" />
                        </td>--%>
                    </tr>
                    
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">                           
                            <asp:Button ID="btnprint" Text="Preview" CssClass="buttonnorm" runat="server" OnClick="btnprint_Click" />                           
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnprint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
