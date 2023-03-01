using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace MiMFa.Exclusive.Technology.MARC
{
    /*
     * Class that writes to MARC transmission format
     */
    public class Writer
    {
        public delegate Record MARCProcess(Record record);

        protected StreamWriter fileStream;

        public Writer(string filename)
        {
            this.fileStream = new StreamWriter(filename);
        }

        public virtual void Write(Record record)
        {
            this.fileStream.Write(record.AsMARC21());
        }
        
        public void Write(Record[] recordArray)
        {
            foreach (Record record in recordArray)
            { 
                Write(record);
            }
        }


        /*
         * Applies a process to a record, and then write that record.
         * note: the process must return a MARCrecord
         */ 
        public void ProcessThenWrite(MARCProcess process, Record record)
        {
            Write(process(record));
        }

        public void ProcessThenWrite(MARCProcess process, Record[] records)
        {
            foreach(Record record in records)
            {
                ProcessThenWrite(process, record);
            }

        }

        public void ProcessThenWrite(MARCProcess process, Reader records)
        {
            foreach (Record record in records)
            {
                ProcessThenWrite(process, record);
            }

        }

        

        public void Close()
        {
            this.fileStream.Close();
            this.fileStream.Dispose();
        }

    }
}
