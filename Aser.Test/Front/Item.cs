//
//  Item.cs
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
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;
using Integer = Kean.Math.Integer;
using Serialize = Kean.Serialize;
namespace Aser.Test.Front
{
	public class Item :
	Rest.ResourceHandler<Back.Item>
	{
		public long Key { get { return this.Backend.Key; } }
		internal Collection.IList<Item> List { private get; set; }
		Item(Uri.Locator locator, Back.Item backend) :
			base(locator, backend)
		{
		}
		protected override Http.Status Put(Back.Item @new)
		{
			this.Backend = @new;
			return Http.Status.Accepted;
		}
		protected override Http.Status Delete()
		{
			return this.List.RemoveFirst(item => item.Backend.Key == this.Backend.Key).NotNull() ? Http.Status.Accepted : Http.Status.InternalServerError;
		}
		public static Item Create(Uri.Locator parentLocator, int key)
		{
			return Item.Create(parentLocator, new Back.Item(key));
		}
		public static Item Create(Uri.Locator parentLocator, Back.Item backend)
		{
			return new Item(parentLocator + backend.Key.AsString(), backend);
		}
	}
}

