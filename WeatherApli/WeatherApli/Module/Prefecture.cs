using System.Collections.Generic;
using System.Runtime.Serialization;


namespace WeatherApli.Module
{
    //都道府県管理クラス
    [DataContract]
    class Prefecture
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<City> Cities { get; set; }
    }
    [DataContract]
    public class City
    {
        [DataMember]
        public string Id{get;set;}
        [DataMember]
        public string Name{ get;set;}

    }

}
