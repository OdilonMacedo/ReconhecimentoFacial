class NotificacaoClient {
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/notificacaoHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.on("Recebida", (message, tipo) => {
            this.connection.invoke("ConfirmacaoRecebida");

            this.messageNotification(message, tipo);
        });

        this.connection.on("AtualizaMensagem", (msg) => {
            this.atualizaMensagensLidas(msg);
        })
        this.connection.onclose((err) => {
            console.log("Conexao fechada");
            if (err) {
                setTimeout(() => {
                    this.startConnection();
                }, 2000);
            }
        });

        this.startConnection();
    }

    startConnection() {
        this.connection.start()
            .then(() => {
                console.log("Conexão iniciada");
            })
            .catch((err) => {
                setTimeout(() => {
                    this.startConnection();
                }, 2000);
                console.error(`Erro ao iniciar a conexão: ${err.message}`);
            });
    }

    messageNotification(message, tipo) {
        MensagemToast(message, tipo);
    }
    atualizaMensagensLidas(numeroMsg) {
        var qtdeNotificacao = document.getElementById('numeroNotificacao');
        if (qtdeNotificacao) {
            qtdeNotificacao.setAttribute('data-count', numeroMsg);
        }
    }
}
