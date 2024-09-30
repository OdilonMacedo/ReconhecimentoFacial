var diretorioUrl = getDiretorioUrl() === "/" ? null : getDiretorioUrl();

function pathJoin(...segments) {
    const separator = '/';
    const joinedSegments = segments
        .filter(segment => segment !== undefined && segment !== null && segment !== '')
        .map(segment => segment.toString().startsWith(separator) ? segment.toString().slice(1) : segment.toString())
        .join(separator);

    return separator + joinedSegments;
}
function ChamadaAjax(url, dados, tipo, mycallback, campoObjeto, callbackErro, showErrorMsg = true) {
    document.getElementById("loading-screen").style.display = "block";
    $.ajax({
        url: url,
        data: dados,
        contentType: 'application/json',
        type: tipo,
        success: function (data) {
            document.getElementById("loading-screen").style.display = "none";
            if (showErrorMsg && data.id === 1) {
                limpaCampo(campoObjeto);
                MostrarMensagem(data.mensagem, 'danger');
                MensagemToast(data.mensagem, "danger");
                try {
                    if (callbackErro && callbackErro != null && callbackErro != undefined) {
                        callbackErro(data);
                    }
                } catch (e) {
                    alert(e.message);
                }
                return false;
            }
            mycallback(data);
        },
        error: function (data) {
            document.getElementById("loading-screen").style.display = "none";
            if (showErrorMsg && data.responseJSON?.mensagem) MostrarMensagem(data.responseJSON.mensagem, 'danger')
            mycallback(data.responseJSON);
        }
    });
}

function FncEnter(eventKey, proximoId) {
    var tecla = (eventKey.keyCode ? eventKey.keyCode : eventKey.which);
    if (tecla == 13) {
        document.getElementById(proximoId).focus();
    }
}

function FecharMensagem() {
    const mensagem = document.getElementById("mensagem");
    const btnClose = mensagem?.querySelector('.btn-close')
    btnClose && btnClose.click();
}

function MostrarMensagem(msg, tipo) {
    const mensagem = document.getElementById("mensagem");

    if (mensagem !== null) {
        mensagem.innerHTML = "";
        mensagem.innerHTML = Mensagem(msg, tipo);
        mensagem.focus();
    } else {
        alert(msg);
    }
}

function MostrarMensagemModal(msg, tipo) {
    const mensagem = document.getElementById("mensagemModal");

    if (mensagem !== null) {
        mensagem.innerHTML = "";
        mensagem.innerHTML = Mensagem(msg, tipo);
        mensagem.focus();
    } else {
        alert(msg);
    }
}

function Mensagem(mensagem, tipo) {
    var content = "";
    if (tipo === "danger" || tipo === "success" || tipo === "info" || tipo === "warning") {
        content += '<div class="alert alert-' + tipo + ' alert-dismissible fade show" role="alert">';
        content += '<strong>' + mensagem + '</strong>';
        content += '<button type="button" class="btn-close text-right" data-bs-dismiss="alert" aria-label="Close"></button>';
        content += '</div>';
    } else {
        alert("Tipo de mensagem desconhecido. Defina o tipo como danger, success, info ou warning.");
    }
    return content;
}

function validaCampoVazio(param, campo) {
    if (param == null || param == "") {
        MostrarMensagem("Campo " + campo + " está vazio", "danger");
        return false;
    }
    return true;
}

function MensagemToast(mensagem, tipo) {
    //var msgToast = document.getElementById('txtMensagemToast');
    //var alertToast = document.getElementById('alertToast');
    var texto = mensagem;
    if (tipo === "danger" || tipo === "success" || tipo === "info" || tipo === "warning") {
        if (tipo === "danger")
            tipo = "error";

        toastr[tipo](texto);

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "3000",
            "hideDuration": "1000",
            "timeOut": "15000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

    } else {
        alert("Tipo de mensagem desconhecido. Defina o tipo como danger, success, info ou warning.");
    }
}

function limpaCampo(input) {
    if (input) {
        input.value = "";
        input.focus();
    }
}

function voltar() {
    ChamadaAjax(diretorioUrl + '/Home/VoltarMenuBela', null, "POST", returnVoltar);
}

function returnVoltar() {

}

function updateDatatable(dados, tabela, tbodyId) {
    var tabelaAtualizada = $('#' + tabela).DataTable(); // Substitua pelo seletor correto da sua tabela

    // Limpe os dados da tabela
    tabelaAtualizada.clear().draw();

    // Inserir os novos dados no tbody
    var htmlTbody = document.getElementById(tbodyId);
    htmlTbody.innerHTML = dados;

    // Atualize a DataTables com os novos dados
    tabelaAtualizada.rows.add($(htmlTbody).find('tr')).draw();
}

function GetErros() {
    var url = diretorioUrl + '/Notificacao/GetErros';
    ChamadaAjax(url, null, "GET", returnGetErros);
}

function returnGetErros(data) {
    document.getElementById('modalNotificacao').innerHTML = data;
    $('#modalMensagens').modal('show');
}

onload = function () {
    GetMsgNaoLidas();
}

function lerMensagem(idLinha) {
    var element = document.getElementById('check_' + idLinha);
    if (element) {
        element.style = "color: green;";
    }
    confirmacaoLeitura(idLinha);
    var url = diretorioUrl + '/Notificacao/GetMensagemEspecifica?id=' + idLinha;

    ChamadaAjax(url, null, "GET", returnMsgEspecifica);
}

function returnMsgEspecifica(data) {
    document.getElementById('detalheMensagemEspecifica').innerHTML = data.mensagem;
}

function GetMsgNaoLidas() {
    var url = diretorioUrl + '/Notificacao/GetMensagensNaoLidas';
    ChamadaAjax(url, null, "GET", atualizaMsgNaoLidas);
}

function confirmacaoLeitura(idItem) {
    var url = diretorioUrl + '/Notificacao/AtualizaMensagens?id=' + idItem;
    ChamadaAjax(url, null, "GET", atualizaMsgNaoLidas);
}

function atualizaMsgNaoLidas(numeroMsg) {
    var qtdeNotificacao = document.getElementById('numeroNotificacao');
    if (qtdeNotificacao) {
        qtdeNotificacao.setAttribute('data-count', numeroMsg);
    }
}