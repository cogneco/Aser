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
		protected virtual Tuple<Rest.ResourceHandler, Rest.Path> Route(Rest.Path path)
		{
			return null;
		}
		public void Process(Path path, Http.Request request, Http.Response response)
		{
			Tuple<Rest.ResourceHandler, Rest.Path> next = null;
			if (path.NotNull() && path.Head.NotEmpty())
			{
				next = this.Route(path);
				if (next.NotNull() && next.Item1.NotNull())
					next.Item1.Process(next.Item2, request, response);
				else
					response.Status = Http.Status.NotFound;
			}
			else
				this.Process(request, response);
		}
		void Process(Http.Request request, Http.Response response)
		{
			Serialize.Storage storage;
			storage = new Json.Serialize.Storage();
			response.ContentType = storage.ContentType;
			Serialize.Data.Node responseBody;
			switch (request.Method)
			{
				case Http.Method.Get:
					responseBody = this.Get(storage, request, response);
					break;
				case Http.Method.Put:
					responseBody = this.Put(storage, request, response);
					break;
				case Http.Method.Patch:
					responseBody = this.Patch(storage, request, response);
					break;
				case Http.Method.Post:
					responseBody = this.Post(storage, request, response);
					break;
				case Http.Method.Delete:
					responseBody = this.Delete(storage, request, response);
					break;
				default:
					responseBody = null;
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
			return this.Put(storage.Load(request.Device), storage, request, response);
		}
		protected virtual Serialize.Data.Node Put(Serialize.Data.Node body, Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Put(body, storage);
		}
		protected virtual Serialize.Data.Node Put(Serialize.Data.Node body, Serialize.Storage storage)
		{
			return null;
		}
		#endregion
		#region Patch
		protected virtual Serialize.Data.Node Patch(Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Patch(storage.Load(request.Device), storage, request, response);
		}
		protected virtual Serialize.Data.Node Patch(Serialize.Data.Node body, Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Patch(body, storage);
		}
		protected virtual Serialize.Data.Node Patch(Serialize.Data.Node body, Serialize.Storage storage)
		{
			return null;
		}
		#endregion
		#region Post
		protected virtual Serialize.Data.Node Post(Serialize.Storage storage, Http.Request request, Http.Response response)
		{
			return this.Post(storage.Load(request.Device), storage, request, response);
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
		protected T Backend { get; set; }
		protected ResourceHandler(Uri.Locator locator, T backend) :
			base(locator)
		{
			this.Backend = backend;
		}
		#region Get
		public override Serialize.Data.Node Get(Serialize.Storage storage)
		{
			Serialize.Data.Node result = this.Backend.Serialize(storage);
			if (result is Serialize.Data.Branch)
				(result as Serialize.Data.Branch).Nodes.Add(new Serialize.Data.String(this.Locator).UpdateName("url"));
			return result;
		}
		#endregion
		#region Put
		protected virtual bool Put(T @new)
		{
			return false;
		}
		protected override Serialize.Data.Node Put(Serialize.Storage storage, Aser.Http.Request request, Aser.Http.Response response)
		{
			Serialize.Data.Node result = new Serialize.Data.Branch();
			T @new = storage.Load<T>(request.Device);
			if (@new.IsNull())
				response.Status = Http.Status.BadRequest;
			else if (!this.Put(@new))
				response.Status = Http.Status.NotModified;
			else
				result = this.Get(storage);
			return result;
		}
		#endregion
		#region Delete
		protected virtual bool Delete()
		{
			return false;
		}
		protected override Serialize.Data.Node Delete(Serialize.Storage storage, Aser.Http.Request request, Aser.Http.Response response)
		{
			Serialize.Data.Node result = new Serialize.Data.Branch();
			if (!this.Delete())
				response.Status = Http.Status.NotModified;
			else
				result = this.Get(storage);
			return result;
		}
		#endregion
	}
}

