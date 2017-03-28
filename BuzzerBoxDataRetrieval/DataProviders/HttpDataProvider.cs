using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using BuzzerBoxDataRetrieval.Helpers;
using BuzzerBoxDataRetrieval.Network;
using BuzzerEntities.Models;
using Newtonsoft.Json;

namespace BuzzerBoxDataRetrieval.DataProviders
{
    /// <summary>
    /// Http-based implementation of <see cref="IDataProvider{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpDataProvider<T> : IDataProvider<T>
    {
        private const string TAG = "HttpDataProvider";

        public event EventHandler<ItemsChangedEventArgs<T>> ItemsChanged;
        protected IHttpConnection connection;
        protected IHttpDataRequestFactory requestFactory;
        protected IStringDataConverter<T> stringConverter;

        public HttpDataProvider(IHttpConnection connection, IHttpDataRequestFactory requestFactory, IStringDataConverter<T> stringConverter, string sessionToken)
        {
            this.connection = connection;
            this.requestFactory = requestFactory;
            this.requestFactory.SetDefaultUrlParameters("sessionToken", sessionToken);
            this.stringConverter = stringConverter;
        }

        public async Task<List<T>> LoadItems()
        {
            var request = requestFactory.CreateGet<T>(true);
            var response = await connection.LoadFromUrl(request);

            if (response.IsSuccess)
            {
                var items = stringConverter.ParseItems(response.Content);
                return items;
            }
            else
            {
                throw new LoadItemException("Could not retrieve content from remote endpoint.", response.Exception);
            }
        }

        public async Task<T> LoadItem(int itemId)
        {
            var request = requestFactory.CreateGet<T>(false).AddPathParameter(null, itemId);
            var response = await connection.LoadFromUrl(request);

            if (response.IsSuccess)
            {
                var item = stringConverter.ParseItem(response.Content);
                return item;
            }
            else
            {
                throw new LoadItemException("Could not retrieve content from remote endpoint.", response.Exception);
            }
        }
    }
}