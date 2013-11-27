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
		#region Get
		protected override bool Get(Http.Request request, Http.Response response)
		{
			return (response.Status = response.Send(this) ? Http.Status.OK : Http.Status.InternalServerError).Success;
		}
		#endregion
		#region Put
		protected override bool Put(Http.Request request, Http.Response response)
		{
			return (response.Status = !request.Receive(this) ? Http.Status.BadRequest : 
				!this.Data.Save() ? Http.Status.InternalServerError :
				Http.Status.OK).Success && response.Send(this);
		}
		#endregion
		#region Delete
		protected override bool Delete(Http.Request request, Http.Response response)
		{
			bool result;
			if (result = (response.Status = this.Delete()).Success)
				response.Send(this);
			return result;
		}
		protected virtual Http.Status Delete()
		{
			return this.Data.Remove() ? Http.Status.OK : Http.Status.InternalServerError;
		}
		#endregion
	}
}

