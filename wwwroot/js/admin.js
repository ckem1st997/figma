$(document).on('click', '.section-image-list div.row .col-3 i.fa-trash', function (e) {

    if (confirm("Bạn muốn muốn xóa sản phẩm này !")) {
        //  console.log($(this).parent().children());
        if ($("#deletefile").val().length == 0)
            $("#deletefile").val($(this).parent().children().attr('data-image'))
        else
            $("#deletefile").val($("#deletefile").val() + "," + $(this).parent().children().attr('data-image'));
        $(this).parent().remove();
        //   console.log(9);

    }
    addlist();

});
//lấy list url product
function addlist() {
    var str = "";
    $(".section-image-list div.row .col-3 a").each(function () {
        //  console.log(1);
        // console.log($(this).attr('data-image'));
        if (str.length == 0)
            str = $(this).attr('data-image');
        else
            str = str + "," + $(this).attr('data-image');
    });
    //   console.log(str);
    $("#Image").val(str);
    //  confirm("Handler for .click() called.");
}

$(document).ready(function () {
    //$('input[type="file"]').change(function (e) {
    //    alert("haha");
    //    console.log(e.target.files);
    //    check_Id(e.target.files);



    //    if (e.target.files.length > 0)
    //        for (var i = 0; i < e.target.files.length; i++) {
    //            var fileName = e.target.files[i].name;
    //            if (!$(".preview p").hasClass(""+fileName+""))
    //                $(".preview").append("<p class=" + fileName + ">" + fileName + "' <p/>")

    //            //  alert('The file "' + fileName + '" has been selected.');

    //        }

    //});

    //$('.section-image-list div.row .col-3 i.fa-trash').on('click', function (e) {
    //    if (confirm("Bạn muốn muốn xóa sản phẩm này !")) {
    //        console.log($(this).parent().children());
    //        if ($("#deletefile").val().length == 0)
    //            $("#deletefile").val($(this).parent().children().attr('data-image'))
    //        else
    //            $("#deletefile").val($("#deletefile").val() + "," + $(this).parent().children().attr('data-image'));
    //        $(this).parent().remove();

    //    }
    //});

    // add img show từ file upload
    $('input[type="file"]#filesadd').change(function (e) {
        setTimeout(function () {

            var d = new Date();
            var date = "/" + d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate() + "";
            var date1 = "" + d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate() + "";

            ////   console.log(date);
            //var str = $("#Image").val().split(",");
            //for (var i = 0; i < str.length; i++) {
            //    //  alert(str[i]);
            //}

            if (e.target.files.length > 0)
                for (var i = 0; i < e.target.files.length; i++) {
                    //   console.log(e.target.files.result)
                    if (readImage(e.target.files[i]) == 1)
                        return;
                    // console.log(e.target.files[i].type.indexOf('image'));
                    var file = e.target.files[i];
                    var img = document.createElement("img");
                    var resultImg = "";
                    var reader = new FileReader();
                    reader.onloadend = function () {
                        img.src = reader.result;
                        resultImg = reader.result;
                    }
                    //     console.log(resultImg);
                    //console.log($("#Image").val().indexOf(file.name));
                    reader.readAsDataURL(file);
                    if ($("#Image").val().indexOf(file.name) == (-1))
                        //  if (!$(".section-image-list div.row col-3:(a)").hasClass("'" + date + "/" + file.name + "'"))
                        $(".section-image-list div.row").append("<div class='col-3 mb-3 position-relative'><a data-image='" + date1 + "/" + file.name + "' data-fancybox='gallery' class= '" + date + "/" + file.name + "' href = '" + date + "/" + file.name + "' ><img style='max-width:100%' src='" + date + "/" + file.name + "' /><span class='show--image-edit'>Xem ảnh</span><i class='far fa-eye show--image-edit i--one'></i></a ><i class='fas fa-trash'></i></div>");
                    else
                        alert("Ảnh đã tồn tại !");

                }
            addlist();
        }, 1000);
    });
});

//async function add_Image(i, n) {
//    var params = {
//        type: 'POST',
//        url: '/Products/AddTempImage',
//        data: { files: i, filename: n },
//        dataType: 'json',
//        success: function (response) {
//            console.log(response);
//        }

//    };
//    jQuery.ajax(params);
//}




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


//tinymce.activeEditor.uploadImages(function (success) {
//    $.post('ajax/post.php', tinymce.activeEditor.getContent()).done(function () {
//        console.log("Uploaded images and posted content as an ajax request.");
//    });
//});
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

$("#test").click(function () {


});
