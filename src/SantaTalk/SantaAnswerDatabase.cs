using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SantaTalk.Models;
using SQLite;

namespace SantaTalk
{
    public class SantaAnswerDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public SantaAnswerDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(SantaResultModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(SantaResultModel)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<List<SantaResultModel>> GetItemsAsync()
        {
            return Database.Table<SantaResultModel>().ToListAsync();
        }

        //public Task<List<SantaResultDisplay>> GetItemsNotDoneAsync()
        //{
        //    return Database.QueryAsync<SantaResultDisplay>("SELECT * FROM [SantasCommentsService]");
        //}

        //public Task<SantaResultDisplay> GetItemAsync(int id)
        //{
        //    return Database.Table<SantaResultDisplay>().Where(i => i. == id).FirstOrDefaultAsync();
        //}

        public Task<int> SaveItemAsync(SantaResultModel item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(SantaResultModel item)
        {
            return Database.DeleteAsync(item);
        }
    }
}