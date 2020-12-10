using Pokemon.ApiContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokemon.Api
{
    public interface IPokemonApiProvider
    {
        Task<List<PokemonSingleResult>> GetPokemonByType(string type);
        Task<PokemonSingleResult> GetPokemonByName(string name);
    }
}
