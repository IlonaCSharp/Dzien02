using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Serializacja
{
    internal class BinarySerializer
    {
        public static void Create()
        { 
            Employee emp = new Employee()
            {
                Id = 123, FirstName = "Jan", LastName ="Kowalski",
                AccessRooms = new List<int>() { 2, 3, 4},
                IsManager = false,
                StartAt = new DateTime(2022, 1, 1)
            };
            emp.SetToken(Guid.NewGuid().ToString());

            // serializacja
            using (FileStream fs = new FileStream("dump.bin",FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, emp);
            }

            //decentralizacja
            using (FileStream fs = new FileStream("dump.bin", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Employee empDeserial =  bf.Deserialize(fs) as Employee;

                if(empDeserial != null)
                {
                    Console.WriteLine(  empDeserial);
                }
            }


        }
    }
}
