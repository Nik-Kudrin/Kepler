using Kepler.Common.Error;

namespace Kepler.Common.Repository
{
    public class ErrorMessageRepository : BaseRepository<ErrorMessage>
    {
        public static ErrorMessageRepository Instance => new ErrorMessageRepository();

        protected ErrorMessageRepository()
        {
        }
    }
}