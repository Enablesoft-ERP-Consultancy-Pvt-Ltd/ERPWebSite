﻿var ProcessList = [{ ProcessId: 5, Process: "DYEING(PCS)", Title: "DYEING(PCS) Report", Type: 1 },
{ ProcessId: 8, Process: "BLOCK PRINTING(MTR)", Title: "BLOCK PRINTING(MTR) Report", Type: 1 },
{ ProcessId: 11, Process: "DIGITAL PRINTING(MTR)", Title: "DIGITAL PRINTING(MTR) Report", Type: 1 },
{ ProcessId: 12, Process: "COMPUTER EMBROIDERY(MTR)", Title: "COMPUTER EMBROIDERY(MTR) Report", Type: 1 },
{ ProcessId: 18, Process: "WASHING(MTR)", Title: "WASHING(MTR) Report", Type: 1 },
{ ProcessId: 34, Process: "STONE WASH(MTR)", Title: "STONE WASH(MTR) Report", Type: 1 },
{ ProcessId: 29, Process: "SCREEN PRINTING (MTR)", Title: "SCREEN PRINTING (MTR) Report", Type: 1 },
{ ProcessId: 10, Process: "COMPUTER EMBROIDERY(PCS)", Title: "COMPUTER EMBROIDERY(PCS)", Type: 2 },
{ ProcessId: 1, Process: "CUTTING", Title: "CUTTING", Type: 2 },
{ ProcessId: 2, Process: "MANUAL EMBROIDERY(PCS)", Title: "MANUAL EMBROIDERY(PCS)", Type: 2 },
{ ProcessId: 42, Process: "PANNEL WEAVING", Title: "PANNEL WEAVING", Type: 2 },
{ ProcessId: 4, Process: "PATCH STITCHING", Title: "PATCH STITCHING", Type: 2 },
{ ProcessId: 38, Process: "TABLE TUFTING", Title: "TABLE TUFTING", Type: 2 },
{ ProcessId: 10, Process: ' ', Title: "Finishing COMPUTER EMBROIDERY(PCS)", Type: 3 },
{ ProcessId: 42, Process: ' ', Title: "Finishing PANNEL WEAVING", Type: 3 },
{ ProcessId: 2, Process: ' ', Title: "Finishing MANUAL EMBROIDERY(PCS)", Type: 3 },
{ ProcessId: 4, Process: ' ', Title: "Finishing PATCH STITCHING", Type: 3 },
{ ProcessId: 13, Process: "STITCHING", Title: "STITCHING", Type: 3 },
{ ProcessId: 21, Process: 'Finishing BLOCK PRINTING', Title: "Finishing BLOCK PRINTING", Type: 3 },
{ ProcessId: 30, Process: ' ', Title: "Finishing COLOUR CUT", Type: 3 },
{ ProcessId: 26, Process: ' ', Title: "Finishing DIGITAL PRINTING (PCS)", Type: 3 },
{ ProcessId: 6, Process: ' ', Title: "Finishing DYEING(PCS)", Type: 3 },
{ ProcessId: 3, Process: ' ', Title: "Finishing FILLING", Type: 3 },
{ ProcessId: 7, Process: ' ', Title: "Finishing FINAL FINISHING", Type: 3 },
{ ProcessId: 27, Process: ' ', Title: "Finishing FOLD &amp; THREAD CUTTING ", Type: 3 },
{ ProcessId: 47, Process: ' ', Title: "Finishing HAND STITCHING", Type: 3 },
{ ProcessId: 28, Process: ' ', Title: "Finishing HEMMING STICHING ", Type: 3 },
{ ProcessId: 41, Process: ' ', Title: "Finishing KANTHA HANDWORK", Type: 3 },
{ ProcessId: 31, Process: ' ', Title: "Finishing KNOTTING", Type: 3 },
{ ProcessId: 20, Process: ' ', Title: "Finishing KNOWTING &amp; WRAPPING", Type: 3 },
{ ProcessId: 23, Process: ' ', Title: "Finishing LABEL STITCHING", Type: 3 },
{ ProcessId: 43, Process: ' ', Title: "Finishing LACE STITCHING", Type: 3 },
{ ProcessId: 17, Process: ' ', Title: "Finishing PACKING", Type: 3 },
{ ProcessId: 39, Process: ' ', Title: "Finishing PANEL MAKING", Type: 3 },
{ ProcessId: 14, Process: ' ', Title: "Finishing PRESING", Type: 3 },
{ ProcessId: 44, Process: ' ', Title: "Finishing QUILTING", Type: 3 },
{ ProcessId: 40, Process: ' ', Title: "Finishing RE-WORK", Type: 3 },
{ ProcessId: 49, Process: ' ', Title: "Finishing SAMPLING", Type: 3 },
{ ProcessId: 24, Process: ' ', Title: "Finishing SCREEN PRINTING", Type: 3 },
{ ProcessId: 22, Process: ' ', Title: "Finishing STONE WASH", Type: 3 },
{ ProcessId: 38, Process: ' ', Title: "Finishing TABLE TUFTING", Type: 3 },
{ ProcessId: 16, Process: ' ', Title: "Finishing TASSEL STITCHING", Type: 3 },
{ ProcessId: 19, Process: ' ', Title: "Finishing THREAD CUTTING", Type: 3 },
{ ProcessId: 32, Process: ' ', Title: "Finishing THREAD CUTTING &amp; TASSLING ", Type: 3 },
{ ProcessId: 37, Process: ' ', Title: "Finishing TUFTING", Type: 3 },
{ ProcessId: 46, Process: ' ', Title: "Finishing UPHOLSTERY", Type: 3 },
{ ProcessId: 25, Process: ' ', Title: "Finishing VACUUM", Type: 3 },
{ ProcessId: 33, Process: ' ', Title: "Finishing WASHING", Type: 3 },
{ ProcessId: 48, Process: ' ', Title: "Finishing WEAVING", Type: 3 },
{ ProcessId: 35, Process: ' ', Title: "Finishing WRAPPING", Type: 3 },
];

