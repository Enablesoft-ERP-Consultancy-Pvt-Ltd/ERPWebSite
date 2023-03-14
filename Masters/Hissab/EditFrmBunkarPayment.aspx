<%@ Page Language="C#" Title="Edit Bunkar Payment" AutoEventWireup="true" EnableEventValidation="false"
    MasterPageFile="~/ERPmaster.master" CodeFile="EditFrmBunkarPayment.aspx.cs" Inherits="Masters_Hissab_EditFrmBunkarPayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%-- <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "EditFrmBunkarPayment.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        //window.setTimeout(function () { document.getElementById('txtKhapWidth').focus(); }, 0);

    </script>
    <script type="text/javascript" language="javascript">
        jQuery(function ($) {
            var focusedElementSelector = "";
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_beginRequest(function (source, args) {
                var fe = document.activeElement;
                focusedElementSelector = "";

                if (fe != null) {
                    if (fe.id) {
                        focusedElementSelector = "#" + fe.id;
                    } else {
                        // Handle Chosen Js Plugin
                        var $chzn = $(fe).closest('.chosen-container[id]');
                        if ($chzn.size() > 0) {
                            focusedElementSelector = '#' + $chzn.attr('id') + ' input[type=text]';
                        }
                    }
                }
            });

            prm.add_endRequest(function (source, args) {
                if (focusedElementSelector) {
                    $(focusedElementSelector).focus();
                }
            });
        });
    </script>
    <script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('DivBazaarCarpetReceive') != null) {
                xPos = $get('DivBazaarCarpetReceive').scrollLeft;
                yPos = $get('DivBazaarCarpetReceive').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('DivBazaarCarpetReceive') != null) {
                $get('DivBazaarCarpetReceive').scrollLeft = xPos;
                $get('DivBazaarCarpetReceive').scrollTop = yPos;
            }
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>
    <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $("[id*=GVCarpetReceive] input[type=text]").on("keypress", function (e) {
        if (e.keyCode == 13) {
            var next = $(this).closest("tr").next().find("input[type=text]"); ;
            if (next.length > 0) {
                next.focus();

            } else {
                next = $("[id*=GVCarpetReceive] input[type=text]").eq(0);
                next.focus();
            }
            return false;
        }
    })
