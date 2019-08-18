using AutoMapper;

namespace Euricom.DevCruise.Controllers.MappingProfiles
{
    public class SessionMapping : Profile
    {
        public SessionMapping()
        {
            CreateMap<Model.Session, ViewModels.Session>();
            CreateMap<Model.Session, ViewModels.SessionDetail>();

            CreateMap<ViewModels.SessionDetail, Model.Session>();
        }
    }
}
