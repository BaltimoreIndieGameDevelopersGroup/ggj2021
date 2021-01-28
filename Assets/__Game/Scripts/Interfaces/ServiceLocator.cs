namespace Game
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Use this class to register components and find registered components without
    /// having to use GetComponent, FindObjectOfType, or using a manual reference.
    /// Examples:
    /// 
    /// Register:
    ///   PlayerController Start() { ServiceLocator.Register<IPlayerController>(this); }
    /// 
    /// Get the player controller:
    ///   var playerController = ServiceLocator.Get<IPlayerController>();
    /// </summary>
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> Services = new Dictionary<Type, object>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            Services = new Dictionary<Type, object>();
        }

        public static void Register<T>(object serviceInstance)
        {
            Services[typeof(T)] = serviceInstance;
        }

        public static T Get<T>()
        {
            return (T)Services[typeof(T)];
        }

        public static void Reset()
        {
            Services.Clear();
        }
    }
}
