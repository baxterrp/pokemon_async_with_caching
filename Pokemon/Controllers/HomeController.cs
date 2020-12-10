using Microsoft.AspNetCore.Mvc;
using Pokemon.Api;
using System.Threading.Tasks;

namespace Pokemon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPokemonApiProvider _pokemonApiProvider;

        public HomeController(IPokemonApiProvider pokemonApiProvider)
        {
            _pokemonApiProvider = pokemonApiProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchByType(string type)
        {
            var pokemon = await _pokemonApiProvider.GetPokemonByType(type);

            return View("Results", pokemon);
        }
    }
}
