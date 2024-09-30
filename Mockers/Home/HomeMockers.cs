using SixConsult.NET.Foundation.Mocker;
using TesteWebCanToWebAPI.Mockers.Home.Interface;
using TesteWebCanToWebAPI.Models;

namespace TesteWebCanToWebAPI.Mockers.Home
{
    public class HomeMockers : IHomeMockers
    {
        private readonly IMocker _mocker;

        public HomeMockers(IMocker mocker)
        {
            _mocker = mocker;
        }

        public ExampleModel ExampleMocker()
        {
            string readMe = "Este é so um template, remova do seu projeto tudo o que não for usar.";

            var example = _mocker.GetFaker<ExampleModel>()
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.ReadMe, readMe)
                .Generate();

            return example;
        }
    }
}
