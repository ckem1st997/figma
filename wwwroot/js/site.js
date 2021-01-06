
$(".product-gallery__thumb a").click(function (e) {
    e.preventDefault();
    $(".product-thumb").removeClass('checked');
    $(this).parent().addClass('checked');
    $(".product-image-feature").attr("src", $(this).attr("data-image"));
    $("div.vegas-slide-inner").attr("style", "background-image:url('" + $(this).attr('data-image') + "'");

});
$(".info .row .col-4").hover(
    function () {
        $(this).toggleClass("animate__animated animate__bounce");
    }
);

document.addEventListener(
    "DOMContentLoaded", () => {
        const menu = new MmenuLight(
            document.querySelector("#nav"),
            "(max-width: 992px)"
        );
        const navigator = menu.navigation({

        });
        const drawer = menu.offcanvas();

        document.querySelector("button[href='#nav']")
            .addEventListener("click", (evnt) => {
                evnt.preventDefault();
                drawer.open();
            });
    }
);
$(document).ready(function () {
    $(".datepicker").datepicker();
    $("#exampleModal5").modal('show');
    if (!$("div.carousel-inner div").hasClass("carousel-item")) {
        $("#carouselExampleIndicators").hide();
    }

    $(window).scroll(function (event) {
        var pos_body = $('html,body').scrollTop();
        // console.log(pos_body);
        //if (pos_body > 0) {
        //    $('.banner img').addClass('animate__animated animate__fadeInLeft');
        //    $('.banner img').addClass('animate__backInRight');
        //}
        if (pos_body > 500) {
            $(".menu-scroll").addClass("co-dinh-menu");
            $("#s1").addClass("h_img");
            $(".menu_Ao_list").addClass("menu1");
        }
        else {
            $(".menu-scroll").removeClass("co-dinh-menu");
            $("#s1").removeClass("h_img");
            $(".menu_Ao_list").removeClass("menu1");
        }
    });
    $('.back-to-top').click(function (event) {
        $('html,body').animate({ scrollTop: 0 }, 1400);
    });
    $(".qty-btn.plus").click(function (event) {
        // console.log($(this).parent().find("input#quantity"));
        var value = parseInt($(this).parent().find("input#quantity").val());
        if ($(this).parent().find("input#quantity").val() > 0) {
            $(this).parent().find("input#quantity").val(value + 1);
        }
        else {
            $(this).next("#quantity").val(1);
        }
    });
    $(".qty-btn.minus").click(function (event) {

        //  console.log($(this).parent().find("input#quantity"));
        var value = parseInt($(this).parent().find("input#quantity").val());
        if ($(this).parent().find("input#quantity").val() > 1) {
            $(this).parent().find("input#quantity").val(value - 1);
        }
        else {
            $(this).next("#quantity").val(1);
        }
    });

    $("#example,.home-index div.banner").vegas({
        delay: 9000,
        slides: [

            $("input.srcBanners:nth-child(2)").val() != null ? { src: "" + $('input.srcBanners:nth-child(2)').val() + "" } : { src: "//product.hstatic.net/1000253775/product/857939080e9ef5c0ac8f_f7991ab4a5504597b32aeadf9062e146_master.jpg" },
            $("input.srcBanners:nth-child(3)").val() != null ? { src: "" + $('input.srcBanners:nth-child(3)').val() + "" } : { src: "//product.hstatic.net/1000253775/product/2eb18cb3bb25407b1934_92d99503a1164e5ba27b74daaa5d2b82_master.jpg" },
            $("input.srcBanners:nth-child(4)").val() != null ? { src: "" + $('input.srcBanners:nth-child(4)').val() + "" } : { src: "//product.hstatic.net/1000253775/product/857939080e9ef5c0ac8f_f7991ab4a5504597b32aeadf9062e146_master.jpg" },

            //{ src: "//product.hstatic.net/1000253775/product/857939080e9ef5c0ac8f_f7991ab4a5504597b32aeadf9062e146_master.jpg" },
            //{ src: "//product.hstatic.net/1000253775/product/2eb18cb3bb25407b1934_92d99503a1164e5ba27b74daaa5d2b82_master.jpg" },

        ],
        animation: 'kenburnsUpLeft',
        transition: 'fade'

    });
    $("#variant-swatch-0 .select-swap .swatch-element").click(function () {
        if (!$(this).hasClass("soldout")) {
            $(".select-swap .swatch-element").removeClass("checked");
            $(this).addClass("checked");
            $("#variant-swatch-1 .select-swap .swatch-element:nth-child(1)").addClass("checked");
        }
    });
    $("#variant-swatch-1 .select-swap .swatch-element").click(function () {
        if (!$(this).hasClass("soldout")) {
            $("#variant-swatch-1 .select-swap .swatch-element").removeClass("checked");
            $(this).addClass("checked");
        }

    });


    $("#add-to-cart").off("click").click(function () {
        if ($("#variant-swatch-0 .select-swap .swatch-element").length || $("#variant-swatch-1 .select-swap .swatch-element").length > 0) {
            if ($("#variant-swatch-0 .select-swap .swatch-element.checked").hasClass("checked") != true && $("#variant-swatch-1 .select-swap .swatch-element.checked").hasClass("checked") != true)
                alert("Xin bạn vui lòng chọn sản phẩm và size yêu thích nha !");
            else {
                var id_SP = $(".id_check_idsp").val();
                var Price_SP = $(".giaban").html();
                var Count_SP = $("#quantity").val();
                var Color_SP = $("#variant-swatch-0 .select-swap .swatch-element.checked ").attr("data-value");
                var Size_SP = $("#variant-swatch-1 .select-swap .swatch-element.checked").attr("data-value");
                AddToCartAjax(Count_SP, id_SP, Color_SP, Size_SP);


            }
        }
        else {
            var id_SP = $(".id_check_idsp").val();
            console.log(id_SP);
            var Price_SP = $(".giaban").html();
            console.log(Price_SP.slice(0, Price_SP.length - 1));
            var Count_SP = $("#quantity").val();
            console.log(Count_SP);
            var Color_SP = $("#variant-swatch-0 .select-swap .swatch-element.checked ").attr("data-value");
            if (Color_SP == undefined)
                Color_SP = ""
            console.log(Color_SP);
            var Size_SP = $("#variant-swatch-1 .select-swap .swatch-element.checked").attr("data-value");
            if (Size_SP == undefined)
                Size_SP = ""
            console.log(Size_SP);
            AddToCartAjax(Count_SP, id_SP, Color_SP, Size_SP);
        }

    });

    $("button#BtnUpdate").click(function () {
        if (confirm("Bạn có muốn cập nhật sản phẩm này ?"))
            UpdateToCart($(this).attr("data-id"), $(this).parent().parent().find(".count-text-class").find("input#quantity").val())
    });
    //
    $("button#BtnDelete").click(function () {
        if (confirm("Bạn có muốn xóa sản phẩm này ?"))
            RemoveFromCart($(this).attr("data-id"))
    });
    $("input#countcart").click(function () {
        UpdateToCartAjax($(this).attr("data-id"), $(this).parent().find("input#quantity").val())
    });
    //
    $("#BtnEmptyCart").hide();
    //
    $("input#CbPay").click(function () {
        if (!$(this).hasClass("checked")) {
            $("input.cb-pay").attr('checked', true);
            $("#BtnEmptyCart").show("slow");
        }
        else {
            $("input.cb-pay").attr('checked', false);
            $("#BtnEmptyCart").hide();
        }
        $(this).toggleClass("checked");
        $("input.cb-pay").toggleClass("tbchecked");
        GetAllCheckBox();
    });
    //
    $("#addpro").click(function () {
        addp();
    });
    //
    $("#BtnEmptyCart").click(function () {
        EmptyCart($("#BtnEmptyCart").attr("data-id"))
    });
    //
    var countChecked = function () {
        $(this).toggleClass("tbchecked");
        var n = $("input.cb-pay.mr-2:checked").length;
        if (n == 0)
            $("#BtnEmptyCart").hide();
        else if (n > 0)
            $("#BtnEmptyCart").show("slow");
        GetAllCheckBox();
    };
    countChecked();
    $("input.cb-pay.mr-2[type=checkbox]").on("click", countChecked);
});
//
function GetAllCheckBox() {
    var listcb = "";
    $("input.cb-pay.mr-2[type=checkbox]").each(function (index) {
        if ($(this).hasClass("tbchecked")) {
            //  console.log(index + ": " + $(this).attr("data-id"));
            listcb = listcb + "," + $(this).attr("data-id");
        }
    });
    $("#BtnEmptyCart").attr("data-id", listcb);
}

