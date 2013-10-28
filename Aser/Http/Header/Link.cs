//
//  Link.cs
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
using Uri = Kean.Uri;
namespace Aser.Http.Header
{
	public class Link
	{
		public Uri.Locator Locator { get; set; }
		public LinkRelation Relatation { get; set; }
		public Link(Uri.Locator locator) :
			this()
		{
			this.Locator = locator;
		}
		public Link()
		{
		}
		public static implicit operator string(Link link)
		{
			IO.Text.Builder result = null;
			if (link.NotNull())
			{
				result += "<" + link.Locator + ">";
				if (link.Relatation != LinkRelation.None)
					result += "; rel=" + link.Relatation.AsString();
			}
			return result; 
		}
		public static explicit operator Link(string link)
		{
			string[] splitted = link.SplitAt(';');
			Link result = null;
			if (splitted.Length > 0 && splitted[0].StartsWith("<") && splitted[0].EndsWith(">"))
			{
				result = new Link(splitted[0].Get(1, -1));
				for (int i = 1; i < splitted.Length; i++)
				{
					string[] parameter = splitted[i].Split(new char[] { '=' }, 2);
					switch (parameter[0].Trim())
					{
						case "rel":
							result.Relatation = parameter[0].Trim().Parse<LinkRelation>();
							break;
					}
				}
			}
			return result; 
		}
	}
}

