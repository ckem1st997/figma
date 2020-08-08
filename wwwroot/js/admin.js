$(document).on('click', '.section-image-list div.row .col-3 i.fa-trash', function (e) {

    if (confirm("Bạn muốn muốn xóa sản phẩm này !")) {
        //  console.log($(this).parent().children());
        //if ($("#deletefile").val().length == 0)
        //    $("#deletefile").val($(this).parent().children().attr('data-image'))
        //else
        //    $("#deletefile").val($("#deletefile").val() + "," + $(this).parent().children().attr('data-image'));

        AJAXSubmitDelete($(this).parent().children().attr('data-image'));
        // console.log($(this).parent().children().attr('data-image'));
        $(this).parent().remove();

    }
});
//lấy list url product
function addlist() {
    var str = "";
    $(".section-image-list div.row .col-3 a").each(function () {
        if (str.length == 0)
            str = $(this).attr('data-image');
        else
            str = str + "," + $(this).attr('data-image');
    });
    $("#Image").val(str);
}

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
        addlist();
    });
});

// delete_cart_table
function addTemp(a) {
    // console.log($("table#cart-view tbody tr.item_2:eq(" + a + ")").attr('id'));
    // lấy theo vị trí
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
    // Check if the file is an image.
    if (file.type.indexOf('image') === -1) {
        //  console.log('File is not an image.', file.type, file);
        //  alert("File của bạn không phải là hình ảnh !");
        return 1;
    }

    //const reader = new FileReader();
    //reader.addEventListener('load', (event) => {
    //    img.src = event.target.result;
    //});
    //reader.readAsDataURL(file);
    return 2;
}

$('#summernote').summernote({
    callbacks: {
        onImageUpload: function (files) {
            for (let i = 0; i < files.length; i++) {
                // console.log(files[i])
                AJAXSubmit(files[i]);
            }
        },
        onChange: function () {
            // console.log('onChange:', contents, $editable);
            var markupStr = $('#summernote').summernote('code');
            //  console.log(markupStr);
            $("#Body").val(markupStr);
        }
    },
    onInit: function () {
        console.log('Summernote is launched');
    }
    ,
    placeholder: 'Hello stand alone ui',
    tabsize: 2,
    height: 120,
    onImageUpload: function (files, editor, welEditable) {
        sendFile(files[0], editor, welEditable);
    },
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ], popover: {
        image: [
            ['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
            ['float', ['floatLeft', 'floatRight', 'floatNone']],
            ['remove', ['removeMedia']]
        ],
        link: [
            ['link', ['linkDialogShow', 'unlink']]
        ],
        table: [
            ['add', ['addRowDown', 'addRowUp', 'addColLeft', 'addColRight']],
            ['delete', ['deleteRow', 'deleteCol', 'deleteTable']],
        ],
        air: [
            ['color', ['color']],
            ['font', ['bold', 'underline', 'clear']],
            ['para', ['ul', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link', 'picture']]
        ]
    },
    disableDragAndDrop: true,
});
function AJAXSubmit(file) {

    formData = new FormData();
    formData.append("filesadd", file);
    $.ajax({
        type: 'POST',
        url: '/Products/createImage',
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

$('.note-editable').change(function () {
    var markupStr = $('#summernote').summernote('code');
    console.log(markupStr);
    $("#Body").val(markupStr);
})

$('#summernote1').summernote({
    callbacks: {
        onImageUpload: function (files) {
            for (let i = 0; i < files.length; i++) {
                // console.log(files[i])
                AJAXSubmit(files[i]);
            }
        },
        onChange: function () {
            // console.log('onChange:', contents, $editable);
            var markupStr = $('#summernote').summernote('code');
            //  console.log(markupStr);
            $("#Body").val(markupStr);
        }
    },
    onInit: function () {
        console.log('Summernote is launched');
    }
    ,
    placeholder: 'Hello stand alone ui',
    tabsize: 2,
    height: 120,
    onImageUpload: function (files, editor, welEditable) {
        sendFile(files[0], editor, welEditable);
    },
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ], popover: {
        image: [
            ['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
            ['float', ['floatLeft', 'floatRight', 'floatNone']],
            ['remove', ['removeMedia']]
        ],
        link: [
            ['link', ['linkDialogShow', 'unlink']]
        ],
        table: [
            ['add', ['addRowDown', 'addRowUp', 'addColLeft', 'addColRight']],
            ['delete', ['deleteRow', 'deleteCol', 'deleteTable']],
        ],
        air: [
            ['color', ['color']],
            ['font', ['bold', 'underline', 'clear']],
            ['para', ['ul', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link', 'picture']]
        ]
    },
    disableDragAndDrop: true,
});

$('#summernote2').summernote({
    callbacks: {
        onImageUpload: function (files) {
            for (let i = 0; i < files.length; i++) {
                // console.log(files[i])
                AJAXSubmit(files[i]);
            }
        },
        onChange: function () {
            // console.log('onChange:', contents, $editable);
            var markupStr = $('#summernote').summernote('code');
            //  console.log(markupStr);
            $("#Body").val(markupStr);
        }
    },
    onInit: function () {
        console.log('Summernote is launched');
    }
    ,
    placeholder: 'Hello stand alone ui',
    tabsize: 2,
    height: 120,
    onImageUpload: function (files, editor, welEditable) {
        sendFile(files[0], editor, welEditable);
    },
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', 'video']],
        ['view', ['fullscreen', 'codeview', 'help']]
    ], popover: {
        image: [
            ['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
            ['float', ['floatLeft', 'floatRight', 'floatNone']],
            ['remove', ['removeMedia']]
        ],
        link: [
            ['link', ['linkDialogShow', 'unlink']]
        ],
        table: [
            ['add', ['addRowDown', 'addRowUp', 'addColLeft', 'addColRight']],
            ['delete', ['deleteRow', 'deleteCol', 'deleteTable']],
        ],
        air: [
            ['color', ['color']],
            ['font', ['bold', 'underline', 'clear']],
            ['para', ['ul', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link', 'picture']]
        ]
    },
    disableDragAndDrop: true,
});

/// create file

async function AJAXSubmitCreate(oFormElement) {
    const formData = new FormData(oFormElement);
    $.ajax({
        type: 'POST',
        url: '/Products/createImage',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            console.log(data.imgNode);
            if (data.imgNode.length > 0) {
                var listimage = data.imgNode.split(",");
                if (listimage.length > 1 || listimage != null)
                    for (var i = 0; i < listimage.length; i++) {
                        //  console.log(listimage[i]);
                        $(".section-image-list div.row").append("<div class='col-3 mb-3 position-relative'><a data-image='" + listimage[i] + "' data-fancybox='gallery' class= '/" + listimage[i] + "' href = '/" + listimage[i] + "' ><img style='max-width:100%' src='/" + listimage[i] + "' /><span class='show--image-edit'>Xem ảnh</span><i class='far fa-eye show--image-edit i--one'></i></a ><i class='fas fa-trash'></i></div>");

                    }
                addlist();
            }
            else {
                alert("Xin vui lòng chọn ảnh !");
            }
        },
        error: function (data) {
            alert(data.responseText);
        }
    });

}

// delete file

async function AJAXSubmitDelete(oFormElement) {
    $.ajax({
        type: 'POST',
        url: '/Products/deleteImage',
        data: { filesadd: oFormElement },
        success: function (res) {
            //  console.log(res.result);
            addlist();
        },
        error: function (res) {
            alert(res.responseText);
        }
    });


}




