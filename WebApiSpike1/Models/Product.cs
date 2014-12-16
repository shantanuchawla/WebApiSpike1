using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApiSpike1.Models
{
    [DataContract]
    public class Product
    {
        [DataMember(Order=1)]
        public int Id { get; set; }
        [DataMember(Order=2)]
        public string Name { get; set; }
        [DataMember(Order=3)]
        public string Category { get; set; }
        [DataMember(Order=4)]
        public decimal Price { get; set; }
    }
}