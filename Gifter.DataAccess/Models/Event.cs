using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gifter.DataAccess.Models
{
    public class Event
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ICollection<GiftGroup> GiftGroups { get; set; }

        public int EventTypeId { get; set; }

        public EventType EventType { get; set; }
    }
}
