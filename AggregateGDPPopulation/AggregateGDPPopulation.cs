using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AggregateGDPPopulation
{
    public class CountryDetails
    {
        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }
    }

    public class Class1
    {

        public async static Task<List<string[]>> CSVtoArray()
        {
            string filePath = "../../../../AggregateGDPPopulation/data/datafile.csv";
            StreamReader sr = new StreamReader(filePath);
            var lines = new List<string[]>();
            while (!sr.EndOfStream)
            {
                string Line = await sr.ReadLineAsync();
                string[] Line1 = Line.Replace("\"", "").Split(',');
                lines.Add(Line1);
            }

            return lines;

        }

        public async static Task<Dictionary<string, string>> CountryContinent()
        {
            string filePath = "../../../../AggregateGDPPopulation/data/country-continent.json";
            StreamReader sr = new StreamReader(filePath);
            string json = await sr.ReadToEndAsync();
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //Console.WriteLine(json);
            return values;
        }

        private static async Task WriteJsonAsync(string filePath, string text)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(text);
            }
        }

        public async static Task Aggregate()
        {

            Dictionary<string, CountryDetails> jsonObject = new Dictionary<string, CountryDetails>();
            Task< List<string[]>> task1 = CSVtoArray();
            Task<Dictionary<string, string>> task2 = CountryContinent();
            var lines = await task1;
            var values = await task2;
            string[] header = lines[0];
            var data = lines.ToArray();
            int indexOfPopulation = Array.IndexOf(header, "Population (Millions) 2012");
            int indexOfGDP = Array.IndexOf(header, "GDP Billions (USD) 2012");
            int indexOfCountries = Array.IndexOf(header, "Country Name");
            for (int i = 1; i < data.Length - 1; i++)
            {
                try
                {
                    jsonObject[values[data[i][indexOfCountries]]].GDP_2012 += float.Parse(data[i][indexOfGDP]);
                    jsonObject[values[data[i][indexOfCountries]]].POPULATION_2012 += float.Parse(data[i][indexOfPopulation]);
                }
                catch
                {
                    CountryDetails Object = new CountryDetails()
                    {
                        GDP_2012 = float.Parse(data[i][indexOfGDP]),
                        POPULATION_2012 = float.Parse(data[i][indexOfPopulation])
                    };
                    jsonObject.Add(values[data[i][indexOfCountries]], Object);
                }

            }
            string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            File.WriteAllText("../../../../AggregateGDPPopulation.Tests/output.json", json);

            await WriteJsonAsync("../../../../AggregateGDPPopulation.Tests/output.json", json);
            //Console.WriteLine(jsonObject);
        }

    }
}
