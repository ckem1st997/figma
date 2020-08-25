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
async function AJAXSubmitCreate(oFormElement) {
    const formData = new FormData(oFormElement);
    $.ajax({
        type: 'POST',
        url: '/Csm/createImage',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            //    console.log(data.imgNode);
            if (data.imgNode.length > 0) {
                var listimage = data.imgNode.split(",");
                if (listimage.length > 1 || listimage != null)
                    for (var i = 0; i < listimage.length; i++) {
                        $(".section-image-list.insert-one div.row").append("<div class='col-3 mb-3 position-relative'><a data-image='" + listimage[i] + "' data-fancybox='gallery' class= '/" + listimage[i] + "' href = '/" + listimage[i] + "' ><img style='max-width:100%' src='/" + listimage[i] + "' /><span class='show--image-edit'>Xem ảnh</span><i class='far fa-eye show--image-edit i--one'></i></a ><i class='fas fa-trash'></i></div>");
                    }
            }
            else {
                alert("Xin vui lòng chọn ảnh !");
            }
            addlistone();

        },
        error: function (data) {
            alert(data.responseText);
        }
    });

}
//

async function AJAXSubmitCreateOne(oFormElement) {
    const formData = new FormData(oFormElement);

    $.ajax({
        type: 'POST',
        url: '/Csm/createImage',
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
async function AJAXSubmitCreateOneT(oFormElement) {
    const formData = new FormData(oFormElement);
    $.ajax({
        type: 'POST',
        url: '/Csm/createImage',
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
async function AJAXSubmitDelete(oFormElement) {
    $.ajax({
        type: 'POST',
        url: '/Csm/deleteImage',
        data: { filesadd: oFormElement },
        success: function (res) {
        },
        error: function (res) {
            alert(res.responseText);
        }
    });


}

////

DecoupledDocumentEditor
    .create(document.querySelector('#editor'), {

        toolbar: {
            items: [
                'heading',
                '|',
                'fontSize',
                'fontFamily',
                '|',
                'bold',
                'italic',
                'underline',
                'strikethrough',
                'highlight',
                '|',
                'alignment',
                '|',
                'numberedList',
                'bulletedList',
                '|',
                'indent',
                'outdent',
                '|',
                'todoList',
                'link',
                'blockQuote',
                'insertTable',
                'mediaEmbed',
                '|',
                'undo',
                'redo',
                'CKFinder',
                'code',
                'fontColor',
                'fontBackgroundColor',
                'exportPdf',
                'imageUpload'
            ]
        },
        language: 'vi',
        image: {
            toolbar: [
                'imageTextAlternative',
                'imageStyle:full',
                'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn',
                'tableRow',
                'mergeTableCells'
            ]
        },
        licenseKey: '',

    })
    .then(editor => {
        window.editor = editor;
        // Set a custom container for the toolbar.
        //  document.querySelector('.#toolbar-container').appendChild(editor.ui.view.toolbar.element);
        const toolbarContainer = document.querySelector('#toolbar-container');
        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        // document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
        editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });

        editor.model.document.on('change:data', () => {
            $("#Body").val(editor.getData());
        });
        if (h != null)
            editor.setData(h);
    })
    .catch(error => {

        //  console.error(error);
    });
//
DecoupledDocumentEditor
    .create(document.querySelector('#editor'), {

        toolbar: {
            items: [
                'heading',
                '|',
                'fontSize',
                'fontFamily',
                '|',
                'bold',
                'italic',
                'underline',
                'strikethrough',
                'highlight',
                '|',
                'alignment',
                '|',
                'numberedList',
                'bulletedList',
                '|',
                'indent',
                'outdent',
                '|',
                'todoList',
                'link',
                'blockQuote',
                'insertTable',
                'mediaEmbed',
                '|',
                'undo',
                'redo',
                'CKFinder',
                'code',
                'fontColor',
                'fontBackgroundColor',
                'exportPdf',
                'imageUpload'
            ]
        },
        language: 'vi',
        image: {
            toolbar: [
                'imageTextAlternative',
                'imageStyle:full',
                'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn',
                'tableRow',
                'mergeTableCells'
            ]
        },
        licenseKey: '',

    })
    .then(editor => {
        window.editor = editor;
        // Set a custom container for the toolbar.
        //  document.querySelector('.#toolbar-container').appendChild(editor.ui.view.toolbar.element);
        const toolbarContainer = document.querySelector('#toolbar-container');
        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        // document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
        editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });

        editor.model.document.on('change:data', () => {
            $("#Body").val(editor.getData());
        });
        if (h != null)
            editor.setData(h);
    })
    .catch(error => {
        //  console.error(error);
    });
//
DecoupledDocumentEditor
    .create(document.querySelector('#editorContent'), {

        toolbar: {
            items: [
                'heading',
                '|',
                'fontSize',
                'fontFamily',
                '|',
                'bold',
                'italic',
                'underline',
                'strikethrough',
                'highlight',
                '|',
                'alignment',
                '|',
                'numberedList',
                'bulletedList',
                '|',
                'indent',
                'outdent',
                '|',
                'todoList',
                'link',
                'blockQuote',
                'insertTable',
                'mediaEmbed',
                '|',
                'undo',
                'redo',
                'CKFinder',
                'code',
                'fontColor',
                'fontBackgroundColor',
                'exportPdf',
                'imageUpload'
            ]
        },
        language: 'vi',
        image: {
            toolbar: [
                'imageTextAlternative',
                'imageStyle:full',
                'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn',
                'tableRow',
                'mergeTableCells'
            ]
        },
        licenseKey: '',

    })
    .then(editor => {
        window.editor = editor;
        // Set a custom container for the toolbar.
        //  document.querySelector('.#toolbar-container').appendChild(editor.ui.view.toolbar.element);
        const toolbarContainer = document.querySelector('#toolbar-containerContent');
        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        // document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
        editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });

        editor.model.document.on('change:data', () => {
            $("#Content").val(editor.getData());
        });
        if (j != null)
            editor.setData(j);
    })
    .catch(error => {
        // console.error(error);
    });

