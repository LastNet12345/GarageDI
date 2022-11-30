using GarageDI.Attributes;
using GarageDI.Constants;
using GarageDI.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using VehicleCollection.Interfaces;

namespace GarageDI.Utils
{
    public static class Extensions
    {
        public static (IVehicle, PropertyInfo[]) GetInstanceAndPropsForType(this VehicleType vehicleType)
        {
            Type type = Type.GetType($"{StaticStringHelper.Assembly}.{vehicleType}", throwOnError: true)!;

            IVehicle? vehicle = Activator.CreateInstance(type) as IVehicle;

            ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

            PropertyInfo[] properties = vehicle.GetPropertiesWithIncludedAttribute();

            return (vehicle, properties);
        } 

        public static PropertyInfo[] GetPropertiesWithIncludedAttribute<T>(this T type)  where T : IVehicle
        {
                 return type.GetType()
                            .GetProperties()
                            .Where(p => p.IsDefined(typeof(Include), true))
                            .OrderBy(p => ((Include)p.GetCustomAttribute(typeof(Include))!).Order)
                            .ToArray();
        }

        public static string GetDisplayText(this PropertyInfo prop)
        {
            var attr = prop.GetCustomAttribute<Beautify>();
            return attr is null ? prop.Name : attr.Text;
        }

    }
}
  
