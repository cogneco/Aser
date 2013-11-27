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
namespace Aser.Test.Front
{
	public class Items :
	Rest.CollectionHandler<Back.Items, Back.Item>
	{
		Items(Uri.Locator locator, params Back.Item[] data) :
			base(locator, new Back.Items(data))
		{
		}
		protected override Rest.ResourceHandler<Back.Item> Map(Back.Item resource)
		{
			return Item.Create(this, resource);
		}
		public static Items Create(Uri.Locator locator)
		{
			Back.Item[] data = new Back.Item[100];
			for (int i = 0; i < 100; i++)
				data[i] = new Back.Item();
			return new Items(locator, data);
		}
	}
}
