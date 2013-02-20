using System.IO;
using System.Security;

namespace POEApi.Transport
{
    public class ChainedTransportService : ITransport
    {
        ITransport transport;

        public ChainedTransportService(string email, SecureString password)
        {
            transport = new CachedTransport(email, new HttpTransport(email, password));
        }

        public ChainedTransportService(string email, SecureString password, string proxyUser, string proxyPassword, string proxyDomain)
        {
            transport = new CachedTransport(email, new HttpTransport(email, password, proxyUser, proxyPassword, proxyDomain));
        }

        public bool Authenticate(string email, SecureString password)
        {
            return transport.Authenticate(email, password);
        }

        public Stream GetStash(int index, string league, bool refresh)
        {
            return transport.GetStash(index, league, refresh);
        }

        public Stream GetStash(int index, string league)
        {
            return transport.GetStash(index, league);
        }

        public Stream GetImage(string url)
        {
            return transport.GetImage(url);
        }


        public Stream GetCharacters()
        {
            return transport.GetCharacters();
        }


        public Stream GetInventory(string characterName)
        {
            return transport.GetInventory(characterName);
        }
    }
}
