// See https://aka.ms/new-console-template for more information
using MyPartsParser.Domain;
using MyPartsParser.Model;
using MyPartsParser.Model.Request;
using MyPartsParser.Model.Response;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

internal class Program
{
    static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }


    public static async Task MainAsync(string[] args)
    {
        using (var client = new HttpClient())
        {
            List<ProductType> prooductList = new List<ProductType>();
            prooductList.AddRange(new List<ProductType>
            {
            new ProductType(1,"ავტონაწილები"),
            new ProductType(4,"დაშლილი ავტომობილები")
            });
            for (int i = 0; i < prooductList.Count; i++)
            {
                var partsListRequest = new PartsListRequest(1, prooductList[i].Type, 1000);
                string requestJson = JsonConvert.SerializeObject(partsListRequest);
                StringContent httpContent = new StringContent(requestJson, System.Text.Encoding.Latin1, "application/json");
                HttpResponseMessage partsListMessage = await client.PostAsync($"https://api2.myparts.ge/api/ka/products/get", httpContent);
                var _partListResponse = partsListMessage.Content.ReadAsStringAsync().Result;
                var searchresponse = JsonSerializer.Deserialize<MyPartsResponse>(_partListResponse, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                var task = SaveData(searchresponse.data.pagination.totalPages, prooductList[i]);
                task.Wait();
            }

        }
    }
    public async static Task SaveData(int pageLength, ProductType product)
    {
        var db = new MyPartsContext();
        using (var client = new HttpClient())
        {
            for (int i = 1; i < pageLength; i++)
            {
                Console.WriteLine($"Total Page  -  {pageLength}");
                try
                {
                    var partsListRequest = new PartsListRequest(i, product.Type, 1000);
                    string requestJson = JsonConvert.SerializeObject(partsListRequest);
                    StringContent httpContent = new StringContent(requestJson, System.Text.Encoding.ASCII, "application/json");
                    HttpResponseMessage partsListMessage = await client.PostAsync($"https://api2.myparts.ge/api/ka/products/get", httpContent);
                    var _partListResponse = await partsListMessage.Content.ReadAsStringAsync();

                    var productSearchresponse = JsonSerializer.Deserialize<MyPartsResponse>(_partListResponse);
                    int index = 0;
                    foreach (var item in productSearchresponse.data.products)
                    { index++;
                        HttpResponseMessage showPhoneNumberResponse = await client.GetAsync($"https://api2.myparts.ge/api/ka/products/1/{item.product_id}/showFullNumber");
                        var showPhoneNumberResponseBody = await showPhoneNumberResponse.Content.ReadAsStringAsync();
                        var _phoneNumberResponse = JsonSerializer.Deserialize<ShowPhoneNumberResponse>(showPhoneNumberResponseBody, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                        //HttpResponseMessage getProductDetail = await client.GetAsync($"https://api2.myparts.ge/api/ka/products/detail/{item.product_id}");
                        //var getProductDetailResponseBody = await getProductDetail.Content.ReadAsStringAsync();
                        //var _getProductResponse = JsonSerializer.Deserialize<GetProductDetailReponse>(getProductDetailResponseBody, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                        var user = new MyPartsUser
                        {
                            Phone = _phoneNumberResponse == null ? null : _phoneNumberResponse.PhoneNumber,
                            UserName =item.product_user,//  _getProductResponse.data.product_user,
                            Address = item.details == null ? null : item.details.Address,//_getProductResponse.data.details.Address,
                            ProductType = product.Description,
                            City = item.loc_id == null ? null : item.loc_id.ToString(),
                        };
                        db.MyPartsUsers.Add(user);
                        Console.WriteLine($"1000 / {index}");
                       // Console.ReadLine();
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
    }
}