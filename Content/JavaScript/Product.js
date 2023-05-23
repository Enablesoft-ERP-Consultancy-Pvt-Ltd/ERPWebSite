
$(function () {




  
    var cstIndex = 0;
    var attributeList = null;
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

        alert(cstIndex);
        var $table = $(this).closest("table").find('tbody');
        cstIndex++;
        var csthtml = $.validator.format($("#propertyTemplate").val());
        var _attributeId = $('#ddlAttributeId').val();
        var _attribute = $("#ddlAttributeId option:selected").text();
        var _attributeVal = $('#txtAttributeValue').val();
        csthtml = csthtml(cstIndex, _attributeId, _attribute, _attributeVal);
        $table.append(csthtml);

        //var itemId = '#ddlPropertyList' + cstIndex + '_Attribute';
        //SelectList(itemId, attributeList, 'Select Property Name');

    });


    $('#tblProperty').on("click", ".btnDel", function () {
        var $row = $(this).closest("tr");
        $row.remove();
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