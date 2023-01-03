using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _datacontext;
        public OwnerRepository(DataContext dataContext)
        {
            _datacontext= dataContext;
        }

        public bool CreateOwner(Owner owner)
        {
            _datacontext.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _datacontext.Owners.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
           return _datacontext.Owners.Where(o => o.Id==ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _datacontext.PokemonOwners.Where(p => p.PokemonId==pokeId).Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _datacontext.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _datacontext.PokemonOwners.Where(p => p.OwnerId==ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExist(int ownerId)
        {
            return _datacontext.Owners.Any(o => o.Id==ownerId);
        }

        public bool Save()
        {
            var saveChanged = _datacontext.SaveChanges();
            return saveChanged>0?true:false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _datacontext.Owners.Update(owner);
            return Save();
        }
    }
}
