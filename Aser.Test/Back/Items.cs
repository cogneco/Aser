//
//  Items.cs
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
namespace Aser.Test.Back
{
	public class Items :
	Rest.ICollection<Item>
	{
		Collection.List<Item> list = new Collection.List<Item>();
		long nextKey;
		public int Count { get { return this.list.Count; } }
		public Items(Generic.IEnumerable<Item> items)
		{
			foreach (Item item in items)
			{
				this.Create(item);
				item.Name = "Name " + item.Key.AsString();
				item.Description = "Description of item " + item.Key.AsString();
			}
		}
		public long Create(Item resource)
		{
			this.list.Add(resource);
			resource.List = this.list;
			return resource.Key = this.nextKey++;
		}
		public Item Open(long key)
		{
			return this.list.Find(item => item.Key == key);
		}
		public Generic.IEnumerable<Item> Open(int limit, int offset)
		{
			for (int i = offset; i < offset + limit && i < this.list.Count; i++)
				yield return this.list[i];
		}
	}
}

