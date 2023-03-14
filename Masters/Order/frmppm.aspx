<%@ Page Title="PPM" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmppm.aspx.cs" Inherits="Masters_Order_frmppm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmppm.aspx";
        }
        function validate() {
            var ddcompany = document.getElementById("<%=DDCompanyName.ClientID %>");
            var ddcustomer = document.getElementById("<%=DDCustomer.ClientID %>");
            var ddorderno = document.getElementById("<%=DDOrderNo.ClientID %>");
            var ddprocess = document.getElementById("<%=DDProcessName.ClientID %>");
            var msg = "";
            if (ddcompany.value == "") {
                msg = msg + 'Please select CompanyName!!!\n';
            }
            if (ddcustomer.value <= "0" || ddcustomer.value == "") {
                msg = msg + 'Please select CustomerName!!!\n';
            }
            if (ddorderno.value <= "0" || ddorderno.value == "") {
                msg = msg + 'Please select OrderNo!!!\n';
            }
            if (ddprocess.value <= "0" || ddprocess.value == "") {
                msg = msg + 'Please select Process Name!!!\n';
            }
            if (msg == "") {
                return true;
            }
            else {
                alert(msg);
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblCompName" Text="CompanyName" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDCompanyName" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblcustomer" Text="Customer Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDCustomer" Width="150px" CssClass="dropdown" runat="server"
                                OnSelectedIndexChanged="DDCustomer_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblOrderNO" Text="P.O.#" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDOrderNo" Width="150px" CssClass="dropdown" runat="server"
                                OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td id="TDPPMNO" runat="server" visible="false">
                            <asp:Label ID="Label7" Text="PPM No." runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDPPMNo" Width="150px" CssClass="dropdown" runat="server" OnSelectedIndexChanged="DDPPMNo_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div>
                    <table>
                        <tr>
                            <td>
                                <div style="overflow: auto; max-height: 250px">
                                    <asp:GridView ID="GVOrderDetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="GVOrderDetail_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkItem" CssClass="checkboxnormal" Text="" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemFinishedid" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemfinishedid" Text='<%#Bind("Item_Finished_id") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="orderdetailid" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderdetailid" Text='<%#Bind("orderdetailid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Item Description" DataField="ItemDescription" ItemStyle-CssClass="labelnormal" />
                                            <asp:BoundField HeaderText="Quantity" DataField="QtyRequired" ItemStyle-CssClass="labelnormal" />
                                            <asp:BoundField HeaderText="Order Date" DataField="Orderdate" ItemStyle-CssClass="labelnormal" />
                                            <asp:BoundField HeaderText="Dispatch Date" DataField="dispatchdate" ItemStyle-CssClass="labelnormal" />
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPPMorderdetailid" Text='<%#Bind("PPMOrderdetailid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <fieldset>
                    <legend>
                        <asp:Label ID="label1" runat="server" Text=""></asp:Label>
                    </legend>
                    <table>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblprocess" Text="Process Name" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDProcessName" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td id="Tdrequirements" runat="server">
                                <asp:Label ID="Label2" Text="Requirements" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtrequirements" runat="server" Width="373px" TextMode="MultiLine"
                                    Height="49px" />
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label3" Text="Process Start Date" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtstartdate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="calstartdate" TargetControlID="txtstartdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label4" Text="Process End Date" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtendate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="calenddate" TargetControlID="txtendate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label8" Text="Process Fin.Com. Date" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtfincomdt" CssClass="textb" Width="115px" runat="server" />
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfincomdt" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td valign="top" id="Tdncpdetails" runat="server">
                                <asp:Label ID="Label5" Text="NCP Details IF any" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtncpdetails" CssClass="textb" Width="144px" runat="server" TextMode="MultiLine"
                                    Height="47px" />
                            </td>
                            <td id="TDcomments" runat="server">
                                <asp:Label ID="Label6" Text="Comments" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtcomments" runat="server" Width="373px" TextMode="MultiLine" Height="49px" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table style="width: 100%;">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnnew" Text="New" CssClass="buttonnorm" runat="server" OnClientClick="NewForm();" />
                            <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClientClick="return validate();"
                                OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="CloseForm();" />
                            <asp:Button ID="btnpreview" Text="Preview" CssClass="buttonnorm" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
                <div>
                    <table>
                        <tr>
                            <td>
                                <div style="overflow: auto; max-height: 250px">
                                    <asp:GridView ID="GVPPMDETAIL" runat="server" AutoGenerateColumns="False" OnRowDeleting="GVPPMDETAIL_RowDeleting"
                                        OnRowEditing="GVPPMDETAIL_RowEditing" OnRowCancelingEdit="GVPPMDETAIL_RowCancelingEdit"
                                        OnRowUpdating="GVPPMDETAIL_RowUpdating" OnRowDataBound="GVPPMDETAIL_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="PPM NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblppmno" Text='<%#Bind("ppmno") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PROCESS NAME">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblprocess" Text='<%#Bind("PROCESS_Name") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="REQUIREMENTS">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrequirement" Text='<%#Bind("requirements") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtrequirementsedit" Width="200px" Text='<%#Bind("requirements") %>'
                                                        TextMode="MultiLine" CssClass="textb" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="START DATE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstartdate" Text='<%#Bind("startdate") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtstartdateedit" Width="90px" Text='<%#Bind("startdate") %>' CssClass="textb"
                                                        runat="server" BackColor="Yellow"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calgstdate" runat="server" TargetControlID="txtstartdateedit"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="END DATE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblenddate" Text='<%#Bind("Enddate") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtenddateedit" Width="90px" Text='<%#Bind("enddate") %>' CssClass="textb"
                                                        runat="server" BackColor="Yellow"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calgenddate" runat="server" TargetControlID="txtenddateedit"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FIN. COM. DATE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfincomdt" Text='<%#Bind("Fincomdate") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtfincomdtgrid" Width="90px" Text='<%#Bind("Fincomdate") %>' CssClass="textb"
                                                        runat="server" BackColor="Yellow"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calgfincomdate" runat="server" TargetControlID="txtfincomdtgrid"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NCP DETAILS IF ANY">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblncpdetails" Text='<%#Bind("ncpdetails") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtncpdetailsedit" Width="150px" Text='<%#Bind("ncpdetails") %>'
                                                        TextMode="MultiLine" CssClass="textb" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="COMMENTS">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcomments" Text='<%#Bind("comments") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtcommentsedit" Width="200px" Text='<%#Bind("comments") %>' TextMode="MultiLine"
                                                        CssClass="textb" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdel" runat="server" Text="Delete" CausesValidation="false"
                                                        CommandName="Delete" CssClass="labelbold" OnClientClick="return confirm('Do you want to delete this row?')"
                                                        ForeColor="Red"> </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblppmdetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField DeleteText="" ShowEditButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hnppmid" Value="0" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
