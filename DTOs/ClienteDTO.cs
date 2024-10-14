using System.ComponentModel.DataAnnotations;
using Cadastro_De_Clientes_Estacionamento.Models;

namespace Cadastro_De_Clientes_Estacionamento.DTOs
{
    public class ClienteDTO
    {
        public Cliente? cliente { get; set; }
        public IEnumerable<Pagamento>? pagamentos { get; set; }

        //public ClienteDTO()
        //{
        //    this.cliente = new Cliente();
        //    this.pagamentos = new List<Pagamento>();
        //}
    }
}