<%@ Page Title="AQL REPORT" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmAqlreport.aspx.cs" Inherits="Masters_ReportForms_frmAqlreport" %>

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
                            <asp:Label Text="Company Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 70%; border-style: dotted">
                            <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="90%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Process Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 70%; border-style: dotted">
                            <asp:DropDownList ID="DDprocessname" runat="server" CssClass="dropdown" Width="90%">
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
                                        <asp:Label ID="Label2" CssClass="labelbold" Text="To Date" runat="server" />
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
                            <asp:Button ID="btngetdata" runat="server" CssClass="buttonnorm" Text="GET DATA"
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
            <table style="width: 100%">
                <tr>
                    <td>
                        <div style="width: 100%; overflow: auto; max-height: 500px">
                            <asp:GridView ID="DGDetail" AutoGenerateColumns="false" runat="server" CssClass="grid-view"
                                Width="100%" EmptyDataText="No data fetched for this combination.." OnRowDataBound="DGDetail_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblaqldate" Text='<%#Bind("Aqldate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblaqllotno" Text='<%#Bind("Aqllotno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Pcs">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbltotalpcs" runat="server" Text='<%#Bind("totalpcs") %>' CssClass="labelbold"
                                                ForeColor="DarkOrange" OnClick="lbltotalpcs_click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sample Pcs">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblsamplepcs" runat="server" Text='<%#Bind("samplepcs") %>' CssClass="labelbold"
                                                ForeColor="DarkOrange" OnClick="lblsamplepcs_click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fail Pcs">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblfailpcs" runat="server" Text='<%#Bind("failpcs") %>' CssClass="labelbold"
                                                ForeColor="DarkOrange" OnClick="lblfailpcs_click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AQL done by">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempname" Text='<%#Bind("empname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Result">
                                        <ItemTemplate>
                                            <asp:Label ID="lblresult" Text='<%#Bind("Aqlstatus") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblaqlid" Text='<%#Bind("Id") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td id="TDexportexcel" runat="server" visible="false" align="right">
                        <asp:Button ID="btnexporttoexcel" Text="Export to Excel" CssClass="buttonnorm" runat="server"
                            OnClick="btnexporttoexcel_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexporttoexcel" />
            <asp:AsyncPostBackTrigger ControlID="DGDetail" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
