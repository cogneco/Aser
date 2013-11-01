//
//  Status.cs
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

namespace Aser.Http
{
	public struct Status
	{
		int code;
		public bool Success
		{
			get { return this.code < 400; }
		}
		public override string ToString()
		{
			return this;
		}
		#region Casts
		public static implicit operator int(Status status)
		{
			return status.code; 
		}
		public static explicit operator Status(int status)
		{
			return new Status() { code = status }; 
		}
		public static implicit operator string(Status status)
		{
			string name;
			switch (status.code)
			{
				case 100:
					name = "Continue";
					break;
				case 101:
					name = "Switching Protocols";
					break;
				case 200:
					name = "OK";
					break;
				case 201:
					name = "Created";
					break;
				case 202:
					name = "Accepted";
					break;
				case 203:
					name = "Non-Authorative Information";
					break;
				case 204:
					name = "No Content";
					break;
				case 205:
					name = "Reset Content";
					break;
				case 206:
					name = "Partial Content";
					break;
				case 300:
					name = "Multiple Choices";
					break;
				case 301:
					name = "Moved Permanently";
					break;
				case 303:
					name = "See Other";
					break;
				case 304:
					name = "Not Modified";
					break;
				case 305:
					name = "Use Proxy";
					break;
				case 307:
					name = "Temporary Redirect";
					break;
				case 308:
					name = "Permanent Redirect";
					break;
				case 400:
					name = "Bad Request";
					break;
				case 401:
					name = "Unauthorized";
					break;
				case 402:
					name = "Payment Required";
					break;
				case 403:
					name = "Forbidden";
					break;
				case 404:
					name = "Not Found";
					break;
				case 405:
					name = "Method Not Allowed";
					break;
				case 406:
					name = "Not Acceptable";
					break;
				case 407:
					name = "Proxy Authentication Required";
					break;
				case 408:
					name = "Request Timeout";
					break;
				case 409:
					name = "Conflict";
					break;
				case 410:
					name = "Gone";
					break;
				case 411:
					name = "Length Required";
					break;
				case 412:
					name = "Precondition Failed";
					break;
				case 413:
					name = "Request Entity Too Large";
					break;
				case 414:
					name = "Request-URI Too Long";
					break;
				case 415:
					name = "Unsupported Media Type";
					break;
				case 416:
					name = "Requested Range Not Satifiable";
					break;
				case 417:
					name = "Expectation Failed";
					break;
				case 419:
					name = "Authentication Timeout";
					break;
				case 426:
					name = "Upgrade Required";
					break;
				case 428:
					name = "Precondition Failed";
					break;
				case 429:
					name = "Too Many Requests";
					break;
				case 431:
					name = "Request Header Fields Too Large";
					break;
				case 500:
					name = "Internal Server Error";
					break;
				case 501:
					name = "Not Implemented";
					break;
				case 502:
					name = "Bad Gateway";
					break;
				case 503:
					name = "Service Unavailable";
					break;
				case 504:
					name = "Gateway Timeout";
					break;
				case 505:
					name = "HttpVersionNotSupported";
					break;
				case 506:
					name = "VariantAlsoNegotiates";
					break;
				case 507:
					name = "Insufficent Storage";
					break;
				case 508:
					name = "Loop Detected";
					break;
				case 509:
					name = "Band Width Limit Exceeded";
					break;
				case 510:
					name = "Not Extended";
					break;
				case 511:
					name = "Network Authetication Required";
					break;
				default:
					name = "Unknown Status";
					break;
			}
			return status.code + " " + name; 
		}
		#endregion
		#region Constants
		// 1xx Informational
		public static Status Continue { get { return (Status)100; } }
		public static Status SwitchingProtocol { get { return (Status)101; } }
		// 2xx Success
		public static Status OK { get { return (Status)200; } }
		public static Status Created { get { return (Status)201; } }
		public static Status Accepted { get { return (Status)202; } }
		public static Status NonAuthorativeInformation { get { return (Status)203; } }
		public static Status NoContent { get { return (Status)204; } }
		public static Status ResetContent { get { return (Status)205; } }
		public static Status PartialContent { get { return (Status)206; } }
		// 3xx Redirection
		public static Status MultipleChoices { get { return (Status)300; } }
		public static Status MovedPermanently { get { return (Status)301; } }
		public static Status SeeOther { get { return (Status)303; } }
		public static Status NotModified { get { return (Status)304; } }
		public static Status UseProxy { get { return (Status)305; } }
		public static Status TemporaryRedirect { get { return (Status)307; } }
		public static Status PermanentRedirect { get { return (Status)308; } }
		// 4xx Client Error
		public static Status BadRequest { get { return (Status)400; } }
		public static Status Unauthorized { get { return (Status)401; } }
		public static Status PaymentRequired { get { return (Status)402; } }
		public static Status Forbidden { get { return (Status)403; } }
		public static Status NotFound { get { return (Status)404; } }
		public static Status MethodNotAllowed { get { return (Status)405; } }
		public static Status NotAcceptable { get { return (Status)406; } }
		public static Status ProxyAutheticationRequired { get { return (Status)407; } }
		public static Status RequestTimeout { get { return (Status)408; } }
		public static Status Conflict { get { return (Status)409; } }
		public static Status Gone { get { return (Status)410; } }
		public static Status LengthRequired { get { return (Status)411; } }
		public static Status PreconditionFailed { get { return (Status)412; } }
		public static Status RequestEntityTooLarge { get { return (Status)413; } }
		public static Status RequestUriTooLong { get { return (Status)414; } }
		public static Status UnsupportedMediaType { get { return (Status)415; } }
		public static Status RequestedRangeNotSatisfiable { get { return (Status)416; } }
		public static Status ExpectationFailed { get { return (Status)417; } }
		public static Status AuthenticationTimeout { get { return (Status)419; } }
		public static Status UpgradeRequired { get { return (Status)426; } }
		public static Status PreconditionRequired { get { return (Status)428; } }
		public static Status TooManyRequests { get { return (Status)429; } }
		public static Status RequestHeaderFieldsTooLarge { get { return (Status)431; } }
		// 5xx Server Error
		public static Status InternalServerError { get { return (Status)500; } }
		public static Status NotImplemented { get { return (Status)501; } }
		public static Status BadGateway { get { return (Status)502; } }
		public static Status ServiceUnavailable { get { return (Status)503; } }
		public static Status GatewayTimeout { get { return (Status)504; } }
		public static Status HttpVersionNotSupported { get { return (Status)505; } }
		public static Status VariantAlsoNegotiates { get { return (Status)506; } }
		public static Status InsufficentStorage { get { return (Status)507; } }
		public static Status LoopDetected { get { return (Status)508; } }
		public static Status BandwithLimitExceeded { get { return (Status)509; } }
		public static Status NotExtended { get { return (Status)510; } }
		public static Status NetworkAuthenticationRequired { get { return (Status)511; } }
		#endregion
	}
}

