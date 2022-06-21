using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mimic_api_2.Database;
using mimic_api_2.Helpers;
using mimic_api_2.Models;
using Newtonsoft.Json;
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
        public ActionResult ObterTodas([FromQuery]PalavraUrlQuery query)
        {
            
            var item = _banco.Palavras.AsQueryable();
            if (query.Data.HasValue)
            {
                item = item.Where(a => a.Criado >query.Data.Value || a.Atualizado > query.Data.Value) ;
            }

            //paginação
            //api/palavras?pagNumero=3&pagRegistro=2
            if (query.PagNumero.HasValue)
            {
                var qtdTotalRegistro = item.Count();
                item = item.Skip((query.PagNumero.Value-1)* query.PagRegistro.Value).Take(query.PagRegistro.Value);

                var paginacao = new Paginacao();
                paginacao.NumeroPagina =query.PagNumero.Value;
                paginacao.RegistroPorPagina = query.PagRegistro.Value;
                paginacao.TotalRegistros=qtdTotalRegistro;
                paginacao.TotalPaginas=(int)Math.Ceiling((double) qtdTotalRegistro/query.PagRegistro.Value);

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginacao));

                if(query.PagNumero> paginacao.TotalPaginas)
                {
                    return NotFound();
                }
            }


            return Ok(item.ToList());
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult ObterPalavras(int id)
        {
            var retorno = _banco.Palavras.Find(id);

            if (retorno == null)
            {
                return NotFound();
            }
    
                return Ok(retorno);

        }

        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
            return Created($"/api/palavras/{palavra.Id}",palavra);
        }



        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id,[FromBody]Palavra palavra)
        {

            var retorno = _banco.Palavras.AsNoTracking().FirstOrDefault(a=>a.Id==id);

            if (retorno == null)
            {
                return NoContent();
            }

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

            if (palavra == null)
            {
                return NotFound();
            }

            palavra.Ativo = false;
            _banco.Palavras.Update(palavra);
           //deletar _banco.Remove(palavra);
            _banco.SaveChanges();
            return NoContent();
        }
    }
}
