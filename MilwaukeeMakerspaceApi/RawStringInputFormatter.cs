using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Mms.Api
{
	public class RawStringInputFormatter : InputFormatter
	{
		public RawStringInputFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
		}


		/// <summary>
		/// Allow anything to be processed
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool CanRead(InputFormatterContext context)
		{
			return true;
		}

		/// <summary>
		/// Handle text/plain or no content type for string results
		/// Handle application/octet-stream for byte[] results
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
		{
			var request = context.HttpContext.Request;

			using (var reader = new StreamReader(request.Body)) {
				var content = await reader.ReadToEndAsync();
				return await InputFormatterResult.SuccessAsync(content);
			}
		}
	}
}
