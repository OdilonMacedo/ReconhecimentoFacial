using Microsoft.AspNetCore.Mvc;
using SixConsult.NET.Foundation.MVC.MvcExceptions;
using SixConsult.NET.Foundation.SignalR.NotificationAsync;
using TesteWebCanToWebAPI.Models;
using TesteWebCanToWebAPI.Services.IServices;

namespace TesteWebCanToWebAPI.Controllers
{
    public class NotificacaoController : Controller
    {
        private readonly NotificaHub _notificaHub;
        private readonly INotificacaoService _notificaService;
        private readonly IWebHostEnvironment _env;


        public NotificacaoController(NotificaHub notificaHub, INotificacaoService notificaService, IWebHostEnvironment env)
        {
            _notificaHub = notificaHub;
            _notificaService = notificaService;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NotificaUsuario([FromBody] NotificacaoViewModel request)
        {
            try
            {
                await _notificaHub.NotificaUsuario(request.Usuario, request.Mensagem, request.Tipo);
                await _notificaService.AdicionarItem(request.Usuario, request.Mensagem);

                var mensagens = await _notificaService.GetErros(request.Usuario);
                var mensagensNaoLidas = mensagens.Where(w => w.MsgLida == false).Count();
                await _notificaHub.AtualizaMensagensLidas(request.Usuario, mensagensNaoLidas);
                return Json(new { Id = 0, Sucesso = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new SixPartialViewCustomException(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetErros()
        {
            var usuario = GetUsuarioRealDev();

            var mensagensNaoLidas = await _notificaService.GetErros(usuario);

            return PartialView("_NotificacaoPartial", mensagensNaoLidas);
        }

        [HttpGet]
        public async Task<int> AtualizaMensagens(string id)
        {
            var usuario = GetUsuarioRealDev();
            await _notificaService.ConfirmaLeitura(id);
            var mensagens = await _notificaService.GetErros(usuario);
            var mensagensNaoLidas = 0;
            if (mensagens != null)
            {
                mensagensNaoLidas = mensagens.Where(w => w.MsgLida == false).Count();
            }
            await _notificaHub.AtualizaMensagensLidas(usuario, mensagensNaoLidas);
            return mensagensNaoLidas;
        }

        [HttpGet]
        public async Task<int> GetMensagensNaoLidas()
        {
            var usuario = GetUsuarioRealDev();
            var mensagens = await _notificaService.GetErros(usuario);
            var mensagensNaoLidas = mensagens.Where(w => w.MsgLida == false).Count();
            return mensagensNaoLidas;
        }

        public string GetUsuarioRealDev()
        {
            if (!_env.IsDevelopment())
                return User.GetUsuarioRealValue();

            try
            {
                return User.GetUsuarioRealValue();
            }
            catch (Exception)
            {
                return "Anonimo";
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMensagemEspecifica(string id)
        {
            var mensagem = await _notificaService.GetErroById(id);

            return Json(mensagem);
        }
    }
}
