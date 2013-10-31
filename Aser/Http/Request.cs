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
		protected Waser.Http.IRequest Backend { get; private set; }
		public abstract Method Method { get; }
		Uri.Locator resource;
		public Uri.Locator Resource
		{
			get
			{
				if (this.resource.IsNull())
				{
					this.resource = new Uri.Locator("http", this.Backend.Headers["Host"], this.Backend.Path);
					foreach (string key in this.Backend.QueryData.Keys)
						this.resource.Query[key] = this.Backend.QueryData.Get(key);
				}
				return this.resource;
			} 
		}
		protected Request(Waser.Http.IRequest backend)
		{
			this.Backend = backend;
		}
		internal static Request Create(Waser.Http.IRequest backend)
		{
			Request result;
			switch (backend.Method)
			{
				default:
					result = null;
					break;
				case Waser.Http.Method.Get:
					result = new Get(backend);
					break;
				case Waser.Http.Method.Put:
					result = null;
					break;
				case Waser.Http.Method.Post:
					result = new Post(backend);
					break;
				case Waser.Http.Method.Delete:
					result = null;
					break;
			}
			return result;
		}
	}
}

