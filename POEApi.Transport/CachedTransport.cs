using System.IO;
using POEApi.Infrastructure;
using System.Security;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    class CachedTransport : ITransport
    {
        private ITransport innerTranport;
        private const string stashKey = "stash";
        private CacheService userCacheService;
        private CacheService commonCacheService;

        public event ThottledEventHandler Throttled;

        public CachedTransport(string email, ITransport innerTranport)
        {
            userCacheService = new CacheService(email);
            commonCacheService = new CacheService();
            this.innerTranport = innerTranport;
            this.innerTranport.Throttled += instance_Throttled;
        }

        private void instance_Throttled(object sender, ThottledEventArgs e)
        {
            if (Throttled != null)
                Throttled(sender, e);
        }

        public bool Authenticate(string email, SecureString password)
        {
            return innerTranport.Authenticate(email, password);
        }

        public Stream GetStash(int index, string league, bool refresh)
        {
            string key = string.Format("{0}-{1}-{2}", league, stashKey, index);

            if (refresh && userCacheService.Exists(key))
                userCacheService.Remove(key);

            if (!userCacheService.Exists(key))
                userCacheService.Set(key, innerTranport.GetStash(index, league));

            return userCacheService.Get(key);
        }

        public Stream GetStash(int index, string league)
        {
            return GetStash(index, league, false);
        }

        public Stream GetImage(string url)
        {
            string key = string.Concat(url.GetHash(), ".png");
            if (!commonCacheService.Exists(key))
                commonCacheService.Set(key, innerTranport.GetImage(url));

            return commonCacheService.Get(key);
        }

        public Stream GetCharacters()
        {
            string key ="characterdata";

            if (!userCacheService.Exists(key))
                userCacheService.Set(key, innerTranport.GetCharacters());

            return userCacheService.Get(key);
        }

        public Stream GetInventory(string characterName)
        {
            if (!userCacheService.Exists(characterName))
                userCacheService.Set(characterName, innerTranport.GetInventory(characterName));

            return userCacheService.Get(characterName);
        }
    }
}