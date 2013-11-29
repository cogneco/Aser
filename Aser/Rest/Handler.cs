//
//  Handler.cs
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
	public abstract class Handler
	{
		Func<Uri.Locator> getUrl;
		Uri.Locator url;
		[Serialize.Parameter]
		public Uri.Locator Url
		{
			get { return this.url ?? (this.getUrl.NotNull() ? (this.url = this.getUrl()) : null); } 
		}
		public virtual Action<Http.Request, Http.Response> this [Path path]
		{
			get
			{ 
				Handler handler;
				return path.IsNull() ? this.Route :
					(handler = this[path.Head]).NotNull() ? handler[path.Tail] : 
					(request, response) => response.Status = Http.Status.NotFound;
			} 
		}
		public abstract bool Exists { get; }
		public virtual Handler this [string head] { get { return null; } }
		protected Handler(Func<Uri.Locator> getUrl)
		{
			this.getUrl = getUrl;
		}
		protected Handler(Uri.Locator url)
		{
			this.url = url;
		}
		#region Process
		public void Process(Http.Request request, Http.Response response)
		{
			this[(Path)request.Locator.Path](request, response);
			response.End();
		}
		protected virtual void Route(Http.Request request, Http.Response response)
		{
			response.ContentType = "application/json";
			switch (request.Method)
			{
				case Http.Method.Get:
					this.Get(request, response);
					break;
				case Http.Method.Put:
					this.Put(request, response);
					break;
				case Http.Method.Patch:
					this.Patch(request, response);
					break;
				case Http.Method.Post:
					this.Post(request, response);
					break;
				case Http.Method.Delete:
					this.Delete(request, response);
					break;
				default:
					response.Status = Http.Status.NotImplemented;
					break;
			}
		}
		#endregion
		#region Get, Put, Post, Delete of this resource
		#region Get
		protected virtual void Get(Http.Request request, Http.Response response)
		{
			if (this.Exists)
			{
				response.Status = Http.Status.OK;
				response.Send(this);
			}
			else
				response.Status = Http.Status.NotFound;
		}
		#endregion
		#region Put
		protected virtual void Put(Http.Request request, Http.Response response)
		{
			response.Status = Http.Status.MethodNotAllowed;
		}
		#endregion
		#region Patch
		protected virtual void Patch(Http.Request request, Http.Response response)
		{
			response.Status = Http.Status.MethodNotAllowed;
		}
		#endregion
		#region Post
		protected virtual void Post(Http.Request request, Http.Response response)
		{
			response.Status = Http.Status.MethodNotAllowed;
		}
		#endregion
		#region Delete
		protected virtual void Delete(Http.Request request, Http.Response response)
		{
			response.Status = Http.Status.MethodNotAllowed;
		}
		#endregion
		#endregion
	}
}