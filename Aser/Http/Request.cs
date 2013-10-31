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
namespace Aser.Http
{
	public abstract class Request
	{
		protected Owin.Types.OwinRequest Backend { get; private set; }
		public abstract Method Method { get; }
		Uri.Locator resource;
		public Uri.Locator Resource
		{
			get
			{
				if (this.resource.IsNull())
					this.resource = this.Backend.Uri.ToString();
				return this.resource;
			} 
		}
		protected Request(Owin.Types.OwinRequest backend)
		{
			this.Backend = backend;
		}
		internal static Request Create(Owin.Types.OwinRequest backend)
		{
			Request result;
			switch (backend.Method)
			{
				default:
					result = null;
					break;
				case "GET":
					result = new Get(backend);
					break;
				case "PUT":
					result = null;
					break;
				case "POST":
					result = new Post(backend);
					break;
				case "DELETE":
					result = null;
					break;
			}
			return result;
		}
	}
}

