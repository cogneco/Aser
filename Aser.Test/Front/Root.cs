//
//  Root.cs
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
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;
using Serialize = Kean.Serialize;
namespace Aser.Test.Front
{
	public class Root :
	Rest.Handler
	{
		public override bool Exists { get { return true; } }
		[Serialize.Parameter]
		public Uri.Locator ItemsUrl { get { return this.Url + "items"; } }
		public override Aser.Rest.Handler this [string head]
		{ 
			get { return head == "items" ? Items.Create(this.Url + "items") : base[head]; }
		}
		Root(Uri.Locator resource) :
			base(resource)
		{
		}
		public static Root Create(Uri.Locator resource)
		{
			return new Root(resource);
		}
	}
}

