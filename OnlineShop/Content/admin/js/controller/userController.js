var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status === true) {
                        btn.html('<button class="btn btn-success">Hoạt động</button>');
                    }
                    else {
                        btn.html('<button class="btn btn-danger">Khóa</button>');
                    }
                }
            });
        });
    }
};
user.init();
