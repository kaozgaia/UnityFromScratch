using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Creational
{
    public interface IFactory<TFactory>
    {
        TProduct Build<TProduct>() where TProduct : IProduct<TFactory>, new();
    }
    
    public interface IProduct<TFactory>
    {
        // Contract Methods for IProduct
    }

    #region Test Generic Base Class

    public sealed class Car : IFactory<Car>
    {
        public TProduct Build<TProduct>() where TProduct : IProduct<Car>, new()
        {
            Debug.Log("Creating Car: " + typeof(TProduct));
            return new TProduct();
        }
        
    }

    #endregion

    #region Abstract Factory

    class Factory<TFactory> where TFactory : IFactory<TFactory>, new()
    {
        public TProduct Create<TProduct>() where TProduct : IProduct<TFactory>, new()
        {
            Debug.Log("Creating Car: " + typeof(TProduct));
            return new TFactory().Build<TProduct>();
        }
    }

    #endregion

    #region Test Clases

    public class Honda : IProduct<Car>
    {

    }

    public class Toyota : IProduct<Car>
    {

    }

    #endregion

    #region Test Class

    public class TestFactoryClass
    {
        public void CreateSomething()
        {
            Factory<Car> carFactory = new Factory<Car>();

            Honda myNewCar = carFactory.Create<Honda>();

        }
    }

    #endregion

}


