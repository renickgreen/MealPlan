using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MealPlan.Models;
using SQLite;

namespace MealPlan.Services
{
    class DatabaseControl
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<DatabaseControl> Instance = new AsyncLazy<DatabaseControl>(async () =>
        {
            var instance = new DatabaseControl();
            CreateTableResult result = await Database.CreateTableAsync<Meal>();
            // CreateTableResult resultPlan = await Database.CreateTableAsync<MealPlanModel>();
            return instance;
        });
        public static readonly AsyncLazy<DatabaseControl> IPlan = new AsyncLazy<DatabaseControl>(async () =>
        {
            var instance = new DatabaseControl();
            CreateTableResult resultPlan = await Database.CreateTableAsync<MealPlanModel>();
            return instance;
        });

        public DatabaseControl()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }
        public Task<List<Meal>> GetItemsAsync()
        {
            return Database.Table<Meal>().ToListAsync();
        }
        public Task<List<MealPlanModel>> GetPlansAsync()
        {
            return Database.Table<MealPlanModel>().ToListAsync();
        }

        public Task<List<Meal>> GetItemsDAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<Meal>("SELECT * FROM [Meal] ORDER BY Date DESC");
        }

        public Task<Meal> GetItemAsync(int id)
        {
            return Database.Table<Meal>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }
        public Task<MealPlanModel> GetPlanAsync(int id)
        {
            return Database.Table<MealPlanModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Meal item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }
        public Task<int> SavePlanAsync(MealPlanModel item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Meal item)
        {
            return Database.DeleteAsync(item);
        }
        public Task<List<Meal>> DeleteAllAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<Meal>("DELETE FROM [Meal]");
        }
    }
}
