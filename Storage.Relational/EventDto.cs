using System;

namespace Storage.Relational
{
	internal class EventDto
	{
		public Guid ID { get; set; }
		public Guid AggregateID { get; set; }
		public int Sequence { get; set; }
		public string Type { get; set; }
		public string Json { get; set; }
	}
}
