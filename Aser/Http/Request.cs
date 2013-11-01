//
//  Request.cs
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
using Uri = Kean.Uri;
using IO = Kean.IO;

namespace Aser.Http
{
	public class Request
	{
		Owin.Types.OwinRequest backend;
		#region Method
		Method? method;
		public Method Method
		{ 
			get
			{
				if (!this.method.HasValue)
					switch (this.backend.Method)
					{
						default:
							this.method = Http.Method.Other;
							break;
						case "GET":
							this.method = Http.Method.Get;
							break;
						case "PUT":
							this.method = Http.Method.Put;
							break;
						case "PATCH":
							this.method = Http.Method.Patch;
							break;
						case "POST":
							this.method = Http.Method.Post;
							break;
						case "DELETE":
							this.method = Http.Method.Delete;
							break;
					}
				return this.method ?? Http.Method.Other;
			} 
		}
		#endregion
		#region Locator
		Uri.Locator locator;
		public Uri.Locator Locator
		{
			get
			{
				if (this.locator.IsNull())
					this.locator = this.backend.Uri.ToString();
				return this.locator;
			} 
		}
		#endregion
		#region Device
		IO.IByteInDevice device;
		public IO.IByteInDevice Device
		{
			get
			{
				if (this.device.IsNull())
					this.device = IO.ByteDevice.Wrap(this.backend.Body); 
				return this.device; 
			}
		}
		#endregion;
		#region Constructors
		internal Request(Owin.Types.OwinRequest backend)
		{
			this.backend = backend;
		}
		#endregion
	}
}

