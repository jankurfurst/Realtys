
namespace Realtys.Database
{
    public class RE_database
    {
        //readonly SQLiteAsyncConnection _database;

        //public RE_database(string dbPath)
        //{
        //    _database = new SQLiteAsyncConnection(dbPath);
        //    _database.CreateTableAsync<Mortgage>().Wait();
        //    _database.CreateTableAsync<RealEstate>().Wait();
        //}

        ///// <summary>
        ///// Function to get all saved items from RealEstate table
        ///// </summary>
        ///// <returns>List of RealEstate items</returns>
        //public Task<List<RealEstate>> GetREs_Async()
        //{
        //    return _database.Table<RealEstate>().ToListAsync();
        //}

        ///// <summary>
        ///// Function to get one saved item from RealEstate table
        ///// </summary>
        ///// <param name="id"> ID of required item</param>
        ///// <returns>RealEstate item or null</returns>
        //public Task<RealEstate> GetRE_Async(int id)
        //{
        //    return _database.Table<RealEstate>()
        //                    //.Where(i => i.ID == id)
        //                    .FirstOrDefaultAsync(i => i.ID == id);
        //}

        ///// <summary>
        ///// Function to get one saved item from Mortgage table
        ///// </summary>
        ///// <param name="hypoID"> ID of required item</param>
        ///// <returns> Mortgage item or null</returns>
        //internal Task<Mortgage> GetMortgageAsync(int hypoID)
        //{
        //    return _database.Table<Mortgage>().FirstOrDefaultAsync(i => i.ID == hypoID);
        //}


        //public Task<int> SaveRE_Async(RealEstate re)
        //{
        //    //RealEstate realEstate = eshopDbContext.CarouselItems
        //    //                                        .Where(ci => ci.ID == ID)
        //    //                                        .FirstOrDefault();

        //    //if (realEstate != null)
        //    //{
        //    //    eshopDbContext.CarouselItems.Remove(realEstate);

        //    //    await eshopDbContext.SaveChangesAsync();
        //    //}
        //    //RealEstate realEstate = _database.Table<RealEstate>().FirstOrDefaultAsync(i => i.ID == re.ID);

        //    //if (mort != null)
        //    //{
        //    //    return _database.UpdateAsync(re);
        //    //}
        //    //else
        //    //{
        //    //    return _database.InsertAsync(re);
        //    //}

        //    if (re.ID != 0)
        //    {
        //        return _database.UpdateAsync(re);
        //    }
        //    else
        //    {
        //        return _database.InsertAsync(re);
        //    }
        //}

        //public Task<int> DeleteRE_Async(RealEstate re)
        //{
        //    return _database.DeleteAsync(re);
        //}
    }
}
