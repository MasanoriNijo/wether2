using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Text;
using WeatherApli.Module;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace WeatherApli
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Dictionary<string, List<City>> _city = new Dictionary<string, List<City>>();

        private WeatherData weatherData = new WeatherData();
        private ObservableCollection<WeatherSummary> Summary = new ObservableCollection<WeatherSummary>();
        private const string WEATHER_URL = "http://weather.livedoor.com/forecast/webservice/json/v1?city=";


        public MainPage()
        {
            this.InitializeComponent();
            ReadJson();
        }

        private async void ReadJson()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Prefecture.json"));
            string json = await FileIO.ReadTextAsync(file);

            List<Prefecture> pref;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(List<Prefecture>));
                pref = serializer.ReadObject(stream) as List<Prefecture>;
            }

            foreach(var item in pref)
            {
                _city.Add(item.Name, item.Cities);
            }

            this.cmbPrefecture.ItemsSource = pref;
            this.cmbPrefecture.DisplayMemberPath = "Name";
            this.cmbPrefecture.SelectedValuePath = "Name";
            this.cmbPrefecture.SelectedIndex = 0;

        }

        private async void ReadWeatherData(string id)
        {
            var hc = new Windows.Web.Http.HttpClient();
            string json = await hc.GetStringAsync(new Uri(WEATHER_URL + id));
            WeatherData weather;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(WeatherData));
                weather = serializer.ReadObject(stream) as WeatherData;
            }
            Summary.Clear();

            foreach(var fst in weather.forecasts)
            {
                WeatherSummary sumry = new WeatherSummary()
                {
                    DataLabel = fst.dateLabel,
                    Telop = fst.telop,
                    Date = fst.date,
                    Url = fst.image.url
                };
                Summary.Add(sumry);

            }




        }


        private void CmbPrefecture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var item = ((ComboBox)sender).SelectedValue.ToString();
            cmbCity.ItemsSource = _city[item];
            cmbCity.DisplayMemberPath = "Name";
            cmbCity.SelectedValuePath = "Id";

        }

        private void CmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = ((ComboBox)sender).SelectedValue?.ToString();
            if (id == null)
            {
                return;
            }
            ReadWeatherData(id);


        }
    }
}
