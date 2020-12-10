using Newtonsoft.Json;
using Pokemon.ApiContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokemon.Api
{
    public class PokemonApiProvider : IPokemonApiProvider
    {
        private readonly string _baseApiUrl = "https://pokeapi.co/api/v2/";
        private static DateTime _timeStamp = DateTime.Now;
        private static List<PokemonSingleResult> _cachedPokemon = new List<PokemonSingleResult>();
        private static string _lastSearch = string.Empty;
        public async Task<PokemonSingleResult> GetPokemonByName(string name) =>
            await SendHttpGetRequest<PokemonSingleResult>("pokemon", name);

        public async Task<List<PokemonSingleResult>> GetPokemonByType(string type)
        {
            if(_lastSearch == type && _cachedPokemon.Any() && DateTime.Now - _timeStamp < TimeSpan.FromMinutes(10))
            {
                return _cachedPokemon;
            }

            var deserializedResult = await SendHttpGetRequest<PokemonTypeResult>("type", type);

            IEnumerable<Task<PokemonSingleResult>> tasks =
                deserializedResult.pokemon.Select(pokemon => GetPokemonByName(pokemon.pokemon.name));

            var results = (await Task.WhenAll(tasks)).Where(result => result != null).ToList();

            _cachedPokemon = results;
            _timeStamp = DateTime.Now;
            _lastSearch = type;

            return results;
        }

        private async Task<T> SendHttpGetRequest<T>(string url, string param)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseApiUrl}/{url}/{param}/");
                var httpResponse = await client.SendAsync(request);
                var stringResult = await httpResponse.Content.ReadAsStringAsync();
                var deserializedResult = JsonConvert.DeserializeObject<T>(stringResult);

                return deserializedResult;
            }
            catch
            {
                return default(T);
            }
        }
    }
}
