$(document).ready(function() {
    //$('head').append('<link rel="stylesheet" href="https://toolbar.mobo.vn/assets/new_toolbar/css/mobotoolbar_v2.min.css" type="text/css" />');
    //TOOLBAR.init();
});
var TOOLBAR = {KEY: null, init: function() {
        var scriptSrc = $('.toolbar_me').attr('src');
        TOOLBAR.KEY = TOOLBAR.GET_KEY('key', scriptSrc);
        TOOLBAR.GET_DATA()
    }, GET_KEY: function(n, s) {
        n = n.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var p = (new RegExp("[\\?&]" + n + "=([^&#]*)")).exec(s);
        return(p === null) ? "" : p[1];
    }, GET_DATA: function() {
        $.ajax({url: "https://toolbar.mobo.vn/index.php/index/tool?key=" + TOOLBAR.KEY, type: 'GET', cache: false, data: {}}).done(function(response) {
            $("body").prepend(response);
        });
    },
}