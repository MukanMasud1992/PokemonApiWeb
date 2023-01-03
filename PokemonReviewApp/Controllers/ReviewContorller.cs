﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewContorller:Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        public ReviewContorller(
            IReviewerRepository reviewerRepository,
            IPokemonRepository pokemonRepository,
            IReviewRepository reviewRepository, 
            IMapper mapper)
        {
            this._reviewRepository = reviewRepository;
            this._mapper= mapper;
            this._pokemonRepository = pokemonRepository;
            this._reviewerRepository = reviewerRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpGet("reviewId")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]

        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviews = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            if (!_reviewRepository.ReviewExists(pokeId))
            {
                return NotFound();
            }
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId,[FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate==null)
            {
                return BadRequest(ModelState);
            }
            var reviews = _reviewRepository.GetReviews().Where(r => r.Title.Trim().ToUpper()==reviewCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();
            if (reviews!=null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Pokemon=_pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer=_reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int reviewId, [FromBody] ReviewDto updateReview)
        {
            if (updateReview==null)
            {
                return BadRequest(ModelState);
            }
            if (reviewId!=updateReview.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewMap = _mapper.Map<Review>(updateReview);
            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something goes wrong when safe entity");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview (int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }
            return NoContent();
        }
    }
}