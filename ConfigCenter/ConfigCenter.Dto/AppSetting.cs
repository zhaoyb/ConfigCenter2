using ServiceStack;
using System.Collections.Generic;

namespace ConfigCenter.Dto
{
    #region Dto

    public class AppSettingDto
    {
        public int Id { get; set; }

        public int AppId { get; set; }

        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public int ConfigType { get; set; }
    }

    #endregion Dto

    #region /appsetting/appsettings

    [Route("/appsetting/appsettings/{AppId}", "GET")]
    public class GetAppSettings : IReturn<GetAppSettingsResponse>
    {
        public int AppId { get; set; }
    }

    public class GetAppSettingsResponse
    {
        public List<AppSettingDto> AppSettings { get; set; }
    }

    #endregion /appsetting/appsettings

    #region /appsetting/create

    [Route("/appsetting/create", "POST")]
    public class CreateAppSetting : IReturn<BaseResponse>
    {
        public int AppId { get; set; }

        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public int ConfigType { get; set; }
    }

    #endregion /appsetting/create

    #region /appsetting/update

    [Route("/appsetting/update/{Id}", "PUT")]
    public class UpdateAppSetting : IReturn<BaseResponse>
    {
        public int Id { get; set; }

        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public int ConfigType { get; set; }
    }

    #endregion /appsetting/update

    #region /appsetting/delete

    [Route("/appsetting/delete/{Id}", "DELETE")]
    public class DeleteAppSetting : IReturn<BaseResponse>
    {
        public int Id { get; set; }
    }

    #endregion /appsetting/delete
}