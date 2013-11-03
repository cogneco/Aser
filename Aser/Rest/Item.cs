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
using DB = Kean.DB;
using Serialize = Kean.Serialize;

namespace Aser.Rest
{
	public abstract class Item<T> :
    DB.Item
        where T : Item<T>, new()
	{
		protected Item()
		{
		}
		protected Item(long key) :
			base(key)
		{
		}
		public virtual Serialize.Data.Node Serialize()
		{
			return Kean.Serialize.Storer.Store<T>(this as T);
		}
		public virtual bool Deserialize(Serialize.Data.Node node)
		{
			return Kean.Serialize.Storer.Load(node, this);
		}
	}
}

