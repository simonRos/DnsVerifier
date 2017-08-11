using DnsClient;
using System.Net;
using System.Threading;
namespace DnsLibTest
{
    class DnsCliVersion
    {
        const bool printInfo = false;
        LookupClient dnsClient = null;
        string server = "";
        public bool VerifyDomain(string domain)
        {
            try
            {
                IDnsQueryResponse result = dnsClient.Query(domain, QueryType.A);              
                if ((result == null) || (result.Answers.Count < 1) || (result.HasError))
                {
                    //if doesn't have www.
                    if (domain.Length < 4 || domain.Substring(0,4) != "www.") {
                        //try with www.
                        return VerifyDomain("www." + domain);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (DnsClient.DnsResponseException)
            {
                this.dnsClient = new LookupClient(new IPEndPoint(IPAddress.Parse(server), 53));
                Thread.Sleep(6000);
                return (VerifyDomain(domain));
            }
            return true;
        }
        public DnsCliVersion(string server)
        {
            this.server = server;
            var endpoint = new IPEndPoint(IPAddress.Parse(server), 53);
            var client = new LookupClient(endpoint);
            client.UseCache = false;
            dnsClient = client;
        }
    }
}
