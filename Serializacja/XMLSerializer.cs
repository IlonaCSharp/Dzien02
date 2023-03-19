using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Serializacja
{
    internal class XMLSerializer
    {
        public static void Create()
        {
            EmployeeXML emp1 = new EmployeeXML()
            {
                Id = 123, FirstName = "Jan", 
                LastName ="Kowalski",
                AccessRooms = new List<int>() { 2, 3, 4},
                IsManager = false,
                StartAt = new DateTime(2022, 1, 1)
            };

            EmployeeXML emp2 = new EmployeeXML()
            {
                Id = 123,
                FirstName = "Zenon",
                LastName = "Nowak",
                AccessRooms = new List<int>() { 2, 3, 4 },
                IsManager = false,
                StartAt = new DateTime(2022, 5, 1),
                ExtraData = new List<string >() {"jeden", "dwa", "hahaha"}
            };
            EmployeeXML emp3 = new EmployeeXML()
            {
                Id = 123,
                FirstName = "Janek",
                LastName = "Kowal",
                AccessRooms = new List<int>() { 2, 7, 4 },
                IsManager = false,
                StartAt = new DateTime(2023, 1, 1)
            };
            emp3.SetToken(Guid.NewGuid().ToString());

            EmployeeXML[] ampArray = new EmployeeXML[]
                { 
                    emp1, emp2, emp3
                };


            // serializacja
            using (FileStream fs = new FileStream("xml.xml",FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(EmployeeXML[]));

                xs.Serialize(fs, ampArray);
            }

            //decentralizacja
            using (FileStream fs = new FileStream("xml.xml", FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(EmployeeXML[]));
                EmployeeXML[] empsDeserial = ( EmployeeXML[])xs.Deserialize(fs);

                if(empsDeserial != null)
                {
                    Console.WriteLine(  empsDeserial.Length);
                }
            }


        }
    }
}
