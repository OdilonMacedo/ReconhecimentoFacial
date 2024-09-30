using System.Text.Json;
using TesteWebCanToWebAPI.Mockers.Notificacao.Interface;
using TesteWebCanToWebAPI.Models;

namespace TesteWebCanToWebAPI.Mockers.Notificacao
{
    public class NotificacaoMocker : INotificacaoMocker
    {
        string caminhoMocker = Path.Combine(Path.GetTempPath(), "database.json");
        public async Task<List<NotificacaoViewModel>> GetErros()
        {
            var listaErros = await LerDados();

            if (listaErros != null && listaErros.Count != 0)
            {
                return listaErros.Take(20).ToList();
            }

            return new List<NotificacaoViewModel>();
        }

        private async Task<List<NotificacaoViewModel>> LerDados()
        {
            await Task.Delay(1000);
            if (!File.Exists(caminhoMocker))
                return new List<NotificacaoViewModel>();

            string json = File.ReadAllText(caminhoMocker);
            if (json != null && !string.IsNullOrEmpty(json))
            {
                return JsonSerializer.Deserialize<List<NotificacaoViewModel>>(json);
            }
            return new List<NotificacaoViewModel>();
        }

        private async Task SalvarDados(List<NotificacaoViewModel> data)
        {
            await Task.Delay(1000);
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(caminhoMocker, json);
        }

        public async Task AdicionarItem(string mensagem)
        {
            NotificacaoViewModel newItem = new NotificacaoViewModel()
            {
                Id = Guid.NewGuid(),
                Mensagem = mensagem,
                MsgLida = false,
                Tipo = "erro",
                Hora = DateTime.Now.ToString()
            };
            List<NotificacaoViewModel> items = await LerDados() ?? new List<NotificacaoViewModel>();
            items.Insert(0, newItem);
            await SalvarDados(items);
        }

        public async Task ConfirmaLeitura(string id)
        {
            var dados = await LerDados();
            foreach (var item in dados)
            {
                if (item.Id.ToString() == id)
                    item.MsgLida = true;
            }
            await SalvarDados(dados);
        }

        public async Task<NotificacaoViewModel> GetErroById(string id)
        {
            await Task.Delay(1000);
            var dados = await LerDados();
            return dados.FirstOrDefault(f => f.Id.ToString() == id);
        }
    }
}
