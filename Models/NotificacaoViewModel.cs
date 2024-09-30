namespace TesteWebCanToWebAPI.Models
{
    public class NotificacaoViewModel
    {
        public Guid? Id { get; set; }
        public string Usuario { get; set; }
        public string Mensagem { get; set; }
        public bool? MsgLida { get; set; }
        public string Tipo { get; set; }
        public string Hora { get; set; }
    }
}
