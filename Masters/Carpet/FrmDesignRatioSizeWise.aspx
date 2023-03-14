<%@ Page Title="Design Ratio Size wise" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmDesignRatioSizeWise.aspx.cs" Inherits="FrmDesignRatioSizeWise" EnableEventValidation="false"
    EnableViewStateMac="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
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
            window.location.href = "FrmDesignRatioSizeWise.aspx";
        }

        function Additem() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {

                window.open('AddItemName.aspx', '', 'width=550px,Height=500px');
            }
        }
        function Addquality() {
            var answer = confirm("Do you want to ADD?")

            var e = document.getElementById('<%=hncategory.ClientID %>');
            var Categoryid = e.value;
            var e1 = document.getElementById('<%=ddlQualityType.ClientID %>');
            var Itemid = e1.options[e1.selectedIndex].value;
            if (answer) {
                window.open('AddQuality.aspx?Category=' + Categoryid + '&Item=' + Itemid + '', '', 'width=701px,Height=501px');
            }
        }

        function AddShadecolor() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddShadeColor.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
            }
        }

    </script>
    <%-- <script type="text/javascript">
          function Jscriptvalidate() {
              $(document).ready(function () {
                  $("#<%=btnsave.ClientID %>").click(function () {
                      var Message = "";
                      var selectedindex = $("#<%=ddlQualityType.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please select Quality Type Name!!\n";
                      }
                      selectedindex = $("#<%=ddlQuality.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please Select Quality Name. !!\n";
                      }

                      selectedindex = $("#<%=ddlDesignName.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please Select Design Name. !!\n";
                      }
                      selectedindex = $("#<%=ddlColorName.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please Select Color Name. !!\n";
                      }
                      selectedindex = $("#<%=ddlSizeShape.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please Select Size Name. !!\n";
                      }
                      selectedindex = $("#<%=ddlShadeColor.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please Select Shade Color Name. !!\n";
                      }
                      selectedindex = $("#<%=ddlWoolType.ClientID %>").attr('selectedIndex');
                      if (selectedindex <= 0) {
                          Message = Message + "Please Select Wool Type Name. !!\n";
                      }



//                      var tbWoolConsump = document.getElementById('<%=tbWoolConsump.ClientID %>');
//                      if (tbWoolConsump.value == "" || tbWoolConsump.value=="0") {
//                          Message = Message + "Please Enter Wool Consumption Charges. !!\n";
//                      }
//                      var tbWeavingRate = document.getElementById('<%=tbWeavingRate.ClientID %>');
//                      if (tbWeavingRate.value == "" || tbWeavingRate.value=="0") {
//                          Message = Message + "Please Enter Weaving Charges. !!\n";
//                      }
//                     
//                      var tbEffectiveDate = document.getElementById('<%=tbEffectiveDate.ClientID %>');
//                      if (tbEffectiveDate.value == "" || tbEffectiveDate.value == "0" || tbEffectiveDate.value==null) {
//                          Message = Message + "Please Enter Effective Date. !!\n";
//                      }

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
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div style="height: 450px">
                <table cellspacing="5" cellpadding="5" width="100%">
                    <%-- <tr>
                        <td>
                            <asp:TextBox ID="txtsize" runat="server" CssClass="textb" Visible="false" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtid" runat="server" Visible="false" CssClass="textb" Width="80px"></asp:TextBox>
                             <asp:Label ID="lblSizeStatus" runat="server" Font-Bold="true" Visible="false" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="tdstyle" style="text-align: left; width: 15%;">
                            <asp:Label ID="lblQualityType" runat="server" Text="Quality Type" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlQualityType" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlQualityType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="refreshitemdr" runat="server" Style="display: none" OnClick="refreshitemdr_Click" />
                            &nbsp;
                            <asp:Button ID="btnadditem" runat="server" CssClass="buttonsmalls" OnClientClick="return Additem();"
                                Width="45px" Text=".." />
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlQualityType"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <%--<td style="text-align:right; width:2%" >
                            &nbsp;
                          
                        </td>--%>
                        <td class="tdstyle" style="text-align: left; width: 15%">
                            <asp:Label ID="lblQuality" runat="server" Text="Quality" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlQuality" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="refreshqualitydr" runat="server" Style="display: none" OnClick="refreshqualitydr_Click" />
                            &nbsp;
                            <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmalls" OnClientClick="return Addquality();"
                                Width="45px" Text=".." />
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddlQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <%--<td style="text-align:right; width:2%">
                          &nbsp;
                          
                        </td>--%>
                        <td class="tdstyle" style="text-align: left; width: 15%">
                            <asp:Label ID="lblDesign" runat="server" Text="Design" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlDesignName" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlDesignName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <%--  <td style="text-align:right; width:50%">
                        &nbsp;</td>--%>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: left; width: 15%">
                            <asp:Label ID="lblColor" runat="server" Text="Color" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlColorName" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlColorName_SelectedIndexChanged">
                                <%--<asp:ListItem Value="0" Text="----Select-----"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <%--<td style="text-align:right; width:2%" >
                            &nbsp;
                          
                        </td>--%>
                        <td class="tdstyle" style="text-align: left; width: 15%">
                            <asp:Label ID="lblSizeShape" runat="server" Text="Size Shape" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlSizeShape" runat="server" Width="200px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSizeShape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <%-- <td style="text-align:right; width:2%">
                          &nbsp;
                          
                        </td>--%>
                        <td style="text-align: left; width: 15%">
                            &nbsp;
                        </td>
                    </tr>
                    <%-- <tr><td colspan="3"> &nbsp;</td></tr>--%>
                    <tr>
                        <td class="tdstyle" style="text-align: left; width: 10%">
                            <asp:Label ID="lblShadeColor" runat="server" Text="Shade Color" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlShadeColor" runat="server" Width="200px" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:Button ID="refreshshadecolor" runat="server" Style="display: none" OnClick="refreshshadecolor_Click" />
                            &nbsp;
                            <asp:Button ID="btnaddshadecolor" runat="server" CssClass="buttonsmalls" OnClientClick="return AddShadecolor();"
                                Width="45px" Text=".." />
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlShadeColor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="lblConsumpAge" runat="server" Text="Consump in %age " Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="tbConsumpAge" runat="server" CssClass="textb" Width="150px" AutoPostBack="true"
                                onkeypress="return isNumberKey(event);" OnTextChanged="tbConsumpAge_TextChanged"></asp:TextBox>
                        </td>
                        <td style="text-align: left; width: 70%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: left; width: 10%">
                            <asp:Label ID="lblWoolType" runat="server" Text="Wool Type" Font-Bold="true"></asp:Label><br />
                            <asp:DropDownList ID="ddlWoolType" runat="server" Width="200px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="lblConsumpGms" runat="server" Text="Consump in gms" Font-Bold="true"></asp:Label><br />
                            <%--<b style="color: Red"> &nbsp; *</b>--%>
                            <asp:TextBox ID="tbConsumpGms" runat="server" CssClass="textb" Width="150px" AutoPostBack="true"
                                onkeypress="return isNumberKey(event);" OnTextChanged="tbConsumpGms_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle" style="text-align: left; width: 80%; padding-top: 5px">
                            <asp:Button ID="btnSubmit" CssClass="buttonnorm" runat="server" OnClick="btnSubmit_Click"
                                Text="Submit" Width="56px" OnClientClick="return preventMultipleSubmissions();" />
                            <%--<asp:Button ID="btnClear0" CssClass="buttonnorm" Text="New" runat="server" Width="56px"
                                 OnClientClick="return ClickNew()"  />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" /> --%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" runat="server" align="right">
                            &nbsp;
                            <%-- <asp:Button ID="btnSave" CssClass="buttonnorm" runat="server" OnClick="btnSave_Click" Text="Save" Width="56px"
                            OnClientClick="return preventMultipleSubmissions();" />  --%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="td1" runat="server" colspan="5">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div id="Gride" style="width: 620px; max-height: 120px; overflow: auto; float: left">
                                <%--OnSelectedIndexChanged="GDConsumptionMaster_SelectedIndexChanged"  OnRowDataBound="GDConsumptionMaster_RowDataBound"--%>
                                <asp:GridView ID="GDConsumptionMaster" runat="server" DataKeyNames="ID" CssClass="grid-views"
                                    Width="100%" EmptyDataText="No. Records found." OnRowDeleting="GDConsumptionMaster_RowDeleting"
                                    OnSelectedIndexChanged="GDConsumptionMaster_SelectedIndexChanged" OnRowDataBound="GDConsumptionMaster_RowDataBound"
                                    AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="gvheaders GVFixedHeader" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <%--<asp:TemplateField HeaderText="Update Status">
                                                <ItemTemplate>
                                                 <asp:LinkButton ID="LinkButton1" runat="server" Text="Click1" OnClick="LinkButton1_Click"/>                                                          

                                                </ItemTemplate>
                                                <ItemStyle Width="400px" />
                                            </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId2" Text='<%#Container.DataItemIndex+1 %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDyerColorName" Text='<%#Bind("DyerColorName") %>' runat="server" />
                                                <%--<asp:Label ID="lblId2" Text='<%#Bind("Id") %>' runat="server" />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyInPer" Text='<%#Bind("QtyInPer")%>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty Per">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyPercentage" Text='<%#Bind("Percentage")%>' runat="server" />%
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" Text='<%#Bind("ItemName") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" Text='<%#Bind("Id") %>' runat="server" />
                                                <asp:Label ID="lblDyerColorId" Text='<%#Bind("DyerColorId") %>' runat="server" />
                                                <asp:Label ID="lblSubItemId" Text='<%#Bind("SubItemID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:Button ID="btnDelete" runat="server" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                    Text="Delete" CommandName="Delete" />
                                                <%--<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                CommandName="Delete" ImageUrl="~/images/remove.png" Text="Delete" />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: left; width: 30%">
                            <asp:Label ID="lblAdjustAge" runat="server" Text="Adjust in %age" Font-Bold="true"></asp:Label>
                            <asp:TextBox ID="tbAdjustAge" runat="server" CssClass="textb" Width="150px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <%-- <asp:Label ID="lblTotalRemainingQty" runat="server" Text="Total Remaining Qty in gms" Font-Bold="true"></asp:Label>--%>
                            <span style="font-weight: bold">Total Remaining Qty in gms:</span>&nbsp;
                            <asp:Label ID="lblTotalRemainingQtyGms" runat="server" Text="Total " Font-Bold="false"></asp:Label><br />
                            <br />
                            <span style="font-weight: bold">Total Remaining Qty in Per:</span>&nbsp;
                            <asp:Label ID="lblTotalRemainingQtyPer" runat="server" Text="Total " Font-Bold="false"></asp:Label>%
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: left; width: 30%">
                            <%-- <asp:Label ID="lblTotalConsumpQty" runat="server" Text="Total Consump Qty in gms" Font-Bold="true"></asp:Label>--%>
                            <span style="font-weight: bold">Total Consump Qty in gms:</span>&nbsp;
                            <asp:Label ID="lblTotalConsumpQtyGms" runat="server" Text="Total" Font-Bold="false"></asp:Label><br />
                            <br />
                            <span style="font-weight: bold">Total Consump Qty in Per:</span>&nbsp;
                            <asp:Label ID="lblTotalConsumpQtyPer" runat="server" Text="Total" Font-Bold="false"></asp:Label>%
                        </td>
                        <td style="text-align: left; width: 30%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle" style="text-align: left; width: 30%">
                            <asp:Label ID="lblConsumpKgSqMt" runat="server" Text="Consumed in kg/Sq Yard." Font-Bold="true"></asp:Label>:
                            <asp:Label ID="lblConsumpKgSqMt2" runat="server" Font-Bold="false"></asp:Label>
                            <%-- <asp:Label ID="lblConsumpKgMt" runat="server" Text="Consumed In kg/Sq. Mt." Font-Bold="true" Visible="false"></asp:Label>
                            <asp:Label ID="lblConsumpKgMt2" runat="server"  Font-Bold="true" Visible="false"></asp:Label>--%>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnSave_Click"
                                OnClientClick="return preventMultipleSubmissions();" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Visible="false"
                                Text="Preview" OnClick="BtnPreview_Click" />
                            <asp:Button ID="Button2" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnColorId" runat="server" />
            <asp:HiddenField ID="hnSubItemId" runat="server" />
            <asp:HiddenField ID="hnID" runat="server" />
            <asp:HiddenField ID="hnID2" runat="server" />
            <asp:HiddenField ID="hnStatus" runat="server" />
            <asp:HiddenField ID="hnEditStatus" runat="server" />
            <asp:HiddenField ID="hncategory" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
