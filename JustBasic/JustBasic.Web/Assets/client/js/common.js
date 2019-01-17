var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {
        //$("#txtKeyword").autocomplete({
        //    minLength: 0,
        //    source: function (request, response) {
        //        $.ajax({
        //            url: "/Product/GetListProductByName",
        //            dataType: "json",
        //            data: {
        //                keyword: request.term
        //            },
        //            success: function (res) {
        //                response(res.data);
        //            }
        //        });
        //    },
        //    focus: function (event, ui) {
        //        $("#txtKeyword").val(ui.item.label);
        //        return false;
        //    },
        //    select: function (event, ui) {
        //        $("#txtKeyword").val(ui.item.label);
        //        return false;
        //    }
        //}).autocomplete("instance")._renderItem = function (ul, item) {
        //    return $("<li>")
        //      .append("<a>" + item.label + "</a>")
        //      .appendTo(ul);
        //};

        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            let Id = parseInt($(this).attr('data-id'));
            //let quantity = parseInt($(this).attr('data-quantity'));
            let quantity = $('#txt_quantity').val();
            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    Id: Id,
                    Quantity: quantity
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        common.loadData(response.data);
                        location.href = '/gio-hang.html'; 
                    }
                    else {
                        alert(response.message);
                    }
                }
            });
        });
        $('#btnLogout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#frmLogout').submit();
        });

        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var Id = parseInt($(this).data('id'));
            common.deleteItem(Id);
        });
        $('.txtQuantity').off('keyup').on('keyup', function () {
            debugger;
            var quantity = parseInt($(this).val());
            var id = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {
                var amount = quantity * price;
                $('#amount_' + id).text(numeral(amount).format('0,0'));
                $('#lblTotalOrder').text(numeral(common.getTotalOrder()).format('0,0'));
                common.updateAll();
            }
            else {
                $('#amount_' + id).text(0);
                $('#lblTotalOrder').text(0);
            }
        });
        $('.txtQuantity').off('change').on('change', function () {
            debugger;
            var quantity = parseInt($(this).val());
            var id = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {
                var amount = quantity * price;
                $('#amount_' + id).text(numeral(amount).format('0,0'));
                $('#lblTotalOrder').text(numeral(common.getTotalOrder()).format('0,0'));
                common.updateAll();
            }
            else {
                $('#amount_' + id).text(0);
                $('#lblTotalOrder').text(0);
            }
        });
    },
    getTotalOrder: function () {
        debugger;
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price') - $(item).data('promotion'));
        });
        return total;
    },
    deleteItem: function (Id) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                Id: Id,
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    common.loadData(response.data);
                }
            }
        });
    },
    updateAll: function () {
        debugger;
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
                    common.loadData(response.data);
                    console.log('Update ok');
                }
            }
        });
    },
    loadData: function (data) {
        var template = $('#tplCart').html();
        var html = '';
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
                Amount: numeral(item.Quantity * (item.SalePrice - item.PromotionPrice)).format('0,0')
            });
        });

        $('#cartBody').html(html);

        if (html == '') {
            $('#cartContent').html('Không có sản phẩm nào trong giỏ hàng.');
        }
        $('#lblTotalOrder').text(numeral(common.getTotalOrder()).format('0,0') + ' vnđ');
        common.registerEvents();
    }
}
common.init();