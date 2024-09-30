using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixConsult.Net.Foundation.Configuration.Contracts;
using SixConsult.NET.Foundation.Menu.Contracts.Enums;
using SixConsult.NET.Foundation.Menu.InstaladorApi.Adapter.Interface;

namespace TesteWebCanToWebAPI.Controllers
{
    [AllowAnonymous]
    public class InstaladorController : Controller
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _connectionType;
        private readonly IInstaladorModelAdapter _modelAdapter;

        public InstaladorController(EnvironmentConfigurationSix env, IHttpContextAccessor httpContext, IInstaladorModelAdapter modelAdapter)
        {
            _connectionType = env.ConnectionType;
            _httpContext = httpContext;
            _modelAdapter = modelAdapter;
        }

        public IActionResult Install()
        {
            return View();
        }

        public async Task<JsonResult> DoInstalacao()
        {
            var guidProduto = new Guid("");

            var urlProduto = CreateUri();

            #region "Construtor" da model
            await _modelAdapter.CreateModel(urlProduto, guidProduto, "Nome Produto", _connectionType);
            #endregion

            #region Produto
            await _modelAdapter.Add("/caminho/para/imagem.jpg", string.Empty, "Nome do caminho", TipoInstalacao.produto, guidProduto.ToString(), string.Empty, 200);
            #endregion

            #region modulo
            await _modelAdapter.Add("/caminho/para/imagem.jpg", string.Empty, "Nome do caminho", TipoInstalacao.modulo, "guid do modulo", guidProduto.ToString(), 200);
            #endregion

            #region tela
            await _modelAdapter.Add("/caminho/para/imagem.jpg", "/caminho-endpoint-tela", "Nome do caminho", TipoInstalacao.tela, "guid da tela", "guid pai da tela", 200);
            #endregion

            #region subtela
            await _modelAdapter.Add("/caminho/para/imagem.jpg", "/caminho-endpoint-tela", "Nome do caminho", TipoInstalacao.subtela, "guid da subtela", "guid pai da subtela", 200);
            #endregion

            await _modelAdapter.Execute();

            return Json(new { Id = 0, Sucesso = true });
        }

        public Uri CreateUri()
        {
            #region "Validações"
            if (_httpContext == null)
                throw new ArgumentNullException(nameof(_httpContext));

            if (_httpContext.HttpContext == null)
                throw new ArgumentNullException(nameof(_httpContext.HttpContext));
            #endregion

            var request = _httpContext.HttpContext.Request;

            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.PathBase
            };

            if (request.Host.Port != null)
            {
                uriBuilder.Port = request.Host.Port.Value;
            }

            return uriBuilder.Uri;
        }
    }
}
