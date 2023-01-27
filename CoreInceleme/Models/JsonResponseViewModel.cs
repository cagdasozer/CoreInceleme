using System;

namespace CoreInceleme.Models
{
	[Serializable]
	public class JsonResponseViewModel
	{
		public int ResponseCode { get; set; }

		public string ResponseMessage { get; set; } = string.Empty;
	}
}
