


















var ProcessList = [{ ProcessId: 5, Process: "DYEING(PCS)", Title: "DYEING(PCS) Report", Type: 1 },

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


{ ProcessId: 13, Process: "STITCHING", Title: "STITCHING", Type: 3 },



{ ProcessId: 21, Process: 'Finishing BLOCK PRINTING', Title: "Finishing BLOCK PRINTING", Type: 3 },
{ ProcessId: 30, Process: ' ', Title: "Finishing COLOUR CUT", Type: 3 },
{ ProcessId: 10, Process: ' ', Title: "Finishing COMPUTER EMBROIDERY(PCS)", Type: 3 },
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
{ ProcessId: 2, Process: ' ', Title: "Finishing MANUAL EMBROIDERY(PCS)", Type: 3 },
{ ProcessId: 17, Process: ' ', Title: "Finishing PACKING", Type: 3 },
{ ProcessId: 39, Process: ' ', Title: "Finishing PANEL MAKING", Type: 3 },
{ ProcessId: 42, Process: ' ', Title: "Finishing PANNEL WEAVING", Type: 3 },
{ ProcessId: 4, Process: ' ', Title: "Finishing PATCH STITCHING", Type: 3 },
{ ProcessId: 14, Process: ' ', Title: "Finishing PRESING", Type: 3 },
{ ProcessId: 44, Process: ' ', Title: "Finishing QUILTING", Type: 3 },
{ ProcessId: 40, Process: ' ', Title: "Finishing RE-WORK", Type: 3 },
{ ProcessId: 49, Process: ' ', Title: "Finishing SAMPLING", Type: 3 },
{ ProcessId: 24, Process: ' ', Title: "Finishing SCREEN PRINTING", Type: 3 },
{ ProcessId: 13, Process: ' ', Title: "Finishing STITCHING", Type: 3 },
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









$(function () {

    const FROM_PATTERN = 'YYYY-MM-DD HH:mm:ss.SSS';
    const TO_PATTERN = 'DD/MM/YYYY';

    var orderlist = null;

    $('#myModal').on('hidden.bs.modal', function (evt) {
        $('.modal .modal-body').empty();
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



                            var btnHtml = "<a title='Order Detail' class='btnOrder' exthref=" + data.OrderId + "><i class='fa fa fa-info-circle text-red mediumtxt mrm'></i></a>";
                            btnHtml += "<div class='btn-group'><button type='button' data-bs-toggle='dropdown' class='btn btn-dark dropdown-toggle  ptx pbx'>";
                            btnHtml += "Choose Process<span class='caret'></span></button><ul role='menu' class='dropdown-menu dropdown-left-posotion'>";


                            btnHtml += "<li><a class='btnPurchase mrm' exthref=" + data.OrderId + "><i class='fa fa-shopping-cart mrs text-green'></i><strong>Purchase</strong></a></li>";



                            btnHtml += "<li><a class='btnProcess mrm' prhref=" + 5 + " exthref=" + data.OrderId + "><i class='fa fa-pencil mrs text-green'></i><strong>Dyeing(DY)</strong></a></li>";
                            btnHtml += "<li><a class='btnProcess mrm' prhref=" + 8 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>BLOCK PRINTING(MTR)</strong></a></li>";
                            btnHtml += "<li><a class='btnProcess mrm' prhref=" + 11 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>DIGITAL PRINTING(MTR)</strong></a></li>";
                            btnHtml += "<li><a class='btnProcess mrm' prhref=" + 12 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>COMPUTER EMBROIDERY(MTR)</strong></a></li>";

                            btnHtml += "<li><a class='btnProcess mrm' prhref=" + 18 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>WASHING(MTR)</strong></a></li>";
                            btnHtml += "<li><a class='btnProcess mrm' prhref=" + 34 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>STONE WASH(MTR)</strong></a></li>";
                            btnHtml += "<li><a class='btnProcess mrm' prhref=" + 29 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>SCREEN PRINTING (MTR)</strong></a></li>";





                            btnHtml += "<li><a class='btnIssueProcess mrm' prhref=" + 10 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>COMPUTER EMBROIDERY(PCS)</strong></a></li>";
                            btnHtml += "<li><a class='btnIssueProcess mrm' prhref=" + 1 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>CUTTING</strong></a></li>";
                            btnHtml += "<li><a class='btnIssueProcess mrm' prhref=" + 2 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>MANUAL EMBROIDERY(PCS)</strong></a></li>";
                            btnHtml += "<li><a class='btnIssueProcess mrm' prhref=" + 42 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>PANNEL WEAVING</strong></a></li>";
                            btnHtml += "<li><a class='btnIssueProcess mrm' prhref=" + 4 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>PATCH STITCHING</strong></a></li>";
                            btnHtml += "<li><a class='btnIssueProcess mrm' prhref=" + 38 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>TABLE TUFTING</strong></a></li>";




                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 13 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>STITCHING</strong></a></li>";



                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 21 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  BLOCK PRINTING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 30 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  COLOUR CUT </strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 10 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  COMPUTER EMBROIDERY(PCS)</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 26 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  DIGITAL PRINTING (PCS)</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 6 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  DYEING(PCS)</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 3 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  FILLING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 7 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  FINAL FINISHING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 27 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  FOLD &amp; THREAD CUTTING </strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 47 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  HAND STITCHING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 28 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  HEMMING STICHING </strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 41 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  KANTHA HANDWORK</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 31 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  KNOTTING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 20 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  KNOWTING &amp; WRAPPING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 23 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  LABEL STITCHING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 43 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  LACE STITCHING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 2 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  MANUAL EMBROIDERY(PCS)</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 17 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  PACKING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 39 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  PANEL MAKING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 42 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  PANNEL WEAVING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 4 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  PATCH STITCHING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 14 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  PRESING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 44 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  QUILTING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 40 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  RE-WORK</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 49 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  SAMPLING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 24 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  SCREEN PRINTING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 13 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  STITCHING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 22 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  STONE WASH</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 38 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  TABLE TUFTING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 16 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  TASSEL STITCHING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 19 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  THREAD CUTTING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 32 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  THREAD CUTTING &amp; TASSLING </strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 37 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  TUFTING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 46 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  UPHOLSTERY</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 25 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  VACUUM</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 33 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  WASHING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 48 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  WEAVING</strong></a></li>";
                            btnHtml += "<li><a class='btnFinish mrm' prhref=" + 35 + "  exthref=" + data.OrderId + "><i class='fa fa-cogs mrs text-green'></i><strong>Finishing  WRAPPING</strong></a></li>";

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



            table.on('click', 'a.btnOrder', function (e) {
                var elem = $(this);
                var id = elem.attr('exthref');
                var _orderId = parseInt(id);
                OrderDetail(_orderId);
            });

            table.on('click', 'a.btnPurchase', function (e) {
                var elem = $(this);
                var id = elem.attr('exthref');
                var _orderId = parseInt(id);
                PurchaseReport(_orderId);
            });

            table.on('click', 'a.btnProcess', function (e) {
                var elem = $(this);
                var _orderId = parseInt(elem.attr('exthref'));
                var _processId = parseInt(elem.attr('prhref'));
                ProcessReport(_orderId, _processId);
            });

            table.on('click', 'a.btnIssueProcess', function (e) {
                var elem = $(this);
                var _orderId = parseInt(elem.attr('exthref'));
                var _processId = parseInt(elem.attr('prhref'));
                ProcessIssueReport(_orderId, _processId);
            });



            table.on('click', 'a.btnFinish', function (e) {
                var elem = $(this);
                var _orderId = parseInt(elem.attr('exthref'));
                var _processId = parseInt(elem.attr('prhref'));
                FinishItemReport(_orderId, _processId);
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


    $('#myModal').find('h4.modal-title').html("Order Info");
    const obj = { OrderId: _orderId };
    $.ajax({
        url: "Home.aspx/GetOrderDetail",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {

            var result = $.parseJSON(data.d);
            console.log(data.d)

            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr><th>Due Date</th>";
            bodyHtml += "<th>Technique</th><th>Quality</th><th>Design</th>";
            bodyHtml += "<th>Color</th><th>Shape</th><th>Shade</th><th>Size</th>";
            bodyHtml += "<th>Unit</th><th>Ouantity</th><th>Filler</th></tr></thead><tbody>";
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


function PurchaseReport(_orderId) {

    var bodyHtml = "";
    $('h4.modal-title').empty();
    $('h4.modal-title').html("Purchase Report");
    const obj = { OrderId: _orderId };
    $.ajax({
        url: "Home.aspx/GetPurchaseList",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {

            console.log(data.d)

            var result = $.parseJSON(data.d);

            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += " <tr><th>Supplier Name</th><th>PO No</th><th>PO Date</th><th>Delv. Date</th>";
            bodyHtml += " <th>Item Name</th><th>Rate</th><th>PO Qty</th><th>Rec. Qty.</th><th>Pending Qty.</th><th>Delay Days</th><th>PO Status</th></tr></thead><tbody>";
            if (result.data.length > 0) {

                $.each(result.data, function (index, item) {

                    bodyHtml += "<tr><td>" + item.SupplierName + "</td><td>" + item.PONo + "</td><td>" + item.PODate + "</td ><td>" + item.DelvDate + "</td><td>" + item.ItemName + "</td><td>" + item.Rate + "</td><td>" + item.POQty + "</td><td>" + item.RecQty + "</td><td>" + item.PendingQty + "</td><td>" + item.DelayDays + "</td><td>" + item.POStatus + "</td></tr>"

                });
            }
            else {
                bodyHtml += "<tr><td colspan='10'>Data not found</td></tr>";
            }

            bodyHtml += "</tbody></table></div></div></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='row'><div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div></div>";
        },
        complete: function (xhr, status) {
            $('div.modal-body').empty();
            $('div.modal-body').html(bodyHtml);
            $('#myModal').modal('show');
        }
    });


}


function ProcessReport(_orderId, _processId) {

    var bodyHtml = "";

    $('h4.modal-title').empty();
    var item = ProcessList.find(S => S.ProcessId == _processId);
    $('h4.modal-title').html(item.Title);



    const obj = { OrderId: _orderId, ProcessId: _processId };
    $.ajax({
        url: "Home.aspx/GetIndentDetail",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {
            console.log(data.d)
            var result = $.parseJSON(data.d);

            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr><th>Supplier</th><th>Item Description</th>";
            bodyHtml += "<th>Indent No.</th><th>Indent Date</th>";
            bodyHtml += "<th>Req. Date</th>";
            bodyHtml += "<th>Issue Qty.</th><th>Rec. Qty.</th><th>Return Qty.</th>";
            bodyHtml += "<th>Pen. Qty.</th><th>Delay Days</th><th>Status</th></tr></thead><tbody>";

            if (result.data.length > 0) {

                $.each(result.data, function (index, item) {

                    bodyHtml += "<tr><td>" + item.VendorName + "</td><td>" + item.MaterialName + "</td>";
                    bodyHtml += "<td>" + item.IndentNo + "</td><td>" + item.IndentDate + "</td><td>" + item.RequestDate + "</td>";
                    bodyHtml += "<td>" + item.IssueQuantity + "</td><td>" + item.RecQuantity + "</td>";
                    bodyHtml += "<td>" + item.ReturnQty + "</td><td>" + item.PendingQty + "</td><td>" + item.DelayDays + "</td><td>" + item.IStatus + "</td></tr>";

                });
            }
            else {
                bodyHtml += "<tr><td colspan='11'>Data not found</td></tr></tbody>";
            }

            bodyHtml += "</tbody></table></div></div></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='row'><div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div></div>";
        },
        complete: function (xhr, status) {
            $('div.modal-body').empty();
            $('div.modal-body').html(bodyHtml);
            $('#myModal').modal('show');
        }
    });


}








function FinishItemReport(_orderId, _processId) {

    var bodyHtml = "";

    $('h4.modal-title').empty();
    var item = ProcessList.find(S => S.ProcessId == _processId);
    $('h4.modal-title').html(item.Title);



    const obj = { OrderId: _orderId, ProcessId: _processId };
    $.ajax({
        url: "Home.aspx/GetFinishDetail",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {
            console.log(data.d)
            var result = $.parseJSON(data.d);

            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr><th>Supplier</th><th>Item Description</th>";
            bodyHtml += "<th>Issue Date</th>";
            bodyHtml += "<th>Req. Date</th><th>Rec. Date</th>";
            bodyHtml += "<th>Issue Qty.</th><th>Rec. Qty.</th><th>Rate</th>";
            bodyHtml += "<th>Pen. Qty.</th><th>Delay Days</th><th>Status</th></tr></thead><tbody>";

            if (result.data.length > 0) {

                $.each(result.data, function (index, item) {

                    bodyHtml += "<tr><td>" + item.VendorName + "</td><td>" + item.MaterialName + "</td>";
                    bodyHtml += "<td>" + item.IssueDate + "</td><td>" + item.ReqDate + "</td>";
                    bodyHtml += "<td>" + item.RecDate + "</td><td>" + item.IssueQuantity + "</td><td>" + item.RecQuantity + "</td>";
                    bodyHtml += "<td>" + item.Rate + "</td><td>" + item.PendingQty + "</td><td>" + item.DelayDays + "</td><td>" + item.IStatus + "</td></tr>";

                });
            }
            else {
                bodyHtml += "<tr><td colspan='11'>Data not found</td></tr></tbody>";
            }

            bodyHtml += "</tbody></table></div></div></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='row'><div class='col-lg-11'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div></div>";
        },
        complete: function (xhr, status) {
            $('div.modal-body').empty();
            $('div.modal-body').html(bodyHtml);
            $('#myModal').modal('show');
        }
    });


}




















function ProcessIssueReport(_orderId, _processId) {

    var bodyHtml = "";

    $('h4.modal-title').empty();
    var item = ProcessList.find(S => S.ProcessId == _processId);
    $('h4.modal-title').html(item.Title);



    const obj = { OrderId: _orderId, ProcessId: _processId };
    $.ajax({
        url: "Home.aspx/GetIssueDetail",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {
            console.log(data.d)
            var result = $.parseJSON(data.d);

            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr><th>Supplier</th><th>Item Description</th>";
            bodyHtml += "<th>Issue No.</th><th>Issue Date</th>";
            bodyHtml += "<th>Req. Date</th><th>Rec. Date</th>";
            bodyHtml += "<th>Issue Qty.</th><th>Rec. Qty.</th><th>Rate</th>";
            bodyHtml += "<th>Pen. Qty.</th><th>Delay Days</th><th>Status</th></tr></thead><tbody>";

            if (result.data.length > 0) {

                $.each(result.data, function (index, item) {

                    bodyHtml += "<tr><td>" + item.VendorName + "</td><td>" + item.MaterialName + "</td>";
                    bodyHtml += "<td>" + item.IssueNo + "</td><td>" + item.IssueDate + "</td><td>" + item.ReqDate + "</td>";
                    bodyHtml += "<td>" + item.RecDate + "</td><td>" + item.IssueQuantity + "</td><td>" + item.RecQuantity + "</td>";
                    bodyHtml += "<td>" + item.Rate + "</td><td>" + item.PendingQty + "</td><td>" + item.DelayDays + "</td><td>" + item.IStatus + "</td></tr>";

                });
            }
            else {
                bodyHtml += "<tr><td colspan='12'>Data not found</td></tr></tbody>";
            }

            bodyHtml += "</tbody></table></div></div></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='row'><div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div></div>";
        },
        complete: function (xhr, status) {
            $('div.modal-body').empty();
            $('div.modal-body').html(bodyHtml);
            $('#myModal').modal('show');
        }
    });


}