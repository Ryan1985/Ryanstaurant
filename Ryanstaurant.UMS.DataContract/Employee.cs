﻿using System.Runtime.Serialization;
using Ryanstaurant.UMS.Interface;


namespace Ryanstaurant.UMS.DataContract
{
    [DataContract]
    public class Employee : IDataContract
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LoginName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Authority { get; set; }
        [DataMember]
        public string Exception { get; set; }
    }
}
