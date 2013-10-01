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
namespace Aser.Http
{
    public class Server
    {
        class Application :
        Waser.Application
        {
            public Application(Action<Request, Response> process)
            {
                this.Route(".*", Waser.Routing.MatchType.Regex, context => process(new Request(context.Request), new Response(context.Response)));
            }
        }
        Action<Request, Response> process;
        Server(Action<Request, Response> process)
        {
            this.process = process;
        }
        public bool Listen(Uri.Endpoint endpoint)
        {
            bool result = true;
            Waser.IO.IPAddress address;
            string host = endpoint.Host;
            if (host.IsEmpty())
                address = Waser.IO.IPAddress.Any;
            else
                result = Waser.IO.IPAddress.TryParse(host, out address);
            if (result)
            {
                Waser.ApplicationHost.ListenAt(new Waser.IO.IPEndPoint(address, (int?)endpoint.Port ?? 8080));
                Waser.ApplicationHost.Start(new Application(this.process));
            }
            return result;
        }
        public static Server Create(Action<Request, Response> process)
        {
            return new Server(process);
        }
    }
}

