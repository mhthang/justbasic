var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();
    },
    registerEvent: function () {
        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "Yêu cầu nhập tên",
                address: "Yêu cầu nhập địa chỉ",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                phone: {
                    required: "Số điện thoại được yêu cầu",
                    number: "Số điện thoại phải là số."
                }
            }
        });
        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem(productId);
        });
        $('.txtQuantity').off('keyup').on('keyup', function () {
            var quantity = parseInt($(this).val());
            var id = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {
                var amount = quantity * price;
                $('#amount_' + id).text(numeral(amount).format('0,0'));
                $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
                cart.updateAll();
            }
            else {
                $('#amount_' + id).text(0);
                $('#lblTotalOrder').text(0);
            }
        });
        $('.txtQuantity').off('chagne').on('chagne', function () {
            var quantity = parseInt($(this).val());
            var id = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {
                var amount = quantity * price;
                $('#amount_' + id).text(numeral(amount).format('0,0'));
                $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
                cart.updateAll();
            }
            else {
                $('#amount_' + id).text(0);
                $('#lblTotalOrder').text(0);
            }
        });

        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });
        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAll();
        });
        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#divCheckout').show();
        });
        $('#chkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked'))
                cart.getLoginUser();
            else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }
        });
        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                cart.createOrder();
            }
        });
        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() == 'NL') {
                $('.boxContent').hide();
                $('#nganluongContent').show();
            }
            else if ($(this).val() == 'ATM_ONLINE') {
                $('.boxContent').hide();
                $('#bankContent').show();
            }
            else {
                $('.boxContent').hide();
            }
        });
        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() == 'CK') {
                $('#ChuyenKhoanContent').show();
            }
            else {
                $('#ChuyenKhoanContent').hide();
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },

    createOrder: function () {
        fbq('track', 'Purchase');
        var order = {
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            Description: $('#txtDescription').val(),
            PaymentMethod: $('input[name="paymentMethod"]:checked').val(),
            BankCode: $('input[name="bankcode"]:checked').prop('id'),
            Status: false
        }
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    if (response.urlCheckout != undefined && response.urlCheckout != '') {
                        window.location.href = response.urlCheckout;
                    }
                    else {
                        console.log('create order ok');
                        cart.deleteAll();
                        setTimeout(function () {
                            $('.card').remove();
                            $('.success-form').show();
                        });
                    }
                }
                else {
                    $('#divMessage').show();
                    $('#divMessage').text(response.message);
                }
            }
        });
    },
    getTotalOrder: function () {
        debugger;
        var listTextBox = $('.txtQuantity');
        var total = 0;
        var quantity = 0;
        $.each(listTextBox, function (i, item) {
            quantity += parseInt($(item).val());
            total += parseInt($(item).val()) * parseFloat($(item).data('price') - $(item).data('promotion'));
        });
        if (quantity < 2) {
            total += 20000;
        }
        return total;
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                }
            }
        });
    },
    updateAll: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                Id: $(item).data('id'),
                Quantity: $(item).val()
            });
        });
        $.ajax({
            url: '/ShoppingCart/Update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    console.log('Update ok');
                }
            }
        });
    },
    deleteItem: function (Id) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                Id: Id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            Id: item.Id,
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.SalePrice,
                            PriceF: numeral(item.SalePrice).format('0,0'),
                            Promotion: item.PromotionPrice,
                            PromotionF: numeral(item.PromotionPrice).format('0,0'),
                            Quantity: item.Quantity,
                            SizeId: item.SizeId,
                            ColorId: item.ColorId,
                            SizeName: item.SizeName,
                            ColorName: item.ColorName,
                            ShipCost: numeral(20000).format('0,0'),
                            Amount: numeral(item.Quantity * (item.SalePrice - item.PromotionPrice)).format('0,0')
                        });
                    });

                    $('#cartBody').html(html);

                    if (html == '') {
                        $('.card').html('Không có sản phẩm nào trong giỏ hàng.');
                    }
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0') + ' vnđ');
                    cart.registerEvent();
                }
            }
        })
    }
}
cart.init();