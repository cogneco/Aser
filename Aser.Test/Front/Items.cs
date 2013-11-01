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
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;
using Integer = Kean.Math.Integer;
namespace Aser.Test.Front
{
	public class Items :
	Rest.CollectionHandler<Back.Item>
	{
		Collection.IList<Item> data;
		protected override int Count { get { return this.data.Count; } }
		Items(Uri.Locator resource, params Item[] data) :
			base(resource)
		{
			this.data = new Collection.Array.List<Item>(data);
		}
		protected override Rest.ResourceHandler<Back.Item> Route(string identifier)
		{
			int key;
			return int.TryParse(identifier, out key) ? this.data[key] : base.Route(identifier);
		}
		protected override Generic.IEnumerable<Rest.ResourceHandler<Back.Item>> Get(int limit, int offset)
		{
			for (int i = offset; i < offset + limit && i < data.Count; i++)
				yield return data[i];
		}
		protected override Rest.ResourceHandler<Back.Item> Post(Back.Item item)
		{
			item.SetKey(this.data.Count);
			Item result = Item.Create(this.Locator, item);
			this.data.Add(result);
			Console.WriteLine(item);
			Console.WriteLine(new System.Diagnostics.StackTrace());
			return result;
		}
		public static Items Create(Uri.Locator resource)
		{
			Item[] data = new Item[100];
			for (int i = 0; i < 100; i++)
				data[i] = Item.Create(resource, i);
			return new Items(resource, data);
		}
	}
}
