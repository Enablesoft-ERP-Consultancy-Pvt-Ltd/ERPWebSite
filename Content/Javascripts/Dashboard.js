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

            /* alert(orderlist.chartData[0].OrderCount);*/
            let table = $('#tblOrder').DataTable({
                lengthMenu: [[5, 10, 25, 50, -1], [10, 25, 50, "All"]],
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
                            return "<a class='btnPurchase' exthref=" + data.OrderId + "><i class='fa fa-shopping-cart text-red mrm mediumtxt'></i>Purchase</a>";
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

            var SeriesList = [];
            orderlist.chartData.forEach(myFunction);

            function myFunction(item, index, arr) {
                var coOrdinate = [];
                coOrdinate.push((item.OrderStatus == 0 ? "Open" : "Close"));
                coOrdinate.push((item.OrderCount / orderlist.totalSum * 100));
                SeriesList.push(coOrdinate);
            }



            // Add event listener for opening and closing details
            table.on('click', 'a.btnPurchase', function (e) {

                var elem = $(this);
                var id = elem.attr('exthref');
                var _OrderId = parseInt(id);
                var bodyHtml = "";
                $('div.modal-title').empty();

                $('div.modal-title').html("Purchase Report");



                const obj = { OrderId: _OrderId };
                $.ajax({

                    url: "Home.aspx/GetPurchaseList",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: JSON.stringify(obj),
                    success: function (data) {

                        var result = $.parseJSON(data.d);

                        bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
                        bodyHtml += " <table class='table table-hover table-bordered table-striped'>";
                        bodyHtml += " <tr><th>Category</th><th>PO No</th><th>PO Status</th><th>PO Date</th><th>Supplier Name</th>";
                        bodyHtml += " <th>Item Name</th><th>Rate</th><th>PO Qty</th><th>Delv. Date</th><th>Delay Days</th></tr>";
                        if (result.length > 0) {
                            $.each(result, function (item) {

                                bodyHtml += "<tr><td>" + item.category + "</td><td>" + item.PONo + "</td><td>" + item.POStatus + "</td><td>" + item.PODate + "</td ><td>" + item.SupplierName + "</td><td>" + item.ItemName + "</td><td>" + item.Rate + "</td><td>" + item.POQty + "</td><td>" + item.DelvDate + "</td><td>" + item.DelayDays + "</td></tr>"

                            });
                        }
                        else {
                            bodyHtml += "<tr><td colspan='10'>Data not found</td></tr>";
                        }

                        bodyHtml += "</table></div></div></div>"

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














            });




            // Add event listener for opening and closing details
            table.on('click', 'a.btnDyeing', function (e) {

                var elem = $(this);
                var id = elem.attr('exthref');
                var _OrderId = parseInt(id);
                var bodyHtml = "";
                $('div.modal-title').empty();

                $('div.modal-title').html("Purchase Report");



                const obj = { OrderId: _OrderId };
                $.ajax({

                    url: "Home.aspx/GetPurchaseList",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: JSON.stringify(obj),
                    success: function (data) {

                        var result = $.parseJSON(data.d);

                        bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
                        bodyHtml += " <table class='table table-hover table-bordered table-striped'>";
                        bodyHtml += " <tr><th>Category</th><th>PO No</th><th>PO Status</th><th>PO Date</th><th>Supplier Name</th>";
                        bodyHtml += " <th>Item Name</th><th>Rate</th><th>PO Qty</th><th>Delv. Date</th><th>Delay Days</th></tr>";
                        if (result.length > 0) {
                            $.each(result, function (item) {

                                bodyHtml += "<tr><td>" + item.category + "</td><td>" + item.PONo + "</td><td>" + item.POStatus + "</td><td>" + item.PODate + "</td ><td>" + item.SupplierName + "</td><td>" + item.ItemName + "</td><td>" + item.Rate + "</td><td>" + item.POQty + "</td><td>" + item.DelvDate + "</td><td>" + item.DelayDays + "</td></tr>"

                            });
                        }
                        else {
                            bodyHtml += "<tr><td colspan='10'>Data not found</td></tr>";
                        }

                        bodyHtml += "</table></div></div></div>"

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














            });








            console.log(SeriesList);
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






















        },
        error: function (xhr) {



            alert("There is some technical Issue contact with admin!");


        }
    });







    var polist = null;
    var jqxhr = $.getJSON('Home.aspx/GetPOStatusList', function (data) {


        polist = $.parseJSON(data.d);

    });
    jqxhr.done(function () {
        console.log(orderlist);

        $('#tblVendorPO').DataTable({
            lengthMenu: [[5, 10, 25, 50, -1], [10, 25, 50, "All"]],
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