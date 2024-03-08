using System.Diagnostics;

class Program
{
    static async Task Main()
    {
        int orderCount = 2;
        string apiBaseUrl = "https://localhost:44351/api/Gateway";
        string addBasketEndpoint = $"{apiBaseUrl}/add-basket";
        string sendOrderEndpoint = $"{apiBaseUrl}/send-order";

        var stopwatch = Stopwatch.StartNew();

        Parallel.For(0, orderCount, async (i) =>
        {
            await SendHttpRequest(HttpMethod.Post, addBasketEndpoint, "{ \"ProductName\": \"Product1\", \"Quantity\": 1, \"Price\": 10.0 }");

            await SendHttpRequest(HttpMethod.Post, sendOrderEndpoint, "{}");
        });

        stopwatch.Stop();

        string result = $"Toplam {orderCount * 2} isteğin işlenme süresi: {stopwatch.ElapsedMilliseconds} milisaniye";
        Console.WriteLine(result);
        System.IO.File.WriteAllText("load_test_result.txt", result);
    }

    static async Task SendHttpRequest(HttpMethod method, string url, string content)
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(method, url))
            {
                request.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

                using (var response = await httpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"HTTP request failed: {response.StatusCode}");
                    }
                }
            }
        }
    }
}