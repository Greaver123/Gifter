namespace Gifter.Services.DTOS.Wish
{
    public class UpdateWishDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? Price { get; set; }

        public string URL { get; set; }
    }
}
