function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_cpf').val(dados.Cpf);
    $('#txt_endereco').val(dados.Endereco);
    $('#txt_bairro').val(dados.Bairro);
    $('#txt_numero').val(dados.Numero);
    $('#txt_telefone').val(dados.Telefone);
    $('#txt_celular').val(dados.Celular);
    $('#txt_email').val(dados.Email);
    $('#txt_obs').val(dados.Obs);
    $('#cbx_ativo').prop('checked', dados.Ativo);
    $('#ddl_cidade').val(dados.IdCidade);
}
function set_focus_form() {
    $('#txt_nome').focus();
}

function set_dados_grid(dados) {
    return
    '<td>' + dados.Nome + '</td>',
        '<td>' + dados.Telefone + '</td>',
        '<td>' + dados.Celular + '</td>',
        '<td>' + (dados.Ativo ? 'SIM' : 'NÃO') + '</td>';
}


function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Cpf: '',
        Bairro: '',
        Numero: '',
        Endereco: '',
        Telefone: '',
        Celular: '',
        Obs: '',
        Email: '',
        Ativo: true,
        IdCidade: 0
    };
}
function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Cpf: $('#txt_cpf').val(),
        Endereco: $('#txt_endereco').val(),
        Bairro: $('#txt_bairro').val(),
        Numero: $('#txt_numero').val(),
        Telefone: $('#txt_telefone').val(),
        Celular: $('#txt_celular').val(),
        Email: $('#txt_email').val(),
        Obs: $('#txt_obs').val(),
        Ativo: $('#cbx_ativo').prop('checked'),
        IdCidade: $('#ddl_cidade').val()
    };
}
function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Telefone)
        .eq(1).html(param.Celular)
        .eq(3).html(param.Ativo ? 'SIM' : 'NÃO');
}
$(document)
    .ready(function () {       
        $('#txt_telefone').mask("(00) 0000-0000");
        $('#txt_celular').mask("(00) 00000-0000");
        $('#txt_cpf').mask("000.000.000-00");
    });