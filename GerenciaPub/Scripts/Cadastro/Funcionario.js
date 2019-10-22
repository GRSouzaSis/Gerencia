function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#ddl_pessoa').val(dados.IdPessoa);
}
function set_focus_form() {
    $('#ddl_pessoa').focus();
}

function set_dados_grid(dados) {
    return
    '<td>' + dados.Nome + '</td>',
        '<td>' + dados.Celular + '</td>';
        //'<td>' + dados.IdPessoa.Nome + '</td>';
}

function get_dados_inclusao() {
    return {
        Id: 0,
       // Nome: '',
        IdPessoa: 0
    };
}
function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
       // Nome: $('#txt_nome').val(),
        IdPessoa: $('#ddl_pessoa').val()
    };
}
function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.IdPessoa.Nome);
}