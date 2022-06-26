using System;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;

namespace RuFramework.RuConfig
{

    // Class + Converter Address
    #region Class


    [TypeConverter(typeof(AddressConverter))]
    public class Address
    {

        public string oldfirstname = String.Empty;

        // Fields
        private string firstname = String.Empty;
        private string lastname = String.Empty;
        private string zipcode = String.Empty;
        private string city = String.Empty;
        private string streat = String.Empty;

        // Constructor
        public Address(string Firstname = null, string Lastname = null, string Zipcode = null, string City = null, string Streat = null)
        {
            this.firstname = Firstname;
            this.lastname = Lastname;
            this.zipcode = Zipcode;
            this.city = City;
            this.streat = Streat;
        }


        [Category("Behavior"), Description("First name of address."), NotifyParentProperty(true)]
        public virtual String Firstname
        {
            set
            {
                firstname = value;
            }
            get
            {
                oldfirstname = firstname;

                return firstname;
            }
        }

        [Category("Behavior"), Description("Last name of address."), NotifyParentProperty(true)]
        public virtual String Lastname
        {
            set { lastname = value; }
            get { return lastname; }
        }

        [Category("Behavior"), Description("Zipcode of address."), NotifyParentProperty(true)]
        public virtual String Zipcode
        {
            set
            {
                zipcode = value;
            }
            get { return zipcode; }
        }

        [Category("Behavior"), Description("City of address."), NotifyParentProperty(true)]
        public virtual String City
        {
            set { city = value; }
            get { return city; }
        }

        [Category("Behavior"), Description("Streat of address."), NotifyParentProperty(true)]
        public virtual String Streat
        {
            set { streat = value; }
            get { return streat; }
        }

        // ToString() used ConvertTo(typeof(string))
        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }
        public string ToString(CultureInfo culture)
        {
            return (string)TypeDescriptor.GetConverter(GetType()).ConvertTo(null, culture, this, typeof(string));
        }

    }
    #endregion
    #region Converter
    public class AddressConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Address address = new Address();
            if (value == null)
            {
                return address;
            }

            if (value is string)
            {
                string s = (string)value;
                if (s.Length == 0)
                {
                    return address;
                }

                //    <value>[Name: Last=Ruttkowski, First=Klaus, Zipcode=53721, City=Siegburg, Streat=Aggerstr. 77]</value>
                if (s.Substring(0, 9) == "[Address:")
                {
                    int pos = 0;
                    string[] elements = s.Split(new Char[] { ',', ']' });

                    //  int c = elements.Count();
                    if (elements.Count() == 6)
                    {
                        pos = elements[0].IndexOf("Lastname=");
                        if (pos > -1) address.Lastname = elements[0].Substring(pos + 9);
                        pos = elements[1].IndexOf("Firstname=");
                        if (pos > -1) address.Firstname = elements[1].Substring(pos + 10);
                        pos = elements[2].IndexOf("Zipcode=");
                        if (pos > -1) address.Zipcode = elements[2].Substring(pos + 8);
                        pos = elements[3].IndexOf("City=");
                        if (pos > -1) address.City = elements[3].Substring(pos + 5);
                        pos = elements[4].IndexOf("Streat=");
                        if (pos > -1) address.Streat = elements[4].Substring(pos + 7);
                    }
                    //###
                }
                else
                {
                    return new Address();
                }
                return address;
            }

            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is Address))
                {
                    throw new ArgumentException(
                        "Invalid Name", "value");
                }
            }

            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return String.Empty;
                }

                Address address = (Address)value;
                return String.Format("[Address: Lastname={0}, Firstname={1}, Zipcode={2}, City={3}, Streat={4}]",
                    address.Lastname, address.Firstname, address.Zipcode, address.City, address.Streat);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null) throw new ArgumentNullException("propertyValues");

            object firstnamevalue = propertyValues["Firstname"];
            object lastnamevalue = propertyValues["Lastname"];
            object zipcodevalue = propertyValues["Zipcode"];
            object cityvalue = propertyValues["City"];
            object streatvalue = propertyValues["Streat"];
            return new Address((string)firstnamevalue,
                                (string)lastnamevalue,
                                (string)zipcodevalue,
                                (string)cityvalue,
                                (string)streatvalue);
        }
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(Address), attributes).Sort(new string[] { "Lastname", "Firstname", "Zipcode", "City", "Streat" });
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
    #endregion
}
