//
//  CollectionHandler.cs
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
using Kean.Collection.Extension;
using Kean;
using Kean.Extension;
using Generic = System.Collections.Generic;

namespace Aser.Rest
{
	public abstract class CollectionHandler<C, R> :
	Handler
		where C : ICollection<R>
		where R : IResource, new()
	{
		protected C Data { get; private set; }
		protected int DefaultPageSize { get { return 30; } }
		protected CollectionHandler(Uri.Locator locator, C data) :
			base(locator)
		{
			this.Data = data;
		}
		public override Serialize.Data.Node Serialize()
		{
			return null;
		}
		protected abstract ResourceHandler<R> Map(R resource);
		#region Route
		protected virtual ResourceHandler<R> Route(string identifier)
		{
			long key;
			return long.TryParse(identifier, out key) ? this.Map(this.Data.Open(key)) : null;
		}
		protected override Tuple<Handler, Path> Route(Path path)
		{
			Handler result = this.Route(path.Head);
			return result.NotNull() ? Tuple.Create(result, path.Tail) : base.Route(path);
		}
		#endregion
		#region Get
		protected override bool Get(Aser.Http.Request request, Aser.Http.Response response)
		{
			int pageSize = request.Locator.Query.Get("pageSize", this.DefaultPageSize);
			int page = request.Locator.Query.Get("page", 0);
			int last = (this.Data.Count - 1) / pageSize;
			if (page > 0)
			{
				response.Link.Add(new Http.Header.Link(this.Locator + KeyValue.Create("page", "0")) { Relatation = Http.Header.LinkRelation.First });
				if (page <= last)
					response.Link.Add(new Http.Header.Link(this.Locator + KeyValue.Create("page", (page - 1).AsString())) { Relatation = Http.Header.LinkRelation.Prev });
			}
			if (page != last)
			{
				if (page < last)
					response.Link.Add(new Http.Header.Link(this.Locator + KeyValue.Create("page", (page + 1).AsString())) { Relatation = Http.Header.LinkRelation.Next });
				response.Link.Add(new Http.Header.Link(this.Locator + KeyValue.Create("page", last.AsString())) { Relatation = Http.Header.LinkRelation.Last });
			}
			Generic.IEnumerable<ResourceHandler<R>> content = this.Get(pageSize, page * pageSize);
			bool result;
			if (result = content.NotNull())
			{
				response.Status = Http.Status.OK;
				try
				{
					if (!(result = response.Send((Serialize.Data.Node)new Serialize.Data.Collection(content.Map(item => item.Serialize())))))
						response.Status = Http.Status.InternalServerError;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
			else
				response.Status = Http.Status.MethodNotAllowed;
			return result;
		}
		protected virtual Generic.IEnumerable<ResourceHandler<R>> Get(int limit, int offset)
		{
			return this.Data.Open(limit, offset).Map(this.Map);
		}
		#endregion
		#region Post
		protected override bool Post(Http.Request request, Http.Response response)
		{
			bool result = false;
			R @new = request.Receive<R>();
			if (@new.IsNull())
				response.Status = Http.Status.BadRequest;
			else if (this.Post(@new))
			{
				response.Status = Http.Status.Created;
				if (!(result = response.Send(this.Map(@new).Serialize())))
					response.Status = Http.Status.InternalServerError;
			}
			else
				response.Status = Http.Status.InternalServerError;
			return result;
		}
		protected virtual bool Post(R item)
		{
			return this.Data.Create(item) > 0;
		}
		#endregion
		#region Static Open
		#endregion
	}
}

