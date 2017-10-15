using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using minileadgen.core.Models;

namespace minileadgen.core.Helpers
{
    public partial class LeadManager
    {
        static LeadManager defaultInstance = new LeadManager();

        const string accountURL = @"https://fabian.documents.azure.com:443/";
        const string accountKey = @"REDACTED";
        const string databaseId = @"fabsalpha";
        const string collectionId = @"Leads";

        private Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

        private DocumentClient client;

        private LeadManager()
        {
            client = new DocumentClient(new System.Uri(accountURL), accountKey);
        }

        public static LeadManager DefaultManager {
            get {
                return defaultInstance;
            }
            private set {
                defaultInstance = value;
            }
        }

        public List<Lead> Leads { get; private set; }

        public async Task<List<Lead>> GetLeadsAsync()
        {
            try
            {
                // The query excludes Leads that have been Converted to Opportunities
                var query = client.CreateDocumentQuery<Lead>(collectionLink, new FeedOptions { MaxItemCount = -1 })
                      .Where(leadItem => leadItem.IsConverted == false)
                      .AsDocumentQuery();

                Leads = new List<Lead>();
                while (query.HasMoreResults)
                {
                    Leads.AddRange(await query.ExecuteNextAsync<Lead>());
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }
            return Leads;
        }

        public async Task<Lead> InsertLeadAsync (Lead lead)
        {
            try
            {
                var result = await client.CreateDocumentAsync(collectionLink, lead);
                lead.pId = result.Resource.Id;
                Leads.Add(lead);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return lead;
        }
    }
}
