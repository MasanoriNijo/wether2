using System.Collections.Generic;
using System.Runtime.Serialization;


namespace WeatherApli.Module
{
    //都道府県管理クラス
    [DataContract]
    class WeatherData
    {
        [DataMember]
        public List<Forecast> forecasts { get; set; }
    }
    [DataContract]
    public class Forecast
    {
        [DataMember]
        public string dateLabel{get;set;}
        [DataMember]
        public string telop{ get;set;}
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public FImage image { get; set; }
    }
    [DataContract]
    public class FImage
    {
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public int width { get; set; }
        [DataMember]
        public int height { get; set; }
    }

    public class WeatherSummary
    {
        public string DataLabel { get; set; }
        public string Telop { get; set; }
        public string Date { get; set; }
        public string Url { get; set; }
    }


}
