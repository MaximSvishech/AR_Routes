
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Diagnostics;

public class ArrowCreator : MonoBehaviour
{
    [SerializeField] GameObject line;

    #region models

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
        public int id { get; set; }
        public float Angle { get; set; }
        public int Meter { get; set; }
        public string Direction { get; set; }
        public string Additional { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public string name { get; set; }
        public Crs crs { get; set; }
        public List<Feature> features { get; set; }
    }



    #endregion


    // Start is called before the first frame update
    void Start()
    {

        Run("1","3");
        //CreatArrow(new Feature
        //{
        //    properties = new Properties
        //    {
        //        Angle = 188.31f,
        //        Meter = 5
        //    }
        //});
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Run(string sourse, string destination)
    {
        
        //TODO: need to connect to resours file
        var configFileName = $"{sourse}_To_{destination}.json";

        List<Feature> features = GetFromConfig(configFileName);

        if (features == null)
            return;
  
        foreach (var feature in features)
        {
            CreatArrow(feature);
        }
    }

    public void CreatArrow(Feature feature)
    {

        //rotate
        transform.rotation = Quaternion.Euler(0, feature.properties.Angle, 0);

        //Craete Cude
        GameObject tempLine = Instantiate(line, transform.position, transform.rotation);

        //change by meters
        tempLine.transform.localScale = new Vector3(0.2f, 0.5f, feature.properties.Meter);

        //change position
        transform.position += transform.forward * feature.properties.Meter;
    }

    private List<Feature> GetFromConfig(string fileName)
    {
        string jsonString = File.ReadAllText(fileName);
        Root root = JsonConvert.DeserializeObject<Root>(jsonString);
        return root.features;
    }
}
