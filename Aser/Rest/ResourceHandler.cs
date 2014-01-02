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
		where R : class, IResource, new()
	{
		Func<R> loadResource;
		R resource;
		internal protected R Resource
		{
			get { return this.resource ?? (this.loadResource.NotNull() ? (this.resource = this.loadResource()) : null); } 
		}
		public override bool Exists { get { return this.Resource.NotNull(); } }
		public long Key { get { return this.Resource.Key; } }
		protected ResourceHandler(Func<Uri.Locator> getUrl, Func<R> loadResource) :
			base(getUrl)
		{
			this.loadResource = loadResource;
		}
		protected ResourceHandler(Func<Uri.Locator> getUrl, R resource) :
			base(getUrl)
		{
			this.resource = resource;
		}
		#region Put
		protected override void Put (Http.Request request, Http.Response response)
		{
			if (!this.Writable)
				response.Status = Http.Status.Forbidden;
			else if (!request.Receive(this))
				response.Status = Http.Status.BadRequest;
			else if (!this.Resource.Save())
				response.Status = Http.Status.InternalServerError;
			else
			{
				response.Status = Http.Status.OK;
				response.Send(this);
			}
		}
		#endregion
		#region Delete
		protected override void Delete (Http.Request request, Http.Response response)
		{
			if ((response.Status = this.Delete()).Success)
				response.Send(this);
		}
		protected virtual Http.Status Delete ()
		{
			return !this.Exists ? Http.Status.NotFound :
				!this.Writable ? Http.Status.Forbidden :
				this.Resource.Remove() ? Http.Status.OK : 
				Http.Status.InternalServerError;
		}
		#endregion
	}
}