//
function AddToCart(quantity, productId, color, size) {
    if (confirm("Bạn có muốn thêm sản phẩm vào giỏ hàng ?")) {
        $.post("/ShoppingCart/AddToCartAjax", { quantity: quantity, productId: productId, color: color, size: size }, function (data) {
            if (data.result == 1) {
                alert("Thêm thành công !")
            } else {
                alert("Quá trình thực hiện không thành công. Hãy thử lại");
            }
        });
    }
}
//
function addp() {
    var params = {
        type: 'GET',
        url: '/Home/add',
        data: {},
        dataType: "json",
        success: function (res) {
            console.log(res);
            if (res.result == 1)
                alert("Thành công !");
            else
                alert("Thất bại !");
        },

        error: function (errormessage) {
            console.log(errormessage)
        }


    };
    jQuery.ajax(params);
}
//
function EmptyCart(id) {
    var params = {
        type: 'POST',
        url: '/ShoppingCart/EmptyCart',
        data: { ListRecordID: id },
        dataType: "json",
        success: function (res) {
            console.log(res);
            if (res.result == 1)
                if (confirm("Xóa thành công"))
                    window.location.assign('/gio-hang/thong-tin');
        },

        error: function (errormessage) {
            console.log(errormessage)
        }


    };
    jQuery.ajax(params);
}

