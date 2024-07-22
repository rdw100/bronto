using Microsoft.Extensions.Caching.Memory;

namespace Bronto.Shared
{
    /// <summary>
    /// Defines static cache entry property options for memory.
    /// </summary>
    /// <remarks>
    /// Use a factory method for dynamic cache entry options.
    /// </remarks>
    public static class CacheOptions
    {
        public static MemoryCacheEntryOptions Default => new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(120))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetPriority(CacheItemPriority.Normal)
            .SetSize(1024);
    }
}
