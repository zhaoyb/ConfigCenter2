using ServiceStack;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConfigCenter.Dto
{
    #region Dto

    public class AppDto
    {
        [Display(Name = "应用Id")]
        public int Id { get; set; }

        [Display(Name = "应用名称")]
        public string AppId { get; set; }

        [Display(Name = "应用版本")]
        public string Version { get; set; }
    }

    #endregion Dto

    #region /app/apps

    [Route("/app/apps", "GET")]
    public class GetApps : IReturn<GetAppsResponse>
    {
    }

    public class GetAppsResponse
    {
        public List<AppDto> Apps { get; set; }
    }

    #endregion /app/apps


    #region /app/lastversion
    [Route("/app/version/{AppId}", "GET")]
    public class GetAppVersion : IReturn<GetAppVersionResponse>
    {
        public string AppId { get; set; }
    }

    public class GetAppVersionResponse
    {
        public AppDto AppDto { get; set; }
    }


    #endregion



    #region /app/create

    [Route("/app/create", "POST")]
    public class CreateApp : IReturn<BaseResponse>
    {
        public string AppId { get; set; }
    }

    #endregion /app/create

    #region /app/update

    [Route("/app/update", "PUT")]
    public class UpdateApp : IReturn<BaseResponse>
    {
        public string ConfigVersions { get; set; }
    }

    #endregion /app/update

    #region /app/delete

    [Route("/app/delete", "DELETE")]
    public class DeleteApp : IReturn<BaseResponse>
    {
        public int Id { get; set; }
    }

    #endregion /app/delete
}