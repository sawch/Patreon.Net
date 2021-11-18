using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Patreon.Net.Models;

namespace Patreon.Net
{
    public sealed class PatreonResourceArrayAsyncEnumerator<TResource, TRelationships> : IAsyncEnumerator<TResource> where TResource : PatreonResource<TRelationships> where TRelationships : class
    {
        private readonly PatreonClient _client;
        private readonly string _endpoint;

        private TResource[] resources;
        private string nextPageCursor;
        private int currentIndex;

        internal PatreonResourceArrayAsyncEnumerator(PatreonResourceArray<TResource, TRelationships> initialArray, PatreonClient client, string endpoint)
        {
            if(initialArray == null)
                throw new ArgumentNullException(nameof(initialArray));

            _client = client ?? throw new ArgumentNullException(nameof(client));
            _endpoint = endpoint;

            resources = initialArray.Resources;
            nextPageCursor = initialArray.Meta.Pagination.Cursor?.Next;
            currentIndex = -1;
        }

        public TResource Current => resources?[currentIndex];

        public ValueTask DisposeAsync() { resources = null; return ValueTask.CompletedTask; }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (++currentIndex < resources.Length)
                return true;

            if (nextPageCursor == null)
                return false;

            var nextPageArray = await _client.GetAsync<PatreonResourceArray<TResource, TRelationships>>(Endpoints.Page(_endpoint, nextPageCursor)).ConfigureAwait(false);
            if (nextPageArray == null)
                return false;

            resources = nextPageArray.Resources;
            nextPageCursor = nextPageArray.Meta.Pagination.Cursor?.Next;
            currentIndex = 0;
            return true;
        }
    }
}
