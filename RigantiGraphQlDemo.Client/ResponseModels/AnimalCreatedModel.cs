namespace RigantiGraphQlDemo.Client.ResponseModels
{
    public class AnimalCreatedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int FarmId { get; set; }
    }
}