//
DecoupledDocumentEditor
    .create(document.querySelector('#editorGift'), {

        toolbar: {
            items: [
                'heading',
                '|',
                'fontSize',
                'fontFamily',
                '|',
                'bold',
                'italic',
                'underline',
                'strikethrough',
                'highlight',
                '|',
                'alignment',
                '|',
                'numberedList',
                'bulletedList',
                '|',
                'indent',
                'outdent',
                '|',
                'todoList',
                'link',
                'blockQuote',
                'insertTable',
                'mediaEmbed',
                '|',
                'undo',
                'redo',
                'CKFinder',
                'code',
                'fontColor',
                'fontBackgroundColor',
                'exportPdf',
                'imageUpload'
            ]
        },
        language: 'vi',
        image: {
            toolbar: [
                'imageTextAlternative',
                'imageStyle:full',
                'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn',
                'tableRow',
                'mergeTableCells'
            ]
        },
        licenseKey: '',

    })
    .then(editor => {
        window.editor = editor;
        // Set a custom container for the toolbar.
        //  document.querySelector('.#toolbar-container').appendChild(editor.ui.view.toolbar.element);
        const toolbarContainer = document.querySelector('#toolbar-containerGift');
        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
        editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });

        editor.model.document.on('change:data', () => {
            $("#GiftInfo").val(editor.getData());
        });
        if (t != null)
            editor.setData(t);
    })
    .catch(error => {
        //   console.error(error);
    });

//

DecoupledDocumentEditor.create(document.querySelector('#editorDescriptionMeta'), {

    toolbar: {
        items: [
            'heading',
            '|',
            'fontSize',
            'fontFamily',
            '|',
            'bold',
            'italic',
            'underline',
            'strikethrough',
            'highlight',
            '|',
            'alignment',
            '|',
            'numberedList',
            'bulletedList',
            '|',
            'indent',
            'outdent',
            '|',
            'todoList',
            'link',
            'blockQuote',
            'insertTable',
            'mediaEmbed',
            '|',
            'undo',
            'redo',
            'CKFinder',
            'code',
            'fontColor',
            'fontBackgroundColor',
            'exportPdf',
            'imageUpload'
        ]
    },
    language: 'vi',
    image: {
        toolbar: [
            'imageTextAlternative',
            'imageStyle:full',
            'imageStyle:side'
        ]
    },
    table: {
        contentToolbar: [
            'tableColumn',
            'tableRow',
            'mergeTableCells'
        ]
    },
    licenseKey: '',

}).then(editor => {
    window.editor = editor;
    // Set a custom container for the toolbar.
    //  document.querySelector('.#toolbar-container').appendChild(editor.ui.view.toolbar.element);
    const toolbarContainer = document.querySelector('#toolbar-containerDescriptionMeta');
    toolbarContainer.appendChild(editor.ui.view.toolbar.element);
    // document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
    editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });

    editor.model.document.on('change:data', () => {
        $("#DescriptionMeta").val(editor.getData());
    });
    if (d != null)
        editor.setData(d);
}).catch(error => {

    //  console.error(error);
});

//
DecoupledDocumentEditor
    .create(document.querySelector('#editor2'), {

        toolbar: {
            items: [
                'heading',
                '|',
                'fontSize',
                'fontFamily',
                '|',
                'bold',
                'italic',
                'underline',
                'strikethrough',
                'highlight',
                '|',
                'alignment',
                '|',
                'numberedList',
                'bulletedList',
                '|',
                'indent',
                'outdent',
                '|',
                'todoList',
                'link',
                'blockQuote',
                'insertTable',
                'mediaEmbed',
                '|',
                'undo',
                'redo',
                'CKFinder',
                'code',
                'fontColor',
                'fontBackgroundColor',
                'exportPdf',
                'imageUpload'
            ]
        },
        language: 'vi',
        image: {
            toolbar: [
                'imageTextAlternative',
                'imageStyle:full',
                'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn',
                'tableRow',
                'mergeTableCells'
            ]
        },
        licenseKey: '',


    })
    .then(editor => {
        window.editor = editor;
        // Set a custom container for the toolbar.
        //  document.querySelector('.#toolbar-container').appendChild(editor.ui.view.toolbar.element);
        const toolbarContainer = document.querySelector('#toolbar-container2');
        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        // document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
        editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });
        editor.model.document.on('change:data', () => {
            $("#ContactInfo").val(editor.getData());
        });
        if (c != null)
            editor.setData(h);
    })
    .catch(error => {

        //   console.error(error);
    });
