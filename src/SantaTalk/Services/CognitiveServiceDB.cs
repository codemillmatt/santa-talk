using SantaTalk.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SantaTalk.Helpers;
using System.Linq;
using Newtonsoft.Json;

namespace SantaTalk.Services
{
   public class CognitiveServiceDB
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public CognitiveServiceDB()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(VisionModel).Name))
                {
                    await Database.CreateTablesAsync( CreateFlags.None,typeof(VisionModel)).ConfigureAwait(false);

                    initialized = true;
                }
            }
        }

        public async Task<List<VisionModel>> GetItemsAsync()
        {
            
            return  await Database.Table<VisionModel>().ToListAsync();
            
           
        }

        public Task<List<VisionModel>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<VisionModel>("SELECT * FROM [VisionModel] WHERE [Done] = 0");
        }

        public Task<VisionModel> GetItemAsync(int id)
        {
            return Database.Table<VisionModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(VisionModel item)
        {
            if (item.ID != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            { 
               
                return await Database.InsertAsync(item); 

            }
        }

        public Task<int> DeleteItemAsync(VisionModel item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
