using Microsoft.AspNetCore.Mvc;
using mimic_api_2.Database;
using mimic_api_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mimic_api_2.Controllers
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {

        private readonly MimicContext _banco;

        public PalavrasController(MimicContext banco)
        {
            _banco = banco;
        }
        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas()
        {
            return Ok(_banco.Palavras);
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult ObterPalavras(int id)
        {
            return Ok(_banco.Palavras.Find(id));
        }

        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
            return Ok();
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id,[FromBody]Palavra palavra)
        {
            palavra.Id = id;
            _banco.Palavras.Update(palavra);

            _banco.SaveChanges();
            return Ok();
        }



        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            var palavra = _banco.Palavras.Find(id);
            palavra.Ativo = false;
            _banco.Palavras.Update(palavra);
           //deletar _banco.Remove(palavra);
            _banco.SaveChanges();
            return Ok();
        }
    }
}
