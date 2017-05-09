﻿using Dorado.Extensions;
using Dorado.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace L6nDataFetcher.Services
{
    public enum MediaType
    {
        Json,
        Xml,
        Text
    }

    public enum HttpMethod
    {
        Post,
        Get
    }

    public class RequestService
    {
        public RequestService(string url)
        {
            HttpMethod = HttpMethod.Get;
            MediaType = MediaType.Text;
            Url = url;
            UrlBuffer = new StringBuilder(url.Contains("?") ? url : url + "?");
            GetParameters = new Dictionary<string, string>();
            PostParameters = new Dictionary<string, string>();
            Encoder = Encoding.UTF8;
        }

        public Encoding Encoder { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, string> GetParameters { get; private set; }

        public Dictionary<string, string> PostParameters { get; private set; }

        public StringBuilder UrlBuffer { get; private set; }

        public HttpMethod HttpMethod { get; private set; }

        public CookieContainer CookieContainer { get; private set; }

        public MediaType MediaType { get; private set; }

        public RequestService SetCookieContainer(CookieContainer cookieContainer)
        {
            CookieContainer = cookieContainer;
            return this;
        }

        public RequestService SetHttpMethod(HttpMethod httpMethod)
        {
            HttpMethod = httpMethod;
            return this;
        }

        public RequestService SetEncoder(Encoding encoder)
        {
            Encoder = encoder;
            return this;
        }

        public RequestService SetEncoder(string encoderName)
        {
            Encoder = Encoding.GetEncoding(encoderName.ToLower());
            return this;
        }

        public RequestService SetMediaType(MediaType mediaType)
        {
            MediaType = mediaType;
            return this;
        }

        public RequestService AddParams(string name, string value, bool isThrowErrorIfParamExist = false)
        {
            if (GetParameters.ContainsKey(name) && isThrowErrorIfParamExist)
                throw new Exception("Get序列中已包含有此名称的参数");
            GetParameters[name] = value;
            return this;
        }

        public RequestService AddPostParams(string name, string value, bool isThrowErrorIfParamExist = false)
        {
            if (PostParameters.ContainsKey(name) && isThrowErrorIfParamExist)
                throw new Exception("Post序列中已包含有此名称的参数");
            PostParameters[name] = value;
            return this;
        }

        public RequestService RemoteParams(string name)
        {
            if (GetParameters.ContainsKey(name))
                GetParameters.Remove(name);
            return this;
        }

        public RequestService RemovePostParams(string name)
        {
            if (PostParameters.ContainsKey(name))
                PostParameters.Remove(name);
            return this;
        }

        public RequestService ClearParams()
        {
            GetParameters = new Dictionary<string, string>();
            PostParameters = new Dictionary<string, string>();
            return this;
        }

        public RequestService Reset()
        {
            return new RequestService(Url);
        }

        public void JoinParameter()
        {
            if (GetParameters.Count == 0)
                return;

            var index = 0;
            GetParameters.ForEach(n =>
            {
                if (index > 0)
                    UrlBuffer.Append("&");
                var urlEncode = n.Value.UrlEncode();
                if (urlEncode != null)
                    UrlBuffer.Append(string.Format("{0}={1}", n.Key,
                        urlEncode));
                index++;
            });
        }

        public T Convert<T>(string result)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)result;
            }

            switch (MediaType)
            {
                case MediaType.Json:
                    return new JsonNetConverter().Deserialize<T>(result);

                case MediaType.Xml:
                    return SerializeUtility.DeserializeXml<T>(result);

                default:
                    return (T)(object)result;
            }
        }
    }
}