using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SortingGeneriComplexObjects
{
    public static class AppExtensions
    {
        public static IQueryable<T> Query<T>(this IQueryable<T> source, Func<IQueryable<T>, IQueryable<T>> query)
        {
            if (query == null)
                return source;

            return query(source);
        }
        public static IQueryable<T> SortByField<T>(this IQueryable<T> query,
                                            string sortField,
                                            SortDirection direction)
        {
         

            
            if (HasProperty(sortField, typeof(T)))
            {   
             
                Type selectorResultType;

                LambdaExpression selector = GenerateSelector(sortField, typeof(T), out selectorResultType);

                return (IOrderedQueryable<T>)query.Provider.CreateQuery(
                                                      Expression.Call(typeof(Queryable), direction == SortDirection.ASC ? "OrderBy" : "OrderByDescending",
                                                                       new Type[] { typeof(T), selectorResultType },
                                                                       query.Expression,
                                                                       Expression.Quote(selector))                                                      
                                                      );
               
            }
            else
            {
                // if unable to locate the column name return the default query.
                return query;
            }


        }


        private static LambdaExpression GenerateSelector(String propertyName,Type type, out Type resultType)
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(type);
            //  create the selector part, but support child properties
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields.
                String[] childProperties = propertyName.Split('.');
                property = type.GetProperty(childProperties[0], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            resultType = property.PropertyType;
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }

        private static bool HasProperty(string sortField, Type objectType)
        {
            bool IsValidProperty;
            if (sortField.Contains('.'))
            {
                String[] propertyList = sortField.Split('.');                
                var baseProperty = objectType.GetProperty(propertyList[0], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (baseProperty != null)
                {
                    for (int i = 1; i < propertyList.Length; i++)
                    {
                        var childProperty = baseProperty.PropertyType.GetProperty(propertyList[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        //check if property exists
                        if(childProperty == null)
                        {                            
                            return false;                            
                        }                        
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
               var objectProperty = objectType.GetProperty(sortField, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                return objectProperty != null;
            }

                
        }
    }

    public enum SortDirection
    {
        ASC,
        DESC
    }
}
