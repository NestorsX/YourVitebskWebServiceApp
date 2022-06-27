using YourVitebskWebServiceApp.Controllers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using System;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;
using System.Linq;

namespace YourVitebskWebServiceApp.Tests.ControllerTests
{
    public class VacanciesControllerTests
    {
        private readonly Mock<IVacancyRepository> _mockRepo;
        private readonly VacanciesController _controller;

        public VacanciesControllerTests()
        {
            _mockRepo = new Mock<IVacancyRepository>();
            _controller = new VacanciesController(_mockRepo.Object);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.VacanciesGet))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.VacanciesCreate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.VacanciesUpdate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.VacanciesDelete))).Returns(true);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<Vacancy>()
            {
                new Vacancy
                {
                    VacancyId = 1,
                    Title = "Title1",
                    Description = "Description1",
                    Address = "Address1",
                    CompanyName = "CompanyName1",
                    Requirements = "Requirements1",
                    Conditions = "Conditions1",
                    Contacts = "Contacts1",
                    Salary = "Salary1",
                    PublishDate = DateTime.Now
                },
                new Vacancy
                {
                    VacancyId = 2,
                    Title = "Title2",
                    Description = "Description2",
                    Address = "Address2",
                    CompanyName = "CompanyName2",
                    Requirements = "Requirements2",
                    Conditions = "Conditions2",
                    Contacts = "Contacts2",
                    Salary = "Salary2",
                    PublishDate = DateTime.Now
                }
            });

            _mockRepo.Setup(repo => repo.Get(2)).Returns(new Vacancy
            {
                VacancyId = 2,
                Title = "Title2",
                Description = "Description2",
                Address = "Address2",
                CompanyName = "CompanyName2",
                Requirements = "Requirements2",
                Conditions = "Conditions2",
                Contacts = "Contacts2",
                Salary = "Salary2",
                PublishDate = DateTime.Now
            });
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var result = _controller.Index(null);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ReturnsExactNumberOfObjects()
        {
            var result = _controller.Index(null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var objects = Assert.IsType<VacancyIndexViewModel>(viewResult.Model);
            Assert.Equal(2, objects.Data.Count());
        }

        [Fact]
        public void Create_ReturnsView()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new Vacancy
            {
                VacancyId = null,
                Title = "TestTitle2",
                Description = "TestDescription2",
                Address = "TestAddress2",
                CompanyName = "TestCompanyName2",
                Requirements = "TestRequirements2",
                Conditions = "TestConditions2",
                Contacts = "TestContacts2",
                Salary = "TestSalary2",
                PublishDate = DateTime.Now
            };

            var result = _controller.Create(obj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_ReturnsView()
        {
            var result = _controller.Edit(2);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.Edit(-1);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Edit_RedirectsToIndex()
        {
            var newObj = new Vacancy
            {
                VacancyId = 2,
                Title = "TestTitle3",
                Description = "TestDescription3",
                Address = "TestAddress3",
                CompanyName = "TestCompanyName3",
                Requirements = "TestRequirements3",
                Conditions = "TestConditions3",
                Contacts = "TestContacts3",
                Salary = "TestSalary3",
                PublishDate = DateTime.Now
            };

            var result = _controller.Edit(newObj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ReturnsView()
        {
            var result = _controller.ConfirmDelete(2);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.ConfirmDelete(-1);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Delete_RedirectsToIndex()
        {
            var result = _controller.Delete(2);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
