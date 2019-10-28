function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);

    $('#ddl_estado').val(dados.IdEstado.Nome);
    //$('#ddl_estado').prop('disabled', dados.IdEstado <= 0 || dados.IdEstado == undefined);
}
function set_focus_form() {
    $('#txt_nome').focus();
}

function set_dados_grid(dados) {
    var str = '<td>' + dados.Nome + '</td>',
    '<td>' + dados.IdEstado + '</td>';
    return str;
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        IdEstado: 0
    };
}
function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        IdEstado: $('#ddl_estado').val()
    };
}
function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Uf);
}

$(document).on('change', '#txt_nome', function () {
   
        ddl_estado = $('#ddl_estado');

    
        var url = url_listar_estados,
            param = { IdEstado: uf_if };

        ddl_estado.empty();
       // $('#ddl_estado').prop('disabled', true);

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response && response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    ddl_estado.append('<option value=' + response[i].IdEstado + '>' + response[i].Nome + '</option>');
                }
               // $('#ddl_estado').prop('disabled', false);
            }
        });
    


});