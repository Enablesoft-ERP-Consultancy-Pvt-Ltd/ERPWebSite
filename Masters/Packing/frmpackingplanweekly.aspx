<%@ Page Title="PACKING PLAN WEEKLY" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmpackingplanweekly.aspx.cs" Inherits="Masters_Packing_frmpackingplanweekly" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmpackingplanweekly.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function batchnoselected(source, eventArgs) {
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
        function isNumberWith(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }
        function CheckAllForunpack(objref) {
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
    <script type="text/javascript">
        function jScriptValidate() {
            $("#<%=btnaddrow.ClientID %>").click(function () {
                var Message = "";
                var txtbatchno = document.getElementById('<%=txtbatchno.ClientID %>');
                if (txtbatchno.value == "") {
                    Message = Message + "Please Enter Batch No. !!\n";
                }
                selectedindex = $("#<%=DDitemname.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please Select Item Name. !!\n";
                }
                selectedindex = $("#<%=DDquality.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please Select Quality Name. !!\n";
                }
                selectedindex = $("#<%=DDdesign.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please Select Design. !!\n";
                }
                selectedindex = $("#<%=DDcolor.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please Select Color. !!\n";
                }
                selectedindex = $("#<%=DDshape.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please Select Shape. !!\n";
                }
                selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please Select Size. !!\n";
                }
                selectedindex = $("#<%=DDPacktype.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please Select Pack Type. !!\n";
                }
                selectedindex = $("#<%=DDarticleno.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please Select Article No. !!\n";
                }
                var txtplanpcs = document.getElementById('<%=txtplanpcs.ClientID %>');
                if (txtplanpcs.value == "") {
                    Message = Message + "Please Enter Plan Pcs. !!\n";
                }
                //                    
                if (Message == "") {
                    return true;
                }
                else {
                    alert(Message);
                    return false;
                }
            });

            $("#<%=DDquality.ClientID %>").change(function () {
                $("#<%=DDSize.ClientID %>").attr('selectedIndex', 0);
            });
            $("#<%=DDdesign.ClientID %>").change(function () {
                $("#<%=DDSize.ClientID %>").attr('selectedIndex', 0);
            });
            $("#<%=DDcolor.ClientID %>").change(function () {
                $("#<%=DDSize.ClientID %>").attr('selectedIndex', 0);
            });
            $("#<%=DDshape.ClientID %>").change(function () {
                $("#<%=DDSize.ClientID %>").attr('selectedIndex', 0);
            });
            $("#<%=btncompletebatchno.ClientID %>").click(function () {
                var txtbatchno = document.getElementById('<%=txtbatchno.ClientID %>');
                if (txtbatchno.value == "") {
                    alert('Please Enter Batch No. !!');
                    txtbatchno.focus();
                    return false;
                }
            });
        }
    </script>
    <script type="text/javascript" language="javascript">
        //There's a bug in Microsoft's Ajax script that stops the modal popups from working
        //This overrides the the code that causes the error
        Sys.UI.Point = function Sys$UI$Point(x, y) {

            x = Math.round(x);
            y = Math.round(y);

            var e = Function._validateParams(arguments, [
                { name: "x", type: Number, integer: true },
                { name: "y", type: Number, integer: true }
            ]);
            if (e) throw e;
            this.x = x;
            this.y = y;
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScriptValidate);
            </script>
            <div>
                <table border="1px solid grey">
                    <tr>
                        <td>
                            <asp:Label ID="lblbatchno" Text="Batch No." runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                Style="display: none;" />
                            <asp:TextBox ID="txtbatchno" CssClass="textb" Width="200px" runat="server" OnTextChanged="txtbatchno_TextChanged" />
                            <asp:AutoCompleteExtender ID="txtbatchno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                CompletionInterval="20" Enabled="True" ServiceMethod="GetBatchno" EnableCaching="true"
                                CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtbatchno"
                                UseContextKey="True" ContextKey="OPEN#0#0" MinimumPrefixLength="2">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Start Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtstartdate" CssClass="textb" Width="100px" runat="server" BackColor="Yellow" />
                            <asp:CalendarExtender ID="calstart" runat="server" TargetControlID="txtstartdate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text="Comp. Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtcompdate" CssClass="textb" Width="100px" runat="server" BackColor="Yellow" />
                            <asp:CalendarExtender ID="calcompdate" runat="server" TargetControlID="txtcompdate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td id="TDclosebatchno" runat="server" visible="false">
                            <asp:CheckBox ID="Chkclosebatchno" Text="FILL CLOSE BATCH NO." runat="server" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="Chkclosebatchno_CheckedChanged" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table style="border-style: solid; border-width: 1px">
                    <tr>
                        <td>
                            <asp:Label ID="Label23" Text="Category" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcategory" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDcategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" Text="ItemName" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDitemname" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDitemname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label4" Text="QualityName" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDquality" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label5" Text="Design" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDdesign" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDdesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" Text="Colour" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcolor" CssClass="dropdown" runat="server" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="DDcolor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label7" Text="Shape" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDshape" CssClass="dropdown" runat="server" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="DDshape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label8" Text="Size" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDSize" CssClass="dropdown" runat="server" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="DDSize_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label9" Text="PackType" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDPacktype" CssClass="dropdown" runat="server" Width="150px"
                                OnSelectedIndexChanged="DDPacktype_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" Text="ArticleNo." runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDarticleno" CssClass="dropdown" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="DDarticleno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label11" Text="Plan Pcs" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtplanpcs" CssClass="textb" Width="145px" runat="server" onkeypress=" return isNumberWith(event);" />
                        </td>
                        <td runat="server" visible="false">
                            <asp:Label ID="Label12" Text="Pack Pcs" runat="server" CssClass="labelbold" />
                        </td>
                        <td runat="server" visible="false">
                            <asp:TextBox ID="txtpackpcs" CssClass="textb" Width="145px" runat="server" Enabled="false"
                                ReadOnly="true" onkeypress=" return isNumberWith(event);" />
                        </td>
                        <td runat="server" visible="false">
                            <asp:Label ID="Label13" Text="WIP Pcs" runat="server" CssClass="labelbold" />
                        </td>
                        <td runat="server" visible="false">
                            <asp:TextBox ID="txtWippcs" CssClass="textb" Width="145px" Enabled="false" ReadOnly="true"
                                runat="server" />
                        </td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnaddrow" Text="Add Row" CssClass="buttonnorm" runat="server" OnClick="btnaddrow_Click" />
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label15" Text="DEST." runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtdest" CssClass="textb" Width="145px" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="Label16" Text="SHIP Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtshipdate" CssClass="textb" Width="145px" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtshipdate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label17" Text="PO No." runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtpono" CssClass="textb" Width="145px" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="Label14" Text="ESIC No." runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtEsicno" CssClass="textb" Width="145px" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </td>
                </tr>
            </table>
            <div style="max-height: 500px; overflow: auto;">
                <asp:GridView ID="DG" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="DG_RowCancelingEdit"
                    OnRowEditing="DG_RowEditing" OnRowUpdating="DG_RowUpdating">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <PagerStyle CssClass="PagerStyle" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="QUALITY">
                            <ItemTemplate>
                                <asp:Label ID="lblquality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="COLOUR">
                            <ItemTemplate>
                                <asp:Label ID="lblcolour" runat="server" Text='<%#Bind("Colorname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SIZE">
                            <ItemTemplate>
                                <asp:Label ID="lblsize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PACK TYPE">
                            <ItemTemplate>
                                <asp:Label ID="lblpacktype" runat="server" Text='<%#Bind("PackingType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ARTICLE NO.">
                            <ItemTemplate>
                                <asp:Label ID="lblarticleno" runat="server" Text='<%#Bind("Articleno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PLAN PCS">
                            <ItemTemplate>
                                <asp:Label ID="lblplanpcs" runat="server" Text='<%#Bind("Planpcs") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txteditplanpcs" Text='<%#Bind("Planpcs") %>' Width="80px" BackColor="Yellow"
                                    runat="server" onkeypress=" return isNumberWith(event);" />
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PACK PCS">
                            <ItemTemplate>
                                <asp:Label ID="lblpackpcs" runat="server" Text='<%#Bind("Packpcs") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WIP PCS">
                            <ItemTemplate>
                                <asp:Label ID="lblwippcs" runat="server" Text='<%#Bind("Wippcs") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ESIC NO">
                            <ItemTemplate>
                                <asp:Label ID="lblesicno" runat="server" Text='<%#Bind("Esicno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DEST">
                            <ItemTemplate>
                                <asp:Label ID="lbldest" runat="server" Text='<%#Bind("Dest") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SHIP DATE">
                            <ItemTemplate>
                                <asp:Label ID="lblshipdate" runat="server" Text='<%#Bind("shipdate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PO NO">
                            <ItemTemplate>
                                <asp:Label ID="lblpono" runat="server" Text='<%#Bind("Pono") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkpackcarpet" Text="PACK_CARPET" runat="server" CssClass="labelbold"
                                    ForeColor="BlueViolet" OnClick="lblpackcarpet_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkunpackcarpet" Text="UNPACK_CARPET" runat="server" CssClass="labelbold"
                                    ForeColor="BlueViolet" OnClick="lblunpackcarpet_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkdelete" Text="DELETE" runat="server" CssClass="labelbold"
                                    ForeColor="BlueViolet" OnClick="lnkdelete_Click" OnClientClick="return confirm('Do you want to delete this row?')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblid" Text='<%#Bind("id") %>' runat="server"></asp:Label>
                                <asp:Label ID="lbldetailid" runat="server" Text='<%#Bind("Detailid") %>'></asp:Label>
                                <asp:Label ID="lblitemid" runat="server" Text='<%#Bind("itemid") %>'></asp:Label>
                                <asp:Label ID="lblqualityid" runat="server" Text='<%#Bind("qualityid") %>'></asp:Label>
                                <asp:Label ID="lbldesignid" runat="server" Text='<%#Bind("designid") %>'></asp:Label>
                                <asp:Label ID="lblcolorid" runat="server" Text='<%#Bind("colorid") %>'></asp:Label>
                                <asp:Label ID="lblshapeid" runat="server" Text='<%#Bind("shapeid") %>'></asp:Label>
                                <asp:Label ID="lblsizeid" runat="server" Text='<%#Bind("sizeid") %>'></asp:Label>
                                <asp:Label ID="lblpackingtypeid" runat="server" Text='<%#Bind("packtypeid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField CancelText="CANCEL" EditText="EDIT" ShowEditButton="True" UpdateText="UPDATE">
                            <ItemStyle Font-Bold="True" ForeColor="BlueViolet" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </div>
            <table style="width: 100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btncompletebatchno" Text="Complete Batch No." runat="server" CssClass="buttonnorm"
                            OnClick="btncompletebatchno_Click" />
                        <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                        <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();" />
                        <asp:Button ID="btndelete" Text="Delete" runat="server" CssClass="buttonnorm" OnClick="btndelete_Click"
                            OnClientClick="return confirm('Do you want to delete this Packing Plan?');" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnplanid" runat="server" Value="0" />
            <div>
                <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                <asp:ModalPopupExtender ID="ModalPopupExtenderpackcarpet" runat="server" PopupControlID="pnlpackcarpet"
                    TargetControlID="btnModalPopUp" DropShadow="true" BackgroundCssClass="modalBackground"
                    CancelControlID="btnCancel" PopupDragHandleControlID="pnModelPopup" OnOkScript="onOk()">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlpackcarpet" runat="server" Style="background-color: White; border: 3px solid #0DA9D0;
                    border-radius: 12px; padding: 0; max-height: 600px; width: 520px; display: none">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblpack" Text="PACK CARPET" CssClass="labelbold" ForeColor="Red" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label20" Text="Total Pcs" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttotalpackpcs" Width="90px" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label22" Text="Date_Stamp" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdate_stamp" Width="90px" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label21" Text="Pack Pcs" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpackpcs_chk" Width="90px" runat="server" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkcheckpcs" runat="server" CssClass="labelbold" ForeColor="DarkViolet"
                                                Text="Check Pcs" OnClick="lnkcheckpcs_Click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="overflow: auto; width: 500px; margin-top: 10px; max-height: 500px">
                                    <asp:GridView ID="DGStockDetail" runat="server" Style="margin-left: 10px" ForeColor="#333333"
                                        AutoGenerateColumns="False" CssClass="grid-views" EmptyDataText="No Data available to Pack.">
                                        <HeaderStyle CssClass="gvheaders" Height="20px" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
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
                                            <asp:TemplateField HeaderText="Sr No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltstockno" Text='<%#Bind("tstockno") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quality">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblquality" Text='<%#Bind("Quality") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Colour">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcolor" Text='<%#Bind("Colorname") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Size">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsize" Text='<%#Bind("Size") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date_Stamp">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldatestamp" Text='<%#Bind("Date_Stamp") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpackid" Text='<%#Bind("ID") %>' runat="server" />
                                                    <asp:Label ID="lblstockno" Text='<%#Bind("stockno") %>' runat="server" />
                                                    <asp:Label ID="lblpackdetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label18" Text="Date" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtpackdate" runat="server" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtpackdate"
                                    Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td id="TDdatestamp" runat="server" visible="false">
                                <asp:Label ID="lbldatestamp" Text="Date Stamp" runat="server" CssClass="labelbold" />
                            </td>
                            <td id="Tdtxtdatestamp" runat="server" visible="false">
                                <asp:TextBox ID="txtdatestamp" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonnorm" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:Button runat="server" ID="btnModalPopUp2" Style="display: none" />
                <asp:ModalPopupExtender ID="ModalPopupExtenderunpackcarpet" runat="server" PopupControlID="pnlunpackcarpet"
                    TargetControlID="btnModalPopUp2" DropShadow="true" BackgroundCssClass="modalBackground"
                    CancelControlID="btncancelunpack" PopupDragHandleControlID="pnlunpackcarpet"
                    OnOkScript="onOk()">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlunpackcarpet" runat="server" Style="background-color: White; border: 3px solid #0DA9D0;
                    border-radius: 12px; padding: 0; max-height: 600px; width: 750px; display: none">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblpackingsumm" CssClass="labelbold" ForeColor="Red" Text="PACKING SUMMARY"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; width: 300px; margin-top: 10px; max-height: 500px">
                                                <asp:GridView ID="DGPackingsummary" runat="server" Style="margin-left: 10px" ForeColor="#333333"
                                                    AutoGenerateColumns="False" CssClass="grid-views" EmptyDataText="No Packing Summary available"
                                                    AutoGenerateSelectButton="true" OnSelectedIndexChanged="DGPackingsummary_SelectedIndexChanged"
                                                    SelectedRowStyle-BackColor="LightPink">
                                                    <HeaderStyle CssClass="gvheaders" Height="20px" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="DATE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldate" Text='<%#Bind("packDate") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PACK NO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpackno" Text='<%#Bind("packno") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DT. STAMP">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldtstamp" Text='<%#Bind("datestamp") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpackstockplanid" Text='<%#Bind("planid") %>' runat="server" />
                                                                <asp:Label ID="lblpackstockPlandetailid" Text='<%#Bind("plandetailid") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label19" Text="UNPACK CARPET" CssClass="labelbold" ForeColor="Red"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; width: 400px; margin-top: 10px; max-height: 500px">
                                                <asp:GridView ID="DGUnpackcarpet" runat="server" Style="margin-left: 10px" ForeColor="#333333"
                                                    AutoGenerateColumns="False" CssClass="grid-views" OnRowDataBound="DGUnpackcarpet_RowDataBound">
                                                    <HeaderStyle CssClass="gvheaders" Height="20px" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAllForunpack(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chkboxitemunpack" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sr No.">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Stock No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbltstockno" Text='<%#Bind("tstockno") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quality">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblquality" Text='<%#Bind("Quality") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Colour">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcolor" Text='<%#Bind("Colorname") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Size">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsize" Text='<%#Bind("Size") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dt_Stamp">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldatestamp" Text='<%#Bind("Datestamp") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblplanidunpack" Text='<%#Bind("planid") %>' runat="server" />
                                                                <asp:Label ID="lblstockno" Text='<%#Bind("stockno") %>' runat="server" />
                                                                <asp:Label ID="lblplandetailidunpack" Text='<%#Bind("plandetailid") %>' runat="server" />
                                                                <asp:Label ID="lblpacknounpack" Text='<%#Bind("packno") %>' runat="server" />
                                                                <asp:Label ID="lblpackstatus" Text='<%#Bind("pack") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltpcs" Text="Total Pcs" runat="server" CssClass="labelbold" />
                                            <asp:TextBox ID="txttotalunpackpcs" Width="100px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnunpackcarpet" runat="server" Text="Save" CssClass="buttonnorm"
                                    OnClick="btnunpackcarpet_Click" />
                                <asp:Button ID="btncancelunpack" runat="server" Text="Cancel" CssClass="buttonnorm" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