//
DecoupledDocumentEditor
    .create(document.querySelector('#editor3'), {

        toolbar: {
            items: [
                'heading',
                '|',
                'fontSize',
                'fontFamily',
                '|',
                'bold',
                'italic',
                'underline',
                'strikethrough',
                'highlight',
                '|',
                'alignment',
                '|',
                'numberedList',
                'bulletedList',
                '|',
                'indent',
                'outdent',
                '|',
                'todoList',
                'link',
                'blockQuote',
                'insertTable',
                'mediaEmbed',
                '|',
                'undo',
                'redo',
                'CKFinder',
                'code',
                'fontColor',
                'fontBackgroundColor',
                'exportPdf',
                'imageUpload'
            ]
        },
        language: 'vi',
        image: {
            toolbar: [
                'imageTextAlternative',
                'imageStyle:full',
                'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn',
                'tableRow',
                'mergeTableCells'
            ]
        },
        licenseKey: '',


    })
    .then(editor => {
        window.editor = editor;
        // Set a custom container for the toolbar.
        //  document.querySelector('.#toolbar-container').appendChild(editor.ui.view.toolbar.element);
        const toolbarContainer = document.querySelector('#toolbar-container3');
        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        // document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
        editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });
        editor.model.document.on('change:data', () => {
            $("#FooterInfo").val(editor.getData());
        });
        if (f != null)
            editor.setData(h);
    })
    .catch(error => {

        //  console.error(error);
    });

//
DecoupledDocumentEditor
    .create(document.querySelector('#editor4'), {

        toolbar: {
            items: [
                'heading',
                '|',
                'fontSize',
                'fontFamily',
                '|',
                'bold',
                'italic',
                'underline',
                'strikethrough',
                'highlight',
                '|',
                'alignment',
                '|',
                'numberedList',
                'bulletedList',
                '|',
                'indent',
                'outdent',
                '|',
                'todoList',
                'link',
                'blockQuote',
                'insertTable',
                'mediaEmbed',
                '|',
                'undo',
                'redo',
                'CKFinder',
                'code',
                'fontColor',
                'fontBackgroundColor',
                'exportPdf',
                'imageUpload'
            ]
        },
        language: 'vi',
        image: {
            toolbar: [
                'imageTextAlternative',
                'imageStyle:full',
                'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn',
                'tableRow',
                'mergeTableCells'
            ]
        },
        licenseKey: '',


    })
    .then(editor => {
        window.editor = editor;
        // Set a custom container for the toolbar.
        //  document.querySelector('.#toolbar-container').appendChild(editor.ui.view.toolbar.element);
        const toolbarContainer = document.querySelector('#toolbar-container4');
        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        // document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
        editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });
        editor.model.document.on('change:data', () => {
            $("#SaleOffProgram").val(editor.getData());
        });
        if (s != null)
            editor.setData(h);
    })
    .catch(error => {

        // console.error(error);
    });





//DecoupledEditor
//    .create(document.querySelector('#editor'), {
//        image: {

//        },
//        toolbar: { items: ['imageResize','tableColumn', 'tableRow', 'mergeTableCells', 'fontSize', 'fontColor', 'imageStyle:alignRight', '|', 'imageStyle:alignLeft', 'imageStyle:full', '|', 'imageTextAlternative', 'fontFamily', 'ckfinder', 'imageUpload', 'heading', '|', 'bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote', 'underline', 'strikethrough', '|', 'outdent', 'indent', '|', 'bulletedList', 'numberedList', '|', 'undo', 'redo'] },
//    })
//    .then(editor => {
//        const toolbarContainer = document.querySelector('#toolbar-container');
//        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
//        //    editor.keystrokes.set('Ctrl+E', 'bold')
//        // editor.config.get('imageStyle.toolbar');
//        // editor.execute('bold');
//        editor.editing.view.change(writer => { writer.setStyle('height', '300px', editor.editing.view.document.getRoot()); });

//        editor.model.document.on('change:data', () => {
//            $("#Body").val(editor.getData());
//        });

//        if (h != null)
//            editor.setData(h);
//    })
//    .catch(error => {
//        console.error(error);
//    });


//function autochange() {
//    var data = $("input#ProductID").val();
//    $.ajax({
//        type: 'POST',
//        url: '/Csm/AutoCompleteCity',
//        data: { Prefix: data },
//        dataType: "json",
//        success: function (data) {
//            console.log(data);
//        },
//        error: function (data) {
//            console.log(data.responseText);
//        }
//    });
//}






