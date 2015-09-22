using System.Collections.Generic;
using Kepler.Common.Models;

namespace Kepler.Common.CommunicationTypes
{
    public class ImageComparisonMessage
    {
        public List<ImageComparisonInfo> ImageComparisonList { get; set; }
    }
}