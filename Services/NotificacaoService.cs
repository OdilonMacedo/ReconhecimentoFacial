using SixConsult.Net.Foundation.Configuration.Contracts;
using SixConsult.NET.Foundation.ConexaoRest.Interface;
using SixConsult.NET.Foundation.ConexaoRest.Models.Enums;
using TesteWebCanToWebAPI.Mockers.Notificacao.Interface;
using TesteWebCanToWebAPI.Models;
using TesteWebCanToWebAPI.Services.IServices;

namespace TesteWebCanToWebAPI.Services
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly bool _useMocker;
        private readonly INotificacaoMocker _notificacaoMocker;
        private readonly IRestNotificacao _rest;

        public NotificacaoService(IRestNotificacao rest, INotificacaoMocker notificacaoMocker, EnvironmentConfigurationSix env)
        {
            _rest = rest;
            _notificacaoMocker = notificacaoMocker;
            _useMocker = env.UseMocker;
        }

        public async Task<List<NotificacaoViewModel>> GetErros(string usuario)
        {
            if (_useMocker)
            {
                return await _notificacaoMocker.GetErros();
            }

            var listaErros = await _rest.Request<List<NotificacaoViewModel>>(MethodEnum.GET, "");

            if (listaErros != null && listaErros.Count != 0)
            {
                return listaErros.Take(20).ToList();
            }

            return new List<NotificacaoViewModel>();
        }

        public async Task AdicionarItem(string usuario, string mensagem)
        {
            if (_useMocker)
            {
                await _notificacaoMocker.AdicionarItem(mensagem);
            }
            else
            {
                await _rest.Request(MethodEnum.POST, "", mensagem);
            }
        }

        public async Task ConfirmaLeitura(string id)
        {
            if (_useMocker)
            {
                await _notificacaoMocker.ConfirmaLeitura(id);
            }
            else
            {
                await _rest.Request(MethodEnum.POST, "", id);
            }
        }

        public async Task<NotificacaoViewModel> GetErroById(string id)
        {
            if (_useMocker)
            {
                return await _notificacaoMocker.GetErroById(id);
            }
            var erro = await _rest.Request<NotificacaoViewModel>(MethodEnum.GET, "");
            return erro;

        }
    }
}