$(document).bind("ajaxStart", function () {
    $("#ldrdiv").show();
}).bind("ajaxStop", function () {
    $("#ldrdiv").hide();
});


$(function () {

    const FROM_PATTERN = 'YYYY-MM-DD HH:mm:ss.SSS';
    const TO_PATTERN = 'DD/MM/YYYY';
    var orderlist = null;
    $('#myModal').on('hidden.bs.modal', function (evt) {
        $('.modal .modal-body').empty();
    });

    $('#myModal').on('click', 'a.sorting', function (e) {


        var $sorted_items,
            getSorted = function (selector, attrName) {
                return $(
                    $(selector).toArray().sort(function (a, b) {
                        var aVal = parseInt(a.getAttribute(attrName)),
                            bVal = parseInt(b.getAttribute(attrName));
                        return aVal - bVal;
                    })
                );
            };

        $sorted_items = getSorted('#bodyItem .row', 'data-order').clone();
        $('#bodyItem').empty();
        $('#bodyItem').html($sorted_items);


    });

    $.ajax({
        type: "POST",
        url: "Home.aspx/OrderList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        success: function (data) {
            orderlist = $.parseJSON(data.d);
            //BindChart(orderlist);
            /* alert(orderlist.chartData[0].OrderCount);*/
            let table = $('#tblOrder').DataTable({
                lengthMenu: [[50, 100, 150, 200, -1], [50, 100, 150, 200, "All"]],
                data: orderlist.data,
                columns: [
                    { data: 'CustomerCode' },
                    { data: 'CustomerOrderNo' },
                    { data: 'OrderDate' },
                    { data: 'DispatchDate' },
                    {
                        data: 'DelayDays',
                        render: function (data, type) {
                            if (data > 0) {
                                return data + ' Days';
                            }
                            else {
                                return 'No Delay';
                            }
                        },
                    },
                    {
                        data: 'OrderStatus',
                        render: function (data, type) {
                            if (data === 1) {
                                return '<span class="label label-sm label-success">Close</span>';
                            }
                            else {

                                return '<span class="label label-sm label-danger">Open</span>';
                            }
                        },
                    },
                    {
                        data: null,
                        render: function (data, type, row, meta) {

                            var btnHtml = "<a title='Order Detail' class='btnOrder text-red btn btn-default btn-xs mrm' exthref=" + data.OrderId + "><i class='fa fa-info-circle mrs'></i>Order Summary</a>";

                            btnHtml += "<a class='btnSummary text-green btn btn-default btn-xs mrm' exthref=" + data.OrderId + "><i class='fa fa-pencil mrs'></i><strong>Process Summary</strong></a>";
                            btnHtml += "<div class='btn-group'><button type='button' data-bs-toggle='dropdown' class='btn btn-dark dropdown-toggle  ptx pbx'>";
                            btnHtml += "Choose Process<span class='caret'></span></button><ul role='menu' class='dropdown-menu dropdown-left-posotion' style='max-height:400px;overflow:auto;'>";
                            btnHtml += "<li><a class='btnPurchase mrm' prName='PURCHASE' exthref=" + data.OrderId + "' extType='0' title='PURCHASE' extSeq='0'><i class='fa fa-shopping-cart mrs text-green'></i><strong>Purchase</strong></a></li>";

                            //console.log(data);
                            $.each(data.ProcessList, function (index, item) {

                                item.SeqNo = index + 1;
                                /*    console.log(item);*/
                                // var _item = ProcessList.find(S => S.ProcessId == item.ProcessId);
                                if (item.ProcessType == 0) {
                                    item.ProcessType = 1;
                                }
                                console.log(item.ProcessType);
                                var className;
                                switch (item.ProcessType) {
                                    case 1:
                                        className = "btnProcess";
                                        break;
                                    case 2:
                                        className = "btnIssueProcess";
                                        break;
                                    case 3:
                                        className = "btnFinish";
                                        break;
                                }
                                btnHtml += "<li><a class='" + className + " mrm' prhref=" + item.ProcessId + " prName='" + item.ProcessName + "' exthref=" + data.OrderId + " extType=" + item.ProcessType + "  title='" + item.ProcessName + "' extSeq=" + item.SeqNo + "><i class='fa fa-pencil mrs text-green'></i><strong>" + item.ProcessName + "</strong></a></li>";
                            });
                            btnHtml += "</ul></div>";
                            return btnHtml;
                        },
                    },
                ],
                columnDefs: [

                    {
                        targets: 2,
                        render: function (data) {
                            return moment(data).format('DD MMM  YYYY');
                        },
                    },
                    {
                        targets: 3,
                        render: function (data) {
                            return moment(data).format('DD MMM YYYY');
                        },
                    },
                ]

            });

            table.on('click', 'a.btnSummary', function (e) {
                var elem = $(this);
                var orderId = parseInt(elem.attr('exthref'));
                SummaryReport(elem, orderId)
            });

            table.on('click', 'a.btnOrder', function (e) {
                var elem = $(this);
                var id = elem.attr('exthref');
                var _orderId = parseInt(id);
                OrderDetail(_orderId);
            });

            table.on('click', 'a.btnPurchase', function (e) {
                var elem = $(this);
                var _orderId = parseInt(elem.attr('exthref'));
                var _processId = parseInt(elem.attr('prhref'));
                var name = elem.attr('prName');
                var _type = parseInt(elem.attr('extType'));
                ProcessReport(_orderId, _processId, name, _type);
            });

            table.on('click', 'a.btnProcess', function (e) {
                var elem = $(this);
                var _orderId = parseInt(elem.attr('exthref'));
                var _processId = parseInt(elem.attr('prhref'));
                var name = elem.attr('prName');

                var _type = parseInt(elem.attr('extType'));
                ProcessReport(_orderId, _processId, name, _type);
            });

            table.on('click', 'a.btnIssueProcess', function (e) {
                var elem = $(this);
                var _orderId = parseInt(elem.attr('exthref'));
                var _processId = parseInt(elem.attr('prhref'));
                var name = elem.attr('prName');
                var _type = parseInt(elem.attr('extType'));
                ProcessReport(_orderId, _processId, name, _type);
            });



            table.on('click', 'a.btnFinish', function (e) {
                var elem = $(this);
                var _orderId = parseInt(elem.attr('exthref'));
                var _processId = parseInt(elem.attr('prhref'));
                var name = elem.attr('prName');
                var _type = parseInt(elem.attr('extType'));
                ProcessReport(_orderId, _processId, name, _type);
            });



        },
        error: function (xhr) {
            alert("There is some technical Issue contact with admin!");
        }
    });



    var polist = null; var jqxhr = $.getJSON('Home.aspx/GetPOStatusList', function (data) {
        polist = $.parseJSON(data.d);

    });
    jqxhr.done(function () {
        console.log(orderlist);

        $('#tblVendorPO').DataTable({
            lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
            data: polist.data,
            columns: [
                { data: 'VendorName' },
                { data: 'VendorPO' },
                { data: 'IssueDate' },
                { data: 'ExpectedDate' },
                { data: 'Quantity' },
                {
                    data: 'DelayDays',
                    render: function (data, type) {
                        if (data > 0) {
                            return data + ' Days';
                        }
                        else {
                            return 'No Delay';
                        }
                    },
                },
                {
                    data: 'OrderStatus',
                    render: function (data, type) {
                        if (data === 1) {
                            return '<span class="label label-sm label-success">Close</span>';
                        }
                        else {

                            return '<span class="label label-sm label-danger">Open</span>';
                        }
                    },
                },
            ],
            columnDefs: [
                {
                    targets: 2,
                    render: function (data) {
                        return moment(data).format('DD MMM  YYYY');
                    },
                },
                {
                    targets: 3,
                    render: function (data) {
                        return moment(data).format('DD MMM YYYY');
                    },
                },
            ]

        });

    });


    var dyeinglist = null;
    var jqxhr = $.getJSON('Home.aspx/GetDyeingStatusList', function (data) {

        dyeinglist = $.parseJSON(data.d);
    });
    jqxhr.done(function () {
        console.log(dyeinglist);

        $('#tblDyeing').DataTable({
            lengthMenu: [[5, 10, 25, 50, -1], [10, 25, 50, "All"]],
            data: dyeinglist.data,
            columns: [

                { data: 'DyerName' },
                { data: 'IndentNo' },
                { data: 'IssueDate' },
                { data: 'ExpectedDate' },
                { data: 'Quantity' },
                {
                    data: 'DelayDays',
                    render: function (data, type) {
                        if (data > 0) {
                            return data + ' Days';
                        }
                        else {
                            return 'No Delay';
                        }
                    },
                },
                {
                    data: 'DyeingStatus',
                    render: function (data, type) {
                        if (data === 1) {
                            return '<span class="label label-sm label-success">Close</span>';
                        }
                        else {

                            return '<span class="label label-sm label-danger">Open</span>';
                        }
                    },
                },
            ],
            columnDefs: [
                {
                    targets: 2,
                    render: function (data) {
                        return moment(data).format('DD MMM  YYYY');
                    },
                },
                {
                    targets: 3,
                    render: function (data) {
                        return moment(data).format('DD MMM YYYY');
                    },
                },
            ]

        });


    });




});

