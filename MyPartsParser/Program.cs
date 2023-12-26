// See https://aka.ms/new-console-template for more information
using MyPartsParser.Domain;
using MyPartsParser.Model.Request;
using MyPartsParser.Model.Response;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

internal class Program
{
    static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }


    public static async Task MainAsync(string[] args)
    {
        var db = new MyPartsContext();
        using (var client = new HttpClient())
        {
            var partsListRequest = new PartsListRequest(1, 2, 15);
            string requestJson = JsonConvert.SerializeObject(partsListRequest);
            StringContent httpContent = new StringContent(requestJson, System.Text.Encoding.Latin1, "application/json");
            HttpResponseMessage partsListMessage = await client.PostAsync($"https://api2.myparts.ge/api/ka/products/get", httpContent);
            var _partListResponse = partsListMessage.Content.ReadAsStringAsync().Result;
            var searchresponse = JsonSerializer.Deserialize<MyPartsResponse>(_partListResponse, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });


            for (int i = 1; i < searchresponse.data.pagination.totalPages; i++)
            {
                partsListRequest = new PartsListRequest(i, 1, 15);
                requestJson = JsonConvert.SerializeObject(partsListRequest);
                httpContent = new StringContent(requestJson, System.Text.Encoding.ASCII, "application/json");
                partsListMessage = await client.PostAsync($"https://api2.myparts.ge/api/ka/products/get", httpContent);
                _partListResponse = await partsListMessage.Content.ReadAsStringAsync();
                searchresponse = JsonSerializer.Deserialize<MyPartsResponse>(_partListResponse);

                foreach (var item in searchresponse.data.products)
                {
                    HttpResponseMessage showPhoneNumberResponse = await client.GetAsync($"https://api2.myparts.ge/api/ka/products/1/{item.product_id}/showFullNumber");
                    var showPhoneNumberResponseBody = await showPhoneNumberResponse.Content.ReadAsStringAsync();
                    var _phoneNumberResponse = JsonSerializer.Deserialize<ShowPhoneNumberResponse>(showPhoneNumberResponseBody, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                    HttpResponseMessage getProductDetail = await client.GetAsync($"https://api2.myparts.ge/api/ka/products/detail/{item.product_id}");
                    var getProductDetailResponseBody = await getProductDetail.Content.ReadAsStringAsync();
                    var _getProductResponse = JsonSerializer.Deserialize<GetProductDetailReponse>(getProductDetailResponseBody, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    var user = new MyPartsUser
                    {
                        Phone = _phoneNumberResponse.PhoneNumber,
                        UserName = _getProductResponse.data.product_user
                    };
                    db.MyPartsUsers.Add(user);
                }
                db.SaveChanges();
            }
        }
    }
}