using System.Collections.Generic;
using Kepler.Common.Models;

namespace Kepler.Common.CommunicationContracts
{
    public class ImageComparisonContract
    {
        public List<ImageComparisonInfo> ImageComparisonList { get; set; }
    }
}