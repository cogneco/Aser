//
//  Server.cs
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
using Uri = Kean.Uri;
using Error = Kean.Error;
using Owin;
using Tasks = System.Threading.Tasks.Task;

namespace Aser.Http
{
	public class Server :
	IDisposable
	{
		IDisposable backend;
		Action<Request, Response> process;
		Server(Action<Request, Response> process)
		{
			this.process = process;
		}
		~Server ()
		{
			this.Close();
		}
		public Server Listen(Uri.Endpoint endpoint)
		{
			var options = new Microsoft.Owin.Hosting.StartOptions
			{
				//ServerFactory = "Microsoft.Owin.Host.HttpListener",
				ServerFactory = "Nowin",
				Port = (int?)endpoint.Port ?? 8080,
			};
			this.backend = Microsoft.Owin.Hosting.WebApp.Start(options, applicationBuilder => 
				applicationBuilder.UseHandler((request, response) => Error.Log.Call(this.process, new Request(request), new Response(response)))
			);
			return this;
		}
		public bool Run(Uri.Endpoint endpoint)
		{
			bool result;
			this.Listen(endpoint);
			while (true) // TODO: find a good way to shut the server down (sending signal?)
				System.Threading.Thread.Sleep(60000);
			result &= this.Close();
			return result;
		}
		public bool Close()
		{
			bool result;
			if (result = this.backend.NotNull())
			{
				this.backend.Dispose();
				this.backend = null;
			}
			return result;
		}
		#region IDisposable implementation
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		public static Server Create(Action<Request, Response> process)
		{
			return new Server(process);
		}
	}
}

