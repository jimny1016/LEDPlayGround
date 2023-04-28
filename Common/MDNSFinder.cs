using System.Collections.Generic;
using System.Threading.Tasks;
using Zeroconf;

public class MDNSFinder
{
    private readonly string _serviceType;

    public MDNSFinder(string serviceType)
    {
        _serviceType = serviceType;
    }

    public async Task<IReadOnlyList<IZeroconfHost>> FindDevicesAsync()
    {
        return await ZeroconfResolver.ResolveAsync(_serviceType);
    }
}