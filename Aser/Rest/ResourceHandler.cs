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
		public abstract Serialize.Data.Node Serialize();
		#region Process
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
		bool Process(Http.Request request, Http.Response response)
		{
			response.ContentType = "application/json";
			bool result;
			switch (request.Method)
			{
				case Http.Method.Get:
					result = this.Get(request, response);
					break;
				case Http.Method.Put:
					result = this.Put(request, response);
					break;
				case Http.Method.Patch:
					result = this.Patch(request, response);
					break;
				case Http.Method.Post:
					result = this.Post(request, response);
					break;
				case Http.Method.Delete:
					result = this.Delete(request, response);
					break;
				default:
					result = false;
					response.Status = Http.Status.NotImplemented;
					break;
			}
			response.End();
			return result;
		}
		#endregion
		#region Get, Put, Post, Delete of this resource
		#region Get
		protected virtual bool Get(Http.Request request, Http.Response response)
		{
			bool result;
			Serialize.Data.Node content = this.Serialize();
			if (result = content.NotNull())
			{
				response.Status = Http.Status.OK;
				if (!(result = response.Send(content)))
					response.Status = Http.Status.InternalServerError;
			}
			else
				response.Status = Http.Status.MethodNotAllowed;
			return result;
		}
		#endregion
		#region Put
		protected virtual bool Put(Http.Request request, Http.Response response)
		{
			response.Status = Http.Status.MethodNotAllowed;
			return false;
		}
		#endregion
		#region Patch
		protected virtual bool Patch(Http.Request request, Http.Response response)
		{
			response.Status = Http.Status.MethodNotAllowed;
			return false;
		}
		#endregion
		#region Post
		protected virtual bool Post(Http.Request request, Http.Response response)
		{
			response.Status = Http.Status.MethodNotAllowed;
			return false;
		}
		#endregion
		#region Delete
		protected virtual bool Delete(Http.Request request, Http.Response response)
		{
			response.Status = Http.Status.MethodNotAllowed;
			return false;
		}
		#endregion
		#endregion
	}

	public abstract class ResourceHandler<T> :
    ResourceHandler
        where T : Item<T>, new()
	{
		public long Key { get { return this.Backend.Key; } }
		protected T Backend { get; set; }
		protected ResourceHandler(Uri.Locator locator, T backend) :
			base(locator)
		{
			this.Backend = backend;
		}
		public override Serialize.Data.Node Serialize()
		{
			Serialize.Data.Node result = this.Backend.Serialize();
			if (result is Serialize.Data.Branch)
				(result as Serialize.Data.Branch).Nodes.Add(new Serialize.Data.String(this.Locator).UpdateName("url"));
			return result;
		}
		#region Get
		protected override bool Get(Http.Request request, Http.Response response)
		{
			Serialize.Data.Node node = this.Get();
			return (response.Status = node.NotNull() && response.Send(node) ? Http.Status.OK : Http.Status.NotImplemented).Success;
		}
		protected virtual Serialize.Data.Node Get()
		{
			return this.Serialize();
		}
		#endregion
		#region Put
		protected override bool Put(Http.Request request, Http.Response response)
		{
			Http.Status result;
			T @new = request.Receive<T>();
			return (response.Status = @new.IsNull() ? 
				Http.Status.BadRequest : 
				(result = this.Put(@new)).Success && response.Send(this.Serialize()) ? 
				result : 
				Http.Status.InternalServerError
			).Success;
		}
		protected virtual Http.Status Put(T @new)
		{
			return Http.Status.MethodNotAllowed;
		}
		#endregion
		#region Delete
		protected override bool Delete(Http.Request request, Http.Response response)
		{
			bool result;
			if (result = (response.Status = this.Delete()).Success)
				response.Send(this.Serialize());
			return result;
		}
		protected virtual Http.Status Delete()
		{
			return Http.Status.MethodNotAllowed;
		}
		#endregion
	}
}