function BindChart(orderlist) {

    var SeriesList = [];
    orderlist.chartData.forEach(myFunction);
    function myFunction(item, index, arr) {
        var coOrdinate = [];
        coOrdinate.push((item.OrderStatus == 0 ? "Open" : "Close"));
        coOrdinate.push((item.OrderCount / orderlist.totalSum * 100));
        SeriesList.push(coOrdinate);
    }
    //console.log(seriesList);
    $('#pie-chart').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: 'Orders'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Order share',
            data: SeriesList
        }]
    });
}

function OrderDetail(_orderId) {


    var bodyHtml = "";


    $('#myModal').find('h4.modal-title').empty();
    $('#myModal').find('h4.modal-title').append("Order Info");

    const obj = { OrderId: _orderId };
    $.ajax({
        url: "Home.aspx/GetOrderDetail",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {

            var result = $.parseJSON(data.d);
            console.log(data.d)
            bodyHtml += "<div class='row'><div class='col-lg-4'><p><strong>Customer Code:</strong>" + result.data[0].CustomerCode + "</p></div>";
            bodyHtml += "<div class='col-lg-4'><p><strong>Order No:</strong>" + result.data[0].CustomerOrderNo + "</p></div>";
            bodyHtml += "<div class='col-lg-4'><p><strong>ORDER DATE:</strong>" + result.data[0].OrderDate + "</p></div>";
            bodyHtml += "<div class='col-lg-4'><p><strong>DISPATCHED DATE:</strong>" + result.data[0].DispatchDate + "</p></div>";
            bodyHtml += "<div class='col-lg-4'><p><strong>DUE DATE:</strong>" + result.data[0].DueDate + "</p></div></div>";



            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr><th>Due Date</th>";
            bodyHtml += "<th>Technique</th><th>Quality</th><th>Design</th>";
            bodyHtml += "<th>Color</th><th>Shape</th><th>Shade</th><th>Size</th>";
            bodyHtml += "<th>Unit</th><th>Quantity</th><th>Filler</th></tr></thead><tbody>";
            if (result.data.length > 0) {
                $.each(result.data, function (index, item) {
                    bodyHtml += "<tr><td>" + item.DueDate + "</td>";
                    bodyHtml += "<td>" + item.Technique + "</td><td>" + item.Quality + "</td><td>" + item.Design + "</td>";
                    bodyHtml += "<td>" + item.Color + "</td><td>" + item.Shape + "</td><td>" + item.Shade + "</td><td>" + item.Size + "</td>";
                    bodyHtml += "<td>" + item.Unit + "</td><td>" + item.OrderQty + "</td><td>" + item.Filler + "</td></tr>";
                });
            }
            else {
                bodyHtml += "<tr><td colspan='11'>Data not found</td></tr>";
            }

            bodyHtml += "</tbody></table></div></div></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='row'><div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div></div>";
        },
        complete: function (xhr, status) {
            $('#myModal').find('div.modal-body').empty();
            $('#myModal').find('div.modal-body').html(bodyHtml);
            $('#myModal').modal('show');
        }
    });


}

