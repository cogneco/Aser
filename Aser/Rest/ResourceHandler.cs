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
	public abstract class ResourceHandler<R> :
    Handler
		where R : IResource, new()
	{
		protected R Data { get; private set; }
		public long Key { get { return this.Data.Key; } }
		protected ResourceHandler(Uri.Locator locator, R data) :
			base(locator)
		{
			this.Data = data;
		}
		public override Serialize.Data.Node Serialize()
		{
			Serialize.Data.Node result = Kean.Serialize.Storer.Store<R>(this.Data);
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
			R @new = request.Receive<R>();
			return (response.Status = @new.IsNull() ? 
				Http.Status.BadRequest : 
				(result = this.Put(@new)).Success && response.Send(this.Serialize()) ? 
				result : 
				Http.Status.InternalServerError
			).Success;
		}
		protected virtual Http.Status Put(R @new)
		{
			return @new.IsNull() ? Http.Status.BadRequest : (this.Data = @new).Save() ? Http.Status.OK : Http.Status.InternalServerError;
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
			return this.Data.Remove() ? Http.Status.OK : Http.Status.InternalServerError;
		}
		#endregion
	}
}

