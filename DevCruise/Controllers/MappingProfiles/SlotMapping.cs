using System;
using System.Linq;
using AutoMapper;

namespace Euricom.DevCruise.Controllers.MappingProfiles
{
    public class SlotMapping : Profile
    {
        public SlotMapping()
        {
            CreateMap<Model.Slot, ViewModels.Slot>()
                .ForMember(s => s.Speakers, e => e.MapFrom(s => s.SlotSpeakers != null ? s.SlotSpeakers.Select(sp => sp.Speaker.Email) : null))
                .ForMember(s => s.SessionCode, e => e.MapFrom(s => s.Session != null ? s.Session.Code : null));

            CreateMap<Model.Slot, ViewModels.SlotDetail>()
                .ForMember(s => s.Speakers, e => e.MapFrom(s => s.SlotSpeakers != null ? s.SlotSpeakers.Select(sp => sp.Speaker) : null))
                .ForMember(s => s.SessionCode, e => e.MapFrom(s => s.Session));


            CreateMap<ViewModels.Slot, Model.Slot>()
                .ForMember(s => s.SlotSpeakers, e => e.Ignore())
                .ForMember(s => s.Session, e => e.Ignore());

            CreateMap<DateTime, DateTimeOffset>().ConvertUsing(d => new DateTimeOffset(d, TimeSpan.Zero).ToLocalTime());
            CreateMap<DateTimeOffset, DateTime>().ConvertUsing(d => d.UtcDateTime);
        }
    }
}