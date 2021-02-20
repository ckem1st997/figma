function makeid(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
}
$("#rdcode").on('click', function (e) {
    RadomVoucher(makeid(6));
});


$(document).on('click', '.section-image-list div.row .col-3 i.fa-trash', function (e) {

    if (confirm("Bạn muốn muốn xóa sản phẩm này !")) {
        AJAXSubmitDelete($(this).parent().children().attr('data-image'));
        $(this).parent().remove();
    }
    addlistone();
    addtwo();
});
//lấy list url product
function addlistone() {
    var str = "";
    $(".section-image-list.insert-one div.row .col-3 a").each(function () {
        if (str.length == 0)
            str = $(this).attr('data-image');
        else
            str = str + "," + $(this).attr('data-image');
    });
    $("#Image").val(str);
}

//insert-two
function addtwo() {
    var str = "";
    $(".section-image-list.insert-two div.row .col-3 a").each(function () {
        if (str.length == 0)
            str = $(this).attr('data-image');
        else
            str = str + "," + $(this).attr('data-image');
    });
    $("#CoverImage").val(str);
}
//


$(document).ready(function () {
    // add img show từ file upload
    $('input[type="file"]#filesadd').change(function (e) {

        if (e.target.files.length > 0)
            for (var i = 0; i < e.target.files.length; i++) {
                //   console.log(e.target.files.result)
                if (readImage(e.target.files[i]) == 1) {
                    alert("Tệp tải lên phải là hình ảnh !");
                    return;
                }

            }
        //  addlist();
    });
});

// delete_cart_table
function addTemp(a) {

    var id_sp_delete = parseInt($("table#cart-view tbody tr.item_2:eq(" + a + ")").attr('id'));
    $("table#cart-view tbody tr.item_2")[a].remove();
    var i = 0;
    $("span.remove-cart a").each(function () {
        $(this).attr('onclick', 'deleteCart(' + i + ')');
        // console.log(i);
        if (i == parseInt($("span.remove-cart a").length) - 1)
            i = 0;
        i++;
    });
    Delete_Cart(id_sp_delete);
    //  $("span.remove-cart a").attr('onclick', 'deleteCart(a)')
}

function readImage(file) {
    if (file.type.indexOf('image') === -1) {
        return 1;
    }
    return 2;
}


