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
using Serialize = Kean.Serialize;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
namespace Aser.Test.Back
{
	public class Item :
	Rest.IResource
	{
		internal Collection.IList<Item> List { get; set; }
		[Serialize.Parameter]
		public long Key { get; set; }
		[Serialize.Parameter]
		public string Name { get; set; }
		[Serialize.Parameter]
		public string Description { get; set; }
		public Item()
		{
		}
		public bool Save()
		{
			return true;
		}
		public bool Remove()
		{
			return this.List.Remove(item => item.Key == this.Key).NotNull();
		}
		public override string ToString()
		{
			return string.Format("[Item: Key={0}, Name={1}, Description={2}]", this.Key, this.Name, this.Description);
		}
	}
}

