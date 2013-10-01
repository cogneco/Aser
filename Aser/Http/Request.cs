//
//  Request.cs
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
    public class Request
    {
        Waser.Http.IRequest backend;
        Method? method;
        public Method Method
        {
            get
            { 
                if (!this.method.HasValue)
                    switch (this.backend.Method)
                    {
                        default:
                            this.method = Method.Other;
                            break;
                        case Waser.Http.Method.Get:
                            this.method = Method.Get;
                            break;
                        case Waser.Http.Method.Put:
                            this.method = Method.Put;
                            break;
                        case Waser.Http.Method.Post:
                            this.method = Method.Post;
                            break;
                        case Waser.Http.Method.Delete:
                            this.method = Method.Delete;
                            break;
                    }
                return this.method.Value;
            }
        }
        Uri.Locator resource;
        public Uri.Locator Resource
        {
            get
            {
                if (this.resource.IsNull())
                {
                    this.resource = new Uri.Locator("http", this.backend.Headers["Host"], this.backend.Path);
                    foreach (string key in this.backend.QueryData.Keys)
                        this.resource.Query[key] = this.backend.QueryData.Get(key);
                }
                return this.resource;
            } 
        }
        internal Request(Waser.Http.IRequest backend)
        {
            this.backend = backend;
        }
    }
}

