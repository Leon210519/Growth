using System.Threading.Tasks;

namespace Game.Core.SaveSystem
{
    public interface ISaveProvider
    {
        Task SaveAsync(string data);
        Task<string> LoadAsync();
    }
}
