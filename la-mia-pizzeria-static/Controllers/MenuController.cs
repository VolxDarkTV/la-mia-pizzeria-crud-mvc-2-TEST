﻿using la_mia_pizzeria_static.Migrations;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index(string? message)
        {
            using (PizzaContext db = new PizzaContext())
            {
                if(message != null)
                    ViewData["Message"] = message;
                List<Pizza> pizza = db.pizze.ToList<Pizza>();
                return View(pizza);
            }
        }

        public IActionResult Details(int id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizza = db.pizze.FirstOrDefault(m => m.Id == id);
            return View("Show", pizza);
        }

        [HttpGet]
        public IActionResult Create()
        {
            using PizzaContext db = new PizzaContext();
            List<Category> categories = db.categories.ToList();

            PizzaFormModel model = new PizzaFormModel();
            model.Pizza = new Pizza();
            model.Categories = categories;
            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using PizzaContext context = new PizzaContext();
                List<Category> category = context.categories.ToList();
                data.Categories = category;
                return View("Create", data);
            }

            using PizzaContext db = new PizzaContext();
            Pizza pizzaToCreate = new Pizza();
            pizzaToCreate.Nome = data.Pizza.Nome;
            pizzaToCreate.Descrizione = data.Pizza.Descrizione;
            pizzaToCreate.Image = data.Pizza.Image;
            pizzaToCreate.Price = data.Pizza.Price;
            pizzaToCreate.CategoryId = data.Pizza.CategoryId;


            db.pizze.Add(pizzaToCreate);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        //Edit Method
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizzaEdit = db.pizze.FirstOrDefault(m => m.Id == Id);

            if(pizzaEdit == null)
            {
                return NotFound();
            }
            else
            {
                return View(pizzaEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (int Id, Pizza data)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            using PizzaContext db = new PizzaContext();
            Pizza pizzaEdit = db.pizze.FirstOrDefault(m => m.Id == data.Id);
            if(pizzaEdit != null)
            {

                pizzaEdit.Nome = data.Nome;
                pizzaEdit.Descrizione = data.Descrizione;
                pizzaEdit.Image = data.Image;
                pizzaEdit.Price = data.Price;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

        }



        //Delete Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizzaToDelete = db.pizze.Where(m => m.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                db.pizze.Remove(pizzaToDelete);
                db.SaveChanges();

                return RedirectToAction("Index", new {message = "Piatto Eliminato con successo!"});
            }
            else
            {
                return NotFound();
            }
        }
        
    }
}
