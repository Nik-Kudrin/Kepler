using Kepler.Integration.Common.UI.DataGenerator;

namespace Kepler.Integration.Common.UI.Page.Common.Login
{
    public interface ILoginFieldFiller
    {
        void FillUserField(ClientInfoModel clientInfo);
        void ClickSubmitButton();
    }
}