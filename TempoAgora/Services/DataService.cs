using Newtonsoft.Json.Linq;
using TempoAgora.Models;

namespace TempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            try
            {
                Tempo? t = null;

                string chave = "ebda6f022e30cdd44dfcf37d7b267f09";
                string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                             $"q={cidade}&units=metric&appid={chave}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await App.Current.MainPage.DisplayAlert("Erro", "Cidade não encontrada. Verifique o nome e tente novamente.", "OK");
                        return null;
                    }

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();

                        var rascunho = JObject.Parse(json);

                        //DateTime time = new();
                        DateTime sunrise = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunrise"]).ToLocalTime().DateTime;
                        DateTime sunset = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunset"]).ToLocalTime().DateTime;

                        t = new()
                        {
                            lat = (double)rascunho["coord"]["lat"],
                            lon = (double)rascunho["coord"]["lon"],
                            description = (string)rascunho["weather"][0]["description"],
                            main = (string)rascunho["weather"][0]["main"],
                            temp_min = (double)rascunho["main"]["temp_min"],
                            temp_max = (double)rascunho["main"]["temp_max"],
                            speed = (double)rascunho["wind"]["speed"],
                            visibility = (int)rascunho["visibility"],
                            sunrise = sunrise.ToString(),
                            sunset = sunset.ToString()
                        }; // Fecha objeto Tempo
                    } // Fecha if
                } // Fecha using

                return t;
            }
            catch (HttpRequestException)
            {
                await App.Current.MainPage.DisplayAlert("Sem conexão", "Verifique sua conexão com a internet e tente novamente.", "OK");
                return null;
            }
        }
    }
}
