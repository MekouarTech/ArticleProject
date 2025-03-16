using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticalProject.Data
{
    public interface IDataHelper<Table>
    {
        // Read
        List<Table> GetAllData();

        List<Table> GetDataByUserID(string userId);

        List<Table> Search(string searchItem);

        Table Find(int id);

        // Write
        int Add(Table table);

        int Edit(int id, Table table);

        int Delete(int id);
    }
}
