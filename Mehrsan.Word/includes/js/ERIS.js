/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var resource = null;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//$('#divPopulateAlert').slideUp();

$('#btnSubmit').click(function (e) {

    e.preventDefault();
    $('#messageAlert').slideDown();
    //var url = window.location.href;
    //var domainIndex = url.lastIndexOf("/");
    var currentHttpMethod = $('#btnSelectedHttpMethod').html();
    var currentData = $('#txtRequestData').val();
    //var currentUrl = 'http://localhost:62193/api/Word/PostWord';//$('#txtUrl').html();//url.substring(0, domainIndex);http://jsonplaceholder.typicode.com/
    var currentUrl = $('#btnSelectedAction').attr('data-value');//'http://jsonplaceholder.typicode.com/posts/1';
    //var find = '/';
    //var re = new RegExp(find, 'g');
    //currentUrl = currentUrl + '/api/Word/GetMethod/' + $('#txtUrl').val().replace(re, '__');
    $.ajax({
        url: currentUrl,
        type: currentHttpMethod,
        httpMethod:currentHttpMethod,  
        data:currentData,
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        success: function (data) {
            $('#messageAlert').removeClass("alert-danger");
            $('#messageAlert').addClass("alert-success");

            var responsetext = getSuccessResponseText(data);
            $('#divResponse').html(responsetext);
            
        },
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Basic " + "SU5QVVRUOjEyMzQ1Ng==");
            xhr.setRequestHeader("Accept", "application/hal+json");
            },
        error: function (data) {
            $('#messageAlert').removeClass("alert-success");
            $('#messageAlert').addClass("alert-danger");

            var responsetext = getFailureResponseText(data);
            $('#divResponse').html(responsetext);

        }
    }); 

    
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$('#btnPopulate').click(function (e) {

    e.preventDefault();
    var currentUrl = $('#txtRootUrl').val();
    var currentData = $('#txtRequestData').val();
    $.ajax({
        url: currentUrl,
        type: 'Get',
        httpMethod: 'Get',
        data: currentData,
        scriptCharset: "utf-8",
        contentType: "application/x-www-form-urlencoded;charset=utf-8",
        encoding: "UTF-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Basic " + "SU5QVVRUOjEyMzQ1Ng==");
            xhr.setRequestHeader("Accept", "application/hal+json");
        },
        success: function (data) {
            $('#messageAlert').removeClass("alert-danger");
            $('#messageAlert').addClass("alert-success");

            var responsetext = getSuccessResponseText(data);
            $('#ulActions').empty();
            for (i = 0 ; i < data._links.collection.length; i++) {
                                        
                var link = data._links.collection[i];
                var startIndex = data._links.collection[i].name.indexOf('>');
                var endIndex = data._links.collection[i].name.lastIndexOf('>');
                var length = endIndex - startIndex;
                var httpMethod = data._links.collection[i].name.slice(startIndex+1, endIndex);
                 
                var li = '<li tag='+ httpMethod +' data-value=' + link.href + '><a href="#" >' +link.title + '</a></li>';
                $('#ulActions').append($(li));
                $(li).bind("click", setActionText);
                $('#ulActions li').bind("click", setActionText);

                //$('#divPopulateAlert').slideDown(1500);
                //$('#divPopulateAlert').slideUp(500);
            }

            

            $('#divResponse').html(responsetext);

        },
        error: function (data) {
            $('#messageAlert').removeClass("alert-success");
            $('#messageAlert').addClass("alert-danger");

            var responsetext = getFailureResponseText(data);
            $('#divResponse').html(responsetext);

        }
    });


});


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function setActionText() {
    $('#btnSelectedAction').html($(this).text());
    var selectedurl = $(this).attr('data-value');
    var selectedTag = $(this).attr('tag');
    $('#btnSelectedAction').attr('data-value', selectedurl);
    $('#btnSelectedHttpMethod').html(selectedTag);

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$('#ulActions li').on('click', function () {
    $('#btnSelectedAction').html($(this).text());
    
});
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
$('#ulHttpMethods li').on('click', function () {
    $('#btnSelectedHttpMethod').html($(this).text());
});
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getSuccessResponseText(responseObj) {
    var responsetext = '';
    if (responseObj) {
        responsetext = responsetext + JSON.stringify(responseObj, null, "    ");//addItem('data:', responseObj);
        
        
        if(responsetext == '[]')
            responsetext = 'Customer Created Successfully.'

    }
    return responsetext;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getFailureResponseText(responseObj) {
    var responsetext = '';
    if (responseObj) {

    
        responsetext = responsetext + addItem('Ready state:' , responseObj.readyState) ;
        responsetext = responsetext + addItem('Status:' , responseObj.Status);
        responsetext = responsetext + addItem('Status text:' , responseObj.statusText) ;
        responsetext = responsetext + addItem('ResponseText:', responseObj.responseText);

        if (responseObj.responseJSON) {
            responsetext = responsetext + addItem('JSON Message:' , responseObj.responseJSON.Message) ;
            responsetext = responsetext + addItem('JSON Message Details:' , responseObj.responseJSON.MessageDetail) ;
        }
    }
    return responsetext;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function addItem(itemLabel, item) {
    var lineSplitter = '<br><br>'

    var res = '<b>' + itemLabel + '</b>';
    if (item) {
        res = res +'       ' + item;
    }
    else
        res = res + '-';

    res = res + lineSplitter;
    return res;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$('#btnCloseMessageAlert').click(function (e) {

    e.preventDefault();

    var parent = $(this).parent();
    $(parent).slideUp();

});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function parseResponse(data) {

}