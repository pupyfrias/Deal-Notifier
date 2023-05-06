using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebScraping.Core.Application.Models
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            Uri baseAddress = new Uri("https://api.ebay.com/buy/browse/v1/item_summary/search");
            string queryParameter = "?q=phone&filter=price:[20..120],priceCurrency:USD,conditionIds:{1000},itemLocationCountry:US&sort=price&limit=200&aspect_filter=categoryId:9355,Operating System:{Android|iOS|Not Specified},Storage Capacity:{512 GB|256 GB|64 GB|32 GB|128 GB|Not Specified}";
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseAddress;

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "v^1.1#i^1#r^0#p^1#f^0#I^3#t^H4sIAAAAAAAAAOVYbWwURRi+a6/FBkFAgpUQrYsoArs3u3ufS+/Su17LHdIPuLbQIuB+zLXr7Rc7c7aFoqWBakhFEz9Q8QeJFST9gbH6g6AiBI2C/QGG+McIJIqImmhiINEE3L2Wcq0EkF5iE+/PZd55553neeZ9Z2YHdBeXLOqN916e5pxSsKcbdBc4nfRUUFJctHh6YcHcIgfIcXDu6X6429VTeKEc8apicKsgMnQNwbIOVdEQlzWGiIypcTqPZMRpvAoRh0UuGalZwTEU4AxTx7qoK0RZIhYiIO3loc8reHjaJ4gBr2XVrsVs0EOE38f4PaIQ9EGJYRg/sPoRysCEhjCv4RDBAIYlAUMyngbaw7EsB7xUgGZaiLImaCJZ1ywXChDhLFwuO9bMwXpzqDxC0MRWECKciFQn6yKJWFVtQ7k7J1Z4RIck5nEGjW1V6hIsa+KVDLz5NCjrzSUzoggRItzh4RnGBuUi18DcAfys1HQwAFivxycFWCblZ6W8SFmtmyqPb47DtsgSmcq6clDDMu68laKWGsJTUMQjrVorRCJWZv+tzPCKnJKhGSKqopHmSH09Ea42eU2UkUiKbZA3IMJk/aoYCWFACtA+miGhIPkBA1MjEw1HG5F53EyVuibJtmiorFbHUWihhuO1YXK0sZzqtDozksI2olw/3zUNQaDFXtThVczgNs1eV6haQpRlm7degdHRGJuykMFwNML4jqxEIYI3DFkixndmc3EkfTpQiGjD2ODc7vb2dqqdpXSz1c0AQLvX1KxIWkKqPGH52rU+7C/fegApZ6mI0BqJZA53GhaWDitXLQBaKxH2+AIemh3RfSys8HjrPww5nN1jKyJfFcIKAi96AcsDNuinmbxsNuGRJHXbOKDAd5Iqb6YhNhRehKRo5VlGhaYscaw3xbCBFCQlXzBFeoKpFCl4JR9JpyAEEAqCGAz8nwrldlM9CUUT4rzket7ynEnXMGrVJjW5xiMmgs2PS5hfycbb03GlWg3WNzY0NapsJ0zDZUo6dLvVcEPylYpsKdNgzZ8PAexaz58IcR1hKE2IXlLUDVivK7LYObkWmDWlet7EnUmoKJZhQiQjhpHIz16dN3r/cpu4M975O6P+o/PphqyQnbKTi5U9HlkBeEOm7BOIEnXVbde6zlvXD9u8IYt6Qrxl6+Y6qVhbJIfZytLwlZPK0qXQ0yJlQqRnTOu2TdXZN7AGPQ016zzDpq4o0GyiJ1zPqprBvKDAyVbYeUhwmZ9khy3t8/sZT5AOeCfES8wepRsm25aUj63YtewOr9XusR/5YUf2R/c4j4Ie5ycFTicoBwvo+eCh4sJGV+Hdc5GMISXzKQrJrZr17WpCKg07DV42C+51XAY/7hZ/ie/fkb7SvvH80i2O3DeGPetA6egrQ0khPTXnyQHMu95TRN9z3zSGBQzjoT0sC7wtYP71Xhc9xzX7gWd+vzRUW3Oo/zPHFO/6qsHPj/TNBNNGnZzOIoerx+noKL6/qOv93oXxI+bCTYfPvFb3bZPryuaXF1/+ga2cU/HoF4Uz1I+2TN9JnXrj9Kul+/oOP3I1JgkHN8SWDq4+2rt+75VoPMwIM2d9+MHJ6u1bTxz/beWBnzfvNQa2MssXDJxv7mueffbNY8/v/6quovtd/dDGZ6PHtE9LKrc1Rp976/TrJ1c7/3xHaXV8/T3TdTHc5uwvfbF5MDP3wonoknPT/0hTK+46t+PLIxUzqEulS8+wZv/Amsy8x154aWhoaNXVv3bue++Jb8TqrnX7YrOqDx7f8faBra/0l/c0dnTNwx/3tER/pc/+dLYv892U5V5y+fZdpyriT67dVXvo4oyBbbWDF5fsXv/goibdtXZ4Lf8GA/seof0RAAA=");
            var response = httpClient.GetAsync(queryParameter).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<eBayResponse>();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            httpClient.Dispose();
        }
    }
}