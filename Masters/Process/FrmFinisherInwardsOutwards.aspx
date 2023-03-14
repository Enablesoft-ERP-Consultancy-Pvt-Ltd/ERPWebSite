<%@ Page Language="C#" Title="Finisher Inwards/Outwards" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmFinisherInwardsOutwards.aspx.cs" Inherits="Masters_Process_FrmFinisherInwardsOutwards" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmFinisherInwardsOutwards.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(btn) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById(btn).click();
            }
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function YourFunctionName(msg) {
            var txt = msg;
            alert(txt);
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {
                        if (inputlist[i].disabled) {
                        }
                        else {
                            inputlist[i].checked = true;
                        }

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr id="Tr1" runat="server">
                        <td id="TDChallanNo" runat="server" visible="true" class="tdstyle">
                            <asp:Label ID="lbl" Text="Receipt No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtEditChallanNo" runat="server" Width="100px"
                                AutoPostBack="True" OnTextChanged="TxtEditChallanNo_TextChanged"></asp:TextBox>
                        </td>
                        <td id="Td1" runat="server" class="tdstyle">
                            <asp:Label ID="Label10" Text="Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtreceiveDate" runat="server" Width="90px" AutoPostBack="true"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtreceiveDate">
                            </asp:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" Text="  Company Name" runat="server" CssClass="labelbold" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b>
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" CssClass="dropdown" Width="200px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddCompName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCompName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text="Process" runat="server" CssClass="labelbold" />
                            &nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                            <asp:DropDownList ID="ddprocess" runat="server" CssClass="dropdown" Width="150px"
                                OnSelectedIndexChanged="ddprocess_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddprocess"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" Text=" Emp/Contractor" runat="server" CssClass="labelbold" />
                            &nbsp;&nbsp; <b style="color: Red">*</b>&nbsp;&nbsp;
                            <%-- <asp:CheckBox ID="ChkForEdit" runat="server" CssClass="checkboxnormal" Text="For Edit"
                                                OnCheckedChanged="ChkForEdit_CheckedChanged" AutoPostBack="True" />--%>
                            <br />
                            <asp:DropDownList ID="ddemp" runat="server" CssClass="dropdown" Width="200px" OnSelectedIndexChanged="ddemp_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddemp"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label19" Text="Last Receipt No:" runat="server" CssClass="labelbold" />
                            <asp:Label ID="lblLastReceiptNo" runat="server" CssClass="labelbold" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
                <table style="width: 90%">
                    <tr>
                        <td>
                            <div style="width: 100%">
                                <div style="width: 90%; float: left">
                                    <table>
                                        <tr>
                                            <td style="width: 70%">
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label ID="lblLegendText" runat="server" Text="Pending Details" ForeColor="Red"
                                                            Font-Bold="true"></asp:Label></legend>
                                                    <div style="overflow: auto; max-height: 200px">
                                                        <asp:GridView ID="GVIssued" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                            OnSelectedIndexChanged="GVIssued_SelectedIndexChanged" OnRowDataBound="GVIssued_RowDataBound"
                                                            OnSorting="GVIssued_Sorting" AllowSorting="true">
                                                            <HeaderStyle CssClass="gvheaders" ForeColor="White" />
                                                            <AlternatingRowStyle CssClass="gvalts" />
                                                            <RowStyle CssClass="gvrow" />
                                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                                            <Columns>
                                                                <%--<asp:BoundField DataField="IssueOrderId" HeaderText="WOrderNo" >
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:BoundField>--%>
                                                                <asp:TemplateField HeaderText="BZRep No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBZRepNo" runat="server" Text='<%#Bind("BZRepNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Quality" SortExpression="Quality">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="400px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Design" SortExpression="DesignName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDesignName" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="300px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Color" SortExpression="ColorName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblColorName" runat="server" Text='<%#Bind("ColorName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Shape" SortExpression="ShapeName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblShapeName" runat="server" Text='<%#Bind("ShapeName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Size" SortExpression="FinishingMtSize">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinishingMtSize" runat="server" Text='<%#Bind("FinishingMtSize") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Bal Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblHBalPcs" runat="server" Text='<%#Convert.ToInt32(Eval("orderedqty")) -Convert.ToInt32(Eval("Receivedqty")) %>'></asp:Label>
                                                                        <%-- <asp:Label ID="lblHBalPcs" runat="server" Text='<%#Bind("HBalPcs") %>'></asp:Label>--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField HeaderText="Stock No" >
                                                                                <ItemTemplate> 
                                                                                 <div style="width: 120px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                  <asp:Label ID="lblStockNo" runat="server" ToolTip='<%#Bind("StockNo") %>' Text='<%#Bind("StockNo") %>'></asp:Label>
                                                                                 </div> 
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Customer Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustomerCode" runat="server" Text='<%#Bind("CustomerCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Get StockNo">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="BtnShowStockNo" runat="server" Text="Get StockNo" CssClass="dropdown"
                                                                            OnClick="BtnShowStockNo_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFTArea" runat="server" Text='<%# Bind("FTArea") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblFinisherId" runat="server" Text='<%# Bind("FinisherId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("Item_Id") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("ColorId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("SizeId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblCompanyId" runat="server" Text='<%# Bind("CompanyId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblICustomerId" runat="server" Text='<%# Bind("ICustomerId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblItemFinishedId" runat="server" Text='<%# Bind("Item_Finished_Id") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <%--<asp:Label ID="lblIssueOrderId" runat="server" Text='<%# Bind("IssueOrderId") %>' Visible="false"></asp:Label>--%>
                                                                        <%-- <asp:Label ID="hnBalanceQty" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Qty").ToString(),DataBinder.Eval(Container.DataItem, "finishedid").ToString(),DataBinder.Eval(Container.DataItem, "Issue_Detail_Id").ToString()) %>'
                                                                                        Visible="false" />--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </fieldset>
                                            </td>
                                            <td id="TDstockno" runat="server" visible="true" valign="top" style="width: 30%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div style="max-height: 200px; overflow: auto; margin-left: 10%">
                                                                <asp:GridView ID="DGStockDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Stock No. found to Receive."
                                                                    AllowPaging="true" PageSize="50" OnPageIndexChanging="DGStockDetail_PageIndexChanging">
                                                                    <HeaderStyle CssClass="gvheaders" />
                                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                                    <RowStyle CssClass="gvrow" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="Chkboxitem" runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="ItemDescription">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblItemDescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Stock No">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr id="Trsave" runat="server" visible="false">
                                                                    <td>
                                                                        <asp:Label ID="lbltpcs" Text="Total Pcs" CssClass="labelbold" runat="server" />
                                                                        <br />
                                                                        <asp:TextBox ID="txttotalpcsgrid" runat="server" CssClass="textb" Width="90px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnsavefrmgrid" Text="Save" CssClass="buttonnorm" runat="server"
                                                                            OnClick="btnsavefrmgrid_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 80%">
                    <tr>
                        <td>
                            <div style="width: 80%">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="Label1" runat="server" Text="Issue To" ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                                    <div style="width: 80%; float: left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="Label2" Text="Process" runat="server" CssClass="labelbold" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                                        <asp:DropDownList ID="DDIssueJobType" runat="server" CssClass="dropdown" Width="150px"
                                                            OnSelectedIndexChanged="DDIssueJobType_SelectedIndexChanged" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        <%--  <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddprocess"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>--%>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="Label6" Text=" Emp/Contractor" runat="server" CssClass="labelbold" />
                                                        &nbsp;&nbsp; <b style="color: Red">*</b>&nbsp;&nbsp;
                                                        <br />
                                                        <asp:DropDownList ID="DDIssueContractorName" runat="server" CssClass="dropdown" Width="200px"
                                                            OnSelectedIndexChanged="DDIssueContractorName_SelectedIndexChanged" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddemp"
                                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                        </cc1:ListSearchExtender>
                                                    </td>
                                                    <td id="Td4" runat="server" class="tdstyle">
                                                        <asp:Label ID="Label9" Text="Stock No" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox CssClass="textb" ID="txtStockNo" runat="server" Width="90px" AutoPostBack="true"
                                                            OnTextChanged="txtStockNo_TextChanged"></asp:TextBox>
                                                    </td>
                                                    <td id="Td2" runat="server" class="tdstyle" visible="false">
                                                        <asp:Label ID="Label7" Text="Issue Date" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox CssClass="textb" ID="txtIssueDate" runat="server" Width="90px" AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                            TargetControlID="txtIssueDate">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td id="Td3" runat="server" class="tdstyle" visible="false">
                                                        <asp:Label ID="Label8" Text="Required Date" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox CssClass="textb" ID="txtRequiredDate" runat="server" Width="90px" AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                                            TargetControlID="txtRequiredDate">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 80%">
                    <tr>
                        <td>
                            <div style="width: 80%">
                                <td>
                                    <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red"
                                        Font-Size="15px" CssClass="labelbold"></asp:Label>
                                </td>
                                <%--<fieldset>
                                                <legend><asp:Label ID="Label12" runat="server" Text="Enter StockNo" ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                                                <div style="width: 80%; float: left">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                             
                                                             <td id="Td5" runat="server" class="tdstyle">
                                                            &nbsp;<asp:LinkButton ID="popup" runat="server" Text="Add Penality" Width="100px" OnClick="popup_Click"></asp:LinkButton>
                                                            </td>  
                                                                                                                       
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                 </fieldset>--%>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <div style="width: 100%">
                                <div style="width: 80%; float: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label ID="Label17" runat="server" Text="Receive Details" ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                                                    <div style="overflow: auto; max-height: 200px">
                                                        <asp:GridView ID="GVReceive" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                            OnRowDataBound="GVReceive_RowDataBound">
                                                            <HeaderStyle CssClass="gvheaders" />
                                                            <AlternatingRowStyle CssClass="gvalts" />
                                                            <RowStyle CssClass="gvrow" />
                                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                                            <Columns>
                                                                <%--<asp:BoundField DataField="IssueOrderId" HeaderText="WOrderNo" >

                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:BoundField>--%>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblText" runat="server" Text="Update"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="BZRep No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBZRepNo" runat="server" Text='<%#Bind("BZRepNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Quality">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="400px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Design">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDesignName" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Color">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblColorName" runat="server" Text='<%#Bind("ColorName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Shape">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblShapeName" runat="server" Text='<%#Bind("ShapeName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Size">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinishingMtSize" runat="server" Text='<%#Bind("FinishingMtSize") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rec Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRecPcs" runat="server" Text='<%#Bind("Qty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reject Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQualityType" runat="server" Text='<%#Bind("QualityType") %>' Visible="false"></asp:Label>
                                                                        <asp:DropDownList ID="DDReceiveRejectPcs" CssClass="dropdown" Width="120px" runat="server"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="DDReceiveRejectPcs_SelectedIndexChanged">
                                                                            <asp:ListItem Value="1">Finished</asp:ListItem>
                                                                            <asp:ListItem Value="3">Rejected</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Add Penality">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="popup" runat="server" Text="Add Penality" OnClick="popup_Click"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Penality">
                                                                    <ItemTemplate>
                                                                        <div style="width: 120px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                            <asp:Label ID="lblPenalityName" runat="server" ToolTip='<%#Bind("PenalityName") %>'
                                                                                Text='<%#Bind("PenalityName") %>'></asp:Label>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PenalityAmt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPenalityAmt" runat="server" Text='<%#Bind("PenalityAmt") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRecRate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Area">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRecArea" runat="server" Text='<%#Bind("Area") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRecAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TDS Amt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRecTDSAmt" runat="server" Text='<%#Bind("TDSAmt") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinisherId" runat="server" Text='<%# Bind("FinisherId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("Item_Id") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("ColorId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("SizeId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblCompanyId" runat="server" Text='<%# Bind("CompanyId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblItemFinishedId" runat="server" Text='<%# Bind("Item_Finished_Id") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIssueOrderId" runat="server" Text='<%# Bind("IssueOrderId") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblProcess_Rec_Id" runat="server" Text='<%# Bind("Process_Rec_Id") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblProcess_Rec_Detail_Id" runat="server" Text='<%# Bind("Process_Rec_Detail_Id") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblCalType" runat="server" Text='<%# Bind("CalType") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblOrderId" runat="server" Text='<%# Bind("OrderId") %>' Visible="false"></asp:Label>
                                                                        <%-- <asp:Label ID="hnBalanceQty" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Qty").ToString(),DataBinder.Eval(Container.DataItem, "finishedid").ToString(),DataBinder.Eval(Container.DataItem, "Issue_Detail_Id").ToString()) %>'
                                                                                        Visible="false" />--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <div style="width: 100%">
                                <div style="width: 80%; float: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label ID="Label18" runat="server" Text="Issue Details" ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                                                    <div style="overflow: auto; max-height: 200px">
                                                        <asp:GridView ID="GVIssue" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                                            OnRowDataBound="GVIssue_RowDataBound" OnRowDeleting="GVIssue_RowDeleting">
                                                            <HeaderStyle CssClass="gvheaders" />
                                                            <AlternatingRowStyle CssClass="gvalts" />
                                                            <RowStyle CssClass="gvrow" />
                                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                                            <Columns>
                                                                <%--<asp:BoundField DataField="IssueOrderId" HeaderText="WOrderNo" >

                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:BoundField>--%>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                                            CommandName="Delete"></asp:LinkButton></span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="BZRep No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIBZRepNo" runat="server" Text='<%#Bind("BZRepNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Job Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIJobType" runat="server" Text='<%#Bind("JobType") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="400px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Contractor Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIFinisherName" runat="server" Text='<%#Bind("FinisherName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="400px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Issue Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIssuePcs" runat="server" Text='<%#Bind("Qty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Size">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIFinishingMtSize" runat="server" Text='<%#Bind("FinishingMtSize") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Quality">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIQuality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="400px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Design">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIDesignName" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Color">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIColorName" runat="server" Text='<%#Bind("ColorName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Shape">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIShapeName" runat="server" Text='<%#Bind("ShapeName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIRecRate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Area">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIRecArea" runat="server" Text='<%#Bind("Area") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIRecAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="StockNo">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStockNo2" runat="server" Text='<%#Bind("StockNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIFinisherId" runat="server" Text='<%# Bind("FinisherId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblFinisherJobId" runat="server" Text='<%# Bind("FinisherJobId") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblFinisherNameId" runat="server" Text='<%# Bind("FinisherNameId") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIItemId" runat="server" Text='<%# Bind("Item_Id") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIQualityid" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIColorid" runat="server" Text='<%# Bind("ColorId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIdesignid" runat="server" Text='<%# Bind("designId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIshapeid" runat="server" Text='<%# Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIsizeid" runat="server" Text='<%# Bind("SizeId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblICompanyId" runat="server" Text='<%# Bind("CompanyId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIItemFinishedId" runat="server" Text='<%# Bind("Item_Finished_Id") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblIIssueOrderId" runat="server" Text='<%# Bind("IssueOrderId") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblProcessRecDetailId" runat="server" Text='<%#Bind("BazaarProcess_Rec_Detail_Id") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="Issue_Detail_Id" runat="server" Text='<%# Bind("Issue_Detail_Id") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblProcessRecId" runat="server" Text='<%#Bind("BazaarProcess_Rec_Id") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblStockNo" runat="server" Text='<%#Bind("StockNo") %>' Visible="false"></asp:Label>
                                                                        <%-- <asp:Label ID="hnBalanceQty" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Qty").ToString(),DataBinder.Eval(Container.DataItem, "finishedid").ToString(),DataBinder.Eval(Container.DataItem, "Issue_Detail_Id").ToString()) %>'
                                                                                        Visible="false" />--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label13" Text=" Total Balanced Pcs" runat="server" CssClass="labelbold" />
                            <asp:TextBox CssClass="textb" ID="TxtTotalBalPcs" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label14" Text="Total Received Pcs" runat="server" CssClass="labelbold" />
                            <asp:TextBox CssClass="textb" ID="txtTotalRecPcs" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label15" Text="Total Issued Pcs" runat="server" CssClass="labelbold" />
                            <asp:TextBox CssClass="textb" ID="txtTotalIssPcs" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <%--<td>
                                            <asp:Button ID="btnShowdata" runat="server" Text="Click to Show Data" CssClass="buttonnorm"
                                                OnClick="btnShowdata_Click" Width="150px" />
                                        </td>--%>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label16" Text="Remarks" runat="server" CssClass="labelbold" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="textb" ID="TxtRemarks" runat="server" Width="100%" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnInwards" Enabled="true"
                                Visible="true" runat="server" Text="Inwards Report" OnClick="BtnInwards_Click"
                                Width="100px" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnOutwards" Enabled="true"
                                Visible="true" runat="server" Text="Outwards Report" OnClick="BtnOutwards_Click"
                                Width="120px" />
                                 &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnRawMaterial" Enabled="true"
                                Visible="true" runat="server" Text="Raw Material Report" OnClick="BtnRawMaterial_Click"
                                Width="140px" />
                                 &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnSaveOutWardsConsumption" Enabled="true"
                                Visible="true" runat="server" Text="Save OutWards Consumption" OnClick="BtnSaveOutWardsConsumption_Click"
                                Width="180px" />
                                  &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnIssueRawMaterial" Enabled="true"
                                Visible="true" runat="server" Text="Issue Raw Material Report" OnClick="BtnIssueRawMaterial_Click"
                                Width="160px" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                                OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:HiddenField ID="hn_finished" runat="server" />
                            <asp:HiddenField ID="hnstockno" runat="server" />
                            <asp:HiddenField ID="hnrate1" runat="server" />
                            <asp:HiddenField ID="hnorderid" runat="server" />
                            <asp:HiddenField ID="hn_recieve_id" runat="server" />
                            <asp:HiddenField ID="Hn_Qty" runat="server" />
                            <asp:HiddenField ID="Hn_TQty" runat="server" />
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
                    <asp:Label ID="Label11" Text=" QualityType" runat="server" CssClass="labelbold" />
                    &nbsp;&nbsp; <b style="color: Red">*</b>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddQualityName" runat="server" CssClass="dropdown" Width="150px"
                        OnSelectedIndexChanged="ddQualityName_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                    <br />
                    <asp:GridView ID="GVPenalty" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        Width="350px" OnRowDataBound="GVPenalty_RowDataBound">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts1" />
                        <RowStyle CssClass="gvrow1" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <%--<HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkboxitem" runat="server" AutoPostBack="true" OnCheckedChanged="Chkboxitem_CheckedChanged" />
                                    <%--onclick="return CheckBoxClick(this);"--%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Penalty Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblPenalityName" Text='<%#Bind("PenalityName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty" Visible="false">
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
                            <asp:TemplateField HeaderText="Amt" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAmt" Width="50px" BackColor="White" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtAmt_TextChanged" />
                                    <%-- <asp:Label ID="lblAmt" Text='' runat="server" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPenalityId" Text='<%#Bind("PenalityId") %>' runat="server" Visible="false" />
                                    <asp:Label ID="lblPenalityType" Text='<%#Bind("PenalityType") %>' runat="server"
                                        Visible="false" />
                                    <%--<asp:Label ID="lblSrNo" Text='<%#Bind("lblSrNo") %>' runat="server"  Visible="false"/>--%>
                                    <%--<asp:Label ID="lblPPId" Text='<%#Bind("PPId") %>' runat="server" />
                                                    <asp:Label ID="lblIFinishedId" Text='<%#Bind("IFinishedId") %>' runat="server" />
                                                    <asp:Label ID="lblOFinishedId" Text='<%#Bind("FinishedId") %>' runat="server" />
                                                    <asp:Label ID="lblUnitTypeId" Text='<%#Bind("UnitTypeID") %>' runat="server" />
                                                    <asp:Label ID="lblOrderId" Text='<%#Bind("OrderId") %>' runat="server" />
                                                    <asp:Label ID="lblOrderDetailId" Text='<%#Bind("OrderDetailId") %>' runat="server" />--%>
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
            $('#<%=pnlPopPass.ClientID %>').show();
        }
        function HidePopup() {
            $('#mask').hide();
            $('#<%=pnlPopPass.ClientID %>').hide();
        }
        $(".btnPwd").live('click', function () {
            HidePopup();
        });
    </script>
    <div id="mask">
    </div>
    <asp:Panel ID="pnlPopPass" runat="server" BackColor="White" Height="175px" Width="300px"
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
