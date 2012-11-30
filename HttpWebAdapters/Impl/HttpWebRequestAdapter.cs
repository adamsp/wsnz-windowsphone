#region license
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
using System.Security.Cryptography.X509Certificates;

namespace HttpWebAdapters.Adapters {
	public class HttpWebRequestAdapter : IHttpWebRequest {
		private HttpWebRequest request;

		public HttpWebRequestAdapter(HttpWebRequest request) {
			this.request = request;
		}

		public HttpWebRequestMethod Method {
			get { return HttpWebRequestMethod.Parse(request.Method); }
			set { request.Method = value.ToString(); }
		}

        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state) {
            return request.BeginGetResponse(callback, state);
        }

        public IHttpWebResponse EndGetResponse(IAsyncResult result) {
            return new HttpWebResponseAdapter(request.EndGetResponse(result));
        }

        public IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state) {
            return request.BeginGetRequestStream(callback, state);
        }

        public Stream EndGetRequestStream(IAsyncResult result) {
            return request.EndGetRequestStream(result);
        }

		///<summary>
		///Cancels a request to an Internet resource.
		///</summary>
		///<PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		public void Abort() {
			request.Abort();
		}

		///<summary>
		///Gets or sets a value that indicates whether the request should follow redirection responses.
		///</summary>
		///
		///<returns>
		///true if the request should automatically follow redirection responses from the Internet resource; otherwise, false. The default value is true.
		///</returns>
		///
		public bool AllowAutoRedirect {
			get { return request.AllowAutoRedirect; }
			set { request.AllowAutoRedirect = value; }
		}

        /// <summary>
        /// When overridden in a descendant class, gets or sets a value that indicates whether to buffer the data read from the Internet resource.
        /// </summary>
        /// 
        /// <returns>
        /// true to enable buffering of the data received from the Internet resource; false to disable buffering. The default is true.
        /// </returns>
        /// 
        public bool AllowReadStreamBuffering {
            get { return request.AllowReadStreamBuffering; }
            set { request.AllowReadStreamBuffering = value; }
        }

		///<summary>
		///Gets a value that indicates whether a response has been received from an Internet resource.
		///</summary>
		///
		///<returns>
		///true if a response has been received; otherwise, false.
		///</returns>
		///
		public bool HaveResponse {
			get { return request.HaveResponse; }
		}

		///<summary>
		///Gets or sets the cookies associated with the request.
		///</summary>
		///
		///<returns>
		///A <see cref="T:System.Net.CookieContainer"></see> that contains the cookies associated with this request.
		///</returns>
		///
		public CookieContainer CookieContainer {
			get { return request.CookieContainer; }
			set { request.CookieContainer = value; }
		}

		///<summary>
		///Gets the original Uniform Resource Identifier (URI) of the request.
		///</summary>
		///
		///<returns>
		///A <see cref="T:System.Uri"></see> that contains the URI of the Internet resource passed to the <see cref="M:System.Net.WebRequest.Create(System.String)"></see> method.
		///</returns>
		///
		public Uri RequestUri {
			get { return request.RequestUri; }
		}


		///<summary>
		///Gets or sets authentication information for the request.
		///</summary>
		///
		///<returns>
		///An <see cref="T:System.Net.ICredentials"></see> that contains the authentication credentials associated with the request. The default is null.
		///</returns>
		///<PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		public ICredentials Credentials {
			get { return request.Credentials; }
			set { request.Credentials = value; }
		}

		///<summary>
		///Gets or sets a <see cref="T:System.Boolean"></see> value that controls whether default credentials are sent with requests.
		///</summary>
		///
		///<returns>
		///true if the default credentials are used; otherwise false. The default value is false.
		///</returns>
		///
		///<exception cref="T:System.InvalidOperationException">You attempted to set this property after the request was sent.</exception><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="USERNAME" /></PermissionSet>
		public bool UseDefaultCredentials {
			get { return request.UseDefaultCredentials; }
			set { request.UseDefaultCredentials = value; }
		}

		///<summary>
		///Specifies a collection of the name/value pairs that make up the HTTP headers.
		///</summary>
		///
		///<returns>
		///A <see cref="T:System.Net.WebHeaderCollection"></see> that contains the name/value pairs that make up the headers for the HTTP request.
		///</returns>
		///
		///<exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream"></see>, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)"></see>, <see cref="M:System.Net.HttpWebRequest.GetResponse"></see>, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)"></see> method. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
		public WebHeaderCollection Headers {
			get { return request.Headers; }
			set { request.Headers = value; }
		}

		///<summary>
		///Gets or sets the value of the Content-type HTTP header.
		///</summary>
		///
		///<returns>
		///The value of the Content-type HTTP header. The default value is null.
		///</returns>
		///
		public string ContentType {
			get { return request.ContentType; }
			set { request.ContentType = value; }
		}

		///<summary>
		///Gets or sets the value of the Accept HTTP header.
		///</summary>
		///
		///<returns>
		///The value of the Accept HTTP header. The default value is null.
		///</returns>
		///
		public string Accept {
			get { return request.Accept; }
			set { request.Accept = value; }
		}

		///<summary>
		///Gets or sets the value of the User-agent HTTP header.
		///</summary>
		///
		///<returns>
		///The value of the User-agent HTTP header. The default value is null.The value for this property is stored in <see cref="T:System.Net.WebHeaderCollection"></see>. If WebHeaderCollection is set, the property value is lost.
		///</returns>
		///
		public string UserAgent {
			get { return request.UserAgent; }
			set { request.UserAgent = value; }
		}
	}
}