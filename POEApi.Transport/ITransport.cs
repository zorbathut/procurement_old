using System.IO;
using System.Security;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    public interface ITransport
    {
        bool Authenticate(string email, SecureString password);
        Stream GetStash(int index, string league);
        Stream GetStash(int index, string league, bool refresh);
        Stream GetImage(string url);
        Stream GetCharacters();
        Stream GetInventory(string characterName);
        Stream GetPOEExInfoRates();
        event ThottledEventHandler Throttled;
    }
}
