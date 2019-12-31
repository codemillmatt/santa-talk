using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{

   
    public class CognitiveServiceModel
    {

        //Latitude & Longitude

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string ImagePath { get; set; }

        public string Name
        {
            get
            {
                return "Category - "+categories[0].name;
            }
        }
        public string Score
        {
            get
            {
                if (categories[0].score <= .3)
                {
                    return "Naught";
                }
                else
                    return "Nice";
            }
        }


        [JsonProperty]
      
        public List<Category> categories { get; set; }
        [JsonProperty]
       
        public Color color { get; set; }
        [JsonProperty]
       
        public Description description { get; set; }
        [JsonProperty]

      
        public string requestId { get; set; }
        [JsonProperty]
      
        public Metadata metadata { get; set; }






    }
  
    public class Category
    {
       
        [JsonProperty]
      
        public string name { get; set; }
        [JsonProperty]
       
        public double score { get; set; }
        [JsonProperty]
      
        public Detail detail { get; set; }
    }
    [Table("Detail_TB")]
    public class Detail
    {
       
       
        [JsonProperty]
        public List<object> celebrities { get; set; }
    }

    
    [Table("Color_TB")]
    public class Color
    {
      
        [JsonProperty]
       
        public string dominantColorForeground { get; set; }
        [JsonProperty]
      
        public string dominantColorBackground { get; set; }
        [JsonProperty]
       
        public List<string> dominantColors { get; set; }
        [JsonProperty]
        
        public string accentColor { get; set; }
        [JsonProperty]
     
        public bool isBwImg { get; set; }

    }

    [Table("Description_TB")]
    public class Description
    {
        [PrimaryKey, AutoIncrement]
        [Column("Description_ID")]
        public int Description_ID { get; set; }
        [Column("Vision_ID")]
        [ForeignKey(typeof(CognitiveServiceModel))]
        public int Vision_ID { get; set; }
        [JsonProperty]
        [Column("tags")]
        public List<string> tags { get; set; }
        [JsonProperty]
        [Column("captions")]
        public List<object> captions { get; set; }
    }
    [Table("MetaData_TB")]
    public class Metadata
    {
        [PrimaryKey, AutoIncrement]
        [Column("MetaData_ID")]
        public int MetaData_ID { get; set; }
        [ForeignKey(typeof(CognitiveServiceModel))]
        [Column("Vision_ID")]
        public int Vision_ID { get; set; }
        [JsonProperty]
        [Column("width")]
        public int width { get; set; }
        [JsonProperty]
        [Column("height")]
        public int height { get; set; }
        [JsonProperty]
        [Column("format")]
        public string format { get; set; }
    }



}
