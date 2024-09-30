$(document).ready(function () {
    ChamadaAjax(diretorioUrl + '/Instalador/DoInstalacao', null, "POST", returnInstall);
});

function returnInstall(dados) {
    MostrarMensagem("Produto instalado com sucesso", "success");
    MensagemToast("Produto instalado com sucesso", "success");
}