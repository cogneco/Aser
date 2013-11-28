//
//  Path.cs
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
using Uri = Kean.Uri;
using Kean;
using Kean.Extension;

namespace Aser.Rest
{
	public class Path :
    IEquatable<Path>
	{
		public string Head { get; private set; }
		public Path Tail { get; private set; }
		Path()
		{
		}
		#region Object Overrides
		public override int GetHashCode()
		{
			return this.Head.Hash() ^ this.Tail.Hash();
		}
		public override bool Equals(object other)
		{
			return other is Path && (this as IEquatable<Path>).Equals(other as Path);
		}
		public override string ToString()
		{
			return this.Tail.NotNull() ? this.Head + "/" + this.Tail.ToString() : this.Head;
		}
		#endregion
		#region IEquatable implementation
		bool IEquatable<Path>.Equals(Path other)
		{
			return other.NotNull() && this.Head == other.Head && this.Tail.SameOrEquals(other.Tail);
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Path left, Path right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(Path left, Path right)
		{
			return !left.SameOrEquals(right);
		}
		#endregion
		public static implicit operator Path(Uri.Path path)
		{
			Path result = Path.Build(path.Last, null);
			Console.WriteLine("path: " + result);
			return result;
		}
		static Path Build(Uri.PathLink current, Path tail)
		{
			Path result = current.NotNull() ? Path.Build(current.Tail, current.Head.NotEmpty() ? new Path() { Head = current.Head, Tail = tail } : tail) : tail;
			return result;
		}
	}
}

