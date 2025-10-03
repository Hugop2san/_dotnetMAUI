using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using kogui.Models;
using System.Reflection.Metadata.Ecma335; 

namespace kogui.Services
{
    public class CorService //REQUISITO 2
    {

        private readonly HttpClient _http; 
        public CorService(HttpClient http) => _http = http;


        public async Task<string> GetColorNameFromHexAsync(string hex)
        {

            if (string.IsNullOrWhiteSpace(hex))
                return null;

            var clean = hex.Trim().TrimStart('#');          
            var url = $"https://www.thecolorapi.com/id?hex={clean}"; //api so aceit sem o #   

            try
            {
                var res = await _http.GetAsync(url);

                if (!res.IsSuccessStatusCode) // tratamento na busca da requisição...
                    return null;

                var json = await res.Content.ReadAsStringAsync(); //meu json agr é uma string

                using var doc = JsonDocument.Parse(json);// obj com nome e vlor
                
                //verifico se há propriedade name if=>true -> retorna valores de 'value' em -> val
                if( doc.RootElement.TryGetProperty("name", out var nameElem) && nameElem.TryGetProperty("value", out var val)) 
                {
                    return val.GetString(); //saida "cor"
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de conexão: {ex.Message}");
                return null;
            }




            return null;
        }

    }
}
