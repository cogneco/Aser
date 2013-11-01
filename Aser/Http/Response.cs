//
//  Response.cs
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
using Kean;
using Kean.Extension;
using IO = Kean.IO;

namespace Aser.Http
{
	public class Response
	{
		Owin.Types.OwinResponse backend;
		#region ContentType
		public string ContentType { get { return this.backend.ContentType; } set { this.backend.ContentType = value; } }
		#endregion
		#region Link
		public Header.Links Link { get; private set; }
		#endregion
		#region Status
		public Status Status { get { return (Status)this.backend.StatusCode; } set { this.backend.StatusCode = value; } }
		#endregion
		#region Writer
		IO.ICharacterWriter writer;
		public IO.ICharacterWriter Writer
		{
			get
			{ 
				if (this.writer.IsNull())
					this.writer = IO.CharacterWriter.Open(IO.CharacterDevice.Open(IO.ByteDeviceCombiner.Open(this.Device)));
				return this.writer;
			}
		}
		#endregion
		#region Device
		IO.IByteOutDevice device;
		public IO.IByteOutDevice Device
		{
			get
			{
				if (this.device.IsNull())
				{
					this.backend.SetHeader("Link", (string)this.Link);
					this.device = IO.ByteDevice.Wrap(this.backend.Body); 
				}
				return this.device; 
			}
		}
		#endregion
		internal Response(Owin.Types.OwinResponse backend)
		{
			this.backend = backend;
			this.Link = new Header.Links();
		}
		public bool End()
		{
			bool result;
			if (!(result = this.Status.Success))
				this.Writer.WriteLine((string)this.Status);
			return result;
		}
	}
}

