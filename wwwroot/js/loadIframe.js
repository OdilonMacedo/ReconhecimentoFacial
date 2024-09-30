function resizeIframe(iframeId) {
    const iframe = document.getElementById(iframeId);
    if (iframe) {
        iframe.style.cssText = "border:none; position: absolute; top: 0; left: 0; width: 100%; height: 100%;";
        iframe.style.width = window.innerWidth + 'px';
        iframe.style.height = window.innerHeight + 'px';
    }
}

function loadIframe(iframeId, urlA, urlB) {
    const iframe = document.getElementById(iframeId);

    fetch(urlA)
        .then(response => {
            if (response.ok) {
                iframe.src = urlA;
                iframe.addEventListener('load', resizeIframe(iframeId));
                window.addEventListener('resize', resizeIframe(iframeId));
            } else {
                iframe.src = urlB;
            }
        })
        .catch(error => {
            console.error('Erro ao carregar a URL:', error);
            iframe.src = urlB;
        });


}







