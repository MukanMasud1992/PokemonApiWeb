using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _datacontext;
        private readonly IMapper _mapper;
        public CountryRepository(DataContext dataContext, IMapper mapper)
        {
            _datacontext = dataContext;
            _mapper = mapper;

        }
        public bool CountryExists(int id)
        {
            return _datacontext.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _datacontext.Countries.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _datacontext.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _datacontext.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _datacontext.Countries.Where(c => c.Id==id).FirstOrDefault();
        }

        public Country GetGountryByOwner(int ownerId)
        {
            return _datacontext.Owners.Where(o => o.Id==ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return _datacontext.Owners.Where(c => c.Country.Id==countryId).ToList();
        }

        public bool Save()
        {
            var saveChange = _datacontext.SaveChanges();
            return saveChange>0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _datacontext.Update(country);
            return Save();
        }
    }
}
