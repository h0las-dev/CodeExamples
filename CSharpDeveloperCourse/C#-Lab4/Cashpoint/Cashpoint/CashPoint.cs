namespace Cashpoint
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;

    [Serializable]
    public sealed class CashPoint
    {
        [DataMember(Name = "banknotes")]
        private Dictionary<uint, byte> banknotes = new Dictionary<uint, byte>();

        [NonSerialized]
        private uint total = 0;

        [NonSerialized]
        private int[] granted = { 1 };

        public uint Total
        {
            get
            {
                return this.total;
            }
        }

        public int Count
        {
            get
            {
                var count = 0;
                foreach (var banknote in this.banknotes)
                {
                    count += Convert.ToInt32(banknote.Value);
                }

                return count;
            }
        }

        public static CashPoint LoadFromXmlFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("Should be not null", "filename");
            }

            using (var stream = new FileStream(filename, FileMode.Open))
            {
                var xmlSerializer = new DataContractSerializer(typeof(CashPoint));
                var cashpoint = (CashPoint)xmlSerializer.ReadObject(stream);

                cashpoint.total = 0;
                foreach (var banknote in cashpoint.banknotes)
                {
                    cashpoint.total += banknote.Key * banknote.Value;
                }

                cashpoint.CalculateGrants();

                return cashpoint;
            }
        }

        public void SaveToXmlFile(string filename)
        {
            using (var writer = XmlWriter.Create(filename, new XmlWriterSettings { Indent = true }))
            {
                var xmlSerializer = new DataContractSerializer(typeof(CashPoint));
                xmlSerializer.WriteObject(writer, this);
            }
        }

        public void AddBanknote(uint value)
        {
            if (!this.banknotes.ContainsKey(value))
            {
                this.banknotes.Add(value, 1);
            }
            else
            {
                this.banknotes[value] = Convert.ToByte(this.banknotes[value] + 1);
            }

            this.total += value;
            this.UpdateAddedGrants(value, 1);
        }

        public void AddBanknote(uint value, byte count)
        {
            if (count != 0)
            {
                if (!this.banknotes.ContainsKey(value))
                {
                    this.banknotes.Add(value, count);
                }
                else
                {
                    this.banknotes[value] = Convert.ToByte(this.banknotes[value] + count);
                }

                this.total += value * count;

                this.UpdateAddedGrants(value, count);
            }
        }

        public void RemoveBanknote(uint value)
        {
            if (this.banknotes.ContainsKey(value))
            {
                if (Convert.ToByte(this.banknotes[value]) > 1)
                {
                    this.banknotes[value] = Convert.ToByte(this.banknotes[value] - 1);
                }
                else
                {
                    this.banknotes.Remove(value);
                }

                this.total -= value;

                this.UpdateRemovedGrants(value, 1);
            }
        }

        public void RemoveBanknote(uint value, byte count)
        {
            if (this.banknotes.ContainsKey(value) && count != 0)
            {
                if (Convert.ToByte(this.banknotes[value]) > count)
                {
                    this.banknotes[value] = Convert.ToByte(this.banknotes[value] - count);
                }
                else if (Convert.ToByte(this.banknotes[value]) == count)
                {
                    this.banknotes.Remove(value);
                }
                else
                {
                    throw new Exception();
                }

                this.total -= value * count;

                this.UpdateRemovedGrants(value, count);
                //CalculateGrants();
            }
        }

        public bool CanGrant(uint value)
        {
            if (value > this.total)
            {
                return false;
            }

            if (this.granted[(int)value] != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CalculateGrants()
        {
            this.granted = new int[this.total + 1];
            this.granted[0] = 1;

            foreach (var banknote in this.banknotes)
            {
                for (var i = (int)this.total; i >= 0; i--)
                {
                    if (this.granted[i] != 0)
                    {   
                        for (var b = banknote.Key;
                            b <= banknote.Key * banknote.Value;
                            b += banknote.Key)
                        {
                            this.granted[i + b]++;
                        }
                    }
                }
            }
        }

        private void UpdateAddedGrants(uint banknote, byte amount)
        {
            Array.Resize(ref this.granted, (int)this.total + 1);

            for (var i = (int)this.total; i >= 0; i--)
            {
                if (this.granted[i] != 0)
                {
                    for (var b = banknote; b <= banknote * amount; b += banknote)
                    {
                        this.granted[i + b]++;
                    }
                }
            }
        }

        private void UpdateRemovedGrants(uint banknote, byte amount)
        {
            var res = 0;

            if (!this.banknotes.ContainsKey(banknote))
            {
                res = 0;
            }
            else
            {
                res = this.banknotes[banknote] - amount;
            }

            Array.Resize(ref this.granted, Convert.ToInt32(this.total + 1));

            for (var i = (int)this.total; i >= 0; i--)
            {
                if (this.granted[i] != 0)
                {
                    for (var b = banknote; b <= banknote * amount; b += banknote)
                    {
                        if ((i + b < this.Total) && (i + b > 0))
                        {
                            if ((res == 0 && this.granted[i + b] != 0) || (res != 0 && b > banknote * res))
                            {
                                this.granted[i + b]--;
                            }
                        }
                    }
                }
            }
        }
    }
}