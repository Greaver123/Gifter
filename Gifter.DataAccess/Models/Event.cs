using System;
using System.ComponentModel.DataAnnotations;

namespace Gifter.DataAccess.Models
{
    public class Event
    {
        public int Id { get; set; }

        public int EventTypeId { get; set; }

        public int GiftGroupId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        public EventType EventType { get; set; }
    }
}
