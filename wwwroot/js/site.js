// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(".product-gallery__thumb a").click(function (e) {
    e.preventDefault();
    $(".product-thumb").removeClass('checked');
    $(this).parent().addClass('checked');
    $(".product-image-feature").attr("src", $(this).attr("data-image"));

    $("div.vegas-slide-inner").attr("style", "background-image:url('" + $(this).attr('data-image') + "'");
    // alert("url('"+$(this).attr('data-image')+"'");

});
$(".info .row .col-4").hover(
    function () {
        $(this).toggleClass("animate__animated animate__bounce");
    }
);

//$(".navbar-nav li ul li").hover(
//    function () {
//        console.log($(this).children().text());
//        $(this).children().toggleClass("animate__animated animate__bounce");
//    }
//);
//$("img").hover(
//    function () {
//        $(this).toggleClass("animate__animated animate__pulse");
//    }
//);


$(document).ready(function () {
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
        if (pos_body > 10) {
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
    var a = $(".srcBanners").val();
    var listimage = a.split(",");
    console.log(listimage);
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
    $(".card-product img").hover(function (event) {
        //console.log($(this).parent().next("div.card-tool"));
        //$(this).parent().next("div.card-tool").css("opacity", "1");
        //$(this).parent().next("div.card-tool").find(".card-tool-btn").css("bottom", "35%");
        //var hide_btn = $(this).parent().children()[1];
        //$(this).parent().children()[2].css("opacity", "1");
        //$(this).parent().children()[2].css("bottom", "35%");
        //$(".card-tool").css("opacity", "1");
        //$(".card-tool .card-tool-btn").css("bottom", "35%");

    });
    //$(".card-product img").mouseout(function (event) {
    //    $(".card-tool").css("opacity", "0");
    //    $(".card-tool .card-tool-btn").css("bottom", "0%");

    //});
    $("#variant-swatch-1 .select-swap .swatch-element").click(function () {
        console.log("haha");
        if (!$(this).hasClass("soldout")) {
            $("#variant-swatch-1 .select-swap .swatch-element").removeClass("checked");
            $(this).addClass("checked");
        }

    });
    $("#variant-swatch-0 .select-swap .swatch-element").click(function () {
        console.log("haha");
        if (!$(this).hasClass("soldout")) {
            $(".select-swap .swatch-element").removeClass("checked");
            $(this).addClass("checked");
        }
        //var i = $(".id_check_idsp").html();
        //var l = $("#variant-swatch-0 .select-swap .swatch-element.checked").attr("data-value");
    });

    $("#add-to-cart").off("click").click(function () {
        if ($("#variant-swatch-0 .select-swap .swatch-element.checked").hasClass("checked") != true || $("#variant-swatch-1 .select-swap .swatch-element.checked").hasClass("checked") != true)
            alert("Xin bạn vui lòng chọn sản phẩm và size yêu thích nha !");
        else {
            //var id_SP1 = $("span.sku-number").html();
            //var name_SP1 = $("div.product-title h1").html();
            //var price_SP1 = $("div#price-preview .pro-price").html().replace(',', '').replace('₫', '');
            //var sl1 = $("#quantity").val();
            //var image_sp1 = $("#variant-swatch-0 .select-swap .swatch-element.checked .lbSwatch").css("background-image").replace('url("', '').replace('")', '');
            //var size1 = $("#variant-swatch-1 .select-swap .swatch-element.checked").attr("data-value");
            //var loai_SP1 = $("#variant-swatch-0 .select-swap .swatch-element.checked").attr("data-value");
            //Test_Cart(id_SP1, name_SP1, price_SP1, sl1, image_sp1, size1, loai_SP1);
            //$('#site-overlay').addClass("active");
            //$('#site-nav--mobile').addClass("active");
        }
    });

});

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







