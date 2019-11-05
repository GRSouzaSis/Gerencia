function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

function formatar_mensagem_aviso(mensagens) {
    var ret = '';

    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }

    return '<ul>' + ret + '</ul>';
}

$(document).ready(function () {
    $('#txt_mesa').focus();
    $('#txt_mesa').mask("000000");
})
    .on('click', '#btn_incluir', function () {
        var btn = $(this),
            url = url_confirmar,
            param = get_dados_form();

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response.Resultado == "OK") {
                if (param.Id == 0) {
                    param.Id = response.IdSalvo;
                    var table = $('#grid_cadastro').find('tbody');
                    var linha = criar_linha_grid(param);
                    table.append(linha);
                }
                else {
                    var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
                    preencher_linha_grid(param, linha);
                }

                $('#modal_cadastro').parents('.bootbox').modal('hide');
            }
            else if (response.Resultado == "ERRO") {
                $('#msg_aviso').hide();
                $('#msg_mensagem_aviso').hide();
                $('#msg_erro').show();
            }
            else if (response.Resultado == "AVISO") {
                $('#msg_mensagem_aviso').html(formatar_mensagem_aviso(response.Mensagens));
                $('#msg_aviso').show();
                $('#msg_mensagem_aviso').show();
                $('#msg_erro').hide();
            }
        });

        addDiv();
        $('#txt_mesa').val() = '';
    })
    .on('keyup', '#txt_filtro', function () {
        var filtro = $(this),
            url = url_filtro,
            param = { 'filtro': filtro.val() }

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                if (response.length > 0) {
                    $('#grid_cadastro').removeClass('invisivel');
                    $('#mensagem_grid').addClass('invisivel');

                    for (var i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }
                }
                else {
                    $('#grid_cadastro').addClass('invisivel');
                    $('#mensagem_grid').removeClass('invisivel');
                }
            }
        });
    });

document.addEventListener('keydown', function (e) {
    if (e.key == "Enter") {
        document.getElementById("btn_incluir").click();
    }
});

function getDataHora() {
    // variáveis
    var data = new Date();
    var hora = data.getHours();
    var minutos = data.getMinutes();
    var segundos = data.getSeconds();
    var mes = data.getMonth() + 1;
    var dia = data.getDate();

    // zero à esquerda se necessário
    dia = dia < 10 ? '0' + dia : dia;
    mes = mes < 10 ? '0' + mes : mes;
    hora = hora < 10 ? '0' + hora : hora;
    minutos = minutos < 10 ? '0' + minutos : minutos;
    segundos = segundos < 10 ? '0' + segundos : segundos;


    // monta resultado
    var resultado = dia + "/" + mes + "/" + data.getFullYear() + " " + hora + ':' + minutos + ':' + segundos;


    return resultado;
}

function get_dados_form() {
    return {
        Id: $('#txt_mesa').val()
    };
}

function res() {
   
    bootbox.dialog({
        title: 'Funções ',
        message: getDataHora()
    });

}


function addDiv() {
    var divAtual = cloneform = $('#controle_mesas').html();
    var divNova = document.createElement("div");
    divNova.setAttribute("data-id", $('#txt_mesa').val());
    divNova.setAttribute("class", "mesas");
    divNova.setAttribute("onclick", "res()");
    divNova.style.backgroundColor = 'red';
    var conteudoNovo = document.createTextNode($('#txt_mesa').val());
    divNova.appendChild(conteudoNovo); //adiciona o nó de texto à nova div criada
    $('#controle_mesas').append(divNova);// adiciona o novo elemento criado e seu conteúdo ao DOM
}

function get_dados_inclusao() {
    return {
        MesaId: 0,
        MesaNome: 0,
        MesaAtivo: 0,
        MoviId: 0,
        MoviNroPessoa: 0,
        MoviDataEntrada: null,
        MoviDataSaida: null,
        MesaAtivo: true
    };
}
function get_dados_form() {
    return {
        MesaNome: $('#txt_mesa').val()
    };
}


/*
function criar_div_mesa() {

    var ret =  ' <div data-id=' + MesaId + ' onclick=' + res() + ' class=' + mesas + '>' +
        ' <div class=' + conteudo_mesa + '>' +
        '<p>' + MesaId + '</p>' +
        '</div>'
    '</div>';
    return ret;
}*/