using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _dataContext;
        public PokemonRepository(DataContext dataContext) 
        {
            this._dataContext = dataContext;
        }
        public ICollection<Pokemon> GetPokemons()
        {
            return _dataContext.Pokemon.OrderBy(p => p.Id).ToList();
        }
        public Pokemon GetPokemon(int Id)
        {
            return _dataContext.Pokemon.Where(p => p.Id==Id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _dataContext.Pokemon.Where(p => p.Name==name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _dataContext.Reviews.Where(p => p.Pokemon.Id==pokeId);

            if (review.Count()<=0)
            {
                return 0;
            }
            return ((decimal)review.Sum(r => r.Rating)/review.Count());
        }
        public bool PokemonExists(int pokeId)
        {
            return _dataContext.Pokemon.Any(p => p.Id==pokeId);
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _dataContext.Owners.Where(a=>a.Id==ownerId).FirstOrDefault();
            var pokemonCategoryEntity = _dataContext.Categories.Where(a => a.Id==categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };

            _dataContext.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = pokemonCategoryEntity,
                Pokemon = pokemon
            };
            _dataContext.Add(pokemonCategory);

            _dataContext.Add(pokemon);

            return Save();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved>0?true:false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _dataContext.Update(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _dataContext.Remove(pokemon);
            return Save();
        }
    }
}
