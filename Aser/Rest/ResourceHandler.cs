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
		Func<R> loadData;
		R data;
		protected R Data
		{
			get
			{
				if (this.data.IsNull() && this.loadData.NotNull())
					this.data = this.loadData();
				return this.data;
			}
		}
		public long Key { get { return this.Data.Key; } }
		protected ResourceHandler(Uri.Locator locator, Func<R> loadData) :
			base(locator)
		{
			this.loadData = loadData;
		}
		#region Put
		protected override void Put(Http.Request request, Http.Response response)
		{
			if (!request.Receive(this))
				response.Status = Http.Status.BadRequest;
			else if (!this.Data.Save())
				response.Status = Http.Status.InternalServerError;
			else
			{
				response.Status = Http.Status.OK;
				response.Send(this);
			}
		}
		#endregion
		#region Delete
		protected override void Delete(Http.Request request, Http.Response response)
		{
			if ((response.Status = this.Delete()).Success)
				response.Send(this);
		}
		protected virtual Http.Status Delete()
		{
			return this.Data.Remove() ? Http.Status.OK : Http.Status.InternalServerError;
		}
		#endregion
	}
}

