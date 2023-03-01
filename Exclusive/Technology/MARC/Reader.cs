using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiMFa.Exclusive.Technology.MARC
{
    /*This MARCReader is implemented differently then the Pymarc reader is
     * 
     */
    public class Reader : IEnumerable
    {
        string recordFileName = "";
        StreamReader marcStream;

        public Reader(string fileName)
        {
            this.recordFileName = fileName;
            this.marcStream = new StreamReader(fileName,Encoding.Default);

        }

        public void Close()
        {
            this.marcStream.Close();
        }


        public IEnumerator GetEnumerator()
        {
            return new MARCRecordEnumerator(this);
        }


        public class MARCRecordEnumerator : IEnumerator
        {
            Record current = null;
            StreamReader file;

            public MARCRecordEnumerator(Reader reader)
            {
                this.file = reader.marcStream;
            }

            public bool MoveNext()
            {
                if (this.file.Peek() != -1)
                {
                    this.ReadRecord();
                    return true;
                }
                else
                {
                    return false;
                }

            
            }

            private void ReadRecord()
            {
                char[] headerBuffer = new char[5];
                char[] recordBuffer;
                int recordLength;
                this.file.ReadBlock(headerBuffer, 0, 5);
                recordLength = Int32.Parse(new string(headerBuffer));
                recordBuffer = new char[recordLength - headerBuffer.Length];
                this.file.ReadBlock(recordBuffer,0,recordLength-5);
                this.current=new Record(new string(headerBuffer)+new string(recordBuffer));
            }

            public void Reset()
            { 
            
            }
            public object Current
            {
                get
                {
                    return this.current;
                }
            }





            public void Dispose()
            {
                this.file.Close();
            }
        }
        
    }
}
