//
//  Links.cs
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using IO = Kean.IO;
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;
namespace Aser.Http.Header
{
	public class Links :
	Collection.List<Link>
	{
		public Links()
		{
		}
		public Links(Generic.IEnumerable<Link> items) :
			base()
		{
			this.Add(items);
		}
		public static implicit operator string(Links links)
		{
			return (links.NotNull() && links.Count > 0) ? ((Generic.IEnumerable<Link>)links).Map(link => (string)link).Join(", ") : null;
		}
		public static explicit operator Links(string links)
		{
			return links.NotEmpty() ? new Links(((Generic.IEnumerable<string>)links.SplitAt(',')).Map(link => (Link)link)) : null;
		}
	}
}

