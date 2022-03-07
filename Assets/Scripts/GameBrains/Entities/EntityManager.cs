using System.Collections.Generic;
using UnityEngine;

namespace GameBrains.Entities
{
    public class EntityManager : MonoBehaviour
    {
        static int nextId = 0;

        static readonly Dictionary<int, Entity> EntityIdDictionary
            = new Dictionary<int, Entity>();
        static readonly Dictionary<string, Entity> EntityNameDictionary
            = new Dictionary<string, Entity>();

        public static int NextID => nextId++;

        public static void Add(Entity entity)
        {
            if (!EntityIdDictionary.ContainsKey(entity.ID))
            {
                EntityIdDictionary.Add(entity.ID, entity);
            }

            // TODO: catch duplicate name errors
            if (!EntityNameDictionary.ContainsKey(entity.name))
            {
                EntityNameDictionary.Add(entity.name, entity);
            }
        }

        public static void Remove(Entity entity)
        {
            EntityIdDictionary.Remove(entity.ID);
            EntityNameDictionary.Remove(entity.name);
        }

        public static T Find<T>(int entityId) where T : Entity
        {
            if (!EntityIdDictionary.ContainsKey(entityId))
            {
                return null;
            }

            return EntityIdDictionary[entityId] as T;
        }

        public static T Find<T>(string entityName) where T : Entity
        {
            if (!EntityNameDictionary.ContainsKey(entityName))
            {
                return null;
            }

            return EntityNameDictionary[entityName] as T;
        }

        public static List<T> FindAll<T>() where T : Entity
        {
            List<T> resultList = new List<T>();
            foreach (Entity entity in EntityIdDictionary.Values)
            {
                T entityT = entity as T;

                if (entityT != null)
                {
                    resultList.Add(entityT);
                }
            }

            return resultList;
        }
    }
}