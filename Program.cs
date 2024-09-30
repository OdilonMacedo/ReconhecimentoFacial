using Microsoft.AspNetCore.Authentication;
using Sixconsult.NET.Foundation.MVC.Filters;
using SixConsult.Net.Foundation.Configuration.Contracts;
using SixConsult.NET.Foundation.ConexaoRest;
using SixConsult.NET.Foundation.ConexaoRest.Interface;
using SixConsult.NET.Foundation.HttpClientHandlerSix;
using SixConsult.NET.Foundation.HttpClientHandlerSix.Extensions;
using SixConsult.NET.Foundation.Menu.Identity;
using SixConsult.NET.Foundation.Menu.Identity.Authorization;
using SixConsult.NET.Foundation.Menu.InstaladorApi.Adapter;
using SixConsult.NET.Foundation.Menu.InstaladorApi.Adapter.Interface;
using SixConsult.NET.Foundation.Menu.InstaladorApi.Instalador;
using SixConsult.NET.Foundation.Menu.InstaladorApi.Instalador.Interface;
using SixConsult.NET.Foundation.Mocker;
using SixConsult.NET.Foundation.SignalR.NotificationAsync;
using TesteWebCanToWebAPI.Config;
using TesteWebCanToWebAPI.Mockers.Home;
using TesteWebCanToWebAPI.Mockers.Home.Interface;
using TesteWebCanToWebAPI.Mockers.Notificacao;
using TesteWebCanToWebAPI.Mockers.Notificacao.Interface;
using TesteWebCanToWebAPI.Services;
using TesteWebCanToWebAPI.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;

#region Injeta as configuraçoes de conexao

var env = configuration.Get<EnvironmentConfigurationSix>();

builder.Services.AddSingleton(env);
builder.Services.AddSingleton(env.Security);

builder.Services.AddScoped<SixDelagatingHandler>();

#endregion

#region Authentication

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ISixIdentity, SixIdentity>();

builder.Services.AddAuthentication("CustomScheme")
    .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", options => { });

#endregion

#region Dependencias

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddHttpClientSix<IRest, Rest>();
builder.Services.AddHttpClientSix<IRestInstalador, RestInstalador>(env.SixWebApiIdentity);
builder.Services.AddHttpClientSix<IRestNotificacao, RestNotificacao>(env.WebApiNotificacao);

builder.Services.AddSingleton<IInstaladorApi, InstaladorApi>();
builder.Services.AddSingleton<IInstaladorModelAdapter, InstaladorModelAdapter>();
builder.Services.AddSingleton<IMocker, Mocker>();
builder.Services.AddSingleton<IHomeMockers, HomeMockers>();
builder.Services.AddSingleton<INotificacaoMocker, NotificacaoMocker>();

#region Registro dos componentes 
//builder.Services.AddComponentCustom<IExemploService, ExemploService<IRest>, ExemploControllers>(); //Remover e colocar o seu componente
#endregion

builder.Services.AddSingleton<IHomeService, HomeService>();
builder.Services.AddSingleton<INotificacaoService, NotificacaoService>();
#endregion

#region Notificações Asincronas SignalR

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
});

builder.Services.AddSingleton<NotificaHub>();

#endregion

builder.Services.AddCors();
builder.Services.AddRazorPages();
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ActionResultExceptionFilter>();
});
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapHub<NotificaHub>("/notificacaoHub");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ExampleViewDesktop}/{id?}");

app.Run();
