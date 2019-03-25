using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static External.Migration.Import;

namespace External.Migration
{
    public interface DataReader
    {

        void Initialize();

        // public String[] splitLine();

        //void Close();

        void Load(FileStream fs);

    }
}
