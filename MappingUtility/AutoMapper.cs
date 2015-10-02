using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using MappingUtility.Exceptions;

namespace MappingUtility
{
   public class PropertyMaper
   {
      public PropertyInfo SourceProperty { get; set; }
      public PropertyInfo TargetProperty { get; set; }
   }

   public abstract class AutoMapper
   {
      public abstract void MapTypes<S, T>();
      public abstract void ApplyMapping<S, T>(S source, T target);
      public abstract IEnumerable<T> ApplyMappingContainer<S, T>(IEnumerable<S> sourceList);
      protected virtual IList<PropertyMaper> GetIdenticProperties<S, T>()
      {
         var sourceProperties = typeof(S).GetProperties();
         var targetProperties = typeof(T).GetProperties();

         var identicProperties = (from s in sourceProperties
                           from t in targetProperties
                           where s.Name.Equals(t.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                 s.CanRead &&
                                 t.CanWrite &&
                                 s.PropertyType == t.PropertyType
                           select new PropertyMaper
                           {
                              SourceProperty = s,
                              TargetProperty = t
                           }).ToList();
         return identicProperties;
      }

      protected virtual string GetMapingKey<S, T>()
      {
         var className = typeof(T).Name;
         return className;
      }
   }
   public class Mapper : AutoMapper
   {
      private delegate void CopyPublicPropertiesDelegate<S, T>(S source, T target);
      private readonly Dictionary<string, object> _del =new Dictionary<string, object>();

      public override void MapTypes<S, T>()
     {
         var key = GetMapingKey<S, T>();
         if (_del.ContainsKey(key))
            throw new MapperAlreadyRegisteredException(typeof(S), typeof(T));

         var source = typeof(S);
         var target = typeof(T);

         var args = new[] { source, target };
         var mod = typeof(Program).Module;

         var dm = new DynamicMethod(key, null, args, mod);
         var il = dm.GetILGenerator();
         var maps = GetIdenticProperties<S, T>();

         foreach (var map in maps)
         {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt,
                        map.SourceProperty.GetGetMethod(), null);
            il.EmitCall(OpCodes.Callvirt,
                        map.TargetProperty.GetSetMethod(), null);
         }
         il.Emit(OpCodes.Ret);
         var del = dm.CreateDelegate(
                   typeof(CopyPublicPropertiesDelegate<S, T>));
         _del.Add(key, del);
      }
      public override void ApplyMapping<S, T>(S source, T target)
      {
         var key = GetMapingKey<S, T>();

         if (!_del.ContainsKey(key))
            throw new MapperNotRegisteredException(typeof(S), typeof(T));

         var del = (CopyPublicPropertiesDelegate<S, T>)_del[key];

         if(source == null)
            throw new ArgumentNullException();

         del.Invoke(source, target);
      }
      public override IEnumerable<T> ApplyMappingContainer<S, T>(IEnumerable<S> sourceList)
      {
         var key = GetMapingKey<S, T>();

         if (!_del.ContainsKey(key))
            throw new MapperNotRegisteredException(typeof(S), typeof(T));

         List<T> targetList = new List<T>();

         foreach (var s in sourceList)
         {
         T targetItem = Activator.CreateInstance<T>();
         var del = (CopyPublicPropertiesDelegate<S, T>)_del[key];
         del.Invoke(s, targetItem);
            targetList.Add(targetItem);
         }

         return targetList;
      }
   }
}
