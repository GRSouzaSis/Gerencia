function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_codigo').val(dados.Codigo);
    $('#txt_nome').val(dados.Nome);
    $('#txt_preco_custo').val(dados.PrecoCusto);
    $('#txt_preco_venda').val(dados.PrecoVenda);
    $('#txt_quant_estoque').val(dados.QuantEstoque);
    $('#txt_unidade_medida').val(dados.UnidadeMedida);
    $('#ddl_grupo').val(dados.IdGrupo);
    $('#ddl_marca').val(dados.IdSubGrupo);
    $('#cbx_ativo').prop('checked', dados.Ativo);
    $('#cbx_ativoadicional').prop('checked', dados.AdicionalAtivo);
    $('#cbx_gera_estoque').prop('checked', dados.GeraEstoque);
}

function set_focus_form() {
    var alterando = (parseInt($('#id_cadastro').val()) > 0);
    $('#txt_preco_custo,#txt_preco_venda').mask('#.##0,00', { reverse: true });
    $('#txt_quant_estoque').attr('readonly', alterando); 
    $('#txt_codigo').focus();
}

function set_dados_grid(dados) {
    var str = '<td>' + dados.Codigo + '</td>' +
        '<td>' + dados.Nome + '</td>' +
        '<td>' + dados.QuantEstoque + '</td>' +
        '<td>' + (dados.Ativo ? 'SIM' : 'NÃO') + '</td>';
    return str;
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Codigo: '',
        Nome: '',
        PrecoCusto: '',
        PrecoVenda: '',
        QuantEstoque: '',
        UnidadeMedida: '',
        IdGrupo: 0,
        IdSubGrupo: 0,
        Ativo: true,
        AdicionalAtivo: true,
        GeraEstoque: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Codigo: $('#txt_codigo').val(),
        Nome: $('#txt_nome').val(),
        PrecoCusto: $('#txt_preco_custo').val(),
        PrecoVenda: $('#txt_preco_venda').val(),
        QuantEstoque: $('#txt_quant_estoque').val(),
        UnidadeMedida: $('#txt_unidade_medida').val(),
        IdGrupo: $('#ddl_grupo').val(),
        IdSubGrupo: $('#ddl_marca').val(),
        Ativo: $('#cbx_ativo').prop('checked'),
        AdicionalAtivo: $('#cbx_ativoadicional').prop('checked'),
        GeraEstoque: $('#cbx_gera_estoque').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Codigo).end()
        .eq(1).html(param.Nome).end()
        .eq(2).html(param.QuantEstoque).end()
        .eq(3).html(param.Ativo ? 'SIM' : 'NÃO');
}

$(document)
    .ready(function () {
        $('#txt_preco_custo,#txt_preco_venda').mask('#.##0,00', { reverse: true });
        $('#txt_quant_estoque').mask('00000');
        //<globalization culture="pt-BR" uiCulture="pt-BR" /> // global para converter 
    });
  