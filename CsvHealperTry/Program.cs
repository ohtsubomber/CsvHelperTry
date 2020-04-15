using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvHealperTry
{
    class Program
    {
        static void Main(string[] args)
        {
            var C = new C()
            {
                A = new A()
                {
                    Item = "item"
                },
                B = new B()
                {
                    Item = 0
                }
            };

            var csv = string.Empty;
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    using (var csvWriter = new CsvWriter(sw, CultureInfo.InvariantCulture))
                    {
                        csvWriter.Configuration.RegisterClassMap<CMap>();
                        csvWriter.WriteRecords(new List<C>() { C });
                    }

                }
                csv = Encoding.UTF8.GetString(ms.ToArray());
            }
            Console.WriteLine(csv);
            Console.WriteLine();
            using(var ms = new MemoryStream(Encoding.UTF8.GetBytes(csv)))
            {
                using (var sr = new StreamReader(ms))
                {
                    using (var csvReader = new CsvReader(sr, CultureInfo.InvariantCulture))
                    {
                        csvReader.Configuration.RegisterClassMap<CMap>();
                        var records = csvReader.GetRecords<C>();
                        foreach (var record in records)
                        {
                            Console.WriteLine($"{record.A.Item} {record.B.Item}");
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }

    public class A
    {
        public string Item { get; set; }
    }

    public class B
    {
        public  int Item { get; set; }
    }

    public class C
    {
        public A A { get; set; }
        public B B { get; set; }
    }

    public class AMap : ClassMap<A>
    {
        public AMap()
        {
            Map(m => m.Item).Name("Hoge");
        }
    }

    public class BMap : ClassMap<B>
    {
        public BMap()
        {
            Map(m => m.Item).Name("Fuga");
        }
    }

    public class CMap : ClassMap<C>
    {
        public CMap()
        {
            References<AMap>(m => m.A);
            References<BMap>(m => m.B);
        }
    }

}