/// create file
function AJAXSubmit(file) {

    formData = new FormData();
    formData.append("filesadd", file);
    $.ajax({
        type: 'POST',
        url: '/Csm/createImage',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            $("#summernote").summernote('insertImage', "/" + data.imgNode + "");
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}


async function AJAXSubmitCreate(oFormElement, h, w) {
    var checked = $("#flexCheckChecked").prop("checked");
    var fire;
    checked == true ? fire = 1 : fire = 0;
    const formData = new FormData(oFormElement);
    $.ajax({
        type: 'POST',
        url: '/Upload/CreateImage?width=' + w + '&height=' + h + '',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.result) {
                var listimage = data.imgNode.split(",");
                if (listimage.length > 1 || listimage != null)
                    for (var i = 0; i < listimage.length; i++) {
                        var src = listimage[i].indexOf('htpps:') != -1 ? "/" + listimage[i] + "" : listimage[i];
                        $(".section-image-list.insert-one div.row").append("<div class='col-3 mb-3 position-relative'><a data-image='" + listimage[i] + "' data-fancybox='gallery' class= '/" + listimage[i] + "' href = '/" + listimage[i] + "' ><img style='max-width:100%' src='/" + listimage[i] + "' /><span class='show--image-edit'>Xem ảnh</span><i class='far fa-eye show--image-edit i--one'></i></a ><i class='fas fa-trash'></i></div>");
                    }
            }
            else {
                alert(data.imgNode);
            }
            addlistone();

        },
        error: function (data) {
            alert(data.responseText);
        }
    });

}
async function AJAXSubmitCreateOne(oFormElement, h, w) {
    const formData = new FormData(oFormElement);
    $.ajax({
        type: 'POST',
        url: '/Upload/CreateImage?width=' + w + '&height=' + h + '',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            //    console.log(data.imgNode);
            if (data.imgNode.length > 0) {
                var listimage = data.imgNode.split(",");
                if (listimage.length > 1 || listimage != null) {
                    $(".section-image-list.insert-two div.row").empty();
                    for (var i = 0; i < listimage.length; i++) {

                        $(".section-image-list.insert-two div.row").append("<div class='col-3 mb-3 position-relative'><a data-image='" + listimage[i] + "' data-fancybox='gallery' class= '/" + listimage[i] + "' href = '/" + listimage[i] + "' ><img style='max-width:100%' src='/" + listimage[i] + "' /><span class='show--image-edit'>Xem ảnh</span><i class='far fa-eye show--image-edit i--one'></i></a ><i class='fas fa-trash'></i></div>");
                    }
                }
            }
            else {
                alert("Xin vui lòng chọn ảnh !");
            }
            addtwo();

        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}

//
async function AJAXSubmitCreateOnep(oFormElement) {
    const formData = new FormData(oFormElement);

    $.ajax({
        type: 'POST',
        url: '/Csm/createImage1',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            //    console.log(data.imgNode);
            if (data.imgNode.length > 0) {
                var listimage = data.imgNode.split(",");
                if (listimage.length > 1 || listimage != null) {
                    $(".section-image-list.insert-two div.row").empty();
                    for (var i = 0; i < listimage.length; i++) {

                        $(".section-image-list.insert-two div.row").append("<div class='col-3 mb-3 position-relative'><a data-image='" + listimage[i] + "' data-fancybox='gallery' class= '/" + listimage[i] + "' href = '/" + listimage[i] + "' ><img style='max-width:100%' src='/" + listimage[i] + "' /><span class='show--image-edit'>Xem ảnh</span><i class='far fa-eye show--image-edit i--one'></i></a ><i class='fas fa-trash'></i></div>");
                    }
                }
            }
            else {
                alert("Xin vui lòng chọn ảnh !");
            }
            addtwo();

        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}

//
async function AJAXSubmitCreateOneT(oFormElement, h, w) {
    const formData = new FormData(oFormElement);
    $.ajax({
        type: 'POST',
        url: '/Upload/CreateImage?width=' + w + '&height=' + h + '',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            //console.log(data.imgNode);
            if (data.imgNode.length > 0) {
                var listimage = data.imgNode.split(",");
                if (listimage.length > 1 || listimage != null) {
                    $(".section-image-list.insert-two div.row").empty();
                    for (var i = 0; i < listimage.length; i++) {

                        $(".section-image-list.insert-two div.row").append("<div class='col-3 mb-3 position-relative'><a data-image='" + listimage[i] + "' data-fancybox='gallery' class= '/" + listimage[i] + "' href = '/" + listimage[i] + "' ><img style='max-width:100%' src='/" + listimage[i] + "' /><span class='show--image-edit'>Xem ảnh</span><i class='far fa-eye show--image-edit i--one'></i></a ><i class='fas fa-trash'></i></div>");
                    }
                }
            }
            else {
                alert("Xin vui lòng chọn ảnh !");
            }
            var str = "";
            $(".section-image-list.insert-two div.row .col-3 a").each(function () {
                if (str.length == 0)
                    str = $(this).attr('data-image');
                else
                    str = str + "," + $(this).attr('data-image');
            });
            $("#Image").val(str);

        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}
//

function RadomVoucher(code) {
    $.ajax({
        type: 'POST',
        url: '/Csm/VoucherRandom',
        data: { code: code },
        success: function (res) {
            //  console.log(res);
            if (!res) {
                RadomVoucher(makeid(6));
                // $("#Code").val(code)
            }
            if (res.t)
                $("#Code").val(res.c);
        },
        error: function (res) {
            console.log(res);
        }
    });


}

//
async function AJAXSubmitDelete(oFormElement) {
    $.ajax({
        type: 'POST',
        url: '/Upload/DeleteImage',
        data: { filesadd: oFormElement },
        success: function (res) {
            alert(res.content)
        },
        error: function (res) {
            alert(res.responseText);
        }
    });


}

$("#GroupId").change(function () {
    var id = parseInt($("#GroupId").val());
    switch (id) {
        case 1:
            $("#Height").val(632);
            $("#Width").val(1440);
            break;
        case 2:
            $("#Height").val(330);
            $("#Width").val(465);
            break;
        case 3:
            $("#Height").val(497);
            $("#Width").val(700);
            break;
        case 4:
            $("#Height").val(243);
            $("#Width").val(335);
            break;
    }
});
$('#DescriptionMeta').summernote({
    placeholder: 'Mời bạn soạn thảo',
    tabsize: 2,
    height: 500,              // set editor height
    minHeight: null,             // set minimum height of editor
    maxHeight: null,             // set maximum height of editor
    focus: true,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ]
});

$('#DescriptionMeta').on('summernote.change', function (we, contents, $editable) {
    var markupStr = $('#DescriptionMeta').summernote('code');
    $("input#DescriptionMeta").val(markupStr);
});

//
$('#Body').summernote({
    placeholder: 'Mời bạn soạn thảo',
    tabsize: 2,
    height: 500,              // set editor height
    minHeight: null,             // set minimum height of editor
    maxHeight: null,             // set maximum height of editor
    focus: true,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ]
});

$('#Body').on('summernote.change', function (we, contents, $editable) {
    var markupStr = $('#Body').summernote('code');
    $("input#Body").val(markupStr);
});

//

$('#Content').summernote({
    placeholder: 'Mời bạn soạn thảo',
    tabsize: 2,
    height: 500,              // set editor height
    minHeight: null,             // set minimum height of editor
    maxHeight: null,             // set maximum height of editor
    focus: true,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ]
});

$('#Content').on('summernote.change', function (we, contents, $editable) {
    var markupStr = $('#Content').summernote('code');
    $("input#Content").val(markupStr);
});


//

$('#GiftInfo').summernote({
    placeholder: 'Mời bạn soạn thảo',
    tabsize: 2,
    height: 500,              // set editor height
    minHeight: null,             // set minimum height of editor
    maxHeight: null,             // set maximum height of editor
    focus: true,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ]
});

$('#GiftInfo').on('summernote.change', function (we, contents, $editable) {
    var markupStr = $('#GiftInfo').summernote('code');
    $("input#GiftInfo").val(markupStr);
});
//

$('#ContactInfo').summernote({
    placeholder: 'Mời bạn soạn thảo',
    tabsize: 2,
    height: 500,              // set editor height
    minHeight: null,             // set minimum height of editor
    maxHeight: null,             // set maximum height of editor
    focus: true,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ]
});

$('#ContactInfo').on('summernote.change', function (we, contents, $editable) {
    var markupStr = $('#ContactInfo').summernote('code');
    $("input#ContactInfo").val(markupStr);
});
//

$('#FooterInfo').summernote({
    placeholder: 'Mời bạn soạn thảo',
    tabsize: 2,
    height: 500,              // set editor height
    minHeight: null,             // set minimum height of editor
    maxHeight: null,             // set maximum height of editor
    focus: true,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ]
});

