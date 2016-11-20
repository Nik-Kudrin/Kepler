$("#modal_form").submit(function (e) {
    e.preventDefault();
}).validate({
    rules: {
        name: {
            required: true,
            minlength: 4
        },
        newName: {
            required: true,
            minlength: 4
        },
        image_worker_name: {
            required: true,
            minlength: 4,
            maxlength: 25,
        },
        diffImageSavingPath: {
            required: true,
            minlength: 4,
            maxlength: 40,
        },
        image_worker_url: {
            required: true,
            url: true,
        },
    },
    messages: {
        name: {
            required: "This field is required",
            minlength: "Minimum length of 4 symbol",
        },
        newName: {
            required: "This field is required",
            minlength: "Minimum length of 4 symbol",
        },
        image_worker_name: {
            required: "This field is required",
            minlength: "Minimum length of 4 symbol",
            maxlength: "Maximum length of 25 symbol",
        },
        image_worker_url: {
            required: "This field is required",
            url: "Please, enter valid URL: http://...",
        },
        diffImageSavingPath: {
            required: "This field is required",
            minlength: "Minimum length of 4 symbol",
            maxlength: "Maximum length of 40 symbol",
        },
    },
    submitHandler: function (form) {
        $('.modal_progress_bar_tint, .progress.progress_modal_dialog').show();
        modalAjaxCall();
        return false;  //This doesn't prevent the form from submitting.
    }
});