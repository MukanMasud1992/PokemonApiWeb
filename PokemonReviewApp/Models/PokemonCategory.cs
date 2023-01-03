namespace PokemonReviewApp.Models
{
    public class PokemonCategory//отношения многие ко многим many to many
    {
        public int PokemonId { get; set; }
        public int CategoryId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Category Category { get; set; }
    }
}
