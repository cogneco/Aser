//
//  ResourceHandler.cs
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
using Uri = Kean.Uri;
using Serialize = Kean.Serialize;
using Json = Kean.Json;
using Kean;
using Kean.Extension;
using IO = Kean.IO;
namespace Aser.Rest
{
	public abstract class ResourceHandler
	{
		public Uri.Locator Locator { get; private set; }
		protected ResourceHandler(Uri.Locator locator)
		{
			this.Locator = locator;
		}
		public virtual void Process(Path path, Http.Request request, Http.Response response)
		{
			if (path.IsNull() || path.Head.IsEmpty())
				this.Process(request, response);
		}
		protected virtual void Process(Http.Request request, Http.Response response)
		{
			Serialize.Storage storage;
			string contentType;
			storage = new Json.Serialize.Storage();
			contentType = "application/json";
			response.ContentType = contentType;
			Serialize.Data.Node responseBody;
			response.Status = Http.Status.OK;
			switch (request.Method)
			{
				case Http.Method.Get:
					responseBody = this.Get(storage, request, response);
					break;
				case Http.Method.Put:
					responseBody = this.Put(storage, request, response);
					break;
				case Http.Method.Post:
					responseBody = this.Post((request as Http.Post).Device, storage, request, response);
					break;
				case Http.Method.Delete:
					responseBody = this.Delete(storage, request, response);
					break;
				default:
					break;
			}
			if (responseBody.NotNull())
				storage.Store(responseBody, response.Device);
			else
				response.Status = Http.Status.MethodNotAllowed;
			response.End();
		}

		#region Get, Put, Post, Delete of this resource

		#region Get

		protected virtual Serialize.Data.Node Get(Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Get(storage);
		}
		public virtual Serialize.Data.Node Get(Serialize.Storage storage)
		{
			return null;
		}

		#endregion

		#region Put

		protected virtual Serialize.Data.Node Put(Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Put(storage);
		}
		protected virtual Serialize.Data.Node Put(Serialize.Storage storage)
		{
			return null;
		}

		#endregion

		#region Post

		protected virtual Serialize.Data.Node Post(IO.IByteInDevice device, Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Post(storage.Load(device), storage, request, response);
		}
		protected virtual Serialize.Data.Node Post(Serialize.Data.Node body, Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Post(body, storage);
		}
		protected virtual Serialize.Data.Node Post(Serialize.Data.Node body, Serialize.Storage storage)
		{
			return null;
		}

		#endregion

		#region Delete

		protected virtual Serialize.Data.Node Delete(Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Delete(storage);
		}
		protected virtual Serialize.Data.Node Delete(Serialize.Storage storage)
		{
			return null;
		}

		#endregion

		#endregion

	}
	public abstract class ResourceHandler<T> :
    ResourceHandler
        where T : Item<T>, new()
	{
		T backend;
		protected ResourceHandler(Uri.Locator locator, T backend) :
			base(locator)
		{
			this.backend = backend;
		}
		public override Serialize.Data.Node Get(Serialize.Storage storage)
		{
			Serialize.Data.Node result = this.backend.Serialize(storage);
			if (result is Serialize.Data.Branch)
				(result as Serialize.Data.Branch).Nodes.Add(new Serialize.Data.String(this.Locator).UpdateName("url"));
			return result;
		}
	}
}

