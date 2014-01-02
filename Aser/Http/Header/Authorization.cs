//
//  Authorization.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean;
using Kean.Extension;

namespace Aser.Http.Header
{
	public abstract class Authorization
	{
		protected Authorization()
		{
		}
		public static implicit operator Authorization (string header)
		{
			Authorization result = null;
			if (header.NotEmpty())
			{
				string[] splitted = header.Split(new char[] { ' ' }, 2);
				switch (splitted[0])
				{
					case "Basic":
						result = BasicAuthorization.Parse(splitted[1]);
						break;
				}
			}
			return result;
		}
		public static string Base64Encode (string value)
		{
			return System.Convert.FromBase64String(value).Decode().Join();
		}
		public static string Base64Decode (string value)
		{
			return System.Convert.FromBase64String(value).Decode().Join();
		}
		public static string CalculateDigest (string name, string realm, string password)
		{
			return Authorization.CalculateMD5Hash(name + ":" + realm + ":" + password);
		}
		public static string CalculateMD5Hash (string value)
		{
			return System.Security.Cryptography.MD5.Create()
					.ComputeHash(System.Text.Encoding.ASCII.GetBytes(value))
					.Map(b => b.ToString("x2"))
					.Join();
		}
	}
}

