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
using IO = Kean.IO;
using Serialize = Kean.Serialize;
using Json = Kean.Json;
using Xml = Kean.Xml;
using Uri = Kean.Uri;

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

		public Uri.Locator Url
		{
			get
			{
				if (this.locator.IsNull())
					this.locator = this.backend.Uri.ToString();
				return this.locator;
			} 
		}

		#endregion

		#region Authorization

		Header.Authorization authorization;

		public Header.Authorization Authorization
		{
			get
			{ 
				if (this.authorization.IsNull())
					this.authorization = this.backend.GetHeader("Authorization");
				return this.authorization; 
			} 
		}

		#endregion

		#region Storage

		Serialize.Storage storage;

		Serialize.Storage Storage
		{
			get
			{ 
				if (this.storage.IsNull())
					switch ("application/json") // TODO: switch depending on client preference
					{
						case "application/json":
							this.storage = new Json.Serialize.Storage();
							break;
						case "application/xml":
							this.storage = new Xml.Serialize.Storage();
							break;
					}
				return this.storage;
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

		#region Receive

		public T Receive<T> ()
		{
			return this.Storage.Load<T>(this.Device);
		}

		public bool Receive<T> (T result)
		{
			return this.Storage.LoadInto(result, this.Device);
		}

		#endregion

	}
}

