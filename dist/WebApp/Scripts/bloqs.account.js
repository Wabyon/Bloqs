/// <reference path="_references.js" />

$(function() {
    $(document).on("click", ".btn-maintain-key", function() {
        var $self = $(this);
        var $keytext = $($self.data("key-bind"));
        var url = $self.data("url");
        $.get(url, function (data) {
            $keytext.val(data);
        });
    });
});