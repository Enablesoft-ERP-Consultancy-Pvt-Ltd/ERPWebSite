<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmprocesspendingReport.aspx.cs"
    Inherits="Masters_ReportForms_frmprocesspendingReport" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server" ID="Page">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        // $(document).ready(function () {
        function jScript() {
            $("#CPH_Form_btnpreview").click(function () {
                var Message = "";
                if ($("#CPH_Form_DDCompanyName").val() == "") {
                    Message = Message + "Please,Select Company Name!!!\n";
                }
                if ($("#CPH_Form_DDUnitName").val() == "") {
                    Message = Message + "Please,Select Unit Name!!!\n";
                }
                if ($("#CPH_Form_ddJob")) {
                    var selectedIndex = $('#CPH_Form_ddJob').attr('selectedIndex');

                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Job Name !!!\n";
                    }
                }


                if (Message == "") {
                    return true;
                }
                else {
                    alert(Message);
                    return false;
                }
            });

        }
        // });
     
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScript);
            </script>
            <div class="Maindiv">
                <div class="CenterDiv">
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblCompanyName" runat="server" CssClass="labelbold" Text="COMPANY NAME"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblUnitName" runat="server" CssClass="labelbold" Text="UNIT NAME"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="DDUnitName" runat="server" Width="250px" CssClass="dropdown"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblJob" runat="server" CssClass="labelbold" Text="JOB NAME"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="ddJob" runat="server" Width="250px" CssClass="dropdown" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divProdcode" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblProdcode" runat="server" CssClass="labelbold" Text="PRODUCT CODE"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:TextBox ID="TxtProductCode" runat="server" Width="148px" />
                        </div>
                    </div>
                    <div id="divCategory" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="CATEGORY"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="DDCategory" runat="server" Width="250px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lbluni" runat="server" CssClass="labelbold" Text="ARTICLE NAME"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="DDArticleName" runat="server" Width="250px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDArticleName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divQuality" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblqualityname" runat="server" Text="QUALITY NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="ddquality" runat="server" Width="250px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="ddquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divDesign" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblDesign" runat="server" Text="DESIGN NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="DDDesign" runat="server" Width="250px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divColor" runat="server">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblColorname" runat="server" Text="COLOUR NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="DDColor" runat="server" Width="250px" CssClass="dropdown" OnSelectedIndexChanged="DDColor_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divShape" runat="server">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="Label3" runat="server" Text="SHAPE NAME" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="DDShape" runat="server" Width="250px" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="div4" runat="server">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblSize" runat="server" Text="SIZE" CssClass="labelbold"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="ddSize" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divshade" runat="server" visible="false">
                        <div style="width: 150px; float: left; text-align: right; padding-right: 10px">
                            <asp:Label ID="lblshade" runat="server" CssClass="labelbold" Text="SHADE NAME"></asp:Label>
                        </div>
                        <div style="height: 20px">
                            <asp:DropDownList ID="ddlshade" runat="server" Width="250px" CssClass="dropdown">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="text-align: right; width: 260px">
                        <asp:CheckBox Text="Check For Date" ID="chkDate" CssClass="labelbold" runat="server" />

                        <asp:CheckBox Text="For Excel Export" ID="ChkForExcel" CssClass="labelbold" runat="server" Visible="false" />
                    </div>
                    <div id="divdate" runat="server" style="margin-left: 60px">
                        <table style="width: 400px">
                            <tr>
                                <td>
                                    <asp:Label Text="From Date :" CssClass="labelbold" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFdate" CssClass="textb" runat="server" Width="80px" />
                                    <asp:CalendarExtender ID="calF" runat="server" TargetControlID="txtFdate" Format="dd-MMM-yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" Text="To Date :" CssClass="labelbold" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTdate" CssClass="textb" runat="server" Width="80px" />
                                    <asp:CalendarExtender ID="CalT" runat="server" TargetControlID="txtTdate" Format="dd-MMM-yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <div style="float: right; width: 250px; margin-top: 10px">
                            <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" Width="75px"
                                OnClick="btnpreview_Click" />
                            &nbsp
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" Width="75px"
                                OnClientClick="return CloseForm();" />
                        </div>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="lblMessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                Font-Bold="true" />
                        </div>
                    </div>
                </div>
                <div>
                </div>
            </div>
        </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>
