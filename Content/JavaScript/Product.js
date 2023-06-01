var cstIndex = 0;
$(function () {


    var attributeList = null;


    BindAllProtery();

    $.ajax({
        type: "POST",
        url: "DefineItemCodeOther.aspx/GetAttributeMaster",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        success: function (result) {
            attributeList = jQuery.parseJSON(result.d);
            SelectList("#ddlAttributeId", attributeList, 'Select Property Name');
        },
        error: function (xhr) {
            alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
        }
    });

    $('#tblProperty').on("click", ".btnAdd", function () {

        var _attributeId = $('#ddlAttributeId').val();
        var _attribute = $("#ddlAttributeId option:selected").text();
        var _attributeVal = $('#txtAttributeValue').val();
        var $table = $(this).closest("table").find('tbody');
        SaveData(_attributeId, _attribute, _attributeVal, $table);





    });


    $('#tblProperty').on("click", ".btnDel", function () {
        var $row = $(this).closest("tr");
        deleteData($row)
    });


});


function CloseForm() {
    self.close();
}
function isNumber(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        alert('numeric only');
        return false;
    }
    else {
        return true;
    }
}

function SelectList(item, ListData, InitialText) {
    $(item).empty();
    var itemList = "";
    itemList += "<option value=''>" + InitialText + "</option>";
    if (ListData) {
        $.each(ListData, function (i, lstItem) {
            itemList += "<option value='" + lstItem.ItemId + "'>" + lstItem.ItemName + "</option>";
        });
    }
    $(item).html(itemList);
}

function SaveData(_attributeId, _attribute, _attributeVal, $table) {


    const obj = { attributeId: _attributeId, attribute: _attributeVal };
    $.ajax({
        type: "POST",
        url: "DefineItemCodeOther.aspx/SaveData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(obj),
        success: function (result) {

            if (result.d) {
                var csthtml = $.validator.format($("#propertyTemplate").val());
                csthtml = csthtml(cstIndex, _attributeId, _attribute, _attributeVal);
                $table.append(csthtml);
                cstIndex++;
            }
            else {

                alert('There is some technical issue !Please contact to admin');
            }

        },
        error: function (xhr) {
            alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
        }
    });


}






function deleteData($row) {

    
    const obj = { attributeId: $row.find("input.AttributeId").val(), attribute: $row.find("input.AttributeValue").val() };
    $.ajax({
        type: "POST",
        url: "DefineItemCodeOther.aspx/DeleteProperty",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(obj),
        success: function (result) {

            if (result.d) {
                $row.remove();
            }
            else {

                alert('There is some technical issue !Please contact to admin');
            }
        },
        error: function (xhr) {
            alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
        }
    });


}












function BindAllProtery() {

    $.ajax({
        type: "POST",
        url: "DefineItemCodeOther.aspx/GetItemsPropertyList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            var dataList = jQuery.parseJSON(result.d);
            $.each(dataList, function (key, item) {

                var $table = $("#tblProperty").find("tbody");
                var csthtml = $.validator.format($("#propertyTemplate").val());
                csthtml = csthtml(cstIndex, item.AttributeId, item.AttributeName, item.AttributeValue);
                $table.append(csthtml);
                cstIndex++;
            });

        },
        error: function (xhr) {
            alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
        }
    });
}