namespace MyPartsParser.Model
{
    public class ProductType
    {
        public ProductType(int type, string description)
        {
            Type = type;
            Description = description;
        }

        public int Type { get; set; }
        public string Description { get; set; }
    }
}
