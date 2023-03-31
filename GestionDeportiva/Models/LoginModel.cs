using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionDeportiva.Models
{
	/// <summary>
	/// Class used in the login page
	/// </summary>
	public class LoginModel
	{
		public string LoginUser { get; set;}
		[DataType(DataType.Password)]
		public string LoginPassword { get; set;}
	}
}