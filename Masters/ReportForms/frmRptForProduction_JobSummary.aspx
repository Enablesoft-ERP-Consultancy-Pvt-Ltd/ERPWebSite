<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmRptForProduction_JobSummary.aspx.cs"
    Inherits="Masters_ReportForms_frmRptForProduction_JobSummary" MasterPageFile="~/ERPmaster.master"
    Title="PRODUCTION/JOB SUMMARY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            if (document.getElementById('CPH_Form_DDjob').selectedIndex <= "0") {
                alert('Plz Select Job...')
                document.getElementById('CPH_Form_DDjob').focus()
                return false;
            }
            if (document.getElementById('CPH_Form_txtIdNo').value == "") {
                alert('Plz Enter ID No...')
                document.getElementById('CPH_Form_txtIdNo').focus()
                return false;
            }
        }        
    </script>
    <script runat="server">
        protected void Checkexportchecked(object sender, System.EventArgs e)
        {
            ChkForWithoutWeavingProcess.Visible = false;
            chkwithstockdetail.Visible = false;
            chkwithstockdetail.Checked = false;
            TRForProcessWiseSummary.Visible = false;
            ChkForProcessWiseSummary.Checked = false;
            
            if (chkexport.Checked == true)
            {
                chkwithstockdetail.Visible = true;
                TRForProcessWiseSummary.Visible = true;
            }
        }
    </script>
    <asp:UpdatePanel ID="updatpanel1" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 900px; max-height: 1000px">
                    <div style="width: 319px; margin-left: 300px; height: 146px; margin-top: 20px">
                        <asp:Panel runat="server" ID="panel1" Style="border-style: groove; width: 350px;
                            border-color: Teal; border-width: 1px; background-color: #DEB887">
                            <div style="padding: 0px 0px 0px 20px">
                                <table style="width: 300px; height: 158px;">
                                    <tr runat="server" id="TrCustomerCode">
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCustomerOrderNo" runat="server" Width="200px" CssClass="dropdown"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDCustomerOrderNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="TrOrderNo">
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Order No" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDOrderNo" runat="server" Width="200px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbljob" runat="server" Text="Job" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDjob" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDjob_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUnitName" runat="server" Text="UnitName" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDUnitName" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRcategoty" runat="server">
                                        <td>
                                            <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="200px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRddItemName" runat="server">
                                        <td>
                                            <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="200px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDQuality" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDDesign" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDColor" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDShape" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="200px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDSize" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                                            <br />
                                            <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDate" runat="server" Text="From Date" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromdate" runat="server" Width="95px" CssClass="textb" Height="23px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalenderExtendertxtdate" runat="server" TargetControlID="txtFromdate"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="To Date" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" runat="server" Width="95px" CssClass="textb" Height="23px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TDempwise" runat="server" visible="false">
                                            <asp:CheckBox ID="chkempwise" CssClass="checkboxbold" Text="EMP. WISE" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkexport" Text="Export" CssClass="checkboxbold" runat="server"
                                                AutoPostBack="true" OnCheckedChanged="Checkexportchecked" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkwithstockdetail" Text="Check With Stock No. Detail" CssClass="checkboxbold"
                                                runat="server" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr id="TRDetailWithAllProcess" runat="server" visible="false">
                                        <td colspan="2">
                                            <asp:CheckBox ID="ChkDetailWithAllProcess" Text="Stock No. Detail With All ProcessSize"
                                                CssClass="checkboxbold" runat="server" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr id="TRForSummaryDetail" runat="server" visible="false">
                                        <td colspan="2">
                                            <asp:CheckBox ID="ChkForSummaryDetail" Text="For Summary Detail" CssClass="checkboxbold"
                                                runat="server" Visible="true" />
                                        </td>
                                    </tr>
                                    <tr id="TRFORDAY" runat="server" visible="false">
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkforday" Text="For Date Report" CssClass="checkboxbold" runat="server"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                    <tr id="TRForFinishingDetail" runat="server" visible="false">
                                        <td colspan="2">
                                            <asp:CheckBox ID="ChkForFinishingDetail" Text="For Finishing Detail" CssClass="checkboxbold"
                                                runat="server" />
                                        </td>
                                    </tr>
                                     <tr id="TRForWithoutWeavingProcess" runat="server" visible="false">
                                        <td colspan="2">
                                            <asp:CheckBox ID="ChkForWithoutWeavingProcess" Text="For Without Weaving Process" CssClass="checkboxbold"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    
                                     <tr id="TRForProcessWiseSummary" runat="server" visible="false">
                                        <td colspan="2">
                                            <asp:CheckBox ID="ChkForProcessWiseSummary" Text="For Process Wise Summary" CssClass="checkboxbold"
                                                runat="server" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Print" Width="50px"
                                                OnClick="btnprint_Click" />
                                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" Width="50px"
                                                OnClientClick="return CloseForm();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnprint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
