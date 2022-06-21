using mimic_api_2.Helpers;
using mimic_api_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mimic_api_2.Repositories.Contracts
{
   public interface IPalavraRepository
    {

       List<Palavra> ObterPalavras( PalavraUrlQuery query);

        Palavra Obter(int id);
        void Cadastrar(Palavra palavra);
        void Atualizar( Palavra palavra);

        void Deletar(int id);

    }
}
