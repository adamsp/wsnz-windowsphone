﻿#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.IO;
using System.Net;

namespace HttpWebAdapters {
	public interface IHttpWebResponse : IDisposable {

		/// <summary>
		///Gets the status of the response.
		/// </summary>
		///
		/// <returns>
		///One of the <see cref="T:System.Net.HttpStatusCode"></see> values.
		/// </returns>
		///
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		HttpStatusCode StatusCode { get; }

		/// <summary>
		///Gets the status description returned with the response.
		/// </summary>
		///
		/// <returns>
		///A string that describes the status of the response.
		/// </returns>
		///
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		string StatusDescription { get; }

		/// <summary>
		///When overridden by a descendant class, closes the response stream.
		/// </summary>
		///
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to access the method, when the method is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		void Close();

		/// <summary>
		///When overridden in a descendant class, returns the data stream from the Internet resource.
		/// </summary>
		///
		/// <returns>
		///An instance of the <see cref="T:System.IO.Stream"></see> class for reading data from the Internet resource.
		/// </returns>
		///
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to access the method, when the method is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		Stream GetResponseStream();

		/// <summary>
		///When overridden in a descendant class, gets or sets the content length of data being received.
		/// </summary>
		///
		/// <returns>
		///The number of bytes returned from the Internet resource.
		/// </returns>
		///
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		long ContentLength { get; }

		/// <summary>
		///When overridden in a derived class, gets or sets the content type of the data being received.
		/// </summary>
		///
		/// <returns>
		///A string that contains the content type of the response.
		/// </returns>
		///
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		string ContentType { get; }

		/// <summary>
		///When overridden in a derived class, gets the URI of the Internet resource that actually responded to the request.
		/// </summary>
		///
		/// <returns>
		///An instance of the <see cref="T:System.Uri"></see> class that contains the URI of the Internet resource that actually responded to the request.
		/// </returns>
		///
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		Uri ResponseUri { get; }

		/// <summary>
		///When overridden in a derived class, gets a collection of header name-value pairs associated with this request.
		/// </summary>
		///
		/// <returns>
		///An instance of the <see cref="T:System.Net.WebHeaderCollection"></see> class that contains header values associated with this response.
		/// </returns>
		///
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		WebHeaderCollection Headers { get; }
	}
}