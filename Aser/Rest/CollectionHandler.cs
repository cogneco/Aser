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
	public abstract class CollectionHandler<T> :
	ResourceHandler
		where T : Item<T>, new()
	{
		int defaultPageSize { get; set; }
		protected abstract int Count { get; }
		protected CollectionHandler(Uri.Locator locator) :
			this(locator, 30)
		{
		}
		protected CollectionHandler(Uri.Locator locator, int defaultPageSize) :
			base(locator)
		{
			this.defaultPageSize = defaultPageSize;
		}

		#region Get

		protected virtual Generic.IEnumerable<ResourceHandler<T>> Get(int limit, int offset)
		{
			return null;
		}
		protected override Serialize.Data.Node Get(Serialize.Storage storage, Aser.Http.Request request, Aser.Http.Response response)
		{
			int pageSize = request.Resource.Query.Get("pageSize", this.defaultPageSize);
			int page = request.Resource.Query.Get("page", 0);
			int last = (this.Count - 1) / pageSize;
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
			return this.Get(storage, pageSize, page * pageSize);
		}
		protected virtual Serialize.Data.Node Get(Serialize.Storage storage, int limit, int offset)
		{
			Generic.IEnumerable<ResourceHandler<T>> result = this.Get(limit, offset);
			return result.NotNull() ? new Serialize.Data.Collection(result.Map(item => item.Get(storage))) : null;
		}

		#endregion

		#region Post

		protected virtual ResourceHandler<T> Post(T item)
		{
			return null;
		}
		protected override Serialize.Data.Node Post(Kean.IO.IByteInDevice device, Serialize.Storage storage, Aser.Http.Request request, Aser.Http.Response response)
		{
			ResourceHandler<T> result = this.Post(storage.Load<T>(device));
			return result.NotNull() ? result.Get(storage) : null;
		}

		#endregion

	}
}

