using SixConsult.Net.Foundation.Configuration.Contracts;
using TesteWebCanToWebAPI.Mockers.Home.Interface;
using TesteWebCanToWebAPI.Models;
using TesteWebCanToWebAPI.Services.IServices;

namespace TesteWebCanToWebAPI.Services
{
    public class HomeService : IHomeService
    {
        private readonly bool _useMocker;
        private readonly IHomeMockers _homeMockers;

        public HomeService(EnvironmentConfigurationSix env, IHomeMockers homeMockers)
        {
            _useMocker = env.UseMocker;
            _homeMockers = homeMockers;
        }

        public ExampleModel GetExample()
        {
            if (_useMocker) return _homeMockers.ExampleMocker();

            return new ExampleModel
            {
                Id = 1,
                ReadMe = "Você não está utilizando o Mocker. Este é so um template, remova do seu projeto tudo o que não for usar."
            };
        }
    }
}