$('#FooterInfo').on('summernote.change', function (we, contents, $editable) {
    var markupStr = $('#FooterInfo').summernote('code');
    $("input#FooterInfo").val(markupStr);
});

//

$('#SaleOffProgram').summernote({
    placeholder: 'Mời bạn soạn thảo',
    tabsize: 2,
    height: 500,              // set editor height
    minHeight: null,             // set minimum height of editor
    maxHeight: null,             // set maximum height of editor
    focus: true,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ]
});

$('#SaleOffProgram').on('summernote.change', function (we, contents, $editable) {
    var markupStr = $('#SaleOffProgram').summernote('code');
    $("input#SaleOffProgram").val(markupStr);
});

//
$(document).ready(function () {
    $('#Body').summernote('code', $("input#Body").val());
    $('#DescriptionMeta').summernote('code', $("input#DescriptionMeta").val());
    $('#Content').summernote('code', $("input#Content").val());
    $('#GiftInfo').summernote('code', $("input#GiftInfo").val());
    $('#ContactInfo').summernote('code', $("input#ContactInfo").val());
    $('#FooterInfo').summernote('code', $("input#FooterInfo").val());
    $('#SaleOffProgram').summernote('code', $("input#SaleOffProgram").val());
})


//xhttp.send(JSON.stringify({ "newPassword": "ReallySecurePassword999$$$" }));





