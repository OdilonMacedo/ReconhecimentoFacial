using TesteWebCanToWebAPI.Models;

namespace TesteWebCanToWebAPI.Services.IServices
{
    public interface INotificacaoService
    {
        public Task<List<NotificacaoViewModel>> GetErros(string usuario);
        public Task AdicionarItem(string mensagem, string message);
        public Task<NotificacaoViewModel> GetErroById(string id);
        public Task ConfirmaLeitura(string id);
    }
}