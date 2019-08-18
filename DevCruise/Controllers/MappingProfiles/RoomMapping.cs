using System;
using AutoMapper;
using Euricom.DevCruise.Model;

namespace Euricom.DevCruise.Controllers.MappingProfiles
{
    public class RoomMapping : Profile
    {
        public RoomMapping()
        {
            CreateMap<Room, string>().ConvertUsing(r => Enum.GetName(typeof(Room), r));
        }
    }
}
