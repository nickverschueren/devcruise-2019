using AutoMapper;

namespace DevCruise.Controllers.MappingProfiles
{
    public class SpeakerMapping : Profile
    {
        public SpeakerMapping()
        {
            CreateMap<Model.Speaker, ViewModels.Speaker>();
            CreateMap<Model.Speaker, ViewModels.SpeakerDetail>();

            CreateMap<ViewModels.SpeakerDetail, Model.Speaker>();
        }
    }
}