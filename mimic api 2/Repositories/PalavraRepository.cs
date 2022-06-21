using mimic_api_2.Database;
using mimic_api_2.Helpers;
using mimic_api_2.Models;
using mimic_api_2.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mimic_api_2.Repositories
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly MimicContext _banco;
        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }

        public void Atualizar(Palavra palavra)
        {
            _banco.Palavras.Update(palavra);

            _banco.SaveChanges();
        }

        public void Cadastrar(Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
        }

        public void Deletar(int id)
        {
          var palavra=  Obter(id);
            _banco.Palavras.Update(palavra);
            //deletar _banco.Remove(palavra);
            _banco.SaveChanges();
        }

        public Palavra Obter(int id)
        {
            return _banco.Palavras.Find(id);
        }

        public List<Palavra> ObterPalavras(PalavraUrlQuery query)
        {
            var item = _banco.Palavras.AsQueryable();
            if (query.Data.HasValue)
            {
                item = item.Where(a => a.Criado > query.Data.Value || a.Atualizado > query.Data.Value);
            }

            //paginação
            //api/palavras?pagNumero=3&pagRegistro=2
            if (query.PagNumero.HasValue)
            {
                var qtdTotalRegistro = item.Count();
                item = item.Skip((query.PagNumero.Value - 1) * query.PagRegistro.Value).Take(query.PagRegistro.Value);

                var paginacao = new Paginacao();
                paginacao.NumeroPagina = query.PagNumero.Value;
                paginacao.RegistroPorPagina = query.PagRegistro.Value;
                paginacao.TotalRegistros = qtdTotalRegistro;
                paginacao.TotalPaginas = (int)Math.Ceiling((double)qtdTotalRegistro / query.PagRegistro.Value);

            }
            return item.ToList();
        }
    }
}
