using System.Data.SQLite;
using FastMember;

namespace PP.Data.Models
{
    public static class ModelExtensions
    {


        public static void Restore<T>(this T item, SQLiteConnection con) where T:ModelBase<T>
        {
            var model = con.GetById<T>(item.Id);
            item.Sync(model);
        }

        public static void Sync<T>(this T target, T source)
        {
            var type = TypeAccessor.Create(typeof(T));
            var members = type.GetMembers();

            var oTarget = ObjectAccessor.Create(target);
            var oSource = ObjectAccessor.Create(source);

            foreach(var member in members)
            {
                if(member.Name == "Item") continue;
                if(member.IsDefined(typeof(NonClonable))) continue;
                if (!(typeof(T).GetProperty(member.Name)?.CanWrite ?? false)) continue;
                
                oTarget[member.Name] = oSource[member.Name];
            }
        }

        public static T Clone<T>(this T obj)
        {
            var type = TypeAccessor.Create(typeof(T));
            var clone = type.CreateNew();
            var newObj = ObjectAccessor.Create(clone);
            var oldObj = ObjectAccessor.Create(obj);

            var members = type.GetMembers();
            foreach(var member in members)
            {
                if(member.Name == "Item") continue;
                if(member.IsDefined(typeof(NonClonable))) continue;
                if(typeof(T).GetProperty(member.Name)?.CanWrite ?? false)
                    newObj[member.Name] = oldObj[member.Name];
            }

            return (T)clone;
        }
    }
}