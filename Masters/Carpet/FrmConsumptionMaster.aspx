<%@ Page Title="Consumption Master" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmConsumptionMaster.aspx.cs" Inherits="FrmConsumptionMaster" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <style type="text/css">
        .HeaderFreez
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            z-index: 10;
        }
    </style>
    <script type="text/javascript">

        function SaveData() {

            var id = document.getElementById('<%=hncomp.ClientID %>').value;

            if (id != "20") {
                var answer = confirm("Do you want to Save?")

                if ((answer)) {
                    return true;
                }
                else {
                    return false;
                }

            }
            return true;
        }             
    
    </script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open("../../ReportViewer.aspx");
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function ClickNew() {
            window.location.href = "FrmConsumptionMaster.aspx";
        }

    </script>
    <%--  <script type="text/javascript">
          function Jscriptvalidate() {
              $(document).ready(function () {
                  $("#<%=btnSave.ClientID %>").click(function () {
                      var Message = "";
                      var selectedindex = $("#<%=ddlQualityType.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please select Quality Type Name!!\n";
                      }
                      selectedindex = $("#<%=ddlQuality.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please Select Quality Name. !!\n";
                      }
                     
                      selectedindex = $("#<%=ddlType.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please Select Type Name. !!\n";
                      }


                      var tbWoolConsump = document.getElementById('<%=tbWoolConsump.ClientID %>');
                      if (tbWoolConsump.value == "" || tbWoolConsump.value=="0") {
                          Message = Message + "Please Enter Wool Consumption Charges. !!\n";
                      }
                      var tbWeavingRate = document.getElementById('<%=tbWeavingRate.ClientID %>');
                      if (tbWeavingRate.value == "" || tbWeavingRate.value=="0") {
                          Message = Message + "Please Enter Weaving Charges. !!\n";
                      }
                     
                      var tbEffectiveDate = document.getElementById('<%=tbEffectiveDate.ClientID %>');
                      if (tbEffectiveDate.value == "" || tbEffectiveDate.value == "0" || tbEffectiveDate.value==null) {
                          Message = Message + "Please Enter Effective Date. !!\n";
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
    </script>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- <script type="text/javascript" language="javascript">
             Sys.Application.add_load(Jscriptvalidate);
                </script>--%>
            <div style="height: 450px">
                <table cellspacing="5" cellpadding="5" width="100%">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtsize" runat="server" CssClass="textb" Visible="false" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtid" runat="server" Visible="false" CssClass="textb" Width="80px"></asp:TextBox>
                            <asp:Label ID="lblSizeStatus" runat="server" Font-Bold="true" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <div id="SizeDate" runat="server">
                            <td class="tdstyle" style="text-align: left; width: 10%;">
                                <asp:Label ID="Label1" runat="server" Text="Date for Size" Font-Bold="true"></asp:Label><br />
                                <asp:TextBox ID="tbSizeDate" runat="server" CssClass="textb" Width="150px" ReadOnly="false"
                                    AutoPostBack="true" OnTextChanged="tbSizeDate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="tbSizeDate">
                                </asp:CalendarExtender>
                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbSizeDate"
                                                ErrorMessage="Please Enter Date" ForeColor="Red" SetFocusOnError="true">*</asp:RequiredFieldValidator>--%>
                                <%--<b style="color: Red"> &nbsp; *</b>--%>
                            </td>
                        </div>
                        <td class="tdstyle" style="text-align: left; width: 14%;">
                            <asp:Label ID="lblQualityType" runat="server" Text="Quality Type" Font-Bold="true"></asp:Label><span
                                style="color: Red">*</span><br />
                            <asp:DropDownList ID="ddlQualityType" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlQualityType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <%--<td style="text-align:right; width:2%" >
                            &nbsp;
                          
                        </td>--%>
                        <td class="tdstyle" style="text-align: left; width: 14%">
                            <asp:Label ID="lblQuality" runat="server" Text="Quality" Font-Bold="true"></asp:Label><span
                                style="color: Red">*</span><br />
                            <asp:DropDownList ID="ddlQuality" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <%--<td style="text-align:right; width:2%">
                          &nbsp;
                          
                        </td>--%>
                        <td class="tdstyle" style="text-align: left; width: 14%">
                            <asp:Label ID="lblDesign" runat="server" Text="Design" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlDesignName" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlDesignName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <%--  <td style="text-align:right; width:50%">
                        &nbsp;</td>--%>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: left; width: 14%">
                            <asp:Label ID="lblColor" runat="server" Text="Color" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlColorName" runat="server" Width="200px" CssClass="dropdown">
                                <%--<asp:ListItem Value="0" Text="----Select-----"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <%--<td style="text-align:right; width:2%" >
                            &nbsp;
                          
                        </td>--%>
                        <td class="tdstyle" style="text-align: left; width: 14%">
                            <asp:Label ID="lblSizeShape" runat="server" Text="Size Shape" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlSizeShape" runat="server" Width="200px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <%-- <td style="text-align:right; width:2%">
                          &nbsp;
                          
                        </td>--%>
                        <td class="tdstyle" style="text-align: left; width: 14%">
                            <asp:Label ID="lblType" runat="server" Text="Type" Font-Bold="true"></asp:Label><span
                                style="color: Red">*</span><br />
                            <asp:DropDownList ID="ddlType" runat="server" Width="200px" CssClass="dropdown">
                                <asp:ListItem Value="0" Text="----Select-----"></asp:ListItem>
                                <asp:ListItem Value="1" Text="TYPE-1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="TYPE-2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="TYPE-3"></asp:ListItem>
                                <asp:ListItem Value="4" Text="TYPE-4"></asp:ListItem>
                                <asp:ListItem Value="5" Text="TYPE-5"></asp:ListItem>
                                <asp:ListItem Value="6" Text="TYPE-6"></asp:ListItem>
                                <asp:ListItem Value="7" Text="TYPE-7"></asp:ListItem>
                                <asp:ListItem Value="8" Text="TYPE-8"></asp:ListItem>
                                <asp:ListItem Value="9" Text="TYPE-9"></asp:ListItem>
                                <asp:ListItem Value="10" Text="TYPE-10"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right; width: 10%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <table cellspacing="5" cellpadding="5" width="100%">
                                <tr>
                                    <td class="tdstyle" style="text-align: left; width: 14%">
                                        <asp:Label ID="lblWoolConsump" runat="server" Text="Wool Consump" Font-Bold="true"></asp:Label><span
                                            style="color: Red">*</span><br />
                                        <asp:TextBox ID="tbWoolConsump" runat="server" CssClass="textb" Width="150px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" style="text-align: left; width: 14%">
                                        <asp:Label ID="lblWeavingRate" runat="server" Text="Weaving Rate" Font-Bold="true"></asp:Label><span
                                            style="color: Red">*</span><br />
                                        <asp:TextBox ID="tbWeavingRate" runat="server" CssClass="textb" Width="150px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" style="text-align: left; width: 14%">
                                        <asp:Label ID="lblCommission" runat="server" Text="Commission" Font-Bold="true"></asp:Label><%--<span style="color:Red">*</span>--%><br />
                                        <asp:TextBox ID="tbCommission" runat="server" CssClass="textb" Width="150px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" style="text-align: left; width: 14%">
                                        <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date" Font-Bold="true"></asp:Label><span
                                            style="color: Red">*</span>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEffectiveDate"
                                                ErrorMessage="Please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>--%>
                                        <%--<b style="color: Red"> &nbsp; *</b>--%>
                                        <br />
                                        <asp:TextBox ID="tbEffectiveDate" runat="server" CssClass="textb" Width="150px" ReadOnly="false"></asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEffectiveDate" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"
                                                                                                                                                                            
    ErrorMessage="Invalid date format." ValidationGroup="f1" />--%>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEffectiveDate" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
                                                                                                                                                                            
    ErrorMessage="Invalid date format." ValidationGroup="f1" />--%>
                                        <%-- <asp:RegularExpressionValidator ID="regPurchaseDate" runat="server" Display="None"
ErrorMessage="Please enter date in valid format(mm/dd/yyyy)." ValidationGroup="CheckVal"
ControlToValidate="txtDate" SetFocusOnError="true" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"></asp:RegularExpressionValidator>--%>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="tbEffectiveDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="tdstyle" style="text-align: left; width: 40%; padding-top: 15px;">
                                        <%--  <asp:Button ID="btnSave" CssClass="buttonnorm" runat="server" OnClick="btnSave_Click" Text="Save" Width="56px" ValidationGroup="f1" 
                            OnClientClick="return preventMultipleSubmissions();"/> --%>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" UseSubmitBehavior="false" OnClick="btnSave_Click"
                                            Width="56px" OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';"
                                            CssClass="buttonnorm" />
                                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview"
                                            OnClick="BtnPreview_Click" />
                                        <asp:Button ID="btnClear0" CssClass="buttonnorm" Text="New" runat="server" Width="56px"
                                            OnClientClick="return ClickNew()" />
                                        <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="td1" runat="server" colspan="5">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div id="Gride" style="width: 945px; max-height: 250px; overflow: auto; float: left">
                                <%--OnSelectedIndexChanged="GDConsumptionMaster_SelectedIndexChanged"  OnRowDataBound="GDConsumptionMaster_RowDataBound"--%>
                                <asp:GridView ID="GDConsumptionMaster" runat="server" DataKeyNames="ID" CssClass="grid-views"
                                    Width="80%" EmptyDataText="No. Records found." OnPageIndexChanging="GDConsumptionMaster_PageIndexChanging"
                                    AllowPaging="True" PageSize="150" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <%--<asp:TemplateField HeaderText="Update Status">
                                                <ItemTemplate>

                                                 <asp:LinkButton ID="LinkButton1" runat="server" 
                    Text="Click1" 
                    OnClick="LinkButton1_Click"/>
                                                               

                                                </ItemTemplate>
                                                <ItemStyle Width="400px" />
                                            </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Quality Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityType" Text='<%#Bind("ITEM_NAME") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Design">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesign" Text='<%#Bind("DesignName")%>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color">
                                            <ItemTemplate>
                                                <asp:Label ID="lblColor" Text='<%#Bind("ColorName") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" Text='<%#Bind("SizeShape") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wool Consump">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblWoolConsump" runat="server" Text='<%#Bind("WoolConsump") %>'></asp:Label>--%>
                                                <asp:Label ID="lblWoolConsump" runat="server" Text='<%# String.IsNullOrEmpty(Eval("WoolConsump").ToString()) ?  "" :  string.Format("{0:0.000}",Convert.ToDouble(Eval("WoolConsump").ToString())) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weaving">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblWeaving" runat="server" Text='<%#Bind("WeavingRate") %>'></asp:Label>--%>
                                                <asp:Label ID="lblWeaving" runat="server" Text='<%#String.IsNullOrEmpty(Eval("WeavingRate").ToString()) ? "" : string.Format("{0:0.00}",Convert.ToDouble(Eval("WeavingRate").ToString())) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#Bind("EffectiveDate", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" Text='<%#Bind("TypeName") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Commission">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommission" Text='<%#Bind("Commission") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" Text='<%#Bind("ID") %>' runat="server" />
                                                <%--   <asp:Label ID="lblQualityTypeId" Text='<%#Bind("QualityTypeId") %>' runat="server" />--%>
                                                <asp:Label ID="lblQualityId" Text='<%#Bind("QualityId") %>' runat="server" />
                                                <asp:Label ID="lblDesignId" Text='<%#Bind("DesignID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnID" runat="server" />
            <asp:HiddenField ID="hncomp" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
