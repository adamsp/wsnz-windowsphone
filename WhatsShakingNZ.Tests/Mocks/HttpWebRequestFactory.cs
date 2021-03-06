﻿using System;
using HttpWebAdapters;

namespace WhatsShakingNZ.Tests.Mocks
{
    public class HttpWebRequestFactory : IHttpWebRequestFactory {
        public Func<Uri, IHttpWebRequest> create;

        public IHttpWebRequest Create(Uri url) {
            return create(url);
        }
    }
}