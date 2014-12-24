using System;
using Newtonsoft.Json;

namespace alphatodo
{
	public class ToDoItem
	{
		public string Id { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }

        [JsonProperty(PropertyName = "owner")]
        public string Owner { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

		[JsonProperty(PropertyName = "complete")]
		public bool Complete { get; set; }
	}
}

