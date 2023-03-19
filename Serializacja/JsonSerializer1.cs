using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;


namespace Serializacja
{
    internal class JsonSerializer1
    {
        // https://api.nbp.pl/api/exchangerates/tables/A/?format=json
        class Rates
        {
            [JsonProperty("currency")]
            public string CurrencyName { get; set;}

            [JsonProperty("code")]
            public string CurrencyCode { get; set; }

            [JsonProperty("mid")]
            public double AvarageRate { get; set; }
        }

        public static void NBP()
        {
            WebClient wb = new WebClient();
            string s = wb.DownloadString("https://api.nbp.pl/api/exchangerates/tables/A/?format=json");
            JArray ja =  JArray.Parse(s);
            IList<JToken>  results = ja[0]["rates"].Children().ToList();

            List<Rates> rates = new List<Rates>();

            foreach (JToken token in results) 
            {
               Rates rate =  token.ToObject<Rates>();
                rates.Add(rate);
            }
        }

        class MyUser
        {
            public string FName { get; set; }
            public string LName { get; set; }
            
        }
            public static void ApplyJson()
            {
            //string s = @"
            //    'fname' : 'jan' ,
            //    'lname' : 'kowalewski' ,
            //    'manager' : false
            //    ";


            string s = "{ \"fname\" : \"Jan\", 'lname': \"Kowalewski\", \"manager\" : false}";
     
            MyUser user = JsonConvert.DeserializeObject<MyUser>(s, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            Console.WriteLine(  $"{user.FName} {user.LName}");
        }
        public static void Create()
        {
            EmployeeJson emp1 = new EmployeeJson()
            {
                Id = 123, FirstName = "Jan", 
                LastName ="Kowalski",
                //AccessRooms = new List<int>() { 2, 3, 4},
                IsManager = false,
                StartAt = new DateTime(2022, 1, 1)
            };

            EmployeeJson emp2 = new EmployeeJson()
            {
                Id = 123,
                FirstName = "Zenon",
                LastName = "Nowak",
                //AccessRooms = new List<int>() { 2, 3, 4 },
                IsManager = false,
                StartAt = new DateTime(2022, 5, 1)
            };
            EmployeeJson emp3 = new EmployeeJson()
            {
                Id = 123,
                FirstName = "Janek",
                LastName = "Kowal",
                //AccessRooms = new List<int>() { 2, 3, 4 },
                IsManager = false,
                StartAt = new DateTime(2023, 1, 1)
            };
            //emp.SetToken(Guid.NewGuid().ToString());

            EmployeeJson[] ampArray = new EmployeeJson[]
                { 
                    emp1, emp2, emp3
                };


            // serializacja
            using (FileStream fs = new FileStream("json1.json",FileMode.Create))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(EmployeeJson[]));
                serializer.WriteObject(fs, ampArray);
            }

            //decentralizacja
            using (FileStream fs = new FileStream("json1.json", FileMode.Open))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(EmployeeJson[]));
                
      
                EmployeeJson[] empsDeserial = serializer.ReadObject(fs) as EmployeeJson[];

                if (empsDeserial != null)
                {
                    Console.WriteLine(  empsDeserial.Length);
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            string s =  js.Serialize(ampArray);
            File.WriteAllText("json2.json", s);

               EmployeeJson[] emps2 = js.Deserialize<EmployeeJson[]>(s);
            Console.WriteLine(emps2.Length);


            // serializacja newtonsoft json

           s = JsonConvert.SerializeObject(ampArray, Formatting.Indented, new JsonSerializerSettings
           {
               NullValueHandling = NullValueHandling.Ignore
           });
            File.WriteAllText("json3.json", s);

            emps2 = JsonConvert.DeserializeObject<EmployeeJson[]>(s);
            Console.WriteLine(emps2.Length);

        }
    }
}
