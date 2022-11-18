using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;

namespace Ventas_DAL
{
    public class FEL_DAL:IDisposable
    {
        public void Dispose() { }
        public List<FEL_BE> GetDatosSP(FEL_BE item)
        {
            List<FEL_BE> result = new List<FEL_BE>();
            using (var model = new Base_SQL("sp_fel"))
            {
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<FEL_BE>();
            }
            return result;
        }
    }
}
