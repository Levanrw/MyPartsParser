namespace MyPartsParser.Model.Response
{
    public class MyPartsResponse
    { 
        public MyPartsResponseObject data { get; set; }
    }
    public class MyPartsResponseObject 
    {
        public ProductList[] products { get; set; }
        public Pagination pagination { get; set; }

    }
    public class ProductList
    {
        public int product_id { get; set; }
        public int order_item_id { get; set; }
        public string? cat_name { get; set; }
        public string? comment { get; set; }
        public string? phone { get; set; }
        public int? loc_id { get; set; }
    }


    public class Pagination
    {
        public int totalPages { get; set; }
    }
}
