using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApiContrib.Formatting;
using System.Runtime.Serialization;
using System.Net;

namespace WebApiSpikeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the client fetch.Press any key");
            Console.ReadKey();

            string baseAddress = "http://localhost:9000/";


            Console.WriteLine("posting via webrequest protobuf");

            HttpWebRequestPostProtobuf(baseAddress);


            Console.WriteLine("posting via protobuf");

            HttpClientPostProtobuf(baseAddress);

            Console.WriteLine();

            HttpClientXML(baseAddress);
            Console.WriteLine();
            HttpClientJson(baseAddress);
            Console.WriteLine();
            HttpClientBson(baseAddress);
            Console.WriteLine();
            HttpClientProtobuf(baseAddress);


            Console.WriteLine();
            HttpWebRequestGetProtobuf(baseAddress);
          
            
            Console.WriteLine();
            HttpWebRequestGetProtobuf(baseAddress);
            
            Console.WriteLine();
            HttpClientProtobuf(baseAddress);



            Console.ReadKey();
        }

        private static void HttpClientXML(string baseAddress)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            var response = client.GetAsync(baseAddress + "api/products").Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static void HttpClientJson(string baseAddress)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(baseAddress + "api/products").Result;

            Console.WriteLine(response.Content.Headers.ContentLength);

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static void HttpClientBson(string baseAddress)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/bson"));
            
            Task<HttpResponseMessage> msg = client.GetAsync(baseAddress + "api/products");

            Console.WriteLine(msg.Result.Headers.ToString());
            Console.WriteLine(msg.Result.Content.Headers.ContentLength);
            Console.WriteLine(msg.Result.Content.Headers.ContentType);

            Task<Stream> response = msg.Result.Content.ReadAsStreamAsync();//client.GetStreamAsync(baseAddress + "api/products");

            Console.WriteLine();

            MemoryStream result = new MemoryStream();
            response.Result.CopyTo(result);
            result.Seek(0, SeekOrigin.Begin);
            using (BsonReader reader = new BsonReader(result) { ReadRootValueAsArray = true })
            {
                
                var jsonSerializer = new JsonSerializer();
                var output = jsonSerializer.Deserialize<IEnumerable<Product>>(reader);
                foreach (var item in output)
                {
                    Console.WriteLine(item.Id + "," + item.Name + "," + item.Price + "," + item.Category);
                }
            }


        }

        private static void HttpClientBson1(string baseAddress)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/bson"));

            Task<HttpResponseMessage> msg = client.GetAsync(baseAddress + "api/products");

            Console.WriteLine(msg.Result.Headers.ToString());
            Console.WriteLine(msg.Result.Content.Headers.ContentLength);
            Console.WriteLine(msg.Result.Content.Headers.ContentType);

            Task<Stream> response = msg.Result.Content.ReadAsStreamAsync();//client.GetStreamAsync(baseAddress + "api/products");

            Console.WriteLine();

            MemoryStream result = new MemoryStream();
            response.Result.CopyTo(result);
            result.Seek(0, SeekOrigin.Begin);
            using (BsonReader reader = new BsonReader(result) { ReadRootValueAsArray = true })
            {

                var jsonSerializer = new JsonSerializer();
                var output = jsonSerializer.Deserialize<IEnumerable<Product>>(reader);
                foreach (var item in output)
                {
                    Console.WriteLine(item.Id + "," + item.Name + "," + item.Price + "," + item.Category);
                }
            }


        }

        private static void HttpClientProtobuf(string baseAddress)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

            HttpResponseMessage response = client.GetAsync(baseAddress + "api/products").Result;
            Console.WriteLine(response.Headers.ToString());
            Console.WriteLine(response.Content.Headers.ContentLength);
            Console.WriteLine(response.Content.Headers.ContentType);
           
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
              
                

                var p = response.Content.ReadAsAsync<Product[]>(new[] { new ProtoBufFormatter() }).Result;
                ProtoBufFormatter.Model.UseImplicitZeroDefaults = true;
                //Console.WriteLine("{0}\t{1}\t{2}\t{3}", p.Id,p.Name,p.Category,p.Price);
                foreach (Product item in p)
                {
                    Console.WriteLine("{0}\t{1};\t{2}\t{3}", item.Name, item.Id, item.Price, item.Category);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
 
        }

        private static void HttpClientPostProtobuf(string baseAddress)
        {
            HttpClient client = new HttpClient();
            Product p = new Product { Id = 100, Price = 300, Name = "testpost", Category = "test" };

            var result= client.PostAsync<Product>(baseAddress + "api/products", p, new ProtoBufFormatter());
            Console.WriteLine(result.Result.StatusCode);
            

        }

        private static void HttpWebRequestGetProtobuf(string baseAddress)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseAddress + "api/products");
            request.ContentType = "application/x-protobuf";
            request.Accept = "application/x-protobuf";
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();

            var products= ProtoBuf.Serializer.Deserialize<Product[]>(response.GetResponseStream());

            Console.WriteLine(response.Headers.ToString());
            foreach (Product item in products)
            {
                Console.WriteLine("{0}\t{1};\t{2}\t{3}", item.Name, item.Id, item.Price, item.Category);
            }

        }

        private static void HttpWebRequestPostProtobuf(string baseAddress)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseAddress + "api/products");
            request.ContentType = "application/x-protobuf";
            request.Accept = "application/x-protobuf";
            request.Method = "POST";

            Product p = new Product { Id = 200, Price = 400, Name = "testpost2", Category = "test2" };
            ProtoBuf.Serializer.Serialize<Product>(request.GetRequestStream(), p);
            //request.ContentLength = request.GetRequestStream().Length;
            var response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.Headers.ToString());
            

       
            


        }



    }

    [DataContract]
    public class Product
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public string Category { get; set; }
        [DataMember(Order = 4)]
        public decimal Price { get; set; }
    }
}
