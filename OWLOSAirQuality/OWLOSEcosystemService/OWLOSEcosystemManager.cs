using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWLOSAirQuality.OWLOSEcosystemService
{
    public class OWLOSEcosystemManager
    {
        public List<OWLOSEcosystemServiceClient> OWLOSEcosystemServiceClients = new List<OWLOSEcosystemServiceClient>();

        public OWLOSEcosystemManager()
        {
            OWLOSEcosystemServiceClients.Add(new OWLOSEcosystemServiceClient()
            {
                Name = "OWLOS ORG",
                URL = "http://airquality.owlos.org/Things/",
                Token = "WG8xNTc1T29ONTFDbkdUd1NUOU1xVWRqVHhzbGIwOVJFN2xibU5RNnJpMFFBQUFBY3I4SERwTlVKWENsMjF4a1lWZG9OSlJBTDl0aDAwTWUzMk9ub2JYN2YvQUZmMWdESVZ1akE4c3NTUHcwbHkxWVc3bWd0N1JXcVVWTEZYQzRPajYwdTBIdFBVVHBFb0VjeXhyeGZCZWNDRVhmWWpzSTZDamxsdjAzR0dWY0JFL3dRSHZkL2llWE4wcmE4eFJsVFlFdmtRPT0%3D"
            });

            OWLOSEcosystemServiceClients.Add(new OWLOSEcosystemServiceClient()
            {
                Name = "Local",
                URL = "https://192.168.1.100:5004/Things/",
                Token = "UjJsdzBlbEF2ZmN6dUU2clpOOTBEbWp5SVloaFBxSlBQa0hRTXhKaWNTOFFBQUFBYXJpaW1aUG5SMjhPSDRzWlpNWVhVZVpDbXlSczd1NmR1aEYwYzlvbENvaTQyZWNYVDRHaGkvUlRGOEdreVl5UzM0emg0NWlJK0tNYXljWW8vWHo1My9OdGtvSTJRYXhxc01hZ3JIT2JzRis0NHFuVFc1SVNCTkJPQ2lxRXRYKzEwYnBESnluUkROR2FMalhyN2ZVSkhnPT0="
            });

        }
    }
}
