using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dateContext;
        public ReviewerRepository(DataContext dataContext)
        {
            this._dateContext=dataContext;
        }
        public Reviewer GetReviewer(int reviewerId)
        {
            return _dateContext.Reviewers.Where(r => r.Id==reviewerId).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _dateContext.Reviewers.ToList();
        }

        public bool GetReviewerExists(int reviewerId)
        {
            return _dateContext.Reviewers.Any(r => r.Id==reviewerId);
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _dateContext.Reviews.Where(r => r.Reviewer.Id==reviewerId).ToList();
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _dateContext.Reviewers.Add(reviewer);
            return Save();
        }

        public bool Save()
        {
            var saveChange = _dateContext.SaveChanges();
            return saveChange>0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _dateContext.Update(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _dateContext.Remove(reviewer);
            return Save();
        }
    }
}
