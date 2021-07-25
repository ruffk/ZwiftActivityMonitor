using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ZwiftActivityMonitorV2
{
    class PropertyInfoHelper
    {

        //Get a List of the properties from a type
        public static PropertyInfo[] ListOfPropertiesFromInstance(Type AType)
        {
            if (AType == null) return null;
            
            return AType.GetProperties(BindingFlags.Public);
        }

        //Get a List of the properties from a instance of a class
        public static PropertyInfo[] ListOfPropertiesFromInstance(object InstanceOfAType)
        {
            if (InstanceOfAType == null) return null;

            Type TheType = InstanceOfAType.GetType();
            
            return TheType.GetProperties(BindingFlags.Public);
        }

        //perfect for usage example and Get a Map of the properties from a instance of a class
        public static Dictionary<string, PropertyInfo> DictionaryOfPropertiesFromInstance(object InstanceOfAType)
        {
            if (InstanceOfAType == null) return null;

            Type TheType = InstanceOfAType.GetType();
            PropertyInfo[] Properties = TheType.GetProperties(BindingFlags.Public);
            
            Dictionary<string, PropertyInfo> PropertiesMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo Prop in Properties)
            {
                PropertiesMap.Add(Prop.Name, Prop);
            }
            
            return PropertiesMap;
        }
    }
}
