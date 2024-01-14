using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyPartsParser.Model.Response
{
    public class GetProductDetailReponse
    { 
    public GetProductDetailReponseObject? data { get; set; }
    }
    public class GetProductDetailReponseObject
    {
        public string? product_user { get; set; }
        public ProductDetails? details { get; set; }
    }

    public class ProductDetails
    {
        [property: JsonPropertyName("address")]
        public string? Address { get; set; }
    }
}
