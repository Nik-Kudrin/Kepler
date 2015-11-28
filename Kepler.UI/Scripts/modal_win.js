function loadModalWin(modalWinObj, modalWinAction) {
    switch (modalWinAction) {
        case 'create_proj':
            $('.modal-title').text('Create project');
            $('.modal-body').empty().append('<div class="form-group"><label>Name</label><input class="form-control" name="name" type="text"></div>');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-success">Save</button>');
            $('#modal_form').attr({ 'action': KeplerServiceUrl + 'CreateProject?name=', 'method': 'get' });
            break;
        case 'edit_proj':
            $('.modal-title').text('Edit project');
            $('.modal-body').empty().append('<div class="form-group"><label>Name</label><input class="form-control" type="text" name="proj_name" value=""></div>');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-success">Save</button>');
            break;
        case 'delete_proj':
            $('.modal-title').text('Delete project');
            $('.modal-body').empty().append('Are you sure want to delete ' + modalWinObj + '?');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-danger">Yes</button>');
            break;
        case 'create_branch':
            $('.modal-title').text('Create branch');
            $('.modal-body').empty().append('<div class="form-group"><label>Name</label><input class="form-control" name="proj_name" type="text"></div>');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-success">Save</button>');
            break;
        case 'edit_branch':
            $('.modal-title').text('Edit branch');
            $('.modal-body').empty().append('<div class="form-group"><label>Name</label><input class="form-control" type="text" name="proj_name" value=""></div>');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-success">Save</button>');
            break;
        case 'delete_branch':
            $('.modal-title').text('Delete branch');
            $('.modal-body').empty().append('Are you sure want to delete ' + modalWinObj + '?');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-danger">Yes</button>');
            break;
        case 'create_config':
            $('.modal-title').text('Create config');
            $('.modal-body').empty().append('<div class="fileupload fileupload-new" data-provides="fileupload"><span class="btn btn-file btn-default"><span class="fileupload-new">Select file</span><span class="fileupload-exists">Change</span><input type="file"></span><span class="fileupload-preview"></span><a href="#" class="close fileupload-exists" data-dismiss="fileupload" style="float: none">×</a></div><div class="alert alert-warning">Error AZAZA!!!</div>');
            $('.modal-footer').hide();
            break;
        case 'diff_path_edit':
            $('.modal-title').text('Edit diff image path');
            $('.modal-body').empty().append('<div class="form-group"><label>Path</label><input class="form-control" name="diff_path" type="text"></div>');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-success">Save</button>');
            break;
        case 'add_image_worker':
            $('.modal-title').text('Add image worker');
            $('.modal-body').empty().append('<div class="form-group"><label>Name</label><input class="form-control" name="image_worker_name" type="text"></div><div class="form-group"><label>Url</label><input class="form-control" name="image_worker_url" type="text"></div>');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-success">Save</button>');
            break;
        case 'edit_image_worker':
            $('.modal-title').text('Edit image worker');
            $('.modal-body').empty().append('<div class="form-group"><label>Name</label><input class="form-control" name="image_worker_name" type="text"></div><div class="form-group"><label>Url</label><input class="form-control" name="image_worker_url" type="text"></div>');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-success">Save</button>');
            break;
        case 'remove_image_worker':
            $('.modal-title').text('Remove image worker');
            $('.modal-body').empty().append('Are you sure want to delete it?');
            $('.modal-footer').show().empty().append('<button type="submit" class="btn btn-danger">Yes</button>');
            break;
    }
}

$("#modal_form").submit(function (e) {
    e.preventDefault();
}).validate({
    rules: {
        name: {
            required: true,
            minlength: 4,
            maxlength: 15,
        },
        newName: {
            required: true,
            minlength: 4,
            maxlength: 10,
        },
        image_worker_name: {
            required: true,
            minlength: 4,
            maxlength: 10,
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
            maxlength: "Maximum length of 10 symbol",
        },
        newName: {
            required: "This field is required",
            minlength: "Minimum length of 4 symbol",
            maxlength: "Maximum length of 10 symbol",
        },
        image_worker_name: {
            required: "This field is required",
            minlength: "Minimum length of 4 symbol",
            maxlength: "Maximum length of 10 symbol",
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
        modal_ajax_call();
        return false;  //This doesn't prevent the form from submitting.
    }
});