using SantaTalk.Helpers;
using SantaTalk.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace SantaTalk.Services
{
    public class MobileServiceClientService
    {
        private Helper helper;

        #region Singleton
        private static MobileServiceClientService instance;

        private static readonly object syncRoot = new object();
        public static MobileServiceClientService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MobileServiceClientService();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public MobileServiceClientService()
        {
            InitializeAsync().SafeFireAndForget(false);

            helper = new Helper();
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(PinModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(PinModel)).ConfigureAwait(false);

                    initialized = true;
                }
            }
        }

        public async Task<List<PinModel>> GetPinModelAsync()
        {
            return await Database.Table<PinModel>().ToListAsync();
        }

        public async Task<int> SavePositionAsync(string kidName)
        {
            Location pos = await helper.OnGetCurrentLocation();

            Position position = new Position(pos.Latitude, pos.Longitude);

            PinModel item = new PinModel
            {
                Lat = position.Latitude,
                Log = position.Longitude,
                Label = kidName ?? "S/N",
                Type = (int)PinType.Place
            };

            try
            {
                if (item.Id == null)
                {
                    item.Id = Guid.NewGuid().ToString();

                    return await Database.InsertAsync(item);
                }
                else
                { 
                    return await Database.UpdateAsync(item);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        public Task<int> DeleteItemAsync(PinModel item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
