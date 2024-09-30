var diretorioUrl = getDiretorioUrl() === "/" ? null : getDiretorioUrl();

function AjaxCall(url, data, tipo, mycallback, campo, callbackErro) {
    var xhr;
    var AjaxTimeOut;
    document.getElementById("DivProcessando").style.display = "block";
    if (!window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xhr = new ActiveXObject("Microsoft.XMLHTTP");
    }
    else {
        xhr = new XMLHttpRequest();
    }
    xhr.open(tipo, url);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onreadystatechange = function () {
        if (xhr.readyState != 1) {
            var ndata;
            if (xhr.status != null && xhr.status === 200 && xhr.readyState == 4) {


                try {
                    //Quando a resposta vem no nosso padrão de objeto
                    ndata = JSON.parse(xhr.responseText);
                    if (ndata.id == 1) {
                        limparCampos(campo);
                        exibeErro(ndata.mensagem);
                        try {
                            if (callbackErro && callbackErro != null && callbackErro != undefined) {
                                callbackErro(ndata);
                            }
                        } catch (e) { alert(e.message); }
                    } else {
                        limparErro()
                        mycallback(ndata);
                    }
                } catch (e) {
                    try {
                        //Quando a resposta vem como html livre
                        if (xhr.responseText != null)
                            mycallback(xhr.responseText);
                    } catch (ex) {
                        alert(ex.message);
                    }
                }
                document.getElementById("DivProcessando").style.display = "none";

            } else if (xhr.status == 404 || xhr.status == 12029 || xhr.status == 12007) {

                document.getElementById("DivProcessando").style.display = "none";
                try {
                    xhr.abort();
                } catch (e) { }
                exibeMsgOffline();
            }
            else if (xhr.status == 500) {
                if (confirm("Ocorreu um erro na execução ao servidor!\n\nIsto ocorre por falta de conexão ou necessidade de fazer o login novamente!\n\n\n Deseja visualizar o erro?")) {
                    document.writeln(xhr.responseText);
                    document.writeln('<h1> <a href="@Url.Action("Index", "Home")">Acesse novamente o sistema clicando aqui</a></h1>');
                }

            }
        }
    };
    try {
        xhr.send(data);

    } catch (e) {
        alert(e);
    }
}

function exibeMsgOffline() {
    alert("!!!ATENÇÃO!!!!\n\n\nAguarde até que o coletor ative o wifi!\nQualquer operação efetuada neste intervalo não será salva\nCaso demore entre em contato com a TI");
}

function validaCampoVazio(param, campo) {
    if (param == null || param == "") {
        exibeErro("Campo " + campo + " está vazio");
        return false;
    }
    return true;
}

function limparCampos(campo) {
    if (campo && campo != "") {
        var meuCampo = document.getElementById(campo);
        meuCampo.value = "";
    }
}

function exibeErro(erro) {
    document.getElementById("erro").innerHTML = erro;
    document.getElementById("sucesso").innerHTML = "";
}

function exibeSucesso(sucesso) {
    document.getElementById("sucesso").innerHTML = sucesso;
}

function limparErro() {
    document.getElementById("erro").innerHTML = "";
}

function limparSucesso() {
    document.getElementById("sucesso").innerHTML = "";
}

function limparCampo(obj) {
    if (obj) {
        obj.value = "";
    }
}

function fncEnter(eventKey, idProximo, metodoExecutar) {

    var tecla = (eventKey.keyCode ? eventKey.keyCode : eventKey.which);
    if (tecla == 13) {
        if (idProximo != null || idproximo != undefined) {
            var prox = document.getElementById(idProximo);
            if (prox.tagName == "BUTTON" || prox.tagName == "IMG") {
                prox.click()
            } else {
                prox.focus()
            }
        }
        if (typeof metodoExecutar === 'function') {
            metodoExecutar();
        }
    }
}

function formatarData(dataString) {
    var partes = dataString.split("T");
    var dataPartes = partes[0].split("-");
    var horaPartes = partes[1].split(":");

    var dia = dataPartes[2];
    var mes = dataPartes[1];
    var ano = dataPartes[0];

    var hora = horaPartes[0];
    var minuto = horaPartes[1];
    var segundo = horaPartes[2];

    var dataFormatada = dia + "/" + mes + "/" + ano + " " + hora + ":" + minuto + ":" + segundo;

    return dataFormatada;
}

function voltar() {
    var urlAnterior = document.referrer;

    if (urlAnterior != "") {
        window.location.href = urlAnterior;
    } else {
        history.back();
    }
}

