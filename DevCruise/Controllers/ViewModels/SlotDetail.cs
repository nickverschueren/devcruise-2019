using System;
using Euricom.DevCruise.Model;

namespace Euricom.DevCruise.Controllers.ViewModels
{
    public class SlotDetail
    {
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public Room Room { get; set; }

        public Speaker[] Speakers { get; set; }

        public Session SessionCode { get; set; }
    }
}