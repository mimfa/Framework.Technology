using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiMFa.Exclusive.Technology.MARC
{
    /*
     * Write Marctransmission/MARCRecords as human readable, MARCBreaker format.
     */ 
    public class BreakerWriter : Writer
    {
        public BreakerWriter(string filename): base(filename)
        {
           
        }

        public override void Write(Record record)
        {
            fileStream.Write(record.MARCMakerFormat());
        }

    }
}
