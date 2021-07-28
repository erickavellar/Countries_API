using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    /// <summary>
    /// É uma classe que vai disponibilizar a ligação a internet
    /// </summary>
    public class NetworkService
    {
        //É um método que vai checar se tem ligação com a internet ou não. É um script de codigo definido.
        public Response CheckConnection()
        {
            var client = new WebClient();//é uma variavel que vai testar se tem ligação a internet

            try
            {
                using (client.OpenRead("http://clients3.google.com/generate_204"))//abre o link do google e me retorna um ping
                {
                    return new Response//se correr bem, me retorna uma nova resposta
                    {
                        IsSuccess = true,//aqui o que vou dizer que a minha propriedade é igual a true.O Is Success é da classe response que criei como parametro de resposta "bool".
                    };
                }
            }
            catch
            {

                return new Response //caso contrário, tambem me traz uma resposta
                {
                    IsSuccess = false,// que no caso se não pingou com a internet me traz false
                    Message = "Configure a sua ligação à Internet",//aqui informa uma mensagem tambem da classe response -> messagem
                };
            }
        }
    }
}
