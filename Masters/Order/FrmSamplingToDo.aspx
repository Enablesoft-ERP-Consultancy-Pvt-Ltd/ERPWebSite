<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmSamplingToDo.aspx.cs"
    Inherits="Masters_Order_FrmSamplingToDo" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "FrmSamplingToDo.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function SaveValidation() {
            if (document.getElementById('CPH_Form_DDPriorityLevel') != null) {
                if (document.getElementById('CPH_Form_DDPriorityLevel').options.length == 0) {
                    alert("Priority level must have a value....!");
                    document.getElementById("CPH_Form_DDPriorityLevel").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtBuyerCode').value == "") {
                alert("Pls fill buyer code....!");
                document.getElementById('CPH_Form_TxtBuyerCode').focus();
                return false;
            }
            //            if (document.getElementById('CPH_Form_TxtDNo').value == "") {
            //                alert("Pls fill d no....!");
            //                document.getElementById('CPH_Form_TxtDNo').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtDName').value == "") {
            //                alert("Pls fill d name....!");
            //                document.getElementById('CPH_Form_TxtDName').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtTechnique').value == "") {
            //                alert("Pls fill technique....!");
            //                document.getElementById('CPH_Form_TxtTechnique').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtSize').value == "") {
            //                alert("Pls fill size....!");
            //                document.getElementById('CPH_Form_TxtSize').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtRawMaterialComposition').value == "") {
            //                alert("Pls fill raw material composition ....!");
            //                document.getElementById('CPH_Form_TxtRawMaterialComposition').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtQualityOfRaw').value == "") {
            //                alert("Pls fill quality of raw ....!");
            //                document.getElementById('CPH_Form_TxtQualityOfRaw').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtWashUnWash').value == "") {
            //                alert("Pls fill wash unwash ....!");
            //                document.getElementById('CPH_Form_TxtWashUnWash').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtPileHeight').value == "") {
            //                alert("Pls fill pile height ....!");
            //                document.getElementById('CPH_Form_TxtPileHeight').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtPlWt').value == "") {
            //                alert("Pls fill pl wt ....!");
            //                document.getElementById('CPH_Form_TxtPlWt').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtTWt').value == "") {
            //                alert("Pls fill total wt ....!");
            //                document.getElementById('CPH_Form_TxtTWt').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtDispDate').value == "") {
            //                alert("Pls fill dispatch date ....!");
            //                document.getElementById('CPH_Form_TxtDispDate').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtStatusOfDispatch').value == "") {
            //                alert("Pls fill status of dispatch ....!");
            //                document.getElementById('CPH_Form_TxtStatusOfDispatch').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtCourierNo').value == "") {
            //                alert("Pls  fill courier no....!");
            //                document.getElementById('CPH_Form_TxtCourierNo').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtTrackingNo').value == "") {
            //                alert("Pls fill tracking no ....!");
            //                document.getElementById('CPH_Form_TxtTrackingNo').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtColorNoS').value == "") {
            //                alert("Pls fill color no s ....!");
            //                document.getElementById('CPH_Form_TxtColorNoS').focus();
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_TxtRemark').value == "") {
            //                alert("Pls fill remark ....!");
            //                document.getElementById('CPH_Form_TxtRemark').focus();
            //                return false;
            //            }
            return confirm('Do you want to save data?')
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <span class="labelbold">Priority Level</span>
                        <br />
                        <asp:DropDownList ID="DDPriorityLevel" CssClass="dropdown" Width="100px" runat="server"
                            TabIndex="2">
                            <asp:ListItem Value="0">Normal</asp:ListItem>
                            <asp:ListItem Value="1">Urgent</asp:ListItem>
                            <asp:ListItem Value="2">Top Urgent</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="labelbold">Buyer Code</span>
                        <br />
                        <asp:TextBox ID="TxtBuyerCode" runat="server" Width="150px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">D No</span>
                        <br />
                        <asp:TextBox ID="TxtDNo" runat="server" Width="150px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">D Name</span>
                        <br />
                        <asp:TextBox ID="TxtDName" runat="server" Width="150px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Technique</span>
                        <br />
                        <asp:TextBox ID="TxtTechnique" runat="server" Width="150px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Size</span>
                        <br />
                        <asp:TextBox ID="TxtSize" runat="server" Width="150px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <span class="labelbold">Raw Material Composition</span>
                        <br />
                        <asp:TextBox ID="TxtRawMaterialComposition" runat="server" Width="250px" CssClass="textb"
                            TabIndex="3" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Quality Of Raw</span>
                        <br />
                        <asp:TextBox ID="TxtQualityOfRaw" runat="server" Width="150px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Wash/UnWash</span>
                        <br />
                        <asp:TextBox ID="TxtWashUnWash" runat="server" Width="150px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Pile Height</span>
                        <br />
                        <asp:TextBox ID="TxtPileHeight" runat="server" Width="100px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Pl Wt</span>
                        <br />
                        <asp:TextBox ID="TxtPlWt" runat="server" Width="100px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">T Wt</span>
                        <br />
                        <asp:TextBox ID="TxtTWt" runat="server" Width="100px" CssClass="textb" TabIndex="3"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <span class="labelbold">Disp Date</span>
                        <br />
                        <asp:TextBox ID="TxtDispDate" runat="server" Width="100px" CssClass="textb" TabIndex="3"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtDispDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <span class="labelbold">Status Of Dispatch</span>
                        <br />
                        <asp:TextBox ID="TxtStatusOfDispatch" runat="server" Width="150px" CssClass="textb"
                            TabIndex="4" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Courier No</span>
                        <br />
                        <asp:TextBox ID="TxtCourierNo" runat="server" Width="150px" CssClass="textb" TabIndex="4"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Tracking No</span>
                        <br />
                        <asp:TextBox ID="TxtTrackingNo" runat="server" Width="150px" CssClass="textb" TabIndex="4"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Color No S</span>
                        <br />
                        <asp:TextBox ID="TxtColorNoS" runat="server" Width="150px" CssClass="textb" TabIndex="4"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <span class="labelbold">Remark</span>
                        <br />
                        <asp:TextBox ID="TxtRemark" runat="server" Width="400px" CssClass="textb" TextMode="MultiLine"
                            TabIndex="6" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td colspan="2" align="right">
                        <asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" TabIndex="7" />
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return SaveValidation();"
                            CssClass="buttonnorm" TabIndex="8" />
                        <%--<asp:Button ID="BtnPreview" runat="server" Text="Preview" CssClass="buttonnorm"
                            OnClick="BtnPreview" />--%>
                        <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            CssClass="buttonnorm" TabIndex="9" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div style="width: 900px; height: 200px; overflow: auto">
                            <asp:GridView ID="DGToDoSampling" runat="server" OnRowDataBound="DGToDoSampling_RowDataBound"
                                DataKeyNames="SrNo" OnSelectedIndexChanged="DGToDoSampling_SelectedIndexChanged"
                                AutoGenerateColumns="False" CssClass="grid-views" OnRowDeleting="DGToDoSampling_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="PriorityLevel" HeaderText="PriorityLevel">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuyerCode" HeaderText="BuyerCode">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DNo" HeaderText="DNo">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DName" HeaderText="DName">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Technique" HeaderText="Technique">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Size" HeaderText="Size">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RawMaterialComposition" HeaderText="RawMaterialComposition">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QualityOfRaw" HeaderText="QualityOfRaw">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="WashUnWash" HeaderText="WashUnWash">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PileHeight" HeaderText="PileHeight">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PlWt" HeaderText="PlWt">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TWt" HeaderText="TWt">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DispDate" HeaderText="DispDate">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StatusOfDispatch" HeaderText="StatusOfDispatch">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CourierNo" HeaderText="CourierNo">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TrackingNo" HeaderText="TrackingNo">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ColorNoS" HeaderText="ColorNoS">
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                        <HeaderStyle Width="300px" HorizontalAlign="Center" />
                                        <ItemStyle Width="300px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="75px" />
                                        <ItemStyle HorizontalAlign="Center" Width="75px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
