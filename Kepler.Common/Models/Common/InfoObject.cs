﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Kepler.Common.Models.Common
{
    [DataContract]
    public abstract class InfoObject
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [StringLength(500)]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ObjectStatus Status { get; set; }

        public InfoObject()
        {
            Status = ObjectStatus.Undefined;
        }

        public InfoObject(string Name) : this()
        {
            this.Name = Name;
        }
    }
}