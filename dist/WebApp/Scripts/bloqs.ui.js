/// <reference path="_references.js" />

$(function() {
    $(document).on("click", ".btn-retrieve-list", function () {
        var $self = $(this);
        var url = $self.data("url");
        $.get(url, function (data) {
            $self.closest(".pagination-list-area").first().html(data);
        });
    });

    $(document).on("click", ".btn-modal-view", function () {
        var $self = $(this);
        var url = $self.data("url");
        var $modal = $self.closest(".modal-edit-area").find(".modal").first();
        var $container = $modal.find(".modal-content").first();
        $.get(url, function (data) {
            $container.html(data);
            $modal.modal("show");
            $container.find(".modal-body input:text:visible:not([readonly]):first").first().focus();
        });
    });

    $(document).on("submit", ".modal-content form", function (e) {
		e.preventDefault();
		var $form = $(this);
		var $button = $form.find('input[type=submit]');
		var $modal = $form.closest(".pagination-list-area").find(".modal").first();
		var $container = $modal.find(".modal-content").first();
		var $refresh = $form.closest(".pagination-list-area.modal-edit-area").first().find(".btn-refresh-list").first();

        $.ajax({
			url: $form.attr('action'),
			type: $form.attr('method'),
            data: $form.serialize(),
			beforeSend: function (xhr, settings) {
				$button.attr('disabled', true);
			},
			complete: function (xhr, textStatus) {
				$button.attr('disabled', false);
			},
			success: function (result, textStatus, xhr) {
				if (result) {
				    $container.html(result);
				} else {
				    $modal.modal('hide');
				    if ($refresh !== null && $refresh !== undefined) {
				        $refresh.click();
				    }
				}
			},
			error: function (xhr, textStatus, error) {
			    displayError(error);
			}
		});
	});

    $(document).on("click", "button.btn-metadata", function () {
        var $self = $(this);
        var no = $self.data("no");
        if (no === null || no === undefined) {
            $self.parent("div").remove();
        } else {
            var html = '<div class="form-inline"> ' +
                '<input type="text" name="Metadata[' +
                no +
                '].Key" value="" class="form-control" required /> ' +
                '<input type="text" name="Metadata[' +
                no +
                '].Value" value="" class="form-control" required /> ' +
                '<button type="button" class="btn btn-default btn-metadata"><span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span></button>' +
                '</div>';

            $("#metadata-area").append(html);
            no++;
            $self.data("no", no);
        }
    });

    var displayError = function(errorMessage) {
        $('#error-modal-body').html(errorMessage);
        $('#error-modal').modal();
    }
});