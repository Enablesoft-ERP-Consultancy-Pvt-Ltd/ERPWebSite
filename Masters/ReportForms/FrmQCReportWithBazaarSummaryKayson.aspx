<%@ Page Title="QC Report With Receive Summary" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmQCReportWithBazaarSummaryKayson.aspx.cs" Inherits="Masters_ReportForms_FrmQCReportWithBazaarSummaryKayson" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function CheckAll(HObj, Obj) {

            if (document.getElementById(HObj).checked == true) {
                var gvcheck = document.getElementById(Obj);
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById(Obj);
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
    </script>
    
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <div style="margin: 0% 30% 0% 30%">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDprocess" runat="server" CssClass="dropdown" Width="200px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDprocess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                        <tr id="TRLoom" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
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
     <td valign="top">
         <asp:Label ID="lblemp" Text="Prod Units" runat="server" CssClass="labelbold" />
     </td>
     <td>
         <table>
             <tr>
                 <td>
                     <asp:CheckBox ID="chkallemp" runat="server" Text=" Chk For All" ForeColor="Red" CssClass="checkboxbold"
                         onclick="return CheckAll('CPH_Form_chkallemp','CPH_Form_chkpdunit');" />
                 </td>
             </tr>
             <tr>
                 <td>
                     <div style="overflow: scroll; height: 150px; width: 250px">
                         <asp:CheckBoxList ID="chkpdunit" runat="server" CssClass="checkboxbold">
                         </asp:CheckBoxList>
                     </div>
                 </td>
             </tr>
         </table>
     </td>
 </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfrom" Text="From Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" Text="To Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="calto" TargetControlID="txttodate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 100%">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkexportwithdetail" Text="Export With Detail" runat="server" CssClass="checkboxbold"
                                                            Visible="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <span style="margin-left: 120px">
                                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" /></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
