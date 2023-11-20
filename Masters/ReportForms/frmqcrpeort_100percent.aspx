<%@ Page Title="100%_Qc report" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmqcrpeort_100percent.aspx.cs" Inherits="Masters_ReportForms_frmqcrpeort_100percent" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                $("#<%=btngetdata.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDCompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDprocessname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Process Name. !!\n";
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div style="margin: 0% 20% 0% 20%">
                <table style="width: 100%;" border="1" cellspacing="2">
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Company Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 70%; border-style: dotted">
                            <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="90%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label2" Text="Process Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 70%; border-style: dotted">
                            <asp:DropDownList ID="DDprocessname" runat="server" CssClass="dropdown" Width="90%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label4" Text="Customer Code" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 70%; border-style: dotted">
                            <asp:DropDownList ID="DDCustomerCode" runat="server" CssClass="dropdown" Width="90%"
                                AutoPostBack="true" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label5" Text="Order No" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 70%; border-style: dotted">
                            <asp:DropDownList ID="DDOrderNo" runat="server" CssClass="dropdown" Width="90%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted; width: 30%">
                            &nbsp
                        </td>
                        <td style="border-style: dotted; width: 70%">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <asp:Label ID="lblfromdate" CssClass="labelbold" Text="From Date" runat="server" />
                                        <br />
                                        <asp:TextBox ID="txtfromdate" CssClass="textb" runat="server" Width="90%"></asp:TextBox>
                                        <asp:CalendarExtender ID="calfromdate" runat="server" TargetControlID="txtfromdate"
                                            Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="width: 50%">
                                        <asp:Label ID="Label3" CssClass="labelbold" Text="To Date" runat="server" />
                                        <br />
                                        <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="90%"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                            Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted; width: 30%">
                            &nbsp
                        </td>
                        <td style="border-style: dotted; width: 70%" align="right">
                            <asp:CheckBox ID="ChkDATETIMEREPORTFORBAZAR" runat="server" Text="DATE TIME REPORT FOR BAZAR"
                                CssClass="checkboxbold" Visible="false" />
                            <asp:Button ID="btngetdata" runat="server" CssClass="buttonnorm" Text="Export to Excel"
                                OnClick="btngetdata_Click" />
                            <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="CLOSE" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btngetdata" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
