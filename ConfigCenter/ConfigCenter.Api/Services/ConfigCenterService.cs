using ConfigCenter.Business;
using ConfigCenter.Dto;
using ServiceStack;

namespace ConfigCenter.Api.Services
{
    public class ConfigCenterService : Service
    {
        public object Get(GetAppVersion getAppVersion)
        {
            return new GetAppVersionResponse() { AppDto = AppBusiness.GetAppVersion(getAppVersion.AppId) };
        }



        public object Get(GetAppSettings getAppSettings)
        {
            return new GetAppSettingsResponse() { AppSettings = AppSettingBusiness.GetAppSettings(getAppSettings.AppId) };
        }

    }
}