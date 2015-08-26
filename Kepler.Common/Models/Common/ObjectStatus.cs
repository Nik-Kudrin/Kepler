using System.Runtime.Serialization;

namespace Kepler.Core.Common
{
    public enum ObjectStatus
    {
        Undefined,
        Skipped,
        Pending,
        InProgress,
        Passed,
        Failed
    }
}