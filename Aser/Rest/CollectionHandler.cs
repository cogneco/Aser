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
		Func<C> loadCollection;
		C collection;
		protected C Collection
		{
			get
			{
				if (this.collection.IsNull() && this.loadCollection.NotNull())
					this.collection = this.loadCollection();
				return this.collection;
			}
		}
		protected virtual int DefaultPageSize { get { return 30; } }
		public override Handler this [string head]
		{
			get
			{
				long key;
				return head.NotEmpty() && long.TryParse(head, out key) ? this.Map(this.Collection.Open(key)) : base[head];
			}
		}
		protected CollectionHandler(Uri.Locator locator, Func<C> loadCollection) :
			base(locator)
		{
			this.loadCollection = loadCollection;
		}
		protected abstract ResourceHandler<R> Map(R resource);
		#region Get
		protected override void Get(Http.Request request, Http.Response response)
		{
			int pageSize = request.Locator.Query.Get("pageSize", this.DefaultPageSize);
			int page = request.Locator.Query.Get("page", 0);
			int last = (this.Collection.Count - 1) / pageSize;
			if (page > 0)
			{
				response.Link.Add(new Http.Header.Link(this.Url + KeyValue.Create("page", "0")) { Relatation = Http.Header.LinkRelation.First });
				if (page <= last)
					response.Link.Add(new Http.Header.Link(this.Url + KeyValue.Create("page", (page - 1).AsString())) { Relatation = Http.Header.LinkRelation.Prev });
			}
			if (page != last)
			{
				if (page < last)
					response.Link.Add(new Http.Header.Link(this.Url + KeyValue.Create("page", (page + 1).AsString())) { Relatation = Http.Header.LinkRelation.Next });
				response.Link.Add(new Http.Header.Link(this.Url + KeyValue.Create("page", last.AsString())) { Relatation = Http.Header.LinkRelation.Last });
			}
			Generic.IEnumerable<ResourceHandler<R>> content = this.Get(pageSize, page * pageSize);
			if (content.NotNull())
			{
				response.Status = Http.Status.OK;
				response.Send(content);
			}
			else
				response.Status = Http.Status.MethodNotAllowed;
		}
		protected virtual Generic.IEnumerable<ResourceHandler<R>> Get(int limit, int offset)
		{
			return this.Collection.Open(limit, offset).Map(this.Map);
		}
		#endregion
		#region Post
		protected override void Post(Http.Request request, Http.Response response)
		{
//			request.Receive();
//			if (@new.IsNull())
//				response.Status = Http.Status.BadRequest;
//			else if (this.Post(@new))
//			{
//				response.Status = Http.Status.Created;
//				if (!response.Send(this.Map(@new).Serialize()))
//					response.Status = Http.Status.InternalServerError;
//			}
//			else
//				response.Status = Http.Status.InternalServerError;
		}
		protected virtual bool Post(R item)
		{
			return this.Collection.Create(item) > 0;
		}
		#endregion
	}
}

