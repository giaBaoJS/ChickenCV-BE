using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopCV.UI.Shared.ViewModel
{
	public class FileModel
	{
		public string FileName { get; set; }
		public IFormFile FormFile { get; set; }
	}
}
