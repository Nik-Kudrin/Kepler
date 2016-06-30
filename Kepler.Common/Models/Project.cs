using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Common.Models
{
    public class Project : InfoObject
    {
        public Dictionary<long, Branch> Branches { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long? MainBranchId { get; set; }

        public Project()
        {
            Branches = new Dictionary<long, Branch>();
        }

        public void InitChildObjectsFromDb()
        {
            Branches = BranchRepository.Instance.Find(new {ProjectId = Id})
                .ToDictionary(item => item.Id, item => item);
        }
    }
}