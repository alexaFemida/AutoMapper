using System;
using System.Collections.Generic;
using System.Linq;
using MappingUtility;
using MappingUtility.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MappingUtility.Models;

namespace MappingUtilityTest
{
   [TestClass]
   public class MappingUtilityTest
   {
      [TestMethod]
      [ExpectedException(typeof(MapperNotRegisteredException))]
      public void Mapper_has_not_been_registered_should_throw_mapper_not_registered_exception()
      {
         IosContactName iosContactName = new IosContactName();
         var wcfContactName = new WcfContactName()
                              {
                                 Login = "User1",
                                 FirstName = "Nick",
                                 LastName = "Perry",
                                 Age = 22,
                                 Address = new Address() {City = "London", Street = "Golden Lane"}
                              };
         Mapper mapper = new Mapper();
         mapper.ApplyMapping(wcfContactName, iosContactName);
      }

      [TestMethod]
      [ExpectedException(typeof(MapperAlreadyRegisteredException))]
      public void Given_mapper_already_registered_should_throw_mapper_already_registered_exception()
      {
         Mapper mapper = new Mapper();
         mapper.MapTypes<WcfContactName, IosContactName>();
         mapper.MapTypes<WcfContactName, IosContactName>();
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Should_throw_null_arg_exception_if_source_is_null()
      {
         IosContactName iosContactName = new IosContactName();
         Mapper mapper = new Mapper();
         mapper.MapTypes<WcfContactName, IosContactName>();
         WcfContactName nullWcfContactName = null;
         mapper.ApplyMapping(nullWcfContactName, iosContactName);
      }

      [TestMethod]
      public void Should_set_values_where_name_matches_and_same_type()
      {
         IosContactName iosContactName = new IosContactName();
         var wcfContactName = new WcfContactName()
                              {
                                 Login = "User1",
                                 FirstName = "Nick",
                                 LastName = "Perry",
                                 Age = 22,
                                 Address = new Address() {City = "London", Street = "Golden Lane"}
                              };
         Mapper mapper = new Mapper();
         mapper.MapTypes<WcfContactName, IosContactName>();
         mapper.ApplyMapping(wcfContactName, iosContactName);
         Assert.AreEqual("Nick", iosContactName.FirstName);
      }

      [TestMethod]
      public void Should_ignore_property_where_name_matches_but_different_type()
      {
         IosContactName iosContactName = new IosContactName();
         var wcfContactName = new WcfContactName()
                              {
                                 Login = "User1",
                                 FirstName = "Nick",
                                 LastName = "Perry",
                                 Age = 22,
                                 Address = new Address() {City = "London", Street = "Golden Lane"}
                              };
         Mapper mapper = new Mapper();
         mapper.MapTypes<WcfContactName, IosContactName>();
         mapper.ApplyMapping(wcfContactName, iosContactName);
         Assert.AreEqual(null, iosContactName.Age);
      }

      [TestMethod]
      public void Should_set_values_where_name_matches_and_same_type_in_inner_class()
      {
         IosContactName iosContactName = new IosContactName();
         var wcfContactName = new WcfContactName()
                              {
                                 Login = "User1",
                                 FirstName = "Nick",
                                 LastName = "Perry",
                                 Age = 22,
                                 Address = new Address() {City = "London", Street = "Golden Lane"}
                              };
         Mapper mapper = new Mapper();
         mapper.MapTypes<WcfContactName, IosContactName>();
         mapper.ApplyMapping(wcfContactName, iosContactName);

         Assert.AreEqual(wcfContactName.Address.City, "London");
      }

      [TestMethod]
      public void Should_be_able_to_map_properties_for_each_source()
      {
         var address1 = new Address() {City = "Lviv", Street = "Zolota"};
         var address2 = new Address() {City = "Lviv", Street = "Horodotska"};

         Mapper mapper = new Mapper();

         List<Address> addresses = new List<Address> {address1, address2};

         mapper.MapTypes<Address, AddressDto>();
         IEnumerable<AddressDto> addressDtos = mapper.ApplyMappingContainer<Address, AddressDto>(addresses);

         Assert.IsTrue(addressDtos.Any(item => item.City == address1.City));
         Assert.IsTrue(addressDtos.Any(item => item.City == address2.City));
      }
   }
}
