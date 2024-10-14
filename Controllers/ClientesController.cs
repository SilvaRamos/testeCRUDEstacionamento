using Cadastro_De_Clientes_Estacionamento.DTOs;
using Cadastro_De_Clientes_Estacionamento.Models;
using Locadora_api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cadastro_De_Clientes_Estacionamento.Controllers
{
    public class ClientesController : Controller
    {
        private readonly DataContext _db;

        public ClientesController(DataContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Cliente> clientes = _db.Clientes; 

            return View(clientes);
        }

        public IActionResult CarregaGridClientes()
        {
            IEnumerable<Cliente>? clientes = _db.Clientes.ToList();

            return PartialView("_GridClientes", clientes);
        }

        public IActionResult Detalhes(Guid? id)
        {
            ClienteDTO clienteDto = new ClienteDTO();

            if (id == null)
            {
                return null;
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //busca os dados do cliente
            clienteDto.cliente = _db.Clientes.FirstOrDefault(x => x.Id == id);

            ////busca os pagamentos do cliente
            //clienteDto.pagamentos = _db.Pagamentos.Where(x => x.ClienteId == id);

            //if (clienteDto.cliente == null)
            //{
            //    return null;
            //    //return HttpNotFound();
            //}

            return PartialView("_Detalhes", clienteDto);
        }

        public IActionResult CarregaPagamentos(Guid? id)
        {
            ClienteDTO clienteDto = new ClienteDTO();

            //busca os pagamentos do cliente
            clienteDto.pagamentos = _db.Pagamentos.Where(x => x.ClienteId == id);

            if (clienteDto.pagamentos == null)
            {
                return null;
                //return HttpNotFound();
            }

            return PartialView("_ListaDePagamentos",clienteDto);
        }

        public IActionResult Cadastrar()
        {
            return PartialView("_Cadastrar");
        }
               

        [HttpPost]
        public RetornoDTO Cadastrar(Cliente cliente)
        {
            RetornoDTO retornoValidacaoNovoCliente = this.validaCliente(cliente);

            if (ModelState.IsValid)
            {
                //Cadastro do Cliente permitido
                if ( retornoValidacaoNovoCliente.ResultadoValido )
                {
                    _db.Clientes.Add(cliente);
                    _db.SaveChanges();
                    retornoValidacaoNovoCliente.Mensagem = "Cliente cadatrado com sucesso";

                    //TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
                }
                //else
                //{
                     
                //    //TempData["MensagemSucesso"] = retornoValidacaoNovoCliente.Mensagem;
                //}

                //return RedirectToAction("Index");
            }
            else
            {
                retornoValidacaoNovoCliente.Mensagem = "Não foi possível cadastrar este cliente!";
                //TempData["MensagemErro"] = "Não foi possível cadastrar este cliente!";
            }

            return retornoValidacaoNovoCliente;
        }


        [HttpPost]
        public RetornoDTO PagarBoleto(Pagamento pagamento)
        {
            RetornoDTO retornoPagamentoBoleto = new RetornoDTO();

            if (ModelState.IsValid)
            {
                pagamento.Id = Guid.NewGuid();
                pagamento.dataPagamento = DateTime.UtcNow;

                _db.Pagamentos.Add(pagamento);
                _db.SaveChanges();

                retornoPagamentoBoleto.Mensagem = "Pagamento realizado com sucesso!";
            }
            else
            {
                retornoPagamentoBoleto.Mensagem = "Cadastro não realizado!";
            }

            return retornoPagamentoBoleto;
        }

        //[HttpPost]
        //public IActionResult PagarBoleto(Pagamento pagamento)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        pagamento.Id = Guid.NewGuid();
        //        pagamento.dataPagamento = DateTime.UtcNow;

        //        _db.Pagamentos.Add(pagamento);
        //        _db.SaveChanges();

        //        TempData["MensagemSucesso"] = "Pagamento realizado com sucesso!";

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["MensagemErro"] = "Cadastro não realizado!";
        //    }

        //    return PartialView("_Cadastrar");
        //}



        //Verifica se um cliente já existe no banco a partir do CPF ou CNPJ
        private RetornoDTO validaCliente(Cliente cliente)
        {
            RetornoDTO retorno =  new RetornoDTO() { ResultadoValido = true };
            Cliente? clienteLocalilzado = new Cliente();

            if (cliente.Tipo == 1)
            {
                clienteLocalilzado = _db.Clientes.FirstOrDefault( x => x.CPF == cliente.CPF);
                if(clienteLocalilzado  != null  &&  !String.IsNullOrEmpty( clienteLocalilzado.Id.ToString() ) )
                {
                    retorno.ResultadoValido = false;
                    retorno.Mensagem = "Já existe um usuário cadastrado com o CPF informado.";
                }
            }
            if(cliente.Tipo == 2)
            {
                clienteLocalilzado = _db.Clientes.FirstOrDefault(x => x.CNPJ == cliente.CNPJ);
                if (clienteLocalilzado != null && !String.IsNullOrEmpty(clienteLocalilzado.Id.ToString()))
                {
                    retorno.ResultadoValido = false;
                    retorno.Mensagem = "Já existe um usuário cadastrado com o CNPJ informado.";
                }
            }

            return retorno;
        }


        //// GET: ClientesController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ClientesController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ClientesController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ClientesController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ClientesController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ClientesController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