function ProcessReport(_orderId, _processId, _title, _type) {
    $('h4.modal-title').empty();
    $('h4.modal-title').append("Process Report");
    $('div.modal-body').empty();
    var panelhtml = "<div class='row' data-order='0'></div>";
    $('div.modal-body').html(panelhtml);

    switch (_type) {
        case 0:
            url = "Home.aspx/GetPurchaseList";
            break;
        case 1:
            url = "Home.aspx/GetIndentDetail";
            break;
        case 2:
            url = "Home.aspx/GetIssueDetail";
            break;
        case 3:
            url = "Home.aspx/GetFinishDetail";
            break;
    }
    const obj = { OrderId: _orderId, ProcessId: _processId };
    ReqHtml(url, obj, _title, 0);
}

function OrderSummary(_orderId, index, cust) {

    var bodyHtml = "";
    const obj = { OrderId: _orderId };
    $.ajax({
        url: "Home.aspx/GetOrderDetail",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {

            var result = $.parseJSON(data.d);
            console.log(data.d)


            bodyHtml += "<div class='col-lg-12'><h4 class='box-heading'>ORDER  SUMMARY</h4></div><div class='col-lg-4'><p><strong class='mrm'>Customer Code:</strong>" + cust + "</p></div>";
            bodyHtml += "<div class='col-lg-4'><p><strong class='mrm'>Order No:</strong>" + result.data[0].CustomerOrderNo + "</p></div>";
            bodyHtml += "<div class='col-lg-4'><p><strong class='mrm'>Order Date:</strong>" + result.data[0].OrderDate + "</p></div>";
            bodyHtml += "<div class='col-lg-4'><p><strong class='mrm'>Dispatched Date:</strong>" + result.data[0].DispatchDate + "</p></div>";
            bodyHtml += "<div class='col-lg-4'><p><strong class='mrm'>Due Date:</strong>" + result.data[0].DueDate + "</p></div>";

            bodyHtml += "<div class='col-lg-12'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr>";
            bodyHtml += "<th>Technique</th><th>Quality</th><th>Design</th>";
            bodyHtml += "<th>Color</th><th>Shape</th><th>Shade</th><th>Size</th>";
            bodyHtml += "<th>Unit</th><th>Quantity</th><th>Filler</th></tr></thead><tbody>";
            if (result.data.length > 0) {
                $.each(result.data, function (i, item) {
                    bodyHtml += "<tr>";
                    bodyHtml += "<td>" + item.Technique + "</td><td>" + item.Quality + "</td><td>" + item.Design + "</td>";
                    bodyHtml += "<td>" + item.Color + "</td><td>" + item.Shape + "</td><td>" + item.Shade + "</td><td>" + item.Size + "</td>";
                    bodyHtml += "<td>" + item.Unit + "</td><td>" + item.OrderQty + "</td><td>" + item.Filler + "</td></tr>";
                });
            }
            else {
                bodyHtml += "<tr><td colspan='10'>Data not found</td></tr>";
            }

            bodyHtml += "</tbody></table></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div>";
        },
        complete: function (xhr, status) {





            $('#bodyItem').find('div[data-order="' + index + '"]').html(bodyHtml);


            //$('div.modal-body').append(bodyHtml);
            //$('#myModal').find('div.modal-body').find('div[data-order="' + index + '"]').each(function () {
            //    $(this).html(bodyHtml);
            //})


        }
    });

    return bodyHtml;
}

