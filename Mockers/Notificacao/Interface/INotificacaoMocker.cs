using TesteWebCanToWebAPI.Models;

namespace TesteWebCanToWebAPI.Mockers.Notificacao.Interface
{
    public interface INotificacaoMocker
    {
        public Task<List<NotificacaoViewModel>> GetErros();
        public Task AdicionarItem(string mensagem);
        public Task<NotificacaoViewModel> GetErroById(string id);
        public Task ConfirmaLeitura(string id);
    }
}