</script>--%>
    <%--<script type="text/javascript">
    function showmodalpopup(){
  
       var mpePopup=$find("<%=ModalPopupExtender1.ClientID%>");   
       mpePopup.show()
       setTimeout(txtsetfocus(),100);          //time is in milliseconds
        } 
        function txtsetfocus(){
            $("<%=txtJobName.ClientID%>").focus();
        }
    </script>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; float: left; background-color: #E3E3E3">
                <div style="width: 100%; float: left; background-color: #E3E3E3">
                    <table width="50%">
                        <%-- <tr id="trprifix" runat="server">
                        <td>
                            <asp:HiddenField ID="hncomp" runat="server" />
                        </td>
                        <td align="center" class="tdstyle">
                            <asp:Label ID="lbl" Text="PreFix" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPrefix" runat="server" CssClass="textb" AutoPostBack="True" OnTextChanged="TxtPrefix_TextChanged"></asp:TextBox>
                        </td>
                        <td align="center" class="tdstyle">
                         <asp:Label ID="lblNo" Text=" No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtPostfix" runat="server" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtPostfix_TextChanged"></asp:TextBox>
                        </td>
                    </tr>--%>
                        <tr>
                            <td class="tdstyle">
                                <asp:HiddenField ID="hncomp" runat="server" />
                                <asp:TextBox ID="TxtChallanNo" runat="server" Width="80px" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                    Visible="false"></asp:TextBox>
                                <asp:Label ID="Label5" Text="CompanyName" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDCompanyName" AutoPostBack="true" runat="server" Width="200px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label7" Text="Contractor Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDContractorName" runat="server" AutoPostBack="true" Width="250px"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDContractorName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDContractorName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Quality Type" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDItemName" runat="server" Width="150px" AutoPostBack="true"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDItemName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="Label1" class="tdstyle" runat="server" Text="Bunkar Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDBunkarName" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDBunkarName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="Label2" class="tdstyle" runat="server" Text="Month" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDMonth" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="Label3" class="tdstyle" runat="server" Text="Year" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDYear" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;<br />
                                <asp:Button CssClass="buttonnorm preview_width" ID="BtnShow" Enabled="true" runat="server"
                                    Text="Show" OnClick="BtnShow_Click" Width="100px" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDDDPONO" runat="server">
                                &nbsp;
                            </td>
                            <td style="width: 100px">
                                &nbsp;
                            </td>
                            <td style="width: 100px">
                                &nbsp;
                            </td>
                            <td style="width: 100px">
                                &nbsp;
                            </td>
                            <td class="tdstyle" runat="server" id="td22">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 100%; float: left; background-color: #E3E3E3">
                    <table width="100%">
                        <tr>
                            <td>
                                <div id="DivBazaarCarpetReceive" style="height: 250px; overflow: auto; width: 1200px;">
                                    <asp:GridView ID="GVBunkarCarpetReceive" AutoGenerateColumns="False" runat="server"
                                        DataKeyNames="DetailId" CssClass="grid-views" OnRowDataBound="GVBunkarCarpetReceive_RowDataBound"
                                        OnRowDeleting="GVBunkarCarpetReceive_RowDeleting" ShowFooter="true">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <FooterStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                        CommandName="Delete"></asp:LinkButton></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sr.No">
                                                <ItemTemplate>
                                                    <%#Container.DisplayIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" Visible="false">
                                                <ItemTemplate>
                                                    <div style="width: 300px;">
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bunkar Name" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBunkarName" runat="server" Text='<%#Bind("BunkarName") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanNo" runat="server" Text='<%#Bind("ChallanNo") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ReceiveDate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtRecDate" runat="server" Width="90px" Text='<%#Bind("ReceiveDate","{0:dd-MMM-yyyy}") %>'
                                                        CssClass="textb" AutoPostBack="true" OnTextChanged="TxtRecDate_TextChanged"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtRecDate" Format="dd-MMM-yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemName" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("ItemName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quality" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Design" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("Design") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColor" runat="server" Text='<%#Bind("Color") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shape" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShape" runat="server" Text='<%#Bind("Shape") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Size" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BunkarQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBunkarQty" runat="server" Text='<%#Bind("Qty") %>' Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtBunkarQty" runat="server" Text='<%#Eval("Qty") %>' OnTextChanged="txtBunkarQty_TextChanged"
                                                        onkeypress="return isNumberKey1(event);" AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BZ Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBZWeight" runat="server" Text='<%#Bind("BZWeight") %>' Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtBZWeight" runat="server" Text='<%#Eval("BZWeight") %>' OnTextChanged="txtBZWeight_TextChanged"
                                                        onkeypress="return isNumberKey1(event);" AutoPostBack="True" Width="50px" onFocus="this.select()"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="St Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStWeight" runat="server" Text='<%#Bind("StWeight") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                <%-- <FooterTemplate>
                                           <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" Font-Bold="true" />
                                          </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penality">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenality" runat="server" Text='<%#Bind("PenaltyName") %>' Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="popup" runat="server" Text="Add Penality" OnClick="popup_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PenalityName">
                                                <ItemTemplate>
                                                    <div style="width: 120px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                        <asp:Label ID="lblPenalityName" runat="server" ToolTip='<%#Bind("PenaltyName") %>'
                                                            Text='<%#Bind("PenaltyName") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Area">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArea" runat="server" Text='<%#Bind("Area") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TArea">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTArea" runat="server" Text='<%#Bind("TArea") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lagat">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLagat" runat="server" Text='<%#Bind("Lagat") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Def Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDefWeight" runat="server" Text='<%#Bind("DefWeight") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extra Weight">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraWeight" runat="server" Text='<%#Bind("ExtraWeight") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actual %">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActualPercentage" runat="server" Text='<%#Bind("ActualPercentage") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Less %">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLessPercentage" runat="server" Text='<%#Bind("LessPercentage") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="W Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight PRate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWeightPRate" runat="server" Text='<%#Bind("WeightPRate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="W Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penality Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenalityAmount" runat="server" Text='<%#Bind("PenaltyAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight Penality">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWeightPenality" runat="server" Text='<%#Bind("WeightPAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Paid Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidAmount" runat="server" Text='<%#Bind("PaidAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WOrderId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemId" runat="server" Text='<%#Bind("Item_Id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblQualityId" runat="server" Text='<%#Bind("Qualityid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblItemFinishedId" runat="server" Text='<%#Bind("Item_Finished_Id") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProcess_Rec_Id" runat="server" Text='<%#Bind("Process_Rec_Id") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblProcess_Rec_Detail_ID" runat="server" Text='<%#Bind("Process_Rec_Detail_ID") %>'
                                                        Visible="false"></asp:Label>
                                                    <%-- <asp:Label ID="lblCalType" runat="server" Text='<%# Bind("CalType") %>' Visible="false"></asp:Label>--%>
                                                    <asp:Label ID="hnQty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblId" runat="server" Text='<%#Bind("Id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblDetailId" runat="server" Text='<%#Bind("DetailId") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <table width="80%">
                    <tr>
                        <td colspan="4">
                            <%--<asp:Label ID="Label25" Text="Remarks" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="TxtRemarks" Width="500px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="right">
                            <asp:Label ID="llMessageBox" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:CheckBox ID="chkForFinal" runat="server" Text="Check For Final" CssClass="labelnormalMM"
                                Font-Bold="true" Visible="true" />
                            &nbsp
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click"
                                OnClientClick="if (!validator()) return; this.disabled=true;this.value = 'wait ...';"
                                Width="50px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                Width="50px" />
                            <%--  &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnWeaver" Enabled="true"
                                runat="server" Text="Weaver Report" OnClick="BtnWeaver_Click" Width="100px" />
                                 &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnFinisher" Enabled="true"
                                runat="server" Text="Finisher Report" OnClick="BtnFinisher_Click" Width="120px" />   --%>
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="td4" runat="server">
                            <asp:Label ID="Label18" Text="Total Qty" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="txtTotalQty" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                        </td>
                        <td id="td5" runat="server">
                            <asp:Label ID="Label19" Text="Total Area" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="txtTotalArea" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                        </td>
                        <td id="td6" runat="server">
                            <asp:Label ID="Label20" Text="Total Penality" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="txtTotalPenality" Width="80px" ReadOnly="true" runat="server" CssClass="textb"
                                onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                        </td>
                        <td id="td7" runat="server">
                            <asp:Label ID="Label21" Text="Total Weight Penality" runat="server" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="txtTotalWeightPenality" Width="120px" ReadOnly="true" runat="server"
                                CssClass="textb" onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                        </td>
                        <td id="td8" runat="server">
                            <asp:Label ID="Label22" runat="server" Text="Paid Amount" CssClass="labelbold" />
                            <br>
                            <asp:TextBox ID="txtTotalPaidAmount" Width="80px" ReadOnly="true" runat="server"
                                CssClass="textb" onkeydown="return (event.keyCode!=13);" BackColor="#F0F8FF"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="7">
                            <div style="height: 250px; overflow: auto; width: 950px;">
                            </div>
                        </td>
                        <td id="qulitychk" runat="server" valign="top">
                            <div id="Div2" runat="server" style="height: 250px; overflow: auto;">
                            </div>
                        </td>
                        <td runat="server" id="tdordergrid">
                            <div id="Div1" runat="server" style="height: 250px; overflow: auto; width: 250px;">
                                <asp:HiddenField ID="hnBalQty" runat="server" Visible="false" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server"
                PopupControlID="pnlPopup" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground"
                CancelControlID="btnHide">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                <div class="header">
                    <asp:GridView ID="GVPenalty" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        Width="350px" OnRowDataBound="GVPenalty_RowDataBound">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts1" />
                        <RowStyle CssClass="gvrow1" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkboxitem" runat="server" AutoPostBack="true" OnCheckedChanged="Chkboxitem_CheckedChanged" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Penalty Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblPenalityName" Text='<%#Bind("PenalityName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtQty_TextChanged" onkeypress="return isNumberKey(event);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" Text='<%#Bind("rate") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAmt" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtAmt_TextChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPenalityId" Text='<%#Bind("PenalityId") %>' runat="server" Visible="false" />
                                    <asp:Label ID="lblPenalityType" Text='<%#Bind("PenalityType") %>' runat="server"
                                        Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="header">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnHide" runat="server" Text="Close" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style type="text/css">
        #mask
        {
            position: fixed;
            left: 0px;
            top: 0px;
            z-index: 4;
            opacity: 0.4;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
            filter: alpha(opacity=40); /* second!*/
            background-color: Gray;
            display: none;
            width: 100%;
            height: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ShowPopup() {
            $('#mask').show();
            $('#<%=pnlpopup2.ClientID %>').show();
        }
        function HidePopup() {
            $('#mask').hide();
            $('#<%=pnlpopup2.ClientID %>').hide();
        }
        $(".btnPwd").live('click', function () {
            HidePopup();
        });
    </script>
    <div id="mask">
    </div>
    <asp:Panel ID="pnlpopup2" runat="server" BackColor="White" Height="175px" Width="300px"
        Style="z-index: 111; background-color: White; position: absolute; left: 35%;
        top: 40%; border: outset 2px gray; padding: 5px; display: none">
        <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
            <tr style="background-color: #8B7B8B; height: 1px">
                <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                    align="center">
                    ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                        class="btnPwd" href="#">X</a>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Enter Password:
                </td>
                <td>
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                        ValidationGroup="m" OnClick="btnCheck_Click" />
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
