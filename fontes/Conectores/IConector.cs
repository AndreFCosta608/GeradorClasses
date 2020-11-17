using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GeraClasses.Conectores {
    public interface IConector {
        DataSet getDataSet(string strQuery);
        bool execNonQuery(string str);
        string execScalar(string str);
        void SetaPropert(string servidor, string schema, string usuario, string senha);
    }
}
