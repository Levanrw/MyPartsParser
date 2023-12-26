using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPartsParser.Model.Response
{
    public class GetProductDetailReponse
    { 
    public GetProductDetailReponseObject data { get; set; }
    }
    public class GetProductDetailReponseObject
    {
        public string? product_user { get; set; }
    }
}
