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
		Item(Items parent, Back.Item backend) :
			base(parent.Url + backend.Key.AsString(), backend)
		{
		}
		public static Item Create(Items parent, Back.Item backend)
		{
			return parent.NotNull() && backend.NotNull() ? new Item(parent, backend) : null;
		}
	}
}