function SummaryReport(elem, orderId) {



    $('h4.modal-title').empty();
    $('h4.modal-title').append("Summary Report");
    $('h4.modal-title').append("<a class='sorting close text-green btn btn-default btn-xs mrm'><i class='fa fa-sort mrs'></i><strong>Sequencing</strong></a>");

    $('div.modal-body').empty();

    var panelhtml = "<div class='row' data-order='0'></div>";
    elem.next("div.btn-group").find("ul.dropdown-menu li").each(function (index) {
        ++index;
        panelhtml += "<div class='row' data-order=" + index + "></div>";
    });
    $('div.modal-body').html(panelhtml);

    OrderSummary(orderId, 0, elem.closest("tr").find("td:nth-child(1)").html());
    elem.next("div.btn-group").find("ul.dropdown-menu li").each(function (index) {
        var item = $(this).find("a");
        var _type = parseInt(item.attr('extType'));
        var seq = parseInt(item.attr('extSeq')) + 1;
        var _title = item.attr('title') + " SUMMARY";
        var url;
        var _processId = parseInt(item.attr('prhref'));
        switch (_type) {
            case 0:
                url = "Home.aspx/GetPurchaseList";
                break;
            case 1:
                url = "Home.aspx/GetIndentDetail";
                break;
            case 2:
                url = "Home.aspx/GetIssueDetail";
                break;
            case 3:
                url = "Home.aspx/GetFinishDetail";
                break;
        }
        const obj = { OrderId: orderId, ProcessId: _processId };
        ReqHtml(url, obj, _title, seq);

    });



}

