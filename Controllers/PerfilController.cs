using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using PNTProyecto.Models;
using Microsoft.EntityFrameworkCore;



public class PerfilController : Controller
{
    public IActionResult Index()
    {
         return View();

    }

}