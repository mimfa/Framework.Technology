using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace MiMFa.Exclusive.Technology.MARC
{
    public class Record
    {
        /* from the original pymarc:
    A class for representing a MARC record. Each Record object is made up of
    multiple Field objects. You'll probably want to look at the docs for Field
    to see how to fully use a Record object.
         
    You'll normally want to use a MARCReader object to iterate through 
    MARC records in a file.  
         */

        private string leader = "          22        4500";
        private List<Field> fields;

        public Record()
        {
            this.fields = new List<Field> { };
        
        }
        public Record(string data)
        {
            this.fields = new List<Field> { };
            DecodeMARC(data);
        }

        public void DecodeMARC(string marcRecord)
        { 
            if(marcRecord.Length < Features.LEADER_LEN)
            {
                throw new RecordLeaderInvalid();
            }
            //extract record leader
            this.leader = marcRecord.Substring(0, Features.LEADER_LEN);

            // extract the byte offset where the record data starts
            int baseAddress = Int32.Parse(marcRecord.Substring(12, 5));
            if (baseAddress < 0)
            {
                throw new BaseAddressNotFound();
            }
            if (baseAddress >= marcRecord.Length)
            {
                throw new BaseAddressInvalid();
            }

            //extract directory, base_address-1 is used since the
            //directory ends with an END_OF_FIELD byte
            string directory = marcRecord.Substring(Features.LEADER_LEN, (baseAddress - 1) - Features.LEADER_LEN);
            
            //determine number of fields in a record
            if (directory.Length % Features.DIRECTORY_ENTRY_LEN != 0)
            {
                throw new RecordDirectoryInvalid();
            }
            
            int fieldTotal = directory.Length / Features.DIRECTORY_ENTRY_LEN;
            //if there are no fields then raise error
            if (fieldTotal == 0)
            {
                throw new NoFieldsFound();
            }

            //add fields to the record
            for (int fieldCount = 0; fieldCount < fieldTotal; fieldCount++)
            {
                Field field;
                int entryStart = fieldCount * Features.DIRECTORY_ENTRY_LEN;
                int entryEnd = entryStart + Features.DIRECTORY_ENTRY_LEN;
                string entry = directory.Substring(entryStart, entryEnd - entryStart);
                int entryTag = Int32.Parse(entry.Substring(0, 3));
                int entryLength = Int32.Parse(entry.Substring(3, 4));
                int entryOffset = Int32.Parse(entry.Substring(7,5));
                string entryData = marcRecord.Substring(baseAddress + entryOffset, entryLength - 1);
                if (entryTag < 010)
                {
                    field = new Field(entryTag, entryData);
                }
                else
                {
                    string[] marcSubfields = entryData.Split(Features.SUBFIELD_INDICATOR);
                    List<string> subfields = new List<string> { };
                    int indicatorsArrayLength = marcSubfields[0].Length;
                    
                    string entryIndicator1;
                    string entryIndicator2; 
                    if (indicatorsArrayLength > 1)
                    {
                        entryIndicator1 = marcSubfields[0][0].ToString();
                        entryIndicator2 = marcSubfields[0][1].ToString();
                    }
                    else if (indicatorsArrayLength == 1)
                    {
                        entryIndicator1 = marcSubfields[0][0].ToString();
                        entryIndicator2 = "";
                    }
                    else
                    {
                        entryIndicator1 = "";
                        entryIndicator2 = "";
                    }

                    string[] entryIndicators = new string[2]{entryIndicator1,entryIndicator2};
                    for (int i = 1; i < marcSubfields.Length; i++)
                    { 
                        //not sure about this exactly
                        if (marcSubfields[i].Length != 0)
                        {
                            string code = marcSubfields[i].Substring(0, 1);
                            string data = marcSubfields[i].Substring(1);
                            subfields.Add(code);
                            subfields.Add(data);
                        }
                       
                    }
                    field = new Field(entryTag, entryIndicators, subfields);  
                }

                this.AddField(field); 

            }
        }


        public string AsMARC21()
        {
            string fields = "";
            string directory = "";
            int offset = 0;
            int baseAddress;
            int recordLength;

            /*
        # build the directory
        # each element of the directory includes the tag, the byte length of 
        # the field and the offset from the base address where the field data
        # can be found
             */
            foreach (Field field in this.fields)
            {
                string fieldData = field.AsMARC21();
                fields += fieldData;
                directory += field.tag+fieldData.Length.ToString("0000")+ offset.ToString("00000");
                offset += fieldData.Length;
            }

            directory += Features.END_OF_FIELD;
            fields += Features.END_OF_RECORD;
            baseAddress = Features.LEADER_LEN + directory.Length;
            recordLength = baseAddress + fields.Length;
            this.leader = recordLength.ToString("00000") + this.leader.Substring(5, 7)
                            + baseAddress.ToString("00000") + this.leader.Substring(17);
            return this.leader+directory+fields;
        
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.Append(this.FormatedLeader);
            text.Append("\r\n");
            foreach(Field field in this.Fields)
            { 
                text.Append(field.ToString());
                text.Append("\r\n");
            }
            return text.ToString();
        }

        /*
         * Outputs a string using the MARCBreaker format
         */ 
        public string MARCMakerFormat()
        {
            StringBuilder text = new StringBuilder();
            text.Append(this.FormatedLeader);   //leader must be ASCII so no need to use Mnemonics 
            text.Append("\r\n");
            foreach (Field field in this.Fields)
            {
                text.Append(field.MARCMakerFormat());
                text.Append("\r\n");
            }
            text.Append("\r\n");//needed to end record
            return text.ToString();
        }

        public bool HasField(int fieldTag)
        {
            foreach (Field field in this.fields)
            {
                if (field.tag == fieldTag.ToString("000"))
                {
                    return true;
                }
            }
            return false;
        
        }


        public void AddField(Field field)
        {
            this.fields.Add(field);
        }

        public void AddFields(Field[] fields)
        {
            foreach (Field field in fields)
            {
                this.fields.Add(field);
            }
        }

        public void RemoveField(int tag)
        {

            Field[] fieldsArray = new Field[this.fields.ToArray().Length];
            this.fields.CopyTo(fieldsArray);
            foreach (Field field in fieldsArray)
            {
                if (Int32.Parse(field.tag) == tag)
                    this.fields.Remove(field);
            }
        }

        public void RemoveFields(int[] tags)
        {
            foreach (int tag in tags)
            {
                this.RemoveField(tag);
            }
        
        }

        //used to map one field to another in processing MARCRecords
        public void MapField(int sourceTag, int destinationTag)
        {
            foreach (Field field in this.fields)
            {
                if (Int32.Parse(field.tag) == sourceTag)
                {
                       field.tag = destinationTag.ToString("000");
                }

            }
        }

        public string Leader
        {
            get { return this.leader; }
        }

        //returns a string in the format "=LDR   ..."
        public string FormatedLeader
        {
            get { return "=LDR  " + this.leader; }
        }

        public List<Field> Fields
        {
            get { return this.fields; }
        }

        public Field[] GetField(int fieldNumber)
        {
            List<Field> returnFields = new List<Field> { };
            foreach(Field field in this.fields)
            {
                if (field.tag == fieldNumber.ToString("000"))
                {
                    returnFields.Add(field);
                }
            }
            return returnFields.ToArray();
        }

        public Dictionary<int,Field[]> GetFields(int[] fieldTags)
        {
            Dictionary<int, Field[]> returnDictionary = new Dictionary<int, Field[]> { };
            foreach(int tag in fieldTags)
            {
                returnDictionary.Add(tag,this.GetField(tag));
            }
            return returnDictionary;
        
        }

        public string Title
        {
            get
            {
                string title = null;
                Field[] titleField = this.GetField(245);
                if (titleField.Length > 0)
                {
                    string[] subfields = titleField[0].Subfields;
                    switch (subfields.Length / 2)
                    {
                        case 1: title = subfields[1];
                            break;
                        case 2: title = subfields[1] + subfields[3];
                            break;
                    }
                }
                return title;
            }
        }
        public void SortFields()
        {
            this.fields.Sort(CompareFields);
            
        }

        //compares marcfields for SortFields
        //btw I know this is ugly but this is actually based on the *example* code
        //msdn gives.
        private static int CompareFields(Field field1, Field field2)
        {
            if (field1 == null)
            {
                if (field2 == null)
                {
                    //both fields null, they are equal
                    return 0;
                }
                else
                {
                    //only field1 is null <
                    return -1;
                }
            }
            else
            {
                if (field2 == null)
                {
                    if (field1 == null)
                    {
                        //again both null
                        return 0;
                    }
                    else
                    {
                    return 1;
                    }
                }
                    //all cases of nulls have been delt with
                else
                      {
                        int diff = Int32.Parse(field1.tag) - Int32.Parse(field2.tag);
                        if (diff == 0)
                        {
                            return 0;
                        }
                        else if (diff < 0)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                       }

                    }
               
            }

        public string ISBN
        {
            get 
            {
                string isbn = null;
                Field[] subjectField = this.GetField(20);
                if (subjectField.Length > 0)
                {
                    //this pattern is straight from pymarc
                    Regex pattern = new Regex("^([0-9A-Za-z]+)");
                    isbn = pattern.Match(subjectField[0].Subfields[1]).ToString();

                }
                return isbn;
            }
        }
        public string Author
        {
            get {
                Field[] oneHundred = this.GetField(100);
                if (oneHundred.Length > 0)
                    { return oneHundred[0].FormatField(); }
                Field[] oneTen = this.GetField(110);
                if (oneTen.Length > 0)
                    { return oneTen[0].FormatField(); }
                Field[] oneEleven = this.GetField(111);
                if (oneEleven.Length > 0)
                { return oneEleven[0].FormatField(); }
                return null;
            }
        }
        public string UniformTitle
        {
            get {
                return "";
            }
        }
        public string[] Subjects
        {
            get {
                return null;
            }
        }
        public string[] AddedEntries
        {
            get {
                return null;
            }

        }
        public string Location
        {
            get {
                return "";
            }
        }
        public string Notes
        {
            get {
                return "";
            }

        }
        public string Publisher
        {
            get {
                return "";
            }

        }
        public string PubYear
        {
            get {
                return "";
            }
        }
        
    }
}
