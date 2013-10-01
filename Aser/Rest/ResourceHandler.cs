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
namespace Aser.Rest
{
    public abstract class ResourceHandler
    {
        protected ResourceHandler()
        {
        }
        public virtual void Process(Path path, Http.Request request, Http.Response response)
        {
            if (path.IsNull() || path.Head.IsEmpty())
                this.Process(request, response);
            else
                switch (request.Method)
                {
                    case Http.Method.Get:
                        this.Get(path, request, response);
                        break;
                    case Http.Method.Put:
                        this.Put(path, request, response);
                        break;
                    case Http.Method.Post:
                        this.Post(path, request, response);
                        break;
                    case Http.Method.Delete:
                        this.Delete(path, request, response);
                        break;
                    default:
                        break;
                }
        }
        protected virtual void Process(Http.Request request, Http.Response response)
        {
            Serialize.Storage storage;
            string contentType;
            storage = new Json.Serialize.Storage();
            contentType = "application/json";
            response.ContentType = contentType;
            Serialize.Data.Node responseBody;
            switch (request.Method)
            {
                case Http.Method.Get:
                    responseBody = this.Get(storage);
                    break;
                case Http.Method.Put:
                    responseBody = this.Put(storage);
                    break;
                case Http.Method.Post:
                    responseBody = this.Post(storage);
                    break;
                case Http.Method.Delete:
                    responseBody = this.Delete(storage);
                    break;
                default:
                    break;
            }
            if (responseBody.NotNull())
            {
                response.Status = Http.Status.OK;
                storage.Store(responseBody, response.Device);
            }
            else
                response.Status = Http.Status.MethodNotAllowed;
            response.End();
        }
        protected virtual Serialize.Data.Node Get(Serialize.Storage storage)
        {
            return null;
        }
        protected virtual Serialize.Data.Node Put(Serialize.Storage storage)
        {
            return null;
        }
        protected virtual Serialize.Data.Node Post(Serialize.Storage storage)
        {
            return null;
        }
        protected virtual Serialize.Data.Node Delete(Serialize.Storage storage)
        {
            return null;
        }
        protected virtual void Get(Path path, Http.Request request, Http.Response response)
        {
        }
        protected virtual void Put(Path path, Http.Request request, Http.Response response)
        {
        }
        protected virtual void Post(Path path, Http.Request request, Http.Response response)
        {
        }
        protected virtual void Delete(Path path, Http.Request request, Http.Response response)
        {
        }
    }
    public abstract class ResourceHandler<T> :
    ResourceHandler
        where T : Item<T>, new()
    {
        T backend;
        protected ResourceHandler(T backend)
        {
            this.backend = backend;
        }
        protected override Serialize.Data.Node Get(Serialize.Storage storage)
        {
            return this.backend.Serialize(storage);
        }
    }
}

