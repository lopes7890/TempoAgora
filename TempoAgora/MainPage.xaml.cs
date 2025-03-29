using System.Threading.Tasks;
using TempoAgora.Models;
using TempoAgora.Services;

namespace TempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                lbl_res.Text = "";

                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latidude: {t.lat} \n" +
                            $"Longitude: {t.lon} \n" +
                            $"Nascer do sol: {t.sunrise} \n" +
                            $"Por do sol: {t.sunset} \n" +
                            $"Temp Máx: {t.temp_max} \n" +
                            $"Temp min: {t.temp_min} \n" +
                            $"Velocidade do vento: {t.speed} \n" +
                            $"Descrição: {t.description} \n" +
                            $"Visibilidade: {t.visibility} \n";

                        lbl_res.Text = dados_previsao;

                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de previsão";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade";
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "ok");
            }
        }
    }

}
