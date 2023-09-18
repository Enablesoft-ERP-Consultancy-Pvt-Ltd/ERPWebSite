<%@ Page Title="" Language="C#" MasterPageFile="~/App.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="styleSection" runat="Server">

    <link href="<%= Page.ResolveUrl("~/lib/datatables-tabletools/css/dataTables.tableTools.min.css")%>"
        rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/lib/datatables.net-bs5/css/dataTables.bootstrap5.min.css")%>"
        rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodySection" runat="Server">
    <!--BEGIN TITLE & BREADCRUMB PAGE-->
    <div id="title-breadcrumb-option-demo" class="page-title-breadcrumb">
        <div class="page-header pull-left">
            <div class="page-title">IExpro Dashboard </div>
        </div>
        <div class="clearfix"></div>
    </div>
    <!--END TITLE & BREADCRUMB PAGE-->
    <!--BEGIN CONTENT-->
    <div class="page-content">
        <div class="row">




            <div class="accordion" id="accordionExample">
                <div class="accordion-item">
                    <%--      <h2 class="accordion-header" id="headingOne">
                        <button class="accordion-button bg-black" type="button" data-bs-toggle="collapse"
                            data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                            Raw Materials Vendor PO Status
                        </button>
                    </h2>--%>
                    <div id="collapseOne" class="accordion-collapse collapse" aria-labelledby="headingOne"
                        data-bs-parent="#accordionExample">
                        <div class="accordion-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="table-responsive">
                                        <table id="tblVendorPO" class="table table-hover table-striped table-bordered table-advanced tablesorter display">
                                            <thead>
                                                <tr>
                                                    <th>Vendor Name</th>
                                                    <th>PO No</th>
                                                    <th>Issue Date</th>
                                                    <th>Expected Date</th>
                                                    <th>Quantity</th>
                                                    <th>Delay Days</th>
                                                    <th>Order Status</th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="headingTwo">
                        <button class="accordion-button collapsed bg-danger" type="button" data-bs-toggle="collapse"
                            data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                            Order Statistics
                        </button>
                    </h2>
                    <div id="collapseTwo" class="accordion-collapse collapse show" aria-labelledby="headingTwo"
                        data-bs-parent="#accordionExample">
                        <div class="accordion-body">
                            <div class="row">
                                <div class="col-lg-12">
                          
                                        <table id="tblOrder" class="table table-hover table-striped table-bordered table-advanced tablesorter display">
                                            <thead>
                                                <tr>
                                                    <th>Customer Code</th>
                                                    <th>Order No</th>
                                                    <th>Order Date</th>
                                                    <th>Dispatch Date</th>
                                                    <th>Delay Days</th>
                                                    <th>Order Status</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                           
                            </div>
                        </div>
                    </div>
                </div>
                <div class="accordion-item">
                    <%--  <h2 class="accordion-header" id="heading3">
                        <button class="accordion-button collapsed bg-blue" type="button" data-bs-toggle="collapse"
                            data-bs-target="#collapse3" aria-expanded="false" aria-controls="collapse3">
                            Dyeing Program Status
                        </button>
                    </h2>--%>
                    <div id="collapse3" class="accordion-collapse collapse" aria-labelledby="heading3"
                        data-bs-parent="#accordionExample">
                        <div class="accordion-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="table-responsive">
                                        <table id="tblDyeing" class="table table-hover table-striped table-bordered table-advanced tablesorter display">
                                            <thead>
                                                <tr>
                                                    <th>Dyer Name</th>
                                                    <th>Indent No</th>
                                                    <th>Issue Date</th>
                                                    <th>Expected Date</th>
                                                    <th>Quantity</th>
                                                    <th>Delay Days</th>
                                                    <th>Order Status</th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Modal -->



















            <div id="myModal" tabindex="-1" role="dialog" aria-labelledby="modal-default-label"
                aria-hidden="true" class="modal fade">
                <div class="modal-dialog modal-xl">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title"></h4>

                            <a class="sorting"><i class='fa fa-sort mrs text-green'></i><strong></strong></a>
                            <button type="button" data-bs-dismiss="modal" aria-hidden="true"
                                class="close">
                                &times;
                            </button>
                        </div>
                        <div id="bodyItem" class="modal-body">
                        </div>
                        <div class="modal-footer">
                            <button type="button" data-bs-dismiss="modal" class="btn btn-lg btn-red">
                                <i class="fa fa-times"></i>Close</button>
                        </div>

                    </div>
                </div>
            </div>




        </div>


    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptSection" runat="Server">

    <script src="<%= Page.ResolveUrl("~/lib/moment.js/moment.min.js")%>"></script>
    <!--LOADING SCRIPTS FOR PAGE-->
    <script src="<%= Page.ResolveUrl("~/lib/jquery-highcharts/highcharts.js")%>"></script>
    <script src="<%= Page.ResolveUrl("~/lib/jquery-highcharts/data.js")%>"></script>
    <script src="<%= Page.ResolveUrl("~/lib/jquery-highcharts/drilldown.js")%>"></script>
    <script src="<%= Page.ResolveUrl("~/lib/jquery-highcharts/exporting.js")%>"></script>
    <script src="<%= Page.ResolveUrl("~/lib/datatables.net/js/jquery.dataTables.min.js")%>"></script>
    <script src="<%= Page.ResolveUrl("~/lib/datatables-tabletools/js/dataTables.tableTools.min.js")%>"></script>
    <script src="<%= Page.ResolveUrl("~/lib/datatables.net-bs5/js/dataTables.bootstrap5.min.js")%>"></script>
    <script src="<%= Page.ResolveUrl("~/Content/Javascripts/Dashboard.js?1.97")%>"></script></asp:Content>