function ReqHtml(_url, _Req, _title, _seq) {


    var bodyHtml = "";
    $.ajax({
        url: _url,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(_Req),
        success: function (data) {
            console.log(data.d)
            var result = $.parseJSON(data.d);
            bodyHtml += "<div class='col-lg-12'><h4 class='box-heading'>" + _title + "</h4></div><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr><th>Supplier</th><th>Design No.</th><th>Item Description</th>";
            bodyHtml += "<th>Indent No.</th><th>Req. Date</th>";
            bodyHtml += "<th>Issue Date</th><th>Rec. Date</th>";
            bodyHtml += "<th>Reqd Qty.</th><th> Issued Qty.</th><th>Rec. Qty.</th><th>Loss Qty.</th><th>Return Qty.</th>";
            bodyHtml += "<th>Pen. Qty.</th><th>Reqd Bal. Qty.</th><th>Delay Days</th><th>Status</th></tr></thead><tbody>";
            if (result.data.length > 0) {
                $.each(result.data, function (index, item) {

                    if (item.ChallanNo == undefined || item.ChallanNo == null)
                        item.ChallanNo = "----";
                    if (item.DesignName == undefined || item.DesignName == null)
                        item.DesignName = "----";

                    bodyHtml += "<tr><td>" + item.VendorName + "</td><td>" + item.DesignName + "</td><td>" + item.MaterialName + "</td>";
                    bodyHtml += "<td>" + item.ChallanNo + "</td><td>" + item.RequestDate + "</td><td>" + item.IssueDate + "</td><td>" + item.ReceiveDate + "</td>";
                    bodyHtml += "<td>" + item.RequiredQty + "</td><td>" + item.IssueQty + "</td><td>" + item.ReceiveQty + "</td><td>" + item.LossQty + "</td><td>" + item.ReturnQty + "</td>";
                    bodyHtml += "<td>" + item.PendingQty + "</td><td>" + item.ReqdBalQty + "</td><td>" + item.DelayDays + "</td><td>" + item.IStatus + "</td></tr>";
                });
            }
            else {
                bodyHtml += "<tr><td colspan='16'>Data not found</td></tr></tbody>";
            }
            bodyHtml += "</tbody></table></div></div>"
        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='col-lg-12'><h4 class='box-heading'>" + _title + "</h4></div><div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div>";
        },
        complete: function (xhr, status) {

            $('#bodyItem').find('div[data-order="' + _seq + '"]').html(bodyHtml);
            $('#myModal').modal('show');



        }
    });
    return bodyHtml;
}