//
function AddToCartAjax(sl, id, cl, se) {
    var params = {
        type: 'POST',
        url: '/ShoppingCart/AddToCartAjax',
        data: { quantity: sl, productId: id, color: cl, size: se },
        dataType: "json",
        success: function (res) {
            //  console.log(res);
            if (res.data.result == 1)
                alert("Thêm sản phẩm thành công !");
            else
                alert("Thêm sản phẩm thất bại, xin bạn vui lòng thử lại nha !");
        },

        error: function (errormessage) {
            console.log(errormessage)
        }


    };
    jQuery.ajax(params);
}

//
function UpdateToCartAjax(id, sl) {
    var params = {
        type: 'POST',
        url: '/ShoppingCart/UpdateCart',
        data: { RecordID: id, quantity: sl },
        dataType: "json",
        success: function (res) {
            // console.log(res);
            if (res.result != 1)
                alert("Chỉnh sửa thất bại, xin bạn vui lòng thử lại nha !");
        },

        error: function (errormessage) {
            console.log(errormessage)
        }


    };
    jQuery.ajax(params);
}

//RemoveFromCart

function RemoveFromCart(id) {
    var params = {
        type: 'POST',
        url: '/ShoppingCart/RemoveFromCart',
        data: { RecordID: id },
        dataType: "json",
        success: function (res) {
            console.log(res);
            if (res.result != 1)
                alert("Xóa thất bại, xin bạn vui lòng thử lại nha !");
            else {
                if (confirm("Xóa thành công"))
                    window.location.assign('/gio-hang/thong-tin')
            }

        },

        error: function (errormessage) {
            console.log(errormessage)
        }


    };
    jQuery.ajax(params);
}


//
function UpdateToCart(id, sl) {
    var params = {
        type: 'POST',
        url: '/ShoppingCart/UpdateCart',
        data: { RecordID: id, quantity: sl },
        dataType: "json",
        success: function (res) {
            console.log(res);
            if (res.result != 1)
                alert("Chỉnh sửa thất bại, xin bạn vui lòng thử lại nha !");
            else {
                if (confirm("Cập nhật thành công"))
                    window.location.assign('/gio-hang/thong-tin')
            }

        },

        error: function (errormessage) {
            console.log(errormessage)
        }


    };
    jQuery.ajax(params);
}



$(document).ready(function () {
    //$(".regular").slick({
    //    dots: true,
    //    infinite: true,
    //    slidesToShow: 3,
    //    slidesToScroll: 3
    //});
    $('.slider-for').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        asNavFor: '.slider-nav'
    });
    $('.slider-nav').slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        asNavFor: '.slider-for',
        dots: false,
        //centerMode: true,
        focusOnSelect: true,
        adaptiveHeight: true
    });
});
new WOW().init();





