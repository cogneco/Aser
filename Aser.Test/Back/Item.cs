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
using DB = Kean.DB;
namespace Aser.Test.Back
{
	public class Item :
	Rest.Item<Item>
	{
		[DB.Index]
		public string Name { get; set; }
		[DB.Data]
		public string Description { get; set; }
		public Item()
		{
		}
		public Item(int key) :
			base(key)
		{
			this.Name = "Name " + key.AsString();
			this.Description = "Description of item " + key.AsString();
		}
		public void SetKey(long key)
		{
			this.Key = key;
		}
		public override string ToString()
		{
			return string.Format("[Item: Key={0}, Name={1}, Description={2}]", this.Key, this.Name, this.Description);
		}
	}
}

