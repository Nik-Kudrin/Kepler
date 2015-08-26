using System;
using System.ServiceModel.Activation;
using Kepler.Core;

namespace Kepler.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
   // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class KeplerService : IKeplerService
    {
        public Build GetBuild(string id)
        {
            var repo = BuildRepository.Instance;

            return repo.Get(Convert.ToInt64(id));
        }

        /* public IEnumerable<Build> GetBuilds()
        {
            throw new NotImplementedException();
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }*/
    }
}