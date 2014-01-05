//
//  FileHandler.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2014 Simon Mika
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
using Kean.IO.Extension;

namespace Aser.Rest
{
	public class FileHandler :
	Handler
	{
		readonly Uri.Locator url;
		public override bool Exists { get { return true; } }
		public override Handler this [string head]
		{
			get { return new FileHandler(this.url + head); }
		}
		public FileHandler(Uri.Locator url) :
			base(() => url)
		{
			this.url = url;
		}
		protected override void Get (Http.Request request, Http.Response response)
		{
			Uri.Locator url = this.url + request.Url.Path.Filename;
			using (IO.IByteInDevice input = IO.ByteDevice.Open(url))
			{
				if (input.NotNull())
				{
					response.Status = Http.Status.OK;
					response.ContentType = url.Path.Mime ?? "text/plain";
					response.Device.Write(input);
					response.Device.Flush();
				}
				else
					response.Status = Http.Status.NotFound;
			}
		}
	}